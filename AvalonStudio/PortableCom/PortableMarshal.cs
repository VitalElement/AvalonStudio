using SharpDX;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace PortableCom
{
    public class PortableMarshal
    {
        public static int QueryInterface(IntPtr nativePtr, ref Guid iid, out IntPtr ppv)
        {
            int result;

            unsafe
            {
                IntPtr ppv_ = IntPtr.Zero;

                fixed (void* guid = &iid)
                {
                    SharpDX.Result __result__;
                    __result__ =
                    CorDebug.LocalInterop.Calliint((void*)nativePtr, guid, &ppv_, ((void**)(*(void**)nativePtr))[0]);
                    result = __result__.Code;
                }

                ppv = ppv_;
            }

            return result;
        }

        public static int AddRef (IntPtr nativePtr)
        {
            int result;

            unsafe
            {
                    SharpDX.Result __result__;
                    __result__ =
                    CorDebug.LocalInterop.Calliint((void*)nativePtr, ((void**)(*(void**)nativePtr))[1]);
                    result = __result__.Code;
            }

            return result;
        }

        public static int Release(IntPtr nativePtr)
        {
            int result;

            unsafe
            {
                SharpDX.Result __result__;
                __result__ =
                CorDebug.LocalInterop.Calliint((void*)nativePtr, ((void**)(*(void**)nativePtr))[2]);
                result = __result__.Code;
            }

            return result;
        }


    }
}
