using System;
using System.Collections.Generic;
using System.Text;

namespace CorApi.Portable
{
    public partial class Value
    {
        public ReferenceValue CastToReferenceValue()
        {
            return QueryInterfaceOrNull<ReferenceValue>();
        }

        public ObjectValue CastToObjectValue()
        {
            return QueryInterface<ObjectValue>();
        }

        public StringValue CastToStringValue()
        {
            return QueryInterface<StringValue>();
        }

        public HandleValue CastToHandleValue()
        {
            return QueryInterfaceOrNull<HandleValue>();
        }

        public HeapValue CastToHeapValue()
        {
            return QueryInterfaceOrNull<HeapValue>();
        }

        public Type ExactType
        {
            get
            {
                Value2 v2 = QueryInterface<Value2>();
                Type dt;
                v2.GetExactType(out dt);
                return dt;
            }
        }
    }
}
