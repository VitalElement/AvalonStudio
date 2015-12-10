namespace AvalonStudio.Controls
{
    using AvalonStudio.MVVM;
    using Models.LanguageServices;
    using Perspex;
    using Perspex.Input;
    using ReactiveUI;
    using SharpDX.Collections;

    public class IntellisenseViewModel : ViewModel<ObservableCollection<CompletionDataViewModel>>
    {
        private EditorModel editor;
        private EditorViewModel editorViewModel;

        public IntellisenseViewModel(EditorModel editor, EditorViewModel viewModel) : base(new ObservableCollection<CompletionDataViewModel>())
        {
            this.editorViewModel = viewModel;
            this.editor = editor;
            isVisible = false;
        }

        public void Clear ()
        {
            Model.Clear();
        }

        public void OnKeyDown (KeyEventArgs e)
        {
            if(!isVisible)
            {
                IsVisible = true;

                var location = editor.TextDocument.GetLocation(editorViewModel.CaretIndex);
                var results = editor.LanguageService.CodeCompleteAt(editor.ProjectFile.Location, location.Line, location.Column, EditorModel.UnsavedFiles);

                Clear();

                foreach (var result in results)
                {
                    if (result.Suggestion.ToLower().Contains(editorViewModel.WordAtCaret))
                    {
                        CompletionDataViewModel currentCompletion = null;

                        currentCompletion = Model.BinarySearch(c => c.Title, result.Suggestion);

                        if (currentCompletion == null)
                        {
                            Add(result);
                            //Completions.Add(new CodeSuggestionViewModel(DeclarationViewModel.Create(new Declaration() { Type = codeCompletion.CursorKind, Spelling = typedText }), hint));
                        }
                        else
                        {
                            //currentCompletion.NumOverloads++;
                        }
                    }
                    
                }
            }
        }

        public void Add(CodeCompletionData data)
        {
            Model.Add(new CompletionDataViewModel(data));
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
            set { this.RaiseAndSetIfChanged(ref isVisible, value); }
        }

    }
}
