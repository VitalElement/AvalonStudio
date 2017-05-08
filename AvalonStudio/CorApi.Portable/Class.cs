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
            int length = 0;
            if (typeArguments != null)
            {
                types = new Type[typeArguments.Length];
                for (int i = 0; i < typeArguments.Length; i++)
                    types[i] = typeArguments[i];
                length = typeArguments.Length;
            }

            Type pType = null;

            QueryInterface<Class2>().GetParameterizedType((int)elementType, length, types, out pType);

            return pType;
        }
    }
}
