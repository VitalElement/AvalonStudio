using System;
using System.Collections.Generic;
using System.Text;

namespace CorApi.Portable
{
    public partial class AppDomain
    {
        /** The name of the CorAppDomain */
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

        public int Id
        {
            get
            {
                GetID(out int result);

                return result;
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
