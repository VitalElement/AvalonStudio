using Avalonia.Threading;
using AvalonStudio.Extensibility;
using AvalonStudio.Extensibility.Plugin;
using AvalonStudio.MVVM;
using AvalonStudio.Shell;
using ReactiveUI;
using System.Collections.ObjectModel;

namespace AvalonStudio.Debugging
{
    public class CallStackViewModel : ToolViewModel, IExtension
    {
        private IDebugManager2 _debugManager;

        private FrameViewModel selectedFrame;

        public CallStackViewModel()
        {
            Title = "CallStack";

            Dispatcher.UIThread.InvokeAsync(() => { IsVisible = false; });

            Frames = new ObservableCollection<FrameViewModel>();
        }

        public FrameViewModel SelectedFrame
        {
            get
            {
                return selectedFrame;
            }
            set
            {
                selectedFrame = value;

                if (selectedFrame != null)
                {
                    var shell = IoC.Get<IShell>();

                    _debugManager.SelectedFrame = selectedFrame.Model;

                    shell?.OpenDocument(shell?.CurrentSolution?.FindFile(selectedFrame.Model.SourceLocation.FileName), selectedFrame.Line, -1, -1, true, true);
                }

                this.RaisePropertyChanged(nameof(SelectedFrame));
            }
        }

        public ObservableCollection<FrameViewModel> Frames { get; set; }

        public override Location DefaultLocation
        {
            get { return Location.BottomRight; }
        }

        public void BeforeActivation()
        {
        }

        public void Activation()
        {
            _debugManager = IoC.Get<IDebugManager2>();

            _debugManager.TargetStopped += _debugManager_TargetStopped;

            _debugManager.DebugSessionStarted += (sender, e) => { IsVisible = true; };

            _debugManager.DebugSessionEnded += (sender, e) =>
            {
                IsVisible = false;
                Clear();
            };
        }

        private void _debugManager_TargetStopped(object sender, Mono.Debugging.Client.TargetEventArgs e)
        {
            Frames.Clear();

            for (int i = 0; i < e.Backtrace.FrameCount; i++)
            {
                var frame = e.Backtrace.GetFrame(i);

                Frames.Add(new FrameViewModel(_debugManager, frame));
            }
        }

        public void Clear()
        {
            Frames.Clear();
        }
    }
}