using Avalonia.Threading;
using AvalonStudio.Extensibility;
using AvalonStudio.Extensibility.Plugin;
using AvalonStudio.MVVM;

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
                DebugManager.TargetStopped += _debugManager_TargetStopped;

                DebugManager.DebugSessionStarted += (sender, e) => { IsVisible = true; };

                DebugManager.DebugSessionEnded += (sender, e) =>
                {
                    IsVisible = false;
                    Clear();
                };
            }
        }

        private void _debugManager_TargetStopped(object sender, Mono.Debugging.Client.TargetEventArgs e)
        {
            if (e.IsStopEvent)
            {
                var currentFrame = e.Backtrace.GetFrame(0);

                var locals = currentFrame.GetAllLocals();

                InvalidateObjects(locals);
            }
        }
    }
}