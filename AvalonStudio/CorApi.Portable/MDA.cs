// Copyright (c) 2017 Vital Element Avalon Studio - Dan Walmsley dan at walms dot co dot uk
// 
// This code is licensed for use only with AvalonStudio. It is not permitted for use in any 
// project unless explicitly authorized.
//

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
                    uint count = 0;
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

        public String XML
        {
            get
            {
                unsafe
                {
                    uint count = 0;
                    GetXML(0, out count, IntPtr.Zero);

                    if (count == 0)
                    {
                        return null;
                    }

                    var temp = stackalloc char[(int)count];
                    GetXML(count, out count, (IntPtr)temp);

                    return new string(temp, 0, (int)count - 1);
                }
            }
        }
    }
}
