namespace AvalonStudio.Debugging
{
    using MVVM;
    using MVVM.DataVirtualization;
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using ReactiveUI;
    using Avalonia.Threading;
    using Extensibility.Plugin;
    using Extensibility;
    public class MemoryViewModel : ToolViewModel, IExtension
    {
        private IDebugManager2 _debugManager;
        public const string ToolId = "CIDMEM001";
        const int Columns = 32;

        public MemoryViewModel()
        {
            dataProvider = new MemoryViewDataProvider(Columns);

            integerSizeOptions = new MutuallyExclusiveEnumerationCollection<MemoryViewDataProvider.IntegerSize>(MemoryViewDataProvider.IntegerSize.U8, (v) =>
            {
                var address = currentAddress;

                dataProvider.IntegerDisplaySize = v;
                MemoryData = new AsyncVirtualizingCollection<MemoryBytesViewModel>(dataProvider, 40, 60000);

                SetAddress(address);
                this.RaisePropertyChanged(nameof(ValueColumnWidth));
            });

            Address = "0";
            IsVisible = false;
        }

        private MutuallyExclusiveEnumerationCollection<MemoryViewDataProvider.IntegerSize> integerSizeOptions;
        public MutuallyExclusiveEnumerationCollection<MemoryViewDataProvider.IntegerSize> IntegerSizeOptions
        {
            get { return integerSizeOptions; }
            set { this.RaiseAndSetIfChanged(ref integerSizeOptions, value); }
        }

        public double ValueColumnWidth
        {
            get
            {
                if (dataProvider.IntegerDisplaySize == MemoryViewDataProvider.IntegerSize.NoData)
                {
                    return 0;
                }

                double fontSpace = 6.5;
                // number of bytes * 2 * font space...
                var byteSpace = (Columns * 2) * fontSpace;

                var spaces = Columns / ((int)dataProvider.IntegerDisplaySize);

                return byteSpace + (spaces * fontSpace) + 5;
                //return (fontSpace * 2) + 5;
                // number of spaces * font space...
            }
        }


        private MemoryViewDataProvider dataProvider;
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

            dataProvider.SetDebugger(debugger);
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
            get { return address; }
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

        new public async void Invalidate()
        {
            if (MemoryData == null)
            {
                Dispatcher.UIThread.InvokeAsync(() =>
                {
                    MemoryData = new AsyncVirtualizingCollection<MemoryBytesViewModel>(dataProvider, 15, 500);
                });
            }
            else
            {
                Dispatcher.UIThread.InvokeAsync(() =>
                {
                    MemoryData.CleanPagesAround((ulong)selectedIndex);
                });

                var pages = memoryData.Pages.ToList();

                if (debugger != null)
                {
                    foreach (var page in pages)
                    {
                        foreach (var item in page.Value.Items)
                        {
                            if (item.Data != null)
                            {
                                await item.Data.InvalidateAsync(debugger);
                            }
                        }
                    }
                }
            }
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


        private AsyncVirtualizingCollection<MemoryBytesViewModel> memoryData;
        public AsyncVirtualizingCollection<MemoryBytesViewModel> MemoryData
        {
            get { return memoryData; }
            set { this.RaiseAndSetIfChanged(ref memoryData, value); }
        }

        public override Location DefaultLocation
        {
            get
            {
                return Location.MiddleTop;
            }
        }
    }
}
