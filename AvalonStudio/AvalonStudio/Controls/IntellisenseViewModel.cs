namespace AvalonStudio.Controls
{
    using Languages;
    using Languages.ViewModels;
    using MVVM;
    using Perspex;
    using ReactiveUI;
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public class IntellisenseViewModel : ViewModel, IIntellisenseControl, IDisposable
    {
        private EditorModel editor;
        private EditorViewModel editorViewModel;

        public IntellisenseViewModel(EditorModel editor, EditorViewModel viewModel)
        {
            completionData = new List<CompletionDataViewModel>();
            this.editorViewModel = viewModel;
            this.editor = editor;
            isVisible = false;
        }

        public Task<CodeCompletionResults> DoCompletionRequestAsync(int line, int column, string filter)
        {
            return editor.DoCompletionRequestAsync(line, column, filter);
        }

        public void Dispose()
        {
            editor = null;
            editorViewModel = null;
        }

        private CompletionDataViewModel selectedCompletion;
        public CompletionDataViewModel SelectedCompletion
        {
            get { return selectedCompletion; }
            set { this.RaiseAndSetIfChanged(ref selectedCompletion, value); }
        }        

        private Thickness position;
        public Thickness Position
        {
            get { return position; }
            set { this.RaiseAndSetIfChanged(ref position, value); }
        }

        private bool isVisible;
        public bool IsVisible
        {
            get { return isVisible; }
            set
            {
                this.RaiseAndSetIfChanged(ref isVisible, value);
            }
        }

        private IList<CompletionDataViewModel> completionData;
        public IList<CompletionDataViewModel> CompletionData
        {
            get { return completionData; }
            set { this.RaiseAndSetIfChanged(ref completionData, value); }
        }    
    }
}
