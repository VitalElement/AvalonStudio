// Copyright (c) 2017 Vital Element Avalon Studio - Dan Walmsley dan at walms dot co dot uk
// 
// This code is licensed for use only with AvalonStudio. It is not permitted for use in any 
// project unless explicitly authorized.
//

using CorApi.Portable;
using SharpGen.Runtime;
using SharpGen.Runtime.Win32;
using System;
using System.Runtime.InteropServices;

namespace CorDebug
{
    class ManagedCallback2Shadow : ComObjectShadow
    {
        protected class ManagedCallback2Vtbl : ComObjectVtbl
        {
            public ManagedCallback2Vtbl() : base(8 /* count methods there */)
            {
                AddMethod(new OnFunctionRemapOpportunityCallback(OnFunctionRemapOpportunity));
                AddMethod(new OnCreateConnectionCallback(OnCreateConnection));
                AddMethod(new OnChangeConnectionCallback(OnChangeConnection));
                AddMethod(new OnDestroyConnectionCallback(OnDestroyConnection));
                AddMethod(new OnExceptionCallback(OnException));
                AddMethod(new OnExceptionUnwindCallback(OnExceptionUnwind));
                AddMethod(new OnFunctionRemapCompleteCallback(OnFunctionRemapComplete));
                AddMethod(new OnMDANotificationCallback(OnMDANotification));
            }

            [UnmanagedFunctionPointer(CallingConvention.StdCall)]
            private delegate void OnFunctionRemapOpportunityCallback(IntPtr thisPtr, IntPtr appDomainRef, IntPtr threadRef, IntPtr oldFunctionRef, IntPtr newFunctionRef, int oldILOffset);

            [UnmanagedFunctionPointer(CallingConvention.StdCall)]
            private delegate void OnCreateConnectionCallback(IntPtr thisPtr, IntPtr processRef, int dwConnectionId, string connNameRef);

            [UnmanagedFunctionPointer(CallingConvention.StdCall)]
            private delegate void OnChangeConnectionCallback(IntPtr thisPtr, IntPtr processRef, int dwConnectionId);

            [UnmanagedFunctionPointer(CallingConvention.StdCall)]
            private delegate void OnDestroyConnectionCallback(IntPtr thisPtr, IntPtr processRef, int dwConnectionId);

            [UnmanagedFunctionPointer(CallingConvention.StdCall)]
            private delegate void OnExceptionCallback(IntPtr thisPtr, IntPtr appDomainRef, IntPtr threadRef, IntPtr frameRef, int nOffset, CorDebugExceptionCallbackType dwEventType, int dwFlags);

            [UnmanagedFunctionPointer(CallingConvention.StdCall)]
            private delegate void OnExceptionUnwindCallback(IntPtr thisPtr, IntPtr appDomainRef, IntPtr threadRef, CorDebugExceptionUnwindCallbackType dwEventType, int dwFlags);

            [UnmanagedFunctionPointer(CallingConvention.StdCall)]
            private delegate void OnFunctionRemapCompleteCallback(IntPtr thisPtr, IntPtr appDomainRef, IntPtr threadRef, IntPtr functionRef);

            [UnmanagedFunctionPointer(CallingConvention.StdCall)]
            private delegate void OnMDANotificationCallback(IntPtr thisPtr, IntPtr controllerRef, IntPtr threadRef, IntPtr mDARef);

            private static void OnFunctionRemapOpportunity(IntPtr thisPtr, IntPtr appDomainRef, IntPtr threadRef, IntPtr oldFunctionRef, IntPtr newFunctionRef, int oldILOffset)
            {
                var shadow = ToShadow<ManagedCallback2Shadow>(thisPtr);
                var callback = (ManagedCallbackImpl)shadow.Callback;
                callback.OnFunctionRemapOpportunity(new AppDomain(appDomainRef), new Thread(threadRef), new Function(oldFunctionRef), new Function(newFunctionRef), oldILOffset);
            }

            private static void OnCreateConnection(IntPtr thisPtr, IntPtr processRef, int dwConnectionId, string connNameRef)
            {
                var shadow = ToShadow<ManagedCallback2Shadow>(thisPtr);
                var callback = (ManagedCallbackImpl)shadow.Callback;
                callback.OnCreateConnection(new Process(processRef), dwConnectionId, connNameRef);
            }

            private static void OnChangeConnection(IntPtr thisPtr, IntPtr processRef, int dwConnectionId)
            {
                var shadow = ToShadow<ManagedCallback2Shadow>(thisPtr);
                var callback = (ManagedCallbackImpl)shadow.Callback;
                callback.OnChangeConnection(new Process(processRef), dwConnectionId);
            }

            private static void OnDestroyConnection(IntPtr thisPtr, IntPtr processRef, int dwConnectionId)
            {
                var shadow = ToShadow<ManagedCallback2Shadow>(thisPtr);
                var callback = (ManagedCallbackImpl)shadow.Callback;
                callback.OnDestroyConnection(new Process(processRef), dwConnectionId);
            }

            private static void OnException(IntPtr thisPtr, IntPtr appDomainRef, IntPtr threadRef, IntPtr frameRef, int nOffset, CorDebugExceptionCallbackType dwEventType, int dwFlags)
            {
                var shadow = ToShadow<ManagedCallback2Shadow>(thisPtr);
                var callback = (ManagedCallbackImpl)shadow.Callback;
                callback.OnException(new AppDomain(appDomainRef), new Thread(threadRef), new Frame(frameRef), nOffset, dwEventType, dwFlags);
            }

            private static void OnExceptionUnwind(IntPtr thisPtr, IntPtr appDomainRef, IntPtr threadRef, CorDebugExceptionUnwindCallbackType dwEventType, int dwFlags)
            {
                var shadow = ToShadow<ManagedCallback2Shadow>(thisPtr);
                var callback = (ManagedCallbackImpl)shadow.Callback;
                callback.OnExceptionUnwind(new AppDomain(appDomainRef), new Thread(threadRef), dwEventType, dwFlags);
            }

            private static void OnFunctionRemapComplete(IntPtr thisPtr, IntPtr appDomainRef, IntPtr threadRef, IntPtr functionRef)
            {
                var shadow = ToShadow<ManagedCallback2Shadow>(thisPtr);
                var callback = (ManagedCallbackImpl)shadow.Callback;
                callback.OnFunctionRemapComplete(new AppDomain(appDomainRef), new Thread(threadRef), new Function(functionRef));
            }

            private static void OnMDANotification(IntPtr thisPtr, IntPtr controllerRef, IntPtr threadRef, IntPtr mDARef)
            {
                var shadow = ToShadow<ManagedCallback2Shadow>(thisPtr);
                var callback = (ManagedCallbackImpl)shadow.Callback;
                callback.OnMDANotification(new Controller(controllerRef), new Thread(threadRef), new MDA(mDARef));
            }
        }

        protected override CppObjectVtbl Vtbl { get; } = new ManagedCallback2Vtbl();
    }

    public class ManagedCallbackShadow : SharpGen.Runtime.ComObjectShadow
    {
        protected class ManagedCallbackVtbl : ComObjectVtbl
        {
            public ManagedCallbackVtbl() : base(26)
            {
                AddMethod(new OnBreakpointCallback(OnBreakpoint));
                AddMethod(new OnStepCompleteCallback(OnStepComplete));
                AddMethod(new OnBreakCallback(OnBreak));
                AddMethod(new OnExceptionCallback(OnException));
                AddMethod(new OnEvalCompleteCallback(OnEvalComplete));
                AddMethod(new OnEvalExceptionCallback(OnEvalException));
                AddMethod(new OnCreateProcessWCallback(OnCreateProcessW));
                AddMethod(new OnExitProcessCallback(OnExitProcess));
                AddMethod(new OnCreateThreadCallback(OnCreateThread));
                AddMethod(new OnExitThreadCallback(OnExitThread));
                AddMethod(new OnLoadModuleCallback(OnLoadModule));
                AddMethod(new OnUnloadModuleCallback(OnUnloadModule));
                AddMethod(new OnLoadClassCallback(OnLoadClass));
                AddMethod(new OnUnloadClassCallback(OnUnloadClass));
                AddMethod(new OnDebuggerErrorCallback(OnDebuggerError));
                AddMethod(new OnLogMessageCallback(OnLogMessage));
                AddMethod(new OnLogSwitchCallback(OnLogSwitch)); // string here?
                AddMethod(new OnCreateAppDomainCallback(OnCreateAppDomain));
                AddMethod(new OnExitAppDomainCallback(OnExitAppDomain));
                AddMethod(new OnLoadAssemblyCallback(OnLoadAssembly));
                AddMethod(new OnUnloadAssemblyCallback(OnUnloadAssembly));
                AddMethod(new OnControlCTrapCallback(OnControlCTrap));
                AddMethod(new OnNameChangeCallback(OnNameChange));
                AddMethod(new OnUpdateModuleSymbols_Callback(OnUpdateModuleSymbols_));
                AddMethod(new OnEditAndContinueRemapCallback(OnEditAndContinueRemap)); // todo raw bool = int?
                AddMethod(new OnBreakpointSetErrorCallback(OnBreakpointSetError));
            }

            [UnmanagedFunctionPointer(CallingConvention.StdCall)]
            private delegate void OnBreakpointCallback(IntPtr thisPtr, IntPtr appDomain, IntPtr thread, IntPtr breakpoint);

            [UnmanagedFunctionPointer(CallingConvention.StdCall)]
            private delegate void OnStepCompleteCallback(IntPtr thisPtr, IntPtr pAppDomain, IntPtr pThread, IntPtr pStepper, int kReason);

            [UnmanagedFunctionPointer(CallingConvention.StdCall)]
            private delegate void OnBreakCallback(IntPtr thisPtr, IntPtr appDomainRef, IntPtr thread);

            [UnmanagedFunctionPointer(CallingConvention.StdCall)]
            private delegate void OnExceptionCallback(IntPtr thisPtr, IntPtr appDomainRef, IntPtr threadRef, int unhandled); // todo was raw bool int correct?

            [UnmanagedFunctionPointer(CallingConvention.StdCall)]
            private delegate void OnEvalCompleteCallback(IntPtr thisPtr, IntPtr appDomainRef, IntPtr threadRef, IntPtr evalRef);

            [UnmanagedFunctionPointer(CallingConvention.StdCall)]
            private delegate void OnEvalExceptionCallback(IntPtr thisPtr, IntPtr appDomainRef, IntPtr threadRef, IntPtr evalRef);

            [UnmanagedFunctionPointer(CallingConvention.StdCall)]
            private delegate void OnCreateProcessWCallback(IntPtr thisPtr, IntPtr processRef);

            [UnmanagedFunctionPointer(CallingConvention.StdCall)]
            private delegate void OnExitProcessCallback(IntPtr thisPtr, IntPtr processRef);

            [UnmanagedFunctionPointer(CallingConvention.StdCall)]
            private delegate void OnCreateThreadCallback(IntPtr thisPtr, IntPtr appDomainRef, IntPtr thread);

            [UnmanagedFunctionPointer(CallingConvention.StdCall)]
            private delegate void OnExitThreadCallback(IntPtr thisPtr, IntPtr appDomainRef, IntPtr thread);

            [UnmanagedFunctionPointer(CallingConvention.StdCall)]
            private delegate void OnLoadModuleCallback(IntPtr thisPtr, IntPtr appDomainRef, IntPtr moduleRef);

            [UnmanagedFunctionPointer(CallingConvention.StdCall)]
            private delegate void OnUnloadModuleCallback(IntPtr thisPtr, IntPtr appDomainRef, IntPtr moduleRef);

            [UnmanagedFunctionPointer(CallingConvention.StdCall)]
            private delegate void OnLoadClassCallback(IntPtr thisPtr, IntPtr appDomainRef, IntPtr c);

            [UnmanagedFunctionPointer(CallingConvention.StdCall)]
            private delegate void OnUnloadClassCallback(IntPtr thisPtr, IntPtr appDomainRef, IntPtr c);

            [UnmanagedFunctionPointer(CallingConvention.StdCall)]
            private delegate void OnDebuggerErrorCallback(IntPtr thisPtr, IntPtr processRef, SharpGen.Runtime.Result errorHR, int errorCode);

            [UnmanagedFunctionPointer(CallingConvention.StdCall)]
            private delegate void OnLogMessageCallback(IntPtr thisPtr, IntPtr appDomainRef, IntPtr threadRef, int lLevel, string logSwitchNameRef, string messageRef);

            [UnmanagedFunctionPointer(CallingConvention.StdCall)]
            private delegate void OnLogSwitchCallback(IntPtr thisPtr, IntPtr appDomainRef, IntPtr threadRef, int lLevel, int ulReason, string logSwitchNameRef, string parentNameRef); // string here?

            [UnmanagedFunctionPointer(CallingConvention.StdCall)]
            private delegate void OnCreateAppDomainCallback(IntPtr thisPtr, IntPtr processRef, IntPtr appDomainRef);

            [UnmanagedFunctionPointer(CallingConvention.StdCall)]
            private delegate void OnExitAppDomainCallback(IntPtr thisPtr, IntPtr processRef, IntPtr appDomainRef);

            [UnmanagedFunctionPointer(CallingConvention.StdCall)]
            private delegate void OnLoadAssemblyCallback(IntPtr thisPtr, IntPtr appDomainRef, IntPtr assemblyRef);

            [UnmanagedFunctionPointer(CallingConvention.StdCall)]
            private delegate void OnUnloadAssemblyCallback(IntPtr thisPtr, IntPtr appDomainRef, IntPtr assemblyRef);

            [UnmanagedFunctionPointer(CallingConvention.StdCall)]
            private delegate void OnControlCTrapCallback(IntPtr thisPtr, IntPtr processRef);

            [UnmanagedFunctionPointer(CallingConvention.StdCall)]
            private delegate void OnNameChangeCallback(IntPtr thisPtr, IntPtr appDomainRef, IntPtr threadRef);

            [UnmanagedFunctionPointer(CallingConvention.StdCall)]
            private delegate void OnUpdateModuleSymbols_Callback(IntPtr thisPtr, IntPtr appDomainRef, IntPtr moduleRef, System.IntPtr symbolStreamRef);

            [UnmanagedFunctionPointer(CallingConvention.StdCall)]
            private delegate void OnEditAndContinueRemapCallback(IntPtr thisPtr, IntPtr appDomainRef, IntPtr threadRef, IntPtr functionRef, int fAccurate); // todo raw bool = int?

            [UnmanagedFunctionPointer(CallingConvention.StdCall)]
            private delegate void OnBreakpointSetErrorCallback(IntPtr thisPtr, IntPtr appDomainRef, IntPtr threadRef, IntPtr breakpointRef, int dwError);

            private static void OnBreakpoint(IntPtr thisPtr, IntPtr pAppDomain, IntPtr pThread, IntPtr pBreakpoint)
            {
                var shadow = ToShadow<ManagedCallbackShadow>(thisPtr);
                var callback = (ManagedCallbackImpl)shadow.Callback;
                var appDomain = new AppDomain(pAppDomain);
                var thread = new Thread(pThread);
                var breakpoint = new Breakpoint(pBreakpoint);
                callback.OnBreakpoint(appDomain, thread, breakpoint);
            }

            private static void OnStepComplete(IntPtr thisPtr, IntPtr pAppDomain, IntPtr pThread, IntPtr pStepper, int kReason)
            {
                var shadow = ToShadow<ManagedCallbackShadow>(thisPtr);
                var callback = (ManagedCallbackImpl)shadow.Callback;
                var appDomain = new AppDomain(pAppDomain);
                var thread = new Thread(pThread);
                var stepper = new Stepper(pStepper);
                callback.OnStepComplete(appDomain, thread, stepper, (CorDebugStepReason)kReason);
            }

            private static void OnBreak(IntPtr thisPtr, IntPtr appDomainRef, IntPtr thread)
            {
                var shadow = ToShadow<ManagedCallbackShadow>(thisPtr);
                var callback = (ManagedCallbackImpl)shadow.Callback;
                var appDomain = new AppDomain(appDomainRef);
                var threadManaged = new Thread(thread);
                callback.OnBreak(appDomain, threadManaged);
            }

            private static void OnException(IntPtr thisPtr, IntPtr appDomainRef, IntPtr threadRef, int unhandled)
            {
                var shadow = ToShadow<ManagedCallbackShadow>(thisPtr);
                var callback = (ManagedCallbackImpl)shadow.Callback;
                var appDomain = new AppDomain(appDomainRef);
                var threadManaged = new Thread(threadRef);
                callback.OnException(appDomain, threadManaged, new RawBool(unhandled > 0));
            }

            private static void OnEvalComplete(IntPtr thisPtr, IntPtr appDomainRef, IntPtr threadRef, IntPtr evalRef)
            {
                var shadow = ToShadow<ManagedCallbackShadow>(thisPtr);
                var callback = (ManagedCallbackImpl)shadow.Callback;
                var appDomain = new AppDomain(appDomainRef);
                var threadManaged = new Thread(threadRef);
                callback.OnEvalComplete(appDomain, threadManaged, new Eval(evalRef));
            }

            private static void OnEvalException(IntPtr thisPtr, IntPtr appDomainRef, IntPtr threadRef, IntPtr evalRef)
            {
                var shadow = ToShadow<ManagedCallbackShadow>(thisPtr);
                var callback = (ManagedCallbackImpl)shadow.Callback;
                var appDomain = new AppDomain(appDomainRef);
                var threadManaged = new Thread(threadRef);
                callback.OnEvalException(appDomain, threadManaged, new Eval(evalRef));
            }

            private static void OnCreateProcessW(IntPtr thisPtr, IntPtr processRef)
            {
                var shadow = ToShadow<ManagedCallbackShadow>(thisPtr);
                var callback = (ManagedCallbackImpl)shadow.Callback;
                callback.OnCreateProcessW(Process.GetCorProcess(processRef));
            }

            private static void OnExitProcess(IntPtr thisPtr, IntPtr processRef)
            {
                var shadow = ToShadow<ManagedCallbackShadow>(thisPtr);
                var callback = (ManagedCallbackImpl)shadow.Callback;
                callback.OnExitProcess(Process.GetCorProcess(processRef));
            }

            private static void OnCreateThread(IntPtr thisPtr, IntPtr appDomainRef, IntPtr thread)
            {
                var shadow = ToShadow<ManagedCallbackShadow>(thisPtr);
                var callback = (ManagedCallbackImpl)shadow.Callback;
                callback.OnCreateThread(new AppDomain(appDomainRef), new Thread(thread));
            }

            private static void OnExitThread(IntPtr thisPtr, IntPtr appDomainRef, IntPtr thread)
            {
                var shadow = ToShadow<ManagedCallbackShadow>(thisPtr);
                var callback = (ManagedCallbackImpl)shadow.Callback;
                callback.OnExitThread(new AppDomain(appDomainRef), new Thread(thread));
            }

            private static void OnLoadModule(IntPtr thisPtr, IntPtr appDomainRef, IntPtr moduleRef)
            {
                var shadow = ToShadow<ManagedCallbackShadow>(thisPtr);
                var callback = (ManagedCallbackImpl)shadow.Callback;
                callback.OnLoadModule(new AppDomain(appDomainRef), new Module(moduleRef));
            }

            private static void OnUnloadModule(IntPtr thisPtr, IntPtr appDomainRef, IntPtr moduleRef)
            {
                var shadow = ToShadow<ManagedCallbackShadow>(thisPtr);
                var callback = (ManagedCallbackImpl)shadow.Callback;
                callback.OnUnloadModule(new AppDomain(appDomainRef), new Module(moduleRef));
            }

            private static void OnLoadClass(IntPtr thisPtr, IntPtr appDomainRef, IntPtr c)
            {
                var shadow = ToShadow<ManagedCallbackShadow>(thisPtr);
                var callback = (ManagedCallbackImpl)shadow.Callback;
                callback.OnLoadClass(new AppDomain(appDomainRef), new Class(c));
            }

            private static void OnUnloadClass(IntPtr thisPtr, IntPtr appDomainRef, IntPtr c)
            {
                var shadow = ToShadow<ManagedCallbackShadow>(thisPtr);
                var callback = (ManagedCallbackImpl)shadow.Callback;
                callback.OnUnloadClass(new AppDomain(appDomainRef), new Class(c));
            }

            private static void OnDebuggerError(IntPtr thisPtr, IntPtr processRef, SharpGen.Runtime.Result errorHR, int errorCode)
            {
                var shadow = ToShadow<ManagedCallbackShadow>(thisPtr);
                var callback = (ManagedCallbackImpl)shadow.Callback;
                callback.OnDebuggerError(Process.GetCorProcess(processRef), errorHR, errorCode);
            }

            private static void OnLogMessage(IntPtr thisPtr, IntPtr appDomainRef, IntPtr threadRef, int lLevel, string logSwitchNameRef, string messageRef)
            {
                var shadow = ToShadow<ManagedCallbackShadow>(thisPtr);
                var callback = (ManagedCallbackImpl)shadow.Callback;
                callback.OnLogMessage(new AppDomain(appDomainRef), new Thread(threadRef), lLevel, logSwitchNameRef, messageRef);
            }

            private static void OnLogSwitch(IntPtr thisPtr, IntPtr appDomainRef, IntPtr threadRef, int lLevel, int ulReason, string logSwitchNameRef, string parentNameRef)
            {
                var shadow = ToShadow<ManagedCallbackShadow>(thisPtr);
                var callback = (ManagedCallbackImpl)shadow.Callback;
                callback.OnLogSwitch(new AppDomain(appDomainRef), new Thread(threadRef), lLevel, ulReason, logSwitchNameRef, parentNameRef);
            } // string here?

            private static void OnCreateAppDomain(IntPtr thisPtr, IntPtr processRef, IntPtr appDomainRef)
            {
                var shadow = ToShadow<ManagedCallbackShadow>(thisPtr);
                var callback = (ManagedCallbackImpl)shadow.Callback;
                callback.OnCreateAppDomain(Process.GetCorProcess(processRef), new AppDomain(appDomainRef));
            }

            private static void OnExitAppDomain(IntPtr thisPtr, IntPtr processRef, IntPtr appDomainRef)
            {
                var shadow = ToShadow<ManagedCallbackShadow>(thisPtr);
                var callback = (ManagedCallbackImpl)shadow.Callback;
                callback.OnExitAppDomain(Process.GetCorProcess(processRef), new AppDomain(appDomainRef));
            }

            private static void OnLoadAssembly(IntPtr thisPtr, IntPtr appDomainRef, IntPtr assemblyRef)
            {
                var shadow = ToShadow<ManagedCallbackShadow>(thisPtr);
                var callback = (ManagedCallbackImpl)shadow.Callback;
                callback.OnLoadAssembly(new AppDomain(appDomainRef), new Assembly(assemblyRef));
            }

            private static void OnUnloadAssembly(IntPtr thisPtr, IntPtr appDomainRef, IntPtr assemblyRef)
            {
                var shadow = ToShadow<ManagedCallbackShadow>(thisPtr);
                var callback = (ManagedCallbackImpl)shadow.Callback;
                callback.OnUnloadAssembly(new AppDomain(appDomainRef), new Assembly(assemblyRef));
            }

            private static void OnControlCTrap(IntPtr thisPtr, IntPtr processRef)
            {
                var shadow = ToShadow<ManagedCallbackShadow>(thisPtr);
                var callback = (ManagedCallbackImpl)shadow.Callback;
                callback.OnControlCTrap(Process.GetCorProcess(processRef));
            }

            private static void OnNameChange(IntPtr thisPtr, IntPtr appDomainRef, IntPtr threadRef)
            {
                var shadow = ToShadow<ManagedCallbackShadow>(thisPtr);
                var callback = (ManagedCallbackImpl)shadow.Callback;
                callback.OnNameChange(new AppDomain(appDomainRef), new Thread(threadRef));
            }

            private static void OnUpdateModuleSymbols_(IntPtr thisPtr, IntPtr appDomainRef, IntPtr moduleRef, System.IntPtr symbolStreamRef)
            {
                var shadow = ToShadow<ManagedCallbackShadow>(thisPtr);
                var callback = (ManagedCallbackImpl)shadow.Callback;
                callback.OnUpdateModuleSymbols_(new AppDomain(appDomainRef), new Module(moduleRef), symbolStreamRef);
            }

            private static void OnEditAndContinueRemap(IntPtr thisPtr, IntPtr appDomainRef, IntPtr threadRef, IntPtr functionRef, int fAccurate)
            {
                var shadow = ToShadow<ManagedCallbackShadow>(thisPtr);
                var callback = (ManagedCallbackImpl)shadow.Callback;
                callback.OnEditAndContinueRemap(new AppDomain(appDomainRef), new Thread(threadRef), new Function(functionRef), new RawBool(fAccurate > 0));
            } // todo raw bool = int?

            private static void OnBreakpointSetError(IntPtr thisPtr, IntPtr appDomainRef, IntPtr threadRef, IntPtr breakpointRef, int dwError)
            {
                var shadow = ToShadow<ManagedCallbackShadow>(thisPtr);
                var callback = (ManagedCallbackImpl)shadow.Callback;
                callback.OnBreakpointSetError(new AppDomain(appDomainRef), new Thread(threadRef), new Breakpoint(breakpointRef), dwError);
            }
        }


        protected override CppObjectVtbl Vtbl { get; } = new ManagedCallbackVtbl();
    }
}