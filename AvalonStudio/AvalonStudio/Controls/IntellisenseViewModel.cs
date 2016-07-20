using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Avalonia;
using AvalonStudio.Languages;
using AvalonStudio.Languages.ViewModels;
using AvalonStudio.MVVM;
using ReactiveUI;

namespace AvalonStudio.Controls
{
	public class IntellisenseViewModel : ViewModel, IIntellisenseControl, IDisposable
	{
		private IList<CompletionDataViewModel> completionData;
		private EditorModel editor;
		private EditorViewModel editorViewModel;

		private bool isVisible;

		private Thickness position;

		private CompletionDataViewModel selectedCompletion;

		public IntellisenseViewModel(EditorModel editor, EditorViewModel viewModel)
		{
			completionData = new List<CompletionDataViewModel>();
			editorViewModel = viewModel;
			this.editor = editor;
			isVisible = false;
		}

		public Thickness Position
		{
			get { return position; }
			set { this.RaiseAndSetIfChanged(ref position, value); }
		}

		public void Dispose()
		{
			editor = null;
			editorViewModel = null;
		}

		public async Task<CodeCompletionResults> DoCompletionRequestAsync(int line, int column)
		{
			return await editor.DoCompletionRequestAsync(line, column);
		}

		public CompletionDataViewModel SelectedCompletion
		{
			get { return selectedCompletion; }
			set { this.RaiseAndSetIfChanged(ref selectedCompletion, value); }
		}

		public bool IsVisible
		{
			get { return isVisible; }
			set { this.RaiseAndSetIfChanged(ref isVisible, value); }
		}

		public IList<CompletionDataViewModel> CompletionData
		{
			get { return completionData; }
			set { this.RaiseAndSetIfChanged(ref completionData, value); }
		}
	}
}