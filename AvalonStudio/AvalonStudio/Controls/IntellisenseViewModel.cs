namespace AvalonStudio.Controls
{
    using MVVM;
    using Languages;
    using Perspex;
    using Perspex.Input;
    using ReactiveUI;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using TextEditor.Document;
    using Utils;
    using Perspex.Threading;
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

        public void Clear()
        {
            Model.Clear();
        }

        public void SetCompletionResults(CodeCompletionResults results)
        {

        }

        private CompletionDataViewModel selectedCompletion;
        public CompletionDataViewModel SelectedCompletion
        {
            get { return selectedCompletion; }
            set { this.RaiseAndSetIfChanged(ref selectedCompletion, value); }
        }

        private bool IsIntellisenseOpenKey(KeyEventArgs e)
        {
            bool result = false;

            result = (e.Key >= Key.D0 && e.Key <= Key.D9 && e.Modifiers == InputModifiers.None) || (e.Key >= Key.A && e.Key <= Key.Z) || (e.Key >= Key.NumPad0 && e.Key <= Key.NumPad9);

            return result;
        }

        private bool IsIntellisenseFilterModificationKey(KeyEventArgs e)
        {
            bool result = false;

            result = IsIntellisenseOpenKey(e);

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

            if (!result && e.Modifiers == InputModifiers.Shift)
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

        private bool IsAllowedNonFilterModificationKey(KeyEventArgs e)
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
                    case Key.Up:
                    case Key.Down:
                    case Key.None:
                        result = true;
                        break;
                }
            }

            return result;
        }

        private bool IsIntellisenseKey(KeyEventArgs e)
        {
            return IsIntellisenseFilterModificationKey(e);
        }

        private bool IsIntellisenseResetKey(KeyEventArgs e)
        {
            bool result = false;

            if (e.Key == Key.OemPeriod)
            {
                result = true;
            }

            return result;
        }

        private string currentFilter = string.Empty;

        public void OnTextInput(TextInputEventArgs e)
        {
            //currentFilter += e.Text;
        }



        public void OnKeyDown(KeyEventArgs e)
        {
            CompleteOnKeyDown(e);
        }

        private CompletionDataViewModel noSelectedCompletion = new CompletionDataViewModel(null);

        private List<CompletionDataViewModel> unfilteredCompletions = new List<CompletionDataViewModel>();

        //System.Diagnostics.Stopwatch sw = new System.Diagnostics.Stopwatch();

        private bool DoComplete(bool includeLastChar)
        {
            bool result = false;

            if (Model.Count > 0 && SelectedCompletion != noSelectedCompletion)
            {
                int offset = 0;

                if (includeLastChar)
                {
                    offset = 1;
                }

                editorViewModel.TextDocument.Replace(intellisenseStartedAt, editorViewModel.CaretIndex - intellisenseStartedAt - offset, SelectedCompletion.Title);
                editorViewModel.CaretIndex = intellisenseStartedAt + SelectedCompletion.Title.Length + offset;

                result = true;
            }

            return result;
        }

        private void CompleteOnKeyDown(KeyEventArgs e)
        {
            if (IsVisible && e.Modifiers == InputModifiers.None)
            {
                switch (e.Key)
                {
                    case Key.Enter:
                    case Key.Tab:
                    case Key.OemPeriod:
                    case Key.OemMinus:
                    case Key.Space:
                        e.Handled = DoComplete(false);
                        break;


                    // Below might make intellisense less anoying for people not used to it.
                    //case Key.Space:
                    //    if (SelectedCompletion.Title.StartsWith(currentFilter))
                    //    {
                    //        e.Handled = DoComplete(false);
                    //    }
                    //    break;

                    case Key.Down:
                        {
                            int index = Model.IndexOf(SelectedCompletion);

                            if (index < Model.Count - 1)
                            {
                                SelectedCompletion = Model[index + 1];
                            }

                            e.Handled = true;
                        }
                        break;

                    case Key.Up:
                        {
                            int index = Model.IndexOf(SelectedCompletion);

                            if (index > 0)
                            {
                                SelectedCompletion = Model[index - 1];
                            }

                            e.Handled = true;
                        }
                        break;
                }
            }
        }

        private void CompleteOnKeyUp()
        {
            if (IsVisible)
            {
                char behindCaretChar = '\0';
                char behindBehindCaretChar = '\0';

                if (editorViewModel.CaretIndex > 0)
                {
                    behindCaretChar = editorViewModel.TextDocument.GetCharAt(editorViewModel.CaretIndex - 1);
                }

                if (editorViewModel.CaretIndex > 1)
                {
                    behindBehindCaretChar = editorViewModel.TextDocument.GetCharAt(editorViewModel.CaretIndex - 2);
                }

                switch (behindCaretChar)
                {
                    case '(':
                    case '=':
                    case '+':
                    case '-':
                    case '*':
                    case '/':
                    case '%':
                    case '|':
                    case '&':
                    case '!':
                    case '^':
                    case ' ':
                    case ':':
                    case '.':
                        DoComplete(true);
                        return;
                }
            }
        }

        private int intellisenseStartedAt;
        private object intellisenseLock = new object();

        private void Close()
        {
            SelectedCompletion = noSelectedCompletion;
            intellisenseStartedAt = editorViewModel.CaretIndex;
            IsVisible = false;
            currentFilter = string.Empty;
        }

        public async void OnKeyUp(KeyEventArgs e)
        {
            if (IsIntellisenseKey(e))
            {
                var caretIndex = editorViewModel.CaretIndex;

                if (caretIndex <= intellisenseStartedAt)
                {
                    Close();
                    return;
                }

                if (IsIntellisenseResetKey(e))
                {
                    isVisible = false;  // We dont actually want to hide, so use backing field.
                    //currentFilter = string.Empty;
                }

                CompleteOnKeyUp();

                IEnumerable<CompletionDataViewModel> filteredResults = null;

                if (!IsVisible && (IsIntellisenseOpenKey(e) || IsIntellisenseResetKey(e)))
                {
                    IsVisible = true;
                    var caret = editorViewModel.CaretTextLocation;

                    char behindCaretChar = '\0';
                    char behindBehindCaretChar = '\0';

                    if (editorViewModel.CaretIndex > 0)
                    {
                        behindCaretChar = editorViewModel.TextDocument.GetCharAt(editorViewModel.CaretIndex - 1);
                    }

                    if (editorViewModel.CaretIndex > 1)
                    {
                        behindBehindCaretChar = editorViewModel.TextDocument.GetCharAt(editorViewModel.CaretIndex - 2);
                    }

                    if (behindCaretChar != '>')
                    {
                        intellisenseStartedAt = TextUtilities.GetNextCaretPosition(editorViewModel.TextDocument, caretIndex, TextUtilities.LogicalDirection.Backward, TextUtilities.CaretPositioningMode.WordStart);
                    }
                    else
                    {
                        intellisenseStartedAt = caretIndex - 1;
                    }

                    if (IsIntellisenseResetKey(e))
                    {
                        intellisenseStartedAt++;
                    }

                    currentFilter = editorViewModel.TextDocument.GetText(intellisenseStartedAt, caretIndex - intellisenseStartedAt);

                    await editor.DoCompletionRequestAsync(caret.Line, caret.Column, currentFilter);

                    await Task.Factory.StartNew(() =>
                    {
                        lock (intellisenseLock)
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
                                        unfilteredCompletions.Add(CompletionDataViewModel.Create(result));
                                    }
                                    else
                                    {
                                        //currentCompletion.NumOverloads++;
                                    }
                                }
                            }
                        }
                    });

                    filteredResults = unfilteredCompletions.ToList();
                }
                else
                {
                    currentFilter = editorViewModel.TextDocument.GetText(intellisenseStartedAt, caretIndex - intellisenseStartedAt);

                    await Task.Factory.StartNew(() =>
                    {
                        lock (intellisenseLock)
                        {
                            filteredResults = unfilteredCompletions.Where((c) => c.Title.ToLower().Contains(currentFilter.ToLower())).ToList();
                        }
                    });
                }

                CompletionDataViewModel suggestion = null;
                if (currentFilter != string.Empty)
                {
                    IEnumerable<CompletionDataViewModel> newSelectedCompletions = null;

                    lock (intellisenseLock)
                    {
                        filteredResults = newSelectedCompletions = filteredResults.Where((s) => s.Title.StartsWith(currentFilter));   // try find exact match case sensitive

                        if (newSelectedCompletions.Count() == 0)
                        {
                            newSelectedCompletions = filteredResults.Where((s) => s.Title.ToLower().StartsWith(currentFilter.ToLower()));   // try find non-case sensitve match
                        }
                    }

                    if (newSelectedCompletions.Count() == 0)
                    {
                        suggestion = noSelectedCompletion;
                    }
                    else
                    {
                        var newSelectedCompletion = newSelectedCompletions.FirstOrDefault();

                        suggestion = newSelectedCompletion;
                    }
                }
                else
                {
                    suggestion = noSelectedCompletion;
                }                

                if (filteredResults?.Count() > 0)
                {
                    Model = filteredResults.ToList();                   

                    SelectedCompletion = suggestion;

                    // Triggers display update.
                    IsVisible = true;
                }
                else
                {
                    Close();
                }
            }
            else if (IsAllowedNonFilterModificationKey(e))
            {
                // do nothing
            }
            else
            {
                Close();
            }
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

    }
}
