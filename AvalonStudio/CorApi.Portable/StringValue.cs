// Copyright (c) 2017 Vital Element Avalon Studio - Dan Walmsley dan at walms dot co dot uk
// 
// This code is licensed for use only with AvalonStudio. It is not permitted for use in any 
// project unless explicitly authorized.
//

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
                    var count = 0u;
                    GetLength(out count);

                    if (count == 0)
                    {
                        return null;
                    }                    

                    var temp = stackalloc char[(int)count];
                    GetString(count, out count, (IntPtr)temp);

                    return new string(temp, 0, (int)count);
                }
            }
        }
    }
}
