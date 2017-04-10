namespace AvalonStudio.Debugging
{
    using Mono.Debugging.Client;

    public interface IDebugManager2
    {
        BreakpointStore Breakpoints { get; set; }

        void Start();

        void Continue();

        void SteoOver();

        void StepInto();

        void StepInstruction();

        void StepOut();

        bool SessionActive { get; }
    }
}
