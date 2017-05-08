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

        public Value GetStaticFieldValue(int fieldToken, Frame frame)
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
