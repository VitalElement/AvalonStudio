using Avalonia.Threading;
using AvalonStudio.Extensibility;
using AvalonStudio.Extensibility.Plugin;
using AvalonStudio.MVVM;
using Mono.Debugging.Client;

namespace AvalonStudio.Debugging
{
    public class LocalsViewModel : WatchListViewModel, IExtension
    {
        public LocalsViewModel()
        {
            Title = "Locals";

            Dispatcher.UIThread.InvokeAsync(() => { IsVisible = false; });
        }

        public override Location DefaultLocation
        {
            get { return Location.RightBottom; }
        }

        public override void Activation()
        {
            DebugManager = IoC.Get<IDebugManager2>();

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

        private void Update (StackFrame stackFrame)
        {
            var locals = stackFrame.GetAllLocals();

            InvalidateObjects(locals);
        }

        private void DebugManager_FrameChanged(object sender, System.EventArgs e)
        {
            Update(DebugManager.SelectedFrame);
        }
    }
}