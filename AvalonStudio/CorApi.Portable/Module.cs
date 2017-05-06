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
    }
}

