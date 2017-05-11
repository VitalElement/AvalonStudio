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
                    int count = 0;
                    GetName(0, out count, IntPtr.Zero);

                    if (count == 0)
                    {
                        return null;
                    }

                    var temp = stackalloc char[count];
                    GetName(count, out count, (IntPtr)temp);

                    return new string(temp, 0, count - 1);
                }
            }
        }

        public void SetJMCStatus(bool justMyCode, int[] tokensRef)
        {
            var length = 0;

            if(tokensRef!= null)
            {
                length = tokensRef.Length;
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
                QueryInterface<Module2>().JITCompilerFlags = value;
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

