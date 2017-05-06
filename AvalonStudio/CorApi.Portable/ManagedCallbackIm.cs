using System;
using CoreDebugger;
using SharpDX;
using SharpDX.Mathematics.Interop;

namespace CorApi.Portable
{
    public class ManagedCallbackImpl : CoreDebugger.ManagedCallback, ManagedCallback2
    {
        public IntPtr GetNewIntPtr() => CppObject.ToCallbackPtr<ManagedCallback>(this);

        public virtual IDisposable Shadow { get; set; }

        public virtual void Dispose()
        {

        }

        public virtual void OnBreak(AppDomain appDomainRef, Thread thread)
        {

        }

        public virtual void OnBreakpoint(AppDomain appDomainRef, Thread threadRef, Breakpoint breakpointRef)
        {

        }

        public virtual void OnBreakpointSetError(AppDomain appDomainRef, Thread threadRef, Breakpoint breakpointRef, int dwError)
        {

        }

        public virtual void OnCreateAppDomain(Process processRef, AppDomain appDomainRef)
        {

        }

        public virtual void OnCreateProcessW(Process processRef)
        {

        }

        public virtual void OnCreateThread(AppDomain appDomainRef, Thread thread)
        {

        }

        public virtual void OnDebuggerError(Process processRef, Result errorHR, int errorCode)
        {

        }

        public virtual void OnEditAndContinueRemap(AppDomain appDomainRef, Thread threadRef, Function functionRef, RawBool fAccurate)
        {

        }

        public virtual void OnEvalComplete(AppDomain appDomainRef, Thread threadRef, Eval evalRef)
        {

        }

        public virtual void OnEvalException(AppDomain appDomainRef, Thread threadRef, Eval evalRef)
        {

        }

        public virtual void OnException(AppDomain appDomainRef, Thread threadRef, RawBool unhandled)
        {

        }

        public virtual void OnExitProcess(Process processRef)
        {

        }

        public virtual void OnExitThread(AppDomain appDomainRef, Thread thread)
        {

        }

        public virtual void OnLoadClass(AppDomain appDomainRef, Class c)
        {

        }

        public virtual void OnLoadModule(AppDomain appDomainRef, Module moduleRef)
        {

        }

        public virtual void OnLogMessage(AppDomain appDomainRef, Thread threadRef, int lLevel, string logSwitchNameRef, string messageRef)
        {

        }

        public virtual void OnLogSwitch(AppDomain appDomainRef, Thread threadRef, int lLevel, int ulReason, string logSwitchNameRef, string parentNameRef)
        {

        }

        public virtual void OnNameChange(AppDomain appDomainRef, Thread threadRef)
        {

        }

        public virtual void OnStepComplete(AppDomain appDomainRef, Thread threadRef, Stepper stepperRef, CorDebugStepReason reason)
        {

        }

        public virtual void OnUnloadAssembly(AppDomain appDomainRef, Assembly assemblyRef)
        {

        }

        public virtual void OnUnloadClass(AppDomain appDomainRef, Class c)
        {

        }

        public virtual void OnUnloadModule(AppDomain appDomainRef, Module moduleRef)
        {

        }

        public virtual void OnUpdateModuleSymbols_(AppDomain appDomainRef, Module moduleRef, IntPtr symbolStreamRef)
        {

        }

        public void OnMDANotification(Controller controllerRef, Thread threadRef, MDA mDARef)
        {
        }

        public void OnControlCTrap(Process processRef)
        {
        }

        public void OnExitAppDomain(Process processRef, AppDomain appDomainRef)
        {

        }

        public void OnLoadAssembly(AppDomain appDomainRef, Assembly assemblyRef)
        {

        }

        public void OnFunctionRemapOpportunity(AppDomain appDomainRef, Thread threadRef, Function oldFunctionRef, Function newFunctionRef, int oldILOffset)
        {

        }

        public void OnCreateConnection(Process processRef, int dwConnectionId, string connNameRef)
        {

        }

        public void OnChangeConnection(Process processRef, int dwConnectionId)
        {

        }

        public void OnDestroyConnection(Process processRef, int dwConnectionId)
        {

        }

        public void OnException(AppDomain appDomainRef, Thread threadRef, Frame frameRef, int nOffset, CorDebugExceptionCallbackType dwEventType, int dwFlags)
        {

        }

        public void OnExceptionUnwind(AppDomain appDomainRef, Thread threadRef, CorDebugExceptionUnwindCallbackType dwEventType, int dwFlags)
        {

        }

        public void OnFunctionRemapComplete(AppDomain appDomainRef, Thread threadRef, Function functionRef)
        {

        }
    }
}
