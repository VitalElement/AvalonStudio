namespace AvalonStudio.Controls
{
    using Avalonia.Input;
    using Avalonia.Threading;
    using AvalonStudio.Extensibility.Languages.CompletionAssistance;
    using AvalonStudio.Extensibility.Threading;
    using AvalonStudio.Languages;
    using AvalonStudio.Languages.ViewModels;
    using AvalonStudio.Projects;
    using AvalonStudio.TextEditor.Document;
    using AvalonStudio.Utils;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;

    class IntellisenseManager
    {
        private readonly ILanguageService languageService;
        private readonly ISourceFile file;
        private readonly IIntellisenseControl intellisenseControl;
        private readonly ICompletionAssistant completionAssistant;
        private TextEditor.TextEditor editor;
        private bool isProcessingKey;
        private int intellisenseStartedAt;
        private string currentFilter = string.Empty;
        private readonly CompletionDataViewModel noSelectedCompletion = new CompletionDataViewModel(null);
        private readonly List<CompletionDataViewModel> unfilteredCompletions = new List<CompletionDataViewModel>();
        private Key capturedOnKeyDown;
        private readonly JobRunner intellisenseJobRunner;

        private bool IsTriggerChar(char currentChar)
        {
            bool result = false;

            if (char.IsLetter(currentChar) || languageService.IntellisenseTriggerCharacters.Contains(currentChar))
            {
                result = true;
            }

            return result;
        }

        private bool IsLanguageSpecificTriggerChar(char currentChar)
        {
            return languageService.IntellisenseTriggerCharacters.Contains(currentChar);
        }

        private bool IsSearchChar(char currentChar)
        {
            return languageService.IntellisenseSearchCharacters.Contains(currentChar);
        }

        private bool IsCompletionChar(char currentChar)
        {
            return languageService.IntellisenseCompleteCharacters.Contains(currentChar);
        }

        public IntellisenseManager(TextEditor.TextEditor editor, IIntellisenseControl intellisenseControl, ICompletionAssistant completionAssistant, ILanguageService languageService, ISourceFile file)
        {
            intellisenseJobRunner = new JobRunner();

            Task.Factory.StartNew(() => { intellisenseJobRunner.RunLoop(new CancellationToken()); });

            this.intellisenseControl = intellisenseControl;
            this.completionAssistant = completionAssistant;
            this.languageService = languageService;
            this.file = file;
            this.editor = editor;

            this.editor.LostFocus += Editor_LostFocus;
        }

        public void Dispose()
        {
            editor.LostFocus -= Editor_LostFocus;
            editor = null;
        }

        ~IntellisenseManager()
        {
            editor = null;
        }

        private void Editor_LostFocus(object sender, Avalonia.Interactivity.RoutedEventArgs e)
        {
            intellisenseJobRunner.InvokeAsync(() =>
            {
                CloseIntellisense();
            });

            completionAssistant.Close();
        }

        private void SetCompletionData(List<CodeCompletionData> completionData)
        {
            unfilteredCompletions.Clear();

            foreach (var result in completionData)
            {
                CompletionDataViewModel currentCompletion = null;

                currentCompletion = unfilteredCompletions.BinarySearch(c => c.Title, result.Suggestion);

                if (currentCompletion == null)
                {
                    unfilteredCompletions.Add(CompletionDataViewModel.Create(result));
                }
                else
                {
                    currentCompletion.Overloads++;
                }
            }
        }

        private void OpenIntellisense(char currentChar, int caretIndex)
        {
            Dispatcher.UIThread.InvokeTaskAsync(() =>
            {
                if (caretIndex > 1)
                {
                    if (IsLanguageSpecificTriggerChar(currentChar))
                    {
                        intellisenseStartedAt = caretIndex;
                    }
                    else
                    {
                        intellisenseStartedAt = TextUtilities.GetNextCaretPosition(editor.TextDocument, caretIndex, TextUtilities.LogicalDirection.Backward, TextUtilities.CaretPositioningMode.WordStart);
                    }
                }
                else
                {
                    intellisenseStartedAt = 1;
                }
            }).Wait();

            UpdateFilter(caretIndex);
        }

        private void CloseIntellisense()
        {
            intellisenseStartedAt = editor.CaretIndex;
            currentFilter = string.Empty;

            Dispatcher.UIThread.InvokeTaskAsync(() =>
            {
                intellisenseControl.SelectedCompletion = noSelectedCompletion;
                intellisenseControl.IsVisible = false;
            }).Wait();
        }

        private void UpdateFilter(int caretIndex)
        {
            Dispatcher.UIThread.InvokeTaskAsync(() =>
            {
                currentFilter = editor.TextDocument.GetText(intellisenseStartedAt, caretIndex - intellisenseStartedAt).Replace(".", string.Empty);
            }).Wait();

            CompletionDataViewModel suggestion = null;

            var filteredResults = unfilteredCompletions as IEnumerable<CompletionDataViewModel>;

            if (currentFilter != string.Empty)
            {
                filteredResults = unfilteredCompletions.Where(c => c != null && c.Title.ToLower().Contains(currentFilter.ToLower()));

                IEnumerable<CompletionDataViewModel> newSelectedCompletions = null;

                newSelectedCompletions = filteredResults.Where(s => s.Title.StartsWith(currentFilter));
                // try find exact match case sensitive

                if (newSelectedCompletions.Count() == 0)
                {
                    newSelectedCompletions = filteredResults.Where(s => s.Title.ToLower().StartsWith(currentFilter.ToLower()));
                    // try find non-case sensitve match
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
                    CloseIntellisense();
                }
                else
                {
                    var list = filteredResults.ToList();

                    Dispatcher.UIThread.InvokeTaskAsync(() =>
                    {
                        intellisenseControl.CompletionData = list;
                    }).Wait();

                    Dispatcher.UIThread.InvokeTaskAsync(() => intellisenseControl.IsVisible = true).Wait();

                    Dispatcher.UIThread.InvokeTaskAsync(() =>
                    {
                        intellisenseControl.SelectedCompletion = null;
                    }).Wait();

                    Dispatcher.UIThread.InvokeTaskAsync(() =>
                    {
                        intellisenseControl.SelectedCompletion = suggestion;
                    }).Wait();
                }
            }
            else
            {
                CloseIntellisense();
            }
        }

        private bool DoComplete(bool includeLastChar)
        {
            var caretIndex = editor.CaretIndex;

            var result = false;

            if (intellisenseControl.CompletionData.Count > 0 && intellisenseControl.SelectedCompletion != noSelectedCompletion && intellisenseControl.SelectedCompletion != null)
            {
                result = true;

                if (intellisenseStartedAt <= caretIndex)
                {
                    var offset = 0;

                    if (includeLastChar)
                    {
                        offset = 1;
                    }

                    Dispatcher.UIThread.InvokeTaskAsync(() =>
                    {
                        editor.TextDocument.BeginUpdate();

                        if (caretIndex - intellisenseStartedAt - offset > 0)
                        {
                            editor.TextDocument.Replace(intellisenseStartedAt, caretIndex - intellisenseStartedAt - offset,
                                    intellisenseControl.SelectedCompletion.Title);

                            caretIndex = intellisenseStartedAt + intellisenseControl.SelectedCompletion.Title.Length + offset;

                            editor.CaretIndex = caretIndex;
                        }

                        editor.TextDocument.EndUpdate();
                    }).Wait();

                    CloseIntellisense();
                }
            }

            return result;
        }

        public void SetCursor(int index, int line, int column, List<UnsavedFile> unsavedFiles, bool invokeOnRunner = true)
        {
            var action = new Action(() =>
            {
                if (!isProcessingKey)
                {
                    if (intellisenseControl.IsVisible)
                    {
                        CloseIntellisense();
                    }

                    var codeCompleteTask = languageService.CodeCompleteAtAsync(file, index, line, column, unsavedFiles);
                    codeCompleteTask.Wait();
                    SetCompletionData(codeCompleteTask.Result);
                }
            });

            if (invokeOnRunner)
            {
                intellisenseJobRunner.InvokeAsync(action);
            }
            else
            {
                action();
            }
        }

        public void OnTextInput(TextInputEventArgs e, int caretIndex, int line, int column)
        {
            if (e.Source == editor)
            {
                intellisenseJobRunner.InvokeAsync(() =>
                {
                    if (e.Text.Length == 1)
                    {
                        char currentChar = e.Text[0];

                        if (completionAssistant.IsVisible)
                        {
                            if (caretIndex < completionAssistant.CurrentSignatureHelp.Offset)
                            {
                                Dispatcher.UIThread.InvokeTaskAsync(() =>
                                {
                                    completionAssistant.PopMethod();
                                }).Wait();
                            }

                            if (completionAssistant.CurrentSignatureHelp != null)
                            {
                                int index = 0;
                                int level = 0;
                                int offset = completionAssistant.CurrentSignatureHelp.Offset;

                                while (offset < caretIndex)
                                {
                                    var curChar = '\0';

                                    Dispatcher.UIThread.InvokeTaskAsync(() =>
                                    {
                                        curChar = editor.TextDocument.GetCharAt(offset++);
                                    }).Wait();

                                    switch (curChar)
                                    {
                                        case ',':
                                            if (level == 0)
                                            {
                                                index++;
                                            }
                                            break;

                                        case '(':
                                            level++;
                                            break;

                                        case ')':
                                            level--;
                                            break;
                                    }
                                }

                                completionAssistant.SetParameterIndex(index);
                            }
                        }

                        if (currentChar == '(' && (completionAssistant.CurrentSignatureHelp == null || completionAssistant.CurrentSignatureHelp.Offset != editor.CaretIndex))
                        {
                            string currentWord = string.Empty;

                            char behindBehindCaretChar = '\0';

                            Dispatcher.UIThread.InvokeTaskAsync(() =>
                            {
                                behindBehindCaretChar = editor.TextDocument.GetCharAt(caretIndex - 2);
                            }).Wait();

                            if (behindBehindCaretChar.IsWhiteSpace() && behindBehindCaretChar != '\0')
                            {
                                Dispatcher.UIThread.InvokeTaskAsync(() =>
                                {
                                    currentWord = editor.GetPreviousWordAtIndex(editor.CaretIndex - 1);
                                }).Wait();
                            }
                            else
                            {
                                Dispatcher.UIThread.InvokeTaskAsync(() =>
                                {
                                    currentWord = editor.GetWordAtIndex(editor.CaretIndex - 1);
                                }).Wait();
                            }

                            var signatureHelpTask = languageService.SignatureHelp(file, EditorModel.UnsavedFiles.FirstOrDefault(), EditorModel.UnsavedFiles.ToList(), line, column, editor.CaretIndex, currentWord);
                            signatureHelpTask.Wait();

                            var signatureHelp = signatureHelpTask.Result;

                            if (signatureHelp != null)
                            {
                                Dispatcher.UIThread.InvokeTaskAsync(() =>
                                {
                                    completionAssistant.PushMethod(signatureHelp);
                                }).Wait();
                            }
                        }
                        else if (currentChar == ')')
                        {
                            Dispatcher.UIThread.InvokeTaskAsync(() =>
                            {
                                completionAssistant.PopMethod();
                            }).Wait();
                        }

                        if (IsCompletionChar(currentChar))
                        {
                            DoComplete(true);
                        }

                        if (currentChar.IsWhiteSpace() || IsSearchChar(currentChar))
                        {
                            SetCursor(caretIndex, line, column, EditorModel.UnsavedFiles.ToList(), false);
                        }

                        if (IsTriggerChar(currentChar) || IsLanguageSpecificTriggerChar(currentChar))
                        {
                            if (!intellisenseControl.IsVisible)
                            {
                                OpenIntellisense(currentChar, caretIndex);
                            }
                            else if (caretIndex > intellisenseStartedAt)
                            {
                                UpdateFilter(caretIndex);
                            }
                            else
                            {
                                CloseIntellisense();
                                SetCursor(caretIndex, line, column, EditorModel.UnsavedFiles.ToList(), false);
                            }

                            isProcessingKey = intellisenseControl.IsVisible;
                        }
                    }
                });
            }
        }

        public void OnKeyDown(KeyEventArgs e, int caretIndex, int line, int column)
        {
            if (e.Source == editor)
            {
                capturedOnKeyDown = e.Key;

                if (intellisenseControl.IsVisible)
                {
                    switch (capturedOnKeyDown)
                    {
                        case Key.Down:
                            {
                                var index = intellisenseControl.CompletionData.IndexOf(intellisenseControl.SelectedCompletion);

                                if (index < intellisenseControl.CompletionData.Count - 1)
                                {
                                    intellisenseControl.SelectedCompletion = intellisenseControl.CompletionData[index + 1];
                                }

                                e.Handled = true;
                            }
                            break;

                        case Key.Up:
                            {
                                var index = intellisenseControl.CompletionData.IndexOf(intellisenseControl.SelectedCompletion);

                                if (index > 0)
                                {
                                    intellisenseControl.SelectedCompletion = intellisenseControl.CompletionData[index - 1];
                                }

                                e.Handled = true;
                            }
                            break;

                        case Key.Back:
                            if (caretIndex - 1 >= intellisenseStartedAt)
                            {
                                intellisenseJobRunner.InvokeAsync(() =>
                                {
                                    UpdateFilter(caretIndex - 1);
                                });
                            }
                            break;

                        case Key.Enter:
                            intellisenseJobRunner.InvokeAsync(() =>
                            {
                                DoComplete(false);
                            });

                            e.Handled = true;
                            break;
                    }
                }

                if (completionAssistant.IsVisible)
                {
                    if (!e.Handled)
                    {
                        switch (e.Key)
                        {
                            case Key.Down:
                                {
                                    completionAssistant.IncrementSignatureIndex();
                                    e.Handled = true;
                                }
                                break;

                            case Key.Up:
                                {
                                    completionAssistant.DecrementSignatureIndex();
                                    e.Handled = true;
                                }
                                break;
                        }
                    }
                }

                if (e.Key == Key.Escape)
                {
                    if (completionAssistant.IsVisible)
                    {
                        intellisenseJobRunner.InvokeAsync(() =>
                        {
                            Dispatcher.UIThread.InvokeAsync(() =>
                            completionAssistant.Close());
                        });
                    }
                    else if (intellisenseControl.IsVisible)
                    {
                        intellisenseJobRunner.InvokeAsync(() =>
                        {
                            CloseIntellisense();
                        });
                    }
                }
            }
        }

        public void OnKeyUp(KeyEventArgs e, int caretIndex, int line, int column)
        {
            if (e.Source == editor)
            {
                intellisenseJobRunner.InvokeAsync(() =>
                {
                    isProcessingKey = false;

                    if (intellisenseControl.IsVisible && caretIndex < intellisenseStartedAt)
                    {
                        CloseIntellisense();

                        SetCursor(caretIndex, line, column, EditorModel.UnsavedFiles.ToList(), false);
                    }
                });
            }
        }
    }

    static class KeyExtensions
    {
        public static bool IsModifierKey(this Key key)
        {
            bool result = false;

            switch (key)
            {
                case Key.LeftShift:
                case Key.LeftAlt:
                case Key.LeftCtrl:
                case Key.RightAlt:
                case Key.RightCtrl:
                case Key.RightShift:
                    result = true;
                    break;
            }

            return result;
        }

        public static bool IsNavigationKey(this Key key)
        {
            bool result = false;

            switch (key)
            {
                case Key.Left:
                case Key.Up:
                case Key.Right:
                case Key.Down:
                case Key.Escape:
                    result = true;
                    break;

                default:
                    // Do nothing
                    break;
            }

            return result;
        }

        public static bool IsTriggerKey(this Key key)
        {
            bool result = false;

            if (!key.IsNavigationKey())
            {
                if (key >= Key.A && key <= Key.Z)
                {
                    result = true;
                }
            }

            return result;
        }

        public static bool IsLanguageSpecificTriggerKey(this Key key)
        {
            bool result = false;

            switch (key)
            {
                case Key.OemPeriod:
                    result = true;
                    break;
            }

            return result;
        }

        public static bool IsSearchKey(this Key key)
        {
            bool result = false;

            if (!key.IsNavigationKey() && !key.IsTriggerKey())
            {
                switch (key)
                {
                    case Key.OemPeriod:
                    case Key.Space:
                    case Key.OemOpenBrackets:
                    case Key.OemCloseBrackets:
                        result = true;
                        break;
                }
            }

            return result;
        }
    }
}
