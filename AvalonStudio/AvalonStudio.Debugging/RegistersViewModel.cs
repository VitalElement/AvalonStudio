namespace AvalonStudio.Debugging
{
    using Extensibility;
    using Extensibility.Plugin;
    using MVVM;
    using Perspex.Threading;
    using ReactiveUI;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;

    public class RegistersViewModel : ToolViewModel<ObservableCollection<RegisterViewModel>>, IExtension
    {
        private IDebugManager _debugManager;

        public RegistersViewModel() : base(new ObservableCollection<RegisterViewModel>())
        {
            Title = "Registers";
            lastChangedRegisters = new List<RegisterViewModel>();
        }

        private double columnWidth;
        public double ColumnWidth
        {
            get { return columnWidth; }
            set { this.RaiseAndSetIfChanged(ref columnWidth, value); }
        }

        public override Location DefaultLocation
        {
            get
            {
                return Location.Right;
            }
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
            if (firstStopInSession)
            {
                firstStopInSession = false;

                List<Register> registers = null;

                registers = _debugManager.CurrentDebugger.GetRegisters().Values.ToList();

                SetRegisters(registers);
            }
            else
            {
                Dictionary<int, string> changedRegisters = null;

                changedRegisters = _debugManager.CurrentDebugger.GetChangedRegisters();

                Dispatcher.UIThread.InvokeAsync(() =>
                {
                    UpdateRegisters(changedRegisters);
                });
            }
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

        public void BeforeActivation()
        {
            
        }

        public void Activation()
        {
            _debugManager = IoC.Get<IDebugManager>();
            _debugManager.DebugFrameChanged += RegistersViewModel_DebugFrameChanged;
            _debugManager.DebuggerChanged += (sender, e) =>
            {
                firstStopInSession = true;
            };
        }

        private void RegistersViewModel_DebugFrameChanged(object sender, FrameChangedEventArgs e)
        {
            Invalidate();
        }
    }
}
