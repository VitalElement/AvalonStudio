// Copyright (c) 2017 Vital Element Avalon Studio - Dan Walmsley dan at walms dot co dot uk
// 
// This code is licensed for use only with AvalonStudio. It is not permitted for use in any 
// project unless explicitly authorized.
//

using System;

namespace CorApi.Portable
{
    public partial class Module
    {
        public String Name
        {
            get
            {
                unsafe
                {
                    var count = 0u;
                    GetName(0, out count, IntPtr.Zero);

                    if (count == 0)
                    {
                        return null;
                    }

                    var temp = stackalloc char[(int)count];
                    GetName(count, out count, (IntPtr)temp);

                    return new string(temp, 0, (int)count - 1);
                }
            }
        }

        public void SetJMCStatus(bool justMyCode, uint[] tokensRef)
        {
            var length = 0u;

            if(tokensRef!= null)
            {
                length = (uint)tokensRef.Length;
            }

            QueryInterface<Module2>().SetJMCStatus(justMyCode, length, tokensRef);
        }

        public CorDebugJITCompilerFlags JITCompilerFlags
        {
            get
            {
                return QueryInterface<Module2>().JITCompilerFlags;
            }
            set
            {
                QueryInterface<Module2>().SetJITCompilerFlags(value);
            }
        }

        public Process Process
        {
            get
            {
                Process proc = null;
                GetProcess(out proc);
                return Portable.Process.GetCorProcess(proc.NativePointer);
            }
        }
    }
}

