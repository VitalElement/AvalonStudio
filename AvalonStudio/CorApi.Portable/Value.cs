// Copyright (c) 2017 Vital Element Avalon Studio - Dan Walmsley dan at walms dot co dot uk
// 
// This code is licensed for use only with AvalonStudio. It is not permitted for use in any 
// project unless explicitly authorized.
//

using SharpGen.Runtime;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace CorApi.Portable
{
    public partial class Value : ComObject
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

        public BoxValue CastToBoxValue()
        {
            return QueryInterfaceOrNull<BoxValue>();
        }

        public GenericValue CastToGenericValue()
        {
            return QueryInterfaceOrNull<GenericValue>();
        }

        public ArrayValue CastToArrayValue()
        {
            return QueryInterfaceOrNull<ArrayValue>();
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
