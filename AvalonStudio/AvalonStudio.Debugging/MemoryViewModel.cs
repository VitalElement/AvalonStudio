namespace AvalonStudio.Debugging
{
    using Avalonia.Threading;
    using Extensibility;
    using MVVM;
    using ReactiveUI;
    using System;
    using System.Composition;
    using System.Linq;

    [ExportToolControl]
    [Export(typeof(IExtension))]
    [Shared]
    public class MemoryViewModel : ToolViewModel, IActivatableExtension
    {
        private IDebugManager2 _debugManager;
        public const string ToolId = "CIDMEM001";
        private const int Columns = 32;

        public MemoryViewModel() : base("Memory Viewer")
        {
            Address = "0";
            IsVisible = false;
        }

        private IDebugger2 debugger;

        public void SetDebugger(IDebugger2 debugger)
        {
            if (this.debugger != null)
            {
                //this.debugger.StateChanged -= Debugger_StateChanged;
            }

            if (debugger != null)
            {
                //debugger.StateChanged += Debugger_StateChanged;
            }

            this.debugger = debugger;

            // dataProvider.SetDebugger(debugger);
        }

        private bool enabled;

        public bool Enabled
        {
            get { return enabled; }
            set { this.RaiseAndSetIfChanged(ref enabled, value); }
        }

        private string address;

        public string Address
        {
            get
            {
                return address;
            }
            set
            {
                ulong parsedValue = 0;
                string newAddress = string.Empty;
                try
                {
                    parsedValue = Convert.ToUInt64(value, 16);
                    newAddress = string.Format("0x{0:X8}", parsedValue);
                    SetAddress(parsedValue);
                }
                catch (Exception)
                {
                    newAddress = "Unable to evaluate expression.";
                }

                this.RaiseAndSetIfChanged(ref address, newAddress);
            }
        }

        private void SetAddress(ulong address)
        {
            this.currentAddress = address;
            SelectedIndex = (long)(address / Columns);
        }

        public void BeforeActivation()
        {
        }

        public void Activation()
        {
            _debugManager = IoC.Get<IDebugManager2>();

            _debugManager.DebugSessionStarted += (sender, e) => { IsVisible = true; };

            _debugManager.DebugSessionEnded += (sender, e) =>
            {
                IsVisible = false;

                // TODO clear out data ready for GC, this requires a fix in Avalonia.
                //DisassemblyData = null;
            };
        }

        private ulong currentAddress;
        private long selectedIndex;

        public long SelectedIndex
        {
            get { return selectedIndex; }
            set { this.RaiseAndSetIfChanged(ref selectedIndex, value); }
        }

        public override Location DefaultLocation
        {
            get
            {
                return Location.Top;
            }
        }
    }
}