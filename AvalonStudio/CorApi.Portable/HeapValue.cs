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
