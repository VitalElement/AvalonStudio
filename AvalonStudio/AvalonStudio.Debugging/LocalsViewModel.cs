using Avalonia.Threading;
using AvalonStudio.Extensibility;
using AvalonStudio.Extensibility.Studio;
using AvalonStudio.MVVM;
using AvalonStudio.Shell;
using Mono.Debugging.Client;
using System.Composition;
using System.Threading.Tasks;

namespace AvalonStudio.Debugging
{
    [ExportToolControl, Export(typeof(IExtension)), Shared]
    public class LocalsViewModel : WatchListViewModel, IActivatableExtension
    {
        public LocalsViewModel()
        {
            Title = "Locals";

            Dispatcher.UIThread.InvokeAsync(() => { IsVisible = false; });
        }

        public override Location DefaultLocation
        {
            get { return Location.Bottom; }
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

            IoC.Get<IStudio>().DebugPerspective.AddOrSelectTool(this);
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