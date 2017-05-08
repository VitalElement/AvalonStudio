using System;
using System.Collections.Generic;
using System.Text;

namespace CorApi.Portable
{
    public enum CorElementTypeExtra : int
    {
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
}
