namespace AvalonStudio.Controls
{
    using AvalonStudio.Debugging;
    using AvalonStudio.MVVM;
    using System;
    using ReactiveUI;
    using MVVM.DataVirtualization;
    using System.Collections.Generic;
    using System.Linq;
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


    public class DisassemblyViewModel : ToolViewModel
    {
        public DisassemblyViewModel()
        {
            this.dataProvider = new DissasemblyDataProvider();
        }

        private IDebugger debugger;
        private DissasemblyDataProvider dataProvider;

        public void SetDebugger(IDebugger debugger)
        {
            if (this.debugger != null)
            {
                this.debugger.StateChanged -= Debugger_StateChanged;
            }

            this.debugger = debugger;

            if (debugger != null)
            {
                debugger.StateChanged += Debugger_StateChanged;
            }

            dataProvider.SetDebugger(debugger);
        }

        private void Debugger_StateChanged(object sender, EventArgs e)
        {
            if (debugger.State == DebuggerState.Paused)
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


        public void SetAddress(ulong currentAddress)
        {
            // Commented code triggers data virtualization, but perspex needs to virtualize the containers (trying to create over a billion containers here.
            //if (DissasemblyData == null)
            //{
            //    DissasemblyData = new AsyncVirtualizingCollection<InstructionLine>(dataProvider, 100, 60000);
            //}

            ulong startIndex = currentAddress - 20;

            var instructions = debugger.Disassemble(startIndex, 50);

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
                return Location.Right;
            }
        }
    }
}
