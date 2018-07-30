// Copyright (c) 2017 Vital Element Avalon Studio - Dan Walmsley dan at walms dot co dot uk
// 
// This code is licensed for use only with AvalonStudio. It is not permitted for use in any 
// project unless explicitly authorized.
//

using System;
using SharpGen.Runtime;
using System.Diagnostics;
using SharpGen.Runtime.Win32;

namespace CorApi.Portable
{
    public class ManagedCallbackImpl : CallbackBase, ManagedCallback, ManagedCallback2
    {
        private Action<ManagedCallbackType, CorEventArgs> _handleEvent;

        public ManagedCallbackImpl(Action<ManagedCallbackType, CorEventArgs> handleEvent)
        {
            _handleEvent = handleEvent;
        }

        public virtual void OnBreak(AppDomain appDomainRef, Thread thread)
        {
            _handleEvent(ManagedCallbackType.OnBreak, new ThreadEventArgs(appDomainRef, thread, ManagedCallbackType.OnBreak));
        }

        public virtual void OnBreakpoint(AppDomain appDomainRef, Thread threadRef, Breakpoint breakpointRef)
        {
            _handleEvent(ManagedCallbackType.OnBreakpoint, new BreakpointEventArgs(appDomainRef, threadRef, breakpointRef, ManagedCallbackType.OnBreakpoint));
        }

        public virtual void OnBreakpointSetError(AppDomain appDomainRef, Thread threadRef, Breakpoint breakpointRef, int dwError)
        {
            _handleEvent(ManagedCallbackType.OnBreakpointSetError, new BreakpointSetErrorEventArgs(appDomainRef, threadRef, breakpointRef, dwError, ManagedCallbackType.OnBreakpointSetError));
        }

        public virtual void OnCreateAppDomain(Process processRef, AppDomain appDomainRef)
        {
            _handleEvent(ManagedCallbackType.OnCreateAppDomain, new AppDomainEventArgs(processRef, appDomainRef, ManagedCallbackType.OnCreateAppDomain));
        }

        public virtual void OnCreateProcessW(Process processRef)
        {
            _handleEvent(ManagedCallbackType.OnCreateProcess, new ProcessEventArgs(processRef, ManagedCallbackType.OnCreateProcess));
        }

        public virtual void OnCreateThread(AppDomain appDomainRef, Thread thread)
        {
            _handleEvent(ManagedCallbackType.OnCreateThread, new ThreadEventArgs(appDomainRef, thread, ManagedCallbackType.OnCreateThread));
        }

        public virtual void OnDebuggerError(Process processRef, Result errorHR, int errorCode)
        {
            _handleEvent(ManagedCallbackType.OnDebuggerError, new DebuggerErrorEventArgs(processRef, errorHR.Code, errorCode, ManagedCallbackType.OnDebuggerError));
        }

        public virtual void OnEditAndContinueRemap(AppDomain appDomainRef, Thread threadRef, Function functionRef, RawBool fAccurate)
        {
            Debug.Assert(false); //OBSOLETE callback
        }

        public virtual void OnEvalComplete(AppDomain appDomainRef, Thread threadRef, Eval evalRef)
        {
            _handleEvent(ManagedCallbackType.OnEvalComplete, new EvalEventArgs(appDomainRef, threadRef, evalRef, ManagedCallbackType.OnEvalComplete));
        }

        public virtual void OnEvalException(AppDomain appDomainRef, Thread threadRef, Eval evalRef)
        {
            _handleEvent(ManagedCallbackType.OnEvalException, new EvalEventArgs(appDomainRef, threadRef, evalRef, ManagedCallbackType.OnEvalException));
        }

        public virtual void OnException(AppDomain appDomainRef, Thread threadRef, RawBool unhandled)
        {
            _handleEvent(ManagedCallbackType.OnException, new ExceptionEventArgs(appDomainRef, threadRef, unhandled, ManagedCallbackType.OnException));
        }

        public virtual void OnExitProcess(Process processRef)
        {
            _handleEvent(ManagedCallbackType.OnProcessExit, new ProcessEventArgs(processRef, ManagedCallbackType.OnProcessExit));
        }

        public virtual void OnExitThread(AppDomain appDomainRef, Thread thread)
        {
            _handleEvent(ManagedCallbackType.OnThreadExit, new ThreadEventArgs(appDomainRef, thread, ManagedCallbackType.OnThreadExit));
        }

        public virtual void OnLoadClass(AppDomain appDomainRef, Class c)
        {
            _handleEvent(ManagedCallbackType.OnClassLoad, new ClassEventArgs(appDomainRef, c, ManagedCallbackType.OnClassLoad));
        }

        public virtual void OnLoadModule(AppDomain appDomainRef, Module moduleRef)
        {
            _handleEvent(ManagedCallbackType.OnModuleLoad, new ModuleEventArgs(appDomainRef, moduleRef, ManagedCallbackType.OnModuleLoad));
        }

        public virtual void OnLogMessage(AppDomain appDomainRef, Thread threadRef, int lLevel, string logSwitchNameRef, string messageRef)
        {
            _handleEvent(ManagedCallbackType.OnLogMessage, new LogMessageEventArgs(appDomainRef, threadRef, lLevel, logSwitchNameRef, messageRef, ManagedCallbackType.OnLogMessage));
        }

        public virtual void OnLogSwitch(AppDomain appDomainRef, Thread threadRef, int lLevel, int ulReason, string logSwitchNameRef, string parentNameRef)
        {
            _handleEvent(ManagedCallbackType.OnLogSwitch, new LogSwitchEventArgs(appDomainRef, threadRef, lLevel, ulReason, logSwitchNameRef, parentNameRef, ManagedCallbackType.OnLogSwitch));
        }

        public virtual void OnNameChange(AppDomain appDomainRef, Thread threadRef)
        {
            _handleEvent(ManagedCallbackType.OnNameChange, new ThreadEventArgs(appDomainRef, threadRef, ManagedCallbackType.OnNameChange));
        }

        public virtual void OnStepComplete(AppDomain appDomainRef, Thread threadRef, Stepper stepperRef, CorDebugStepReason reason)
        {
            _handleEvent(ManagedCallbackType.OnStepComplete, new StepCompleteEventArgs(appDomainRef, threadRef, stepperRef, reason, ManagedCallbackType.OnStepComplete));
        }

        public virtual void OnUnloadAssembly(AppDomain appDomainRef, Assembly assemblyRef)
        {
            _handleEvent(ManagedCallbackType.OnAssemblyUnload, new AssemblyEventArgs(appDomainRef, assemblyRef, ManagedCallbackType.OnAssemblyUnload));
        }

        public virtual void OnUnloadClass(AppDomain appDomainRef, Class c)
        {
            _handleEvent(ManagedCallbackType.OnClassUnload, new ClassEventArgs(appDomainRef, c, ManagedCallbackType.OnClassUnload));
        }

        public virtual void OnUnloadModule(AppDomain appDomainRef, Module moduleRef)
        {
            _handleEvent(ManagedCallbackType.OnModuleUnload, new ModuleEventArgs(appDomainRef, moduleRef, ManagedCallbackType.OnModuleUnload));
        }

        public virtual void OnUpdateModuleSymbols_(AppDomain appDomainRef, Module moduleRef, IntPtr symbolStreamRef)
        {
            _handleEvent(ManagedCallbackType.OnUpdateModuleSymbols, new UpdateModuleSymbolsEventArgs(appDomainRef, moduleRef, new ComStream(symbolStreamRef), ManagedCallbackType.OnUpdateModuleSymbols));
        }

        // Get process from controller 
        static private Process GetProcessFromController(Controller pController)
        {
            Process p;

            var p2 = pController.QueryInterface<Process>();
            if (p2 != null)
            {
                p = Process.GetCorProcess(p2.NativePointer);
            }
            else
            {
                var a2 = pController.QueryInterfaceOrNull<AppDomain>();
                p = a2.Process;
            }
            return p;
        }

        public void OnMDANotification(Controller controllerRef, Thread threadRef, MDA mDARef)
        {
            string szName = mDARef.Name;
            CorDebugMDAFlags f = mDARef.Flags;
            Process p = GetProcessFromController(controllerRef);

            _handleEvent(ManagedCallbackType.OnMDANotification, new MDAEventArgs(mDARef, threadRef, p));
        }

        public void OnControlCTrap(Process processRef)
        {
            _handleEvent(ManagedCallbackType.OnControlCTrap, new ProcessEventArgs(processRef, ManagedCallbackType.OnControlCTrap));
        }

        public void OnExitAppDomain(Process processRef, AppDomain appDomainRef)
        {
            _handleEvent(ManagedCallbackType.OnAppDomainExit, new AppDomainEventArgs(processRef, appDomainRef));
        }

        public void OnLoadAssembly(AppDomain appDomainRef, Assembly assemblyRef)
        {
            _handleEvent(ManagedCallbackType.OnAssemblyLoad, new AssemblyEventArgs(appDomainRef, assemblyRef, ManagedCallbackType.OnAssemblyLoad));
        }

        public void OnFunctionRemapOpportunity(AppDomain appDomainRef, Thread threadRef, Function oldFunctionRef, Function newFunctionRef, int oldILOffset)
        {
            _handleEvent(ManagedCallbackType.OnFunctionRemapOpportunity, new FunctionRemapOpportunityEventArgs(appDomainRef, threadRef, oldFunctionRef, newFunctionRef, oldILOffset));
        }

        public void OnCreateConnection(Process processRef, int dwConnectionId, string connNameRef)
        {
            // Not Implemented
            Debug.Assert(false);
        }

        public void OnChangeConnection(Process processRef, int dwConnectionId)
        {
            //  Not Implemented
            Debug.Assert(false);
        }

        public void OnDestroyConnection(Process processRef, int dwConnectionId)
        {
            // Not Implemented
            Debug.Assert(false);
        }

        public void OnException(AppDomain appDomainRef, Thread threadRef, Frame frameRef, int nOffset, CorDebugExceptionCallbackType dwEventType, int dwFlags)
        {
            _handleEvent(ManagedCallbackType.OnException2, new Exception2EventArgs(appDomainRef, threadRef, frameRef, nOffset, dwEventType, dwFlags, ManagedCallbackType.OnException2));
        }

        public void OnExceptionUnwind(AppDomain appDomainRef, Thread threadRef, CorDebugExceptionUnwindCallbackType dwEventType, int dwFlags)
        {
            _handleEvent(ManagedCallbackType.OnExceptionUnwind2, new ExceptionUnwind2EventArgs(appDomainRef, threadRef, dwEventType, dwFlags, ManagedCallbackType.OnExceptionUnwind2));
        }

        public void OnFunctionRemapComplete(AppDomain appDomainRef, Thread threadRef, Function functionRef)
        {
            _handleEvent(ManagedCallbackType.OnFunctionRemapComplete, new FunctionRemapCompleteEventArgs(appDomainRef, threadRef, functionRef));
        }
    }
}
