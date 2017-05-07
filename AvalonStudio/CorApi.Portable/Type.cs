using System;
using System.Collections.Generic;
using System.Text;

namespace CorApi.Portable
{
    [Flags]
    public enum CorElementType : int
    {
        // Fields
        ELEMENT_TYPE_ARRAY = 20,
        ELEMENT_TYPE_BOOLEAN = 2,
        ELEMENT_TYPE_BYREF = 0x10,
        ELEMENT_TYPE_CHAR = 3,
        ELEMENT_TYPE_CLASS = 0x12,
        ELEMENT_TYPE_CMOD_OPT = 0x20,
        ELEMENT_TYPE_CMOD_REQD = 0x1f,
        ELEMENT_TYPE_END = 0,
        ELEMENT_TYPE_FNPTR = 0x1b,
        ELEMENT_TYPE_GENERICINST = 0x15,
        ELEMENT_TYPE_I = 0x18,
        ELEMENT_TYPE_I1 = 4,
        ELEMENT_TYPE_I2 = 6,
        ELEMENT_TYPE_I4 = 8,
        ELEMENT_TYPE_I8 = 10,
        ELEMENT_TYPE_INTERNAL = 0x21,
        ELEMENT_TYPE_MAX = 0x22,
        ELEMENT_TYPE_MODIFIER = 0x40,
        ELEMENT_TYPE_MVAR = 0x1e,
        ELEMENT_TYPE_OBJECT = 0x1c,
        ELEMENT_TYPE_PINNED = 0x05 | ELEMENT_TYPE_MODIFIER,
        ELEMENT_TYPE_PTR = 15,
        ELEMENT_TYPE_R4 = 12,
        ELEMENT_TYPE_R8 = 13,
        ELEMENT_TYPE_SENTINEL = 0x01 | ELEMENT_TYPE_MODIFIER,
        ELEMENT_TYPE_STRING = 14,
        ELEMENT_TYPE_SZARRAY = 0x1d,
        ELEMENT_TYPE_TYPEDBYREF = 0x16,
        ELEMENT_TYPE_U = 0x19,
        ELEMENT_TYPE_U1 = 5,
        ELEMENT_TYPE_U2 = 7,
        ELEMENT_TYPE_U4 = 9,
        ELEMENT_TYPE_U8 = 11,
        ELEMENT_TYPE_VALUETYPE = 0x11,
        ELEMENT_TYPE_VAR = 0x13,
        ELEMENT_TYPE_VOID = 1,


        // from corpriv.h (CoreCLR sources)

        // ZAPSIG types
        // ZapSig encoding for ELEMENT_TYPE_VAR and ELEMENT_TYPE_MVAR. It is always followed
        // by the RID of a GenericParam token, encoded as a compressed integer.
        ELEMENT_TYPE_VAR_ZAPSIG = 0x3b,

        // ZapSig encoding for an array MethodTable to allow it to remain such after decoding
        // (rather than being transformed into the TypeHandle representing that array)
        //
        // The element is always followed by ELEMENT_TYPE_SZARRAY or ELEMENT_TYPE_ARRAY
        ELEMENT_TYPE_NATIVE_ARRAY_TEMPLATE_ZAPSIG = 0x3c,

        // ZapSig encoding for native value types in IL stubs. IL stub signatures may contain
        // ELEMENT_TYPE_INTERNAL followed by ParamTypeDesc with ELEMENT_TYPE_VALUETYPE element
        // type. It acts like a modifier to the underlying structure making it look like its
        // unmanaged view (size determined by unmanaged layout, blittable, no GC pointers).
        //
        // ELEMENT_TYPE_NATIVE_VALUETYPE_ZAPSIG is used when encoding such types to NGEN images.
        // The signature looks like this: ET_NATIVE_VALUETYPE_ZAPSIG ET_VALUETYPE <token>.
        // See code:ZapSig.GetSignatureForTypeHandle and code:SigPointer.GetTypeHandleThrowing
        // where the encoding/decoding takes place.
        ELEMENT_TYPE_NATIVE_VALUETYPE_ZAPSIG = 0x3d,

        ELEMENT_TYPE_CANON_ZAPSIG = 0x3e, // zapsig encoding for [mscorlib]System.__Canon
        ELEMENT_TYPE_MODULE_ZAPSIG = 0x3f, // zapsig encoding for external module id#

    }

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
                DebugType(out int result);

                return (CorElementType)result;
            }
        }
    }
}
