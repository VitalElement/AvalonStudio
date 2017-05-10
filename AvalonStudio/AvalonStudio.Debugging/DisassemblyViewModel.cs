using Avalonia.Threading;
using AvalonStudio.Extensibility;
using AvalonStudio.Extensibility.Plugin;
using AvalonStudio.MVVM;
using ReactiveUI;
using System.Threading.Tasks;

namespace AvalonStudio.Debugging
{
    public class DisassemblyViewModel : ToolViewModel, IExtension
    {
        private IDebugger2 _debugger;
        private IDebugManager2 _debugManager;
        
        private bool enabled;

        private ulong selectedIndex;

        public DisassemblyViewModel()
        {
            Dispatcher.UIThread.InvokeAsync(() => { IsVisible = false; });

            Title = "Disassembly";            
        }

        public bool Enabled
        {
            get { return enabled; }
            set { this.RaiseAndSetIfChanged(ref enabled, value); }
        }

        public ulong SelectedIndex
        {
            get { return selectedIndex; }
            set { this.RaiseAndSetIfChanged(ref selectedIndex, value); }
        }

        public override Location DefaultLocation
        {
            get { return Location.RightTop; }
        }

        public void Activation()
        {
            _debugManager = IoC.Get<IDebugManager2>();

            _debugManager.DebugSessionStarted += (sender, e) => { IsVisible = true; };

            _debugManager.DebugSessionEnded += (sender, e) =>
            {
                IsVisible = false;
            };
        }

        public void BeforeActivation()
        {
        }

        public void SetAddress(ulong currentAddress)
        {
            //if (DisassemblyData == null)
            //{
            //    DisassemblyData = new AsyncVirtualizingCollection<InstructionLine>(dataProvider, 100, 6000);

            //    Task.Factory.StartNew(async () =>
            //    {
            //        await Task.Delay(50);

            //        Dispatcher.UIThread.InvokeAsync(() =>
            //        {
            //            SelectedIndex = currentAddress;
            //        });
            //    });
            //}
            //else
            //{
            //    SelectedIndex = currentAddress;
            //}
        }
    }
}