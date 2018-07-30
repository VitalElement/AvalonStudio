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
    public partial class Type
    {
        public Type[] TypeParameters
        {
            get
            {
                var list = new List<Type>();
                TypeEnum etp = null;
                EnumerateTypeParameters(out etp);
                if (etp == null) return null;
                foreach (Type t in new TypeEnumerator(etp))
                {
                    list.Add(t);
                }

                return list.ToArray();
                //return new CorTypeEnumerator (etp);
            }
        }

        public Value GetStaticFieldValue(uint fieldToken, Frame frame)
        {
            Value dv = null;
            GetStaticFieldValue(fieldToken, frame, out dv);
            return dv;
        }

        public CorElementType CorType
        {
            get
            {
                DebugType(out CorElementType result);

                return result;
            }
        }
    }
}
