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
            QueryInterface<Module2>().SetJMCStatus(justMyCode, tokensRef.Length, tokensRef);
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

