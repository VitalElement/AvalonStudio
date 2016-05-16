namespace AvalonStudio.Debugging
{
    using AvalonStudio.Debugging;
    using AvalonStudio.MVVM;
    using System;
    using ReactiveUI;
    using MVVM.DataVirtualization;
    using System.Collections.Generic;
    using System.Linq;
    using Extensibility.Plugin;
    using Extensibility;
    using Avalonia.Threading;

    public abstract class LineViewModel : ViewModel<DisassembledLine>
    {
        public LineViewModel(DisassembledLine model) : base(model)
        {

        }
    }

    public class InstructionLineViewModel : LineViewModel
    {
        public InstructionLineViewModel(InstructionLine model) : base(model)
        {

        }

        public string Instruction
        {
            get
            {
                return Model.Instruction;
            }
        }

        public string Address
        {
            get
            {
                try
                {
                    return string.Format("0x{0:X}", Model.Address);
                }
                catch (Exception)
                {
                    return "";
                }
            }
        }

        public string Symbol
        {
            get
            {
                return string.Format("<{0} + {1}>", Model.FunctionName, Model.Offset);
            }
        }

        new public InstructionLine Model
        {
            get
            {
                return (InstructionLine)base.Model;
            }
        }
    }

    public class SourceLineViewModel : LineViewModel
    {
        public SourceLineViewModel(SourceLine model) : base(model)
        {

        }

        public string LineText
        {
            get
            {
                return Model.LineText;
            }
        }

        new public SourceLine Model
        {
            get
            {
                return (SourceLine)base.Model;
            }
        }
    }


    public class DisassemblyViewModel : ToolViewModel, IExtension
    {
        private IDebugManager _debugManager;
        private IDebugger _debugger;

        public DisassemblyViewModel()
        {
            Dispatcher.UIThread.InvokeAsync(() =>
            {
                IsVisible = false;
            });

            Title = "Dissasembly";
            this.dataProvider = new DissasemblyDataProvider();
        }

        public void Activation()
        {
            _debugManager = IoC.Get<IDebugManager>();
            _debugManager.DebuggerChanged += (sender, e) =>
            {
                SetDebugger(_debugManager.CurrentDebugger);
            };

            _debugManager.DebugFrameChanged += _debugManager_DebugFrameChanged;

            _debugManager.DebugSessionStarted += (sender, e) =>
            {
                IsVisible = true;
            };

            _debugManager.DebugSessionEnded += (sender, e) =>
            {
                IsVisible = false;
                
                // TODO implement clear here.
            };
        }

        private void _debugManager_DebugFrameChanged(object sender, FrameChangedEventArgs e)
        {
            Dispatcher.UIThread.InvokeAsync(() =>
            {
                SetAddress(e.Address);
            });
        }

        public void BeforeActivation()
        {

        }
        
        private DissasemblyDataProvider dataProvider;

        public void SetDebugger(IDebugger debugger)
        {
            if (this._debugger != null)
            {
                this._debugger.StateChanged -= Debugger_StateChanged;
            }

            this._debugger = debugger;

            if (debugger != null)
            {
                debugger.StateChanged += Debugger_StateChanged;
            }

            dataProvider.SetDebugger(debugger);
        }

        private void Debugger_StateChanged(object sender, EventArgs e)
        {
            if (_debugger.State == DebuggerState.Paused)
            {
                Enabled = true;
            }
            else
            {
                Enabled = false;
            }
        }

        private bool enabled;
        public bool Enabled
        {
            get { return enabled; }
            set { this.RaiseAndSetIfChanged(ref enabled, value); }
        }


        public async void SetAddress(ulong currentAddress)
        {
            // Commented code triggers data virtualization, but avalonia needs to virtualize the containers (trying to create over a billion containers here.
            //if (DissasemblyData == null)
            //{
            //    DissasemblyData = new AsyncVirtualizingCollection<InstructionLine>(dataProvider, 100, 60000);
            //}

            ulong startIndex = currentAddress - 20;

            var instructions = await _debugger.DisassembleAsync(startIndex, 50);

            if (instructions != null)
            {
                var result = new List<InstructionLine>();
                
                uint address = (uint)startIndex;
                var instruction = instructions.Where((inst) => inst.Address == currentAddress);

                foreach (var line in instructions)
                {                    
                    if (instruction.Count() == 0)
                    {
                        result.Add(new InstructionLine() { Visible = false });
                    }
                    else
                    {
                        result.Add(line);
                    }
                }

                DisassemblyData = result;
                SelectedIndex = (ulong)DisassemblyData.IndexOf(DisassemblyData.FirstOrDefault(i => i.Address == currentAddress));
            }            
        }

        private ulong selectedIndex;
        public ulong SelectedIndex
        {
            get { return selectedIndex; }
            set { this.RaiseAndSetIfChanged(ref selectedIndex, value); }
        }

        private List<InstructionLine> disassemblyData;
        public List<InstructionLine> DisassemblyData
        {
            get { return disassemblyData; }
            set { this.RaiseAndSetIfChanged(ref disassemblyData, value); }
        }

        public override Location DefaultLocation
        {
            get
            {
                return Location.RightTop;
            }
        }
    }
}
