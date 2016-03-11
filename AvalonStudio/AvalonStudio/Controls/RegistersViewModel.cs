namespace AvalonStudio.Controls
{
    using Debugging;
    using MVVM;
    using Perspex.Media;
    using Perspex.Threading;
    using ReactiveUI;
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class RegistersViewModel : ViewModel<ObservableCollection<RegisterViewModel>>
    {
        public RegistersViewModel() : base(new ObservableCollection<RegisterViewModel>())
        {
            lastChangedRegisters = new List<RegisterViewModel>();
        }        

        private double columnWidth;
        public double ColumnWidth
        {
            get { return columnWidth; }
            set { this.RaiseAndSetIfChanged(ref columnWidth, value); }
        }

        public void SetDebugger(IDebugger debugger)
        {
            firstStopInSession = true;
            this.debugger = debugger;
        }

        private void SetRegisters(List<Register> registers)
        {
            if (registers != null)
            {
                this.Model = new ObservableCollection<RegisterViewModel>();

                foreach (var register in registers)
                {
                    this.Model.Add(new RegisterViewModel(register));
                }               

                ColumnWidth = 0;
                ColumnWidth = double.NaN;
            }
        }

        private bool firstStopInSession;        
        new public void Invalidate()
        {
            Dispatcher.UIThread.InvokeAsync(() =>
            {
                if (firstStopInSession)
                {
                    firstStopInSession = false;

                    List<Register> registers = null;

                    registers = debugger.GetRegisters().Values.ToList();

                    SetRegisters(registers);
                }
                else
                {
                    Dictionary<int, string> changedRegisters = null;

                    changedRegisters = debugger.GetChangedRegisters();

                    this.UpdateRegisters(changedRegisters);
                }
            });
        }

        public void Clear()
        {
            this.Model = new ObservableCollection<RegisterViewModel>();
        }

        private List<RegisterViewModel> lastChangedRegisters;

        private void UpdateRegisters(Dictionary<int, string> updatedValues)
        {
            foreach (var register in lastChangedRegisters)
            {
                register.HasChanged = false;
            }

            lastChangedRegisters.Clear();

            foreach (var value in updatedValues)
            {
                var register = Model.FirstOrDefault((r) => r.Index == value.Key);

                if (register != null)
                {
                    register.Value = value.Value;
                    register.HasChanged = true;

                    lastChangedRegisters.Add(register);
                }
            }

            ColumnWidth = 0;
            ColumnWidth = double.NaN;
        }

        private IDebugger debugger;
    }
}
