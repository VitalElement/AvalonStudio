using AvalonStudio.Projects;
using AvalonStudio.Toolchains;
using AvalonStudio.Utils;
using IridiumJS;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace AvalonStudio.Debugging.TypeScript
{
    public class TypeScriptDebugger : IDebugger
    {
        protected DebuggerState currentState;
        protected JSEngine scriptExecutionEngine;

        public bool DebugMode { get; set; }

        public DebuggerState State => currentState;

        public event EventHandler<EventArgs> StateChanged;

        public event EventHandler<StopRecord> Stopped;

        public Task<LiveBreakPoint> BreakMainAsync()
        {
            throw new NotImplementedException();
        }

        public Task CloseAsync()
        {
            throw new NotImplementedException();
        }

        public Task ContinueAsync()
        {
            throw new NotImplementedException();
        }

        public Task<VariableObject> CreateWatchAsync(string id, string expression)
        {
            throw new NotImplementedException();
        }

        public Task DeleteWatchAsync(string id)
        {
            throw new NotImplementedException();
        }

        public Task<List<InstructionLine>> DisassembleAsync(ulong start, uint count)
        {
            throw new NotSupportedException();
        }

        public Task<List<InstructionLine>> DisassembleAsync(string file, int line, int numLines)
        {
            throw new NotSupportedException();
        }

        public Task<string> EvaluateExpressionAsync(string expression)
        {
            throw new NotImplementedException();
        }

        public Task<Dictionary<int, string>> GetChangedRegistersAsync()
        {
            throw new NotSupportedException();
        }

        public Task<Dictionary<int, Register>> GetRegistersAsync()
        {
            throw new NotSupportedException();
        }

        public object GetSettingsControl(IProject project)
        {
            throw new NotImplementedException();
        }

        public int GetVariableId()
        {
            throw new NotImplementedException();
        }

        public void Initialise()
        {
            // Nothing here
        }

        public Task<List<VariableObject>> ListChildrenAsync(VariableObject variable)
        {
            throw new NotImplementedException();
        }

        public Task<List<Frame>> ListStackFramesAsync()
        {
            throw new NotImplementedException();
        }

        public Task<List<Variable>> ListStackVariablesAsync()
        {
            throw new NotImplementedException();
        }

        public Task<bool> PauseAsync()
        {
            throw new NotImplementedException();
        }

        public void ProvisionSettings(IProject project)
        {
            throw new NotImplementedException();
        }

        public Task<List<MemoryBytes>> ReadMemoryBytesAsync(ulong address, ulong offset, uint count)
        {
            throw new NotSupportedException();
        }

        public Task RemoveAsync(LiveBreakPoint breakPoint)
        {
            throw new NotImplementedException();
        }

        public Task ResetAsync(bool runAfter)
        {
            throw new NotImplementedException();
        }

        public async Task RunAsync()
        {
            // STUB!
            // throw new NotImplementedException();
        }

        public Task<LiveBreakPoint> SetBreakPointAsync(string file, uint line)
        {
            throw new NotImplementedException();
        }

        public Task SetWatchFormatAsync(string id, WatchFormat format)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> StartAsync(IToolChain toolChain, IConsole console, IProject project)
        {
            // Start compiled TypeScript program
            // IridiumJS is a dependency of the toolchain, so we will use it

            // For now we will assume `main.js` as the output file
            // Later it will have to be set in project settings, and read by toolchain
            scriptExecutionEngine = new JSEngine();
            var jsScript = System.IO.File.ReadAllText(Path.Combine(project.LocationDirectory, "main.js"));
            // Inject console
            scriptExecutionEngine.VariableContext.console = new JSConsoleAdapter(console);
            scriptExecutionEngine.Execute(jsScript);

            //return true;
            return false; // hack
        }

        public Task StepInstructionAsync()
        {
            throw new NotImplementedException();
        }

        public Task StepIntoAsync()
        {
            throw new NotImplementedException();
        }

        public Task StepOutAsync()
        {
            throw new NotImplementedException();
        }

        public Task StepOverAsync()
        {
            throw new NotImplementedException();
        }

        public Task StopAsync()
        {
            throw new NotImplementedException();
        }

        public Task<List<VariableObjectChange>> UpdateVariablesAsync()
        {
            throw new NotImplementedException();
        }
    }
}