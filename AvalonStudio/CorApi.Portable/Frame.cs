// Copyright (c) 2017 Vital Element Avalon Studio - Dan Walmsley dan at walms dot co dot uk
// 
// This code is licensed for use only with AvalonStudio. It is not permitted for use in any 
// project unless explicitly authorized.
//

using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace CorApi.Portable
{
    public enum CorFrameType
    {
        ILFrame, NativeFrame, InternalFrame,
    }

    public partial class Frame
    {
        private ILFrame m_ilFrame = null;
        private bool m_ilFrameCached = false;

        private InternalFrame m_iFrame = null;
        private bool m_iFrameCached = false;

        public int IP
        {
            get
            {
                return QueryInterface<NativeFrame>().IP;
            }
        }

        public void GetIP(out int offset, out CorDebugMappingResult mappingResult)
        {
            ILFrame ilframe = GetILFrame();
            if (ilframe == null)
            {
                offset = 0;
                mappingResult = CorDebugMappingResult.MappingNoInfo;
            }
            else
            {

                ilframe.GetIP(out uint uoffset, out mappingResult);
                offset = (int)uoffset;
            }
        }

        public void SetIP(int offset)
        {
            var ilframe = GetILFrame();
            if (ilframe == null)
                throw new Exception("Cannot set an IP on non-il frame");
            ilframe.SetIP(offset);
        }

        public bool CanSetIP(int offset)
        {
            var ilframe = GetILFrame();
            if (ilframe == null)
                return false;

            try
            {
                ilframe.CanSetIP(offset);
                return true;
            }
            catch (SharpGen.Runtime.SharpGenException)
            {
                return false;
            }
        }


        private ILFrame GetILFrame()
        {
            if (!m_ilFrameCached)
            {
                m_ilFrameCached = true;
                m_ilFrame = QueryInterfaceOrNull<ILFrame>();

            }
            return m_ilFrame;
        }

        private InternalFrame GetInternalFrame()
        {
            if (!m_iFrameCached)
            {
                m_iFrameCached = true;

                m_iFrame = QueryInterfaceOrNull<InternalFrame>();
            }
            return m_iFrame;
        }

        public CorFrameType FrameType
        {
            get
            {
                var ilframe = GetILFrame();
                if (ilframe != null)
                    return CorFrameType.ILFrame;

                var iframe = GetInternalFrame();
                if (iframe != null)
                    return CorFrameType.InternalFrame;

                return CorFrameType.NativeFrame;
            }
        }

        public CorDebugInternalFrameType InternalFrameType
        {
            get
            {
                InternalFrame iframe = GetInternalFrame();
                CorDebugInternalFrameType ft;

                if (iframe == null)
                    throw new Exception("Cannot get frame type on non-internal frame");

                iframe.GetFrameType(out ft);
                return ft;
            }
        }


        public void GetNativeIP(out int offset)
        {
            var nativeFrame = QueryInterfaceOrNull<NativeFrame>();
            Debug.Assert(nativeFrame != null);
            nativeFrame.GetIP(out var uoffset);
            offset = (int)uoffset;
        }

        public Value GetArgument(int index)
        {
            var ilframe = GetILFrame();
            if (ilframe == null)
                return null;

            Value value;
            try
            {
                ilframe.GetArgument((uint)index, out value);
            }
            catch (SharpGen.Runtime.SharpGenException e)
            {
                // If you are stopped in the Prolog, the variable may not be available.
                // CORDBG_E_IL_VAR_NOT_AVAILABLE is returned after dubugee triggers StackOverflowException
                if (e.HResult == 0x1304 || e.HResult == -2146233596) //cordbg_e_il_var_not_available //TODO Generate error codes in mapping.xaml.
                {
                    return null;
                }
                else
                {
                    throw;
                }
            }

            return value;
        }

        public int GetArgumentCount()
        {
            var ilframe = GetILFrame();
            if (ilframe == null)
                return -1;

            ValueEnum ve;
            ilframe.EnumerateArguments(out ve);
            uint count;
            ve.GetCount(out count);
            return (int)count;
        }

        public int GetLocalVariablesCount()
        {
            var ilframe = GetILFrame();
            if (ilframe == null)
                return -1;

            ValueEnum ve;
            ilframe.EnumerateLocalVariables(out ve);
            uint count;
            ve.GetCount(out count);
            return (int)count;
        }

        public Value GetLocalVariable(int index)
        {
            var ilframe = GetILFrame();
            if (ilframe == null)
                return null;

            Value value;
            try
            {
                ilframe.GetLocalVariable((uint)index, out value);
            }
            catch (SharpGen.Runtime.SharpGenException e)
            {
                // If you are stopped in the Prolog, the variable may not be available.
                // CORDBG_E_IL_VAR_NOT_AVAILABLE is returned after dubugee triggers StackOverflowException
                if (e.HResult == 0x1304 || e.HResult == -2146233596) //cordbg_e_il_var_not_available //TODO Generate error codes in mapping.xaml.
                {
                    return null;
                }
                else
                {
                    throw;
                }
            }
            return value;
        }

        public IEnumerable TypeParameters
        {
            get
            {
                TypeEnum icdte = null;
                ILFrame ilf = GetILFrame();

                ilf.QueryInterface<ILFrame2>().EnumerateTypeParameters(out icdte);
                return new TypeEnumerator(icdte);        // icdte can be null, is handled by enumerator
            }
        }
    }
}
