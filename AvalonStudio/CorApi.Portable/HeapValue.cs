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
    public partial class HeapValue
    {
        public HandleValue CreateHandle(CorDebugHandleType type)
        {
            HandleValue handle;
            QueryInterface<HeapValue2>().CreateHandle(type, out handle);
            return handle;
        }
    }
}
