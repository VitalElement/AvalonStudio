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
            _debugManager = IoC.Get<IDebugManager2>();

            if (_debugManager != null)
            {
                _debugManager.TargetStopped += _debugManager_TargetStopped;

                _debugManager.DebugSessionStarted += (sender, e) => { IsVisible = true; };

                _debugManager.DebugSessionEnded += (sender, e) =>
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