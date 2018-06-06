using Avalonia.Threading;
using AvalonStudio.Extensibility;
using AvalonStudio.Extensibility.Plugin;
using AvalonStudio.MVVM;
using AvalonStudio.Platforms;
using AvalonStudio.Shell;
using ReactiveUI;
using System.Collections.ObjectModel;
using System.Composition;

namespace AvalonStudio.Debugging
{
    [ExportToolControl]    
    [Export(typeof(IExtension))]
    [Shared]
    public class CallStackViewModel : ToolViewModel, IActivatableExtension
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
                    
                    var file = shell?.CurrentSolution?.FindFile(selectedFrame.Model.SourceLocation.FileName.NormalizePath());

                    if (file != null)
                    {
                        Dispatcher.UIThread.InvokeAsync(async () =>
                        {
                            await shell?.OpenDocumentAsync(file, selectedFrame.Line, -1, -1, false, true);
                        });
                    }
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