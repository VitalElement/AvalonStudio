using Avalonia.Threading;
using AvalonStudio.Extensibility;
using AvalonStudio.Extensibility.Plugin;
using AvalonStudio.MVVM;
using Mono.Debugging.Client;
using System.Composition;
using System.Threading.Tasks;

namespace AvalonStudio.Debugging
{
    public class LocalsViewModel : WatchListViewModel
    {
        [ImportingConstructor]
        public LocalsViewModel(DebugManager2 debugManager) : base(null)
        {
            DebugManager = debugManager;

            Title = "Locals";

            Dispatcher.UIThread.InvokeAsync(() => { IsVisible = false; });            

            if (DebugManager != null)
            {
                DebugManager.FrameChanged += DebugManager_FrameChanged;

                DebugManager.DebugSessionStarted += (sender, e) => { IsVisible = true; };

                DebugManager.DebugSessionEnded += (sender, e) =>
                {
                    IsVisible = false;
                    Clear();
                };
            }
        }

        public override Location DefaultLocation
        {
            get { return Location.RightBottom; }
        }

        private void Update (StackFrame stackFrame)
        {
            var locals = stackFrame.GetAllLocals();

            InvalidateObjects(locals);
        }

        private void DebugManager_FrameChanged(object sender, System.EventArgs e)
        {
            Task.Run(() =>
            {
                Update(DebugManager.SelectedFrame);
            });
        }
    }
}