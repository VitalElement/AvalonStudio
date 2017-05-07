using System;
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
                mappingResult = CorDebugMappingResult.MappingNoInformation;
            }
            else
                ilframe.GetIP(out offset, out mappingResult);
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
            catch (SharpDX.SharpDXException e)
            {
                return false;
            }
        }


        private ILFrame GetILFrame()
        {
            if (!m_ilFrameCached)
            {
                m_ilFrameCached = true;
                m_ilFrame = QueryInterface<ILFrame>();

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
            nativeFrame.GetIP(out offset);
        }
    }
}
