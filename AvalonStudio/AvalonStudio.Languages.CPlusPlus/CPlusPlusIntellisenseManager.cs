namespace AvalonStudio.Languages.CPlusPlus
{
    using Languages;
    using Avalonia.Input;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using TextEditor.Document;
    using Utils;
    using ViewModels;
    using Extensibility.Threading;
    using Avalonia.Threading;
    using Projects;

    public class CompletionAdviceState
    {
        public CompletionAdviceState()
        {
            
        }

        public int SelectedIndex { get; set; }
        public int BracketOpenedAt { get; set; }
        public List<Symbol> Symbols { get; set; }
    }

    class CPlusPlusIntellisenseManager
    {
        public CPlusPlusIntellisenseManager(ILanguageService languageService, IIntellisenseControl intellisenseControl, ICompletionAdviceControl completionAdviceControl, ISourceFile file, TextEditor.TextEditor editor)
        {
            this.languageService = languageService;
            this.intellisenseControl = intellisenseControl;
            this.completionAdviceControl = completionAdviceControl;            
            this.file = file;
            this.editor = editor;

            completionAdviceStack = new Stack<CompletionAdviceState>();
        }

        private ISourceFile file;
        private TextEditor.TextEditor editor;
        private IIntellisenseControl intellisenseControl;
        private ICompletionAdviceControl completionAdviceControl;
        private ILanguageService languageService;
        private string currentFilter = string.Empty;
        private int intellisenseStartedAt;
        private Stack<CompletionAdviceState> completionAdviceStack;
        private CompletionAdviceState currentAdvice;

        private bool IsIntellisenseOpenKey(KeyEventArgs e)
        {
            bool result = false;

            result = (e.Key >= Key.D0 && e.Key <= Key.D9 && e.Modifiers == InputModifiers.None) || (e.Key >= Key.A && e.Key <= Key.Z) || (e.Key >= Key.NumPad0 && e.Key <= Key.NumPad9) || (e.Key == Key.Oem1);

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
                    case Key.Oem1:
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

        private bool IsCompletionKey(KeyEventArgs e)
        {
            bool result = false;

            if (e.Modifiers == InputModifiers.None)
            {
                switch (e.Key)
                {
                    case Key.Enter:
                    case Key.Tab:
                    case Key.OemPeriod:
                    case Key.OemMinus:
                    case Key.Space:
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

        public void OnKeyDown(KeyEventArgs e)
        {
            if (intellisenseControl.IsVisible && e.Modifiers == InputModifiers.None)
            {
                if (!IsCompletionKey(e))
                {
                    switch (e.Key)
                    {
                        case Key.Down:
                            {
                                int index = intellisenseControl.CompletionData.IndexOf(intellisenseControl.SelectedCompletion);

                                if (index < intellisenseControl.CompletionData.Count - 1)
                                {
                                    intellisenseControl.SelectedCompletion = intellisenseControl.CompletionData[index + 1];
                                }

                                e.Handled = true;
                            }
                            break;

                        case Key.Up:
                            {
                                int index = intellisenseControl.CompletionData.IndexOf(intellisenseControl.SelectedCompletion);

                                if (index > 0)
                                {
                                    intellisenseControl.SelectedCompletion = intellisenseControl.CompletionData[index - 1];
                                }

                                e.Handled = true;
                            }
                            break;
                    }
                }
            }
        }

        private CompletionDataViewModel noSelectedCompletion = new CompletionDataViewModel(null);

        private List<CompletionDataViewModel> unfilteredCompletions = new List<CompletionDataViewModel>();

        private async Task<bool> DoComplete(bool includeLastChar)
        {
            bool result = false;

            if (intellisenseControl.CompletionData.Count > 0 && intellisenseControl.SelectedCompletion != noSelectedCompletion)
            {
                int offset = 0;

                if (includeLastChar)
                {
                    offset = 1;
                }

                result = true;

                await Dispatcher.UIThread.InvokeTaskAsync(() =>
                {
                    editor.TextDocument.Replace(intellisenseStartedAt, editor.CaretIndex - intellisenseStartedAt - offset, intellisenseControl.SelectedCompletion.Title);
                    editor.CaretIndex = intellisenseStartedAt + intellisenseControl.SelectedCompletion.Title.Length + offset;
                });
            }

            return result;
        }

        public async Task CompleteOnKeyDown(KeyEventArgs e)
        {
            char behindCaretChar = '\0';

            if (editor.CaretIndex > 0)
            {
                await Dispatcher.UIThread.InvokeTaskAsync(() =>
                {
                    behindCaretChar = editor.TextDocument.GetCharAt(editor.CaretIndex - 1);
                });
            }

            if (behindCaretChar == '(')
            {
                string word = string.Empty;
                await Dispatcher.UIThread.InvokeTaskAsync(() =>
                {
                    word = editor.GetWordAtIndex(editor.CaretIndex - 1);
                });

                var symbols = languageService.GetSymbols(file, new List<UnsavedFile>(), word);

                if (symbols.Count() > 0)
                {
                    var adviceState = new CompletionAdviceState();
                    adviceState.BracketOpenedAt = editor.CaretIndex;
                    adviceState.Symbols = symbols;

                    if(currentAdvice != null)
                    {
                        completionAdviceStack.Push(currentAdvice);
                    }

                    currentAdvice = adviceState;

                    Dispatcher.UIThread.InvokeAsync(() =>
                    {
                        completionAdviceControl.Count = currentAdvice.Symbols.Count;
                        completionAdviceControl.SelectedIndex = 0;
                        completionAdviceControl.Symbol = currentAdvice.Symbols[currentAdvice.SelectedIndex];
                        completionAdviceControl.IsVisible = true;
                    });
                }
            }

            if (intellisenseControl.IsVisible && e.Modifiers == InputModifiers.None)
            {
                if (IsCompletionKey(e))
                {
                    if (e.Key == Key.Enter)
                    {
                        await DoComplete(false);
                    }
                    else
                    {
                        await DoComplete(true);
                    }
                }
            }
        }

        private async Task CompleteOnKeyUp()
        {
            if (intellisenseControl.IsVisible)
            {
                char behindCaretChar = '\0';
                char behindBehindCaretChar = '\0';

                if (editor.CaretIndex > 0)
                {
                    behindCaretChar = editor.TextDocument.GetCharAt(editor.CaretIndex - 1);
                }

                if (editor.CaretIndex > 1)
                {
                    behindBehindCaretChar = editor.TextDocument.GetCharAt(editor.CaretIndex - 2);
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
                        await DoComplete(true);
                        return;
                }
            }
        }

        private void Close()
        {
            intellisenseControl.SelectedCompletion = noSelectedCompletion;
            intellisenseStartedAt = editor.CaretIndex;
            intellisenseControl.IsVisible = false;
            currentFilter = string.Empty;
        }

        private TextLocation GetTextLocation(int index)
        {
            return editor.TextDocument.GetLocation(index);
        }


        public async Task OnKeyUp(KeyEventArgs e)
        {
            bool isVisible = intellisenseControl.IsVisible;

            if (IsIntellisenseKey(e))
            {
                var caretIndex = editor.CaretIndex;

                if (caretIndex <= intellisenseStartedAt)
                {
                    await Dispatcher.UIThread.InvokeTaskAsync(() =>
                    {
                        Close();
                    });
                    return;
                }

                if (IsIntellisenseResetKey(e))
                {
                    isVisible = false;  // We dont actually want to hide, so use backing field.
                    //currentFilter = string.Empty;
                }

                await Dispatcher.UIThread.InvokeTaskAsync(async () =>
                {
                    await CompleteOnKeyUp();
                });

                IEnumerable<CompletionDataViewModel> filteredResults = null;

                if (!intellisenseControl.IsVisible && (IsIntellisenseOpenKey(e) || IsIntellisenseResetKey(e)))
                {
                    TextLocation caret = new TextLocation();

                    char behindCaretChar = '\0';
                    char behindBehindCaretChar = '\0';

                    if (editor.CaretIndex > 0)
                    {
                        await Dispatcher.UIThread.InvokeTaskAsync(() =>
                        {
                            behindCaretChar = editor.TextDocument.GetCharAt(editor.CaretIndex - 1);
                        });
                    }

                    if (editor.CaretIndex > 1)
                    {
                        await Dispatcher.UIThread.InvokeTaskAsync(() =>
                        {
                            behindBehindCaretChar = editor.TextDocument.GetCharAt(editor.CaretIndex - 2);
                        });
                    }

                    if (behindCaretChar == ':' && behindBehindCaretChar == ':')
                    {
                        intellisenseStartedAt = caretIndex;
                    }
                    else if (behindCaretChar == '>' || behindBehindCaretChar == ':' || behindBehindCaretChar == '>')
                    {
                        intellisenseStartedAt = caretIndex - 1;
                    }
                    else
                    {
                        await Dispatcher.UIThread.InvokeTaskAsync(() =>
                        {
                            intellisenseStartedAt = TextUtilities.GetNextCaretPosition(editor.TextDocument, caretIndex, TextUtilities.LogicalDirection.Backward, TextUtilities.CaretPositioningMode.WordStart);
                        });
                    }

                    if (IsIntellisenseResetKey(e))
                    {
                        intellisenseStartedAt++;
                    }

                    await Dispatcher.UIThread.InvokeTaskAsync(() =>
                    {
                        currentFilter = editor.TextDocument.GetText(intellisenseStartedAt, caretIndex - intellisenseStartedAt);

                        caret = GetTextLocation(intellisenseStartedAt);
                    });

                    var codeCompletionResults = await intellisenseControl.DoCompletionRequestAsync(caret.Line, caret.Column);

                    unfilteredCompletions.Clear();

                    if (codeCompletionResults != null)
                    {
                        foreach (var result in codeCompletionResults.Completions)
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

                    filteredResults = unfilteredCompletions;
                }
                else
                {
                    if (intellisenseStartedAt != -1)
                    {
                        await Dispatcher.UIThread.InvokeTaskAsync(() =>
                        {
                            currentFilter = editor.TextDocument.GetText(intellisenseStartedAt, caretIndex - intellisenseStartedAt);
                        });
                    }
                    else
                    {
                        currentFilter = string.Empty;
                    }

                    filteredResults = unfilteredCompletions.Where((c) => c.Title.ToLower().Contains(currentFilter.ToLower()));
                }

                CompletionDataViewModel suggestion = null;

                if (currentFilter != string.Empty)
                {
                    IEnumerable<CompletionDataViewModel> newSelectedCompletions = null;

                    newSelectedCompletions = filteredResults.Where((s) => s.Title.StartsWith(currentFilter));   // try find exact match case sensitive

                    if (newSelectedCompletions.Count() == 0)
                    {
                        newSelectedCompletions = filteredResults.Where((s) => s.Title.ToLower().StartsWith(currentFilter.ToLower()));   // try find non-case sensitve match
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
                    if (filteredResults?.Count() == 1 && filteredResults.First().Title == currentFilter)
                    {
                        await Dispatcher.UIThread.InvokeTaskAsync(() =>
                        {
                            Close();
                        });
                    }
                    else
                    {
                        var list = filteredResults.ToList();                        
                        
                        await Dispatcher.UIThread.InvokeTaskAsync(() =>
                        {
                            
                            intellisenseControl.CompletionData = list;

                            intellisenseControl.SelectedCompletion = suggestion;

                            intellisenseControl.IsVisible = true;
                        });
                    }
                }
                else
                {
                    await Dispatcher.UIThread.InvokeTaskAsync(() =>
                    {
                        Close();
                    });
                }
            }
            else if (IsAllowedNonFilterModificationKey(e))
            {
                // do nothing
            }
            else
            {
                if (intellisenseControl.IsVisible && IsCompletionKey(e))
                {
                    e.Handled = true;
                }

                await Dispatcher.UIThread.InvokeTaskAsync(() =>
                {
                    Close();
                });
            }
        }
    }
}
