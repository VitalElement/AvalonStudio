using System;
using System.Collections.Generic;
using System.Text;

namespace CorApi.Portable
{
    public partial class StringValue
    {
        public String String
        {
            get
            {
                unsafe
                {
                    int count = 0;
                    GetString(0, out count, IntPtr.Zero);

                    if (count == 0)
                    {
                        return null;
                    }

                    var temp = stackalloc char[count];
                    GetString(count, out count, (IntPtr)temp);

                    return new string(temp, 0, count - 1);
                }
            }
        }
    }
}
