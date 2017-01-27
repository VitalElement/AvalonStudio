using AvalonStudio.Projects;
using AvalonStudio.Toolchains;
using AvalonStudio.Utils;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AvalonStudio.Debugging.TypeScript
{
    public class TypeScriptDebugger : IDebugger
    {
        public bool DebugMode
        {
            get
            {
                throw new NotImplementedException();
            }

            set
            {
                throw new NotImplementedException();
            }
        }

        public DebuggerState State
        {
            get
            {
                throw new NotImplementedException();
            }
        }

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
            throw new NotImplementedException();
        }

        public Task<List<InstructionLine>> DisassembleAsync(string file, int line, int numLines)
        {
            throw new NotImplementedException();
        }

        public Task<string> EvaluateExpressionAsync(string expression)
        {
            throw new NotImplementedException();
        }

        public Task<Dictionary<int, string>> GetChangedRegistersAsync()
        {
            throw new NotImplementedException();
        }

        public Task<Dictionary<int, Register>> GetRegistersAsync()
        {
            throw new NotImplementedException();
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
            throw new NotImplementedException();
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
            throw new NotImplementedException();
        }

        public Task RemoveAsync(LiveBreakPoint breakPoint)
        {
            throw new NotImplementedException();
        }

        public Task ResetAsync(bool runAfter)
        {
            throw new NotImplementedException();
        }

        public Task RunAsync()
        {
            throw new NotImplementedException();
        }

        public Task<LiveBreakPoint> SetBreakPointAsync(string file, uint line)
        {
            throw new NotImplementedException();
        }

        public Task SetWatchFormatAsync(string id, WatchFormat format)
        {
            throw new NotImplementedException();
        }

        public Task<bool> StartAsync(IToolChain toolChain, IConsole console, IProject project)
        {
            throw new NotImplementedException();
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