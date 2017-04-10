namespace AvalonStudio.Debugging
{
    using Mono.Debugging.Client;

    public interface IDebugManager2
    {
        BreakpointStore Breakpoints { get; set; }

        void Start();

        void Restart();

        void Stop();

        void Continue();

        void Pause();

        void StepOver();

        void StepInto();

        void StepInstruction();

        void StepOut();

        bool SessionActive { get; }
    }
}
