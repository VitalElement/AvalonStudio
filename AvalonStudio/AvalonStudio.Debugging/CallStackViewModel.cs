using System.Collections.Generic;
using System.Collections.ObjectModel;
using Avalonia.Threading;
using AvalonStudio.Extensibility;
using AvalonStudio.Extensibility.Plugin;
using AvalonStudio.MVVM;
using ReactiveUI;

namespace AvalonStudio.Debugging
{
	public class CallStackViewModel : ToolViewModel, IExtension
	{
		private IDebugManager _debugManager;

		private FrameViewModel selectedFrame;

		public CallStackViewModel()
		{
			Title = "CallStack";

			Dispatcher.UIThread.InvokeAsync(() => { IsVisible = false; });

			Frames = new ObservableCollection<FrameViewModel>();
		}

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
			_debugManager = IoC.Get<IDebugManager>();

			_debugManager.DebugFrameChanged += _debugManager_DebugFrameChanged;

			_debugManager.DebugSessionStarted += (sender, e) => { IsVisible = true; };

			_debugManager.DebugSessionEnded += (sender, e) =>
			{
				IsVisible = false;
				Clear();
			};
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
					Frames.Add(new FrameViewModel(_debugManager, frame));
				}
			}
		}

		private async void _debugManager_DebugFrameChanged(object sender, FrameChangedEventArgs e)
		{
			var updates = await _debugManager.CurrentDebugger.ListStackFramesAsync();

			Dispatcher.UIThread.InvokeAsync(() => { Update(updates); });
		}
	}
}