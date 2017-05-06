using System;

namespace CorApi.Portable
{
    public partial class MDA
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

        public String XML
        {
            get
            {
                unsafe
                {
                    int count = 0;
                    GetXML(0, out count, IntPtr.Zero);

                    if (count == 0)
                    {
                        return null;
                    }

                    var temp = stackalloc char[count];
                    GetXML(count, out count, (IntPtr)temp);

                    return new string(temp, 0, count - 1);
                }
            }
        }
    }
}
