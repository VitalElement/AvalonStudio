namespace AvalonStudio.Controls
{
    using AvalonStudio.MVVM;
    using Models.LanguageServices;
    using Perspex;
    using Perspex.Input;
    using ReactiveUI;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Threading.Tasks;

    public class IntellisenseViewModel : ViewModel<List<CompletionDataViewModel>>
    {
        private EditorModel editor;
        private EditorViewModel editorViewModel;

        public IntellisenseViewModel(EditorModel editor, EditorViewModel viewModel) : base(new List<CompletionDataViewModel>())
        {
            this.editorViewModel = viewModel;
            this.editor = editor;
            isVisible = false;
        }

        public void Clear ()
        {
            Model.Clear();
        }

        public void SetCompletionResults (CodeCompletionResults results)
        {
            
        }

        private CompletionDataViewModel selectedCompletion;
        public CompletionDataViewModel SelectedCompletion
        {
            get { return selectedCompletion; }
            set { this.RaiseAndSetIfChanged(ref selectedCompletion, value); }
        }

        private bool IsIntellisenseFilterModificationKey (KeyEventArgs e)
        {
            bool result = false;

            result = (e.Key >= Key.D0 && e.Key <= Key.Z) || (e.Key >= Key.NumPad0 && e.Key <= Key.NumPad9);

            if (!result)
            {
                switch (e.Key)
                {
                    case Key.Back:                    
                    case Key.OemPeriod:
                        result = true;
                        break;
                }
            }

            if(!result && e.Modifiers == InputModifiers.Shift)
            {
                switch (e.Key)
                {
                    case Key.OemMinus:
                        result = true;
                        break;
                }
            }

            return result;
        }

        private bool IsAllowedNonFilterModificationKey (KeyEventArgs e)
        {
            bool result = false;

            if (e.Key >= Key.LeftShift && e.Key <= Key.RightShift)
            {
                result = true;
            }

            if (!result)
            {
                switch (e.Key)
                {
                    case Key.None:
                        result = true;
                        break;
                }
            }

            return result;
        }

        private bool IsIntellisenseKey(KeyEventArgs e)
        {
            return IsIntellisenseFilterModificationKey(e) || IsAllowedNonFilterModificationKey(e); 
        }

        private bool IsIntellisenseResetKey (KeyEventArgs e)
        {
            bool result = false;

            if(e.Key == Key.OemPeriod)
            {
                result = true;
            }

            return result;
        }

        private string currentFilter = string.Empty;

        public void OnTextInput(TextInputEventArgs e)
        {
            currentFilter += e.Text;            
        }



        public void OnKeyDown(KeyEventArgs e)
        {
            if(e.Key == Key.Back)
            {
                if(currentFilter.Length > 0)
                {
                    currentFilter = currentFilter.Substring(0, currentFilter.Length - 1);
                }
            }
        }

        private CompletionDataViewModel noSelectedCompletion = new CompletionDataViewModel(null);

        private List<CompletionDataViewModel> unfilteredCompletions = new List<CompletionDataViewModel>();

        //System.Diagnostics.Stopwatch sw = new System.Diagnostics.Stopwatch();

        public async void OnKeyUp(KeyEventArgs e)
        {    
            if (IsIntellisenseKey(e))
            {
                if(IsIntellisenseResetKey(e))
                {
                    isVisible = false;  // We dont actually want to hide, so use backing field.
                    currentFilter = string.Empty;
                }

                List<CompletionDataViewModel> filteredResults = null;

                if (!IsVisible && (IsIntellisenseFilterModificationKey(e) || IsIntellisenseResetKey(e)))
                {
                    var caret = editorViewModel.CaretTextLocation;

                    await editor.DoCompletionRequestAsync(caret.Line, caret.Column, currentFilter);
                
                    await Task.Factory.StartNew(() =>
                    {
                        unfilteredCompletions.Clear();

                        foreach (var result in editor.CodeCompletionResults.Completions)
                        {
                            if (result.Suggestion.ToLower().Contains(currentFilter.ToLower()))
                            {
                                CompletionDataViewModel currentCompletion = null;

                                currentCompletion = unfilteredCompletions.BinarySearch(c => c.Title, result.Suggestion);

                                if (currentCompletion == null)
                                {
                                    unfilteredCompletions.Add(new CompletionDataViewModel(result));
                                    //Completions.Add(new CodeSuggestionViewModel(DeclarationViewModel.Create(new Declaration() { Type = codeCompletion.CursorKind, Spelling = typedText }), hint));
                                }
                                else
                                {
                                    //currentCompletion.NumOverloads++;
                                }
                            }
                        }
                    });

                    filteredResults = unfilteredCompletions;
                }
                else
                {
                    await Task.Factory.StartNew(() =>
                    {
                        filteredResults = unfilteredCompletions.Where((c) => c.Title.ToLower().Contains(currentFilter.ToLower())).ToList();
                    });
                }

                if (currentFilter != string.Empty)
                {
                    var newSelectedCompletions = filteredResults.Where((s) => s.Title.StartsWith(currentFilter));

                    SelectedCompletion = null;

                    if (newSelectedCompletions.Count() == 0)
                    {
                        SelectedCompletion = noSelectedCompletion;
                    }
                    else
                    {
                        var newSelectedCompletion = newSelectedCompletions.FirstOrDefault();
                        
                        SelectedCompletion = newSelectedCompletion;                        
                    }
                }
                else
                {
                    SelectedCompletion = noSelectedCompletion;
                }                

                if (filteredResults?.Count > 0)
                {
                    if (selectedCompletion != noSelectedCompletion)
                    {
                        var index = filteredResults.IndexOf(selectedCompletion);

                        Model = filteredResults.Skip(index - 4).Take(25).ToList();
                    }
                    else
                    {
                        Model = filteredResults.Take(25).ToList();
                    }

                    IsVisible = true;
                }
                else
                {
                    currentFilter = string.Empty;
                }
            }
            else
            {
                currentFilter = string.Empty;
                IsVisible = false;
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
