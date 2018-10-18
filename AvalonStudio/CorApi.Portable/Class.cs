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
    public partial class Class
    {
        public Type GetParameterizedType(CorElementType elementType, Type[] typeArguments)
        {
            Type[] types = null;
            uint length = 0;
            if (typeArguments != null)
            {
                types = new Type[typeArguments.Length];
                for (int i = 0; i < typeArguments.Length; i++)
                    types[i] = typeArguments[i];
                length = (uint)typeArguments.Length;
            }

            Type pType = null;

            QueryInterface<Class2>().GetParameterizedType(elementType, length, types, out pType);

            return pType;
        }
    }
}
