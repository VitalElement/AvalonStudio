using System;
using System.Collections.Generic;
using System.Text;
using SharpDX;
using CoreDebugger;
using System.Runtime.InteropServices;

namespace CorApi.Portable
{
    public class ManagedCallbackShadow : SharpDX.ComObjectShadow
    {
        protected override CppObjectVtbl GetVtbl
        {
            get { return Vtbl; }
        }

        private static readonly CaptureEngineOnEventCallbackVtbl Vtbl = new CaptureEngineOnEventCallbackVtbl();

        /// <summary>
        /// Return a pointer to the unmanaged version of this callback.
        /// </summary>
        /// <param name="callback">The callback.</param>
        /// <returns>A pointer to a shadow c++ callback</returns>
        public static IntPtr ToIntPtr(ManagedCallback callback)
        {
            return ToCallbackPtr<ManagedCallback>(callback);
        }

        public class CaptureEngineOnEventCallbackVtbl : ComObjectVtbl
        {
            public CaptureEngineOnEventCallbackVtbl() : base(1)
            {
                AddMethod(new OnBreakPointCallback(OnBreakpointImpl));
            }
            
            //void OnBreakpoint(CoreDebugger.AppDomain appDomainRef, CoreDebugger.Thread threadRef, CoreDebugger.Breakpoint breakpointRef);
            [UnmanagedFunctionPointer(CallingConvention.StdCall)]
            private delegate void OnBreakPointCallback(IntPtr thisPtr, IntPtr appDomainRef, IntPtr threadRef, IntPtr breakpointRef);
            private static void OnBreakpointImpl(IntPtr thisPtr, IntPtr appDomainRef, IntPtr threadRef, IntPtr breakpointRef)
            {
                var shadow = ToShadow<ManagedCallbackShadow>(thisPtr);
                var callback = (ManagedCallback)shadow.Callback;
                var appDomain = new AppDomain(appDomainRef);
                var thread = new Thread(threadRef);
                var breakPoint = new Breakpoint(breakpointRef);
                callback.OnBreakpoint(appDomain, thread, breakPoint);
            }
        }
    }
}
