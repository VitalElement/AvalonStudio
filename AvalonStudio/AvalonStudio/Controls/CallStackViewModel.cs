namespace AvalonStudio.Controls
{
    using AvalonStudio.MVVM;
    using Debugging;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using ReactiveUI;
    using System.Collections.ObjectModel;
    public class CallStackViewModel : ViewModel
    {
        public CallStackViewModel ()
        {
            frames = new ObservableCollection<FrameViewModel>();
        }

        public void Clear()
        {
            Frames.Clear();         
        }

        public void Update(List<Frame> frames)
        {
            if (frames != null)
            {
                Frames.Clear();

                foreach (var frame in frames)
                {
                    this.Frames.Add(new FrameViewModel(frame));
                }
            }
        }

        private FrameViewModel selectedFrame;
        public FrameViewModel SelectedFrame
        {
            get { return selectedFrame; }
            set
            {
                selectedFrame = value;

                if (selectedFrame != null)
                {
                    // TODO implement click interaction
                    //WorkspaceViewModel.Instance.OpenDocument(Workspace.This.SolutionExplorer.Model.FindFile(selectedFrame.Model.FullFileName.NormalizePath()), selectedFrame.Model.Line, 1, true, false, false, true);
                }

                this.RaisePropertyChanged(nameof(SelectedFrame));
            }
        }

        private ObservableCollection<FrameViewModel> frames;
        public ObservableCollection<FrameViewModel> Frames
        {
            get { return frames; }
            set { frames = value; }
        }
    }
}
