namespace AvalonStudio.Controls
{
    using Avalonia.Input;
    using Avalonia.Threading;
    using AvaloniaEdit.Document;
    using AvalonStudio.Controls.Standard.CodeEditor;
    using AvalonStudio.Extensibility.Languages.CompletionAssistance;
    using AvalonStudio.Extensibility.Threading;
    using AvalonStudio.Languages;
    using AvalonStudio.Languages.ViewModels;
    using AvalonStudio.Projects;
    using AvalonStudio.Utils;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;

    internal class IntellisenseManager
    {
        private readonly ILanguageService languageService;
        private readonly ISourceFile file;
        private readonly IIntellisenseControl intellisenseControl;
        private readonly ICompletionAssistant completionAssistant;
        private AvaloniaEdit.TextEditor editor;

        private bool _requestingData;
        private bool _hidden; // i.e. can be technically open, but hidden awaiting completion data..
        private bool _justOpened;
        private int intellisenseStartedAt;
        private string currentFilter = string.Empty;
        private readonly CompletionDataViewModel noSelectedCompletion = new CompletionDataViewModel(null);
        private readonly List<CompletionDataViewModel> unfilteredCompletions = new List<CompletionDataViewModel>();
        private Key capturedOnKeyDown;
        private readonly JobRunner intellisenseJobRunner;
        private readonly JobRunner intellisenseQueryRunner;

        private bool IsTriggerChar(char currentChar, char previousChar, bool isVisible)
        {
            bool result = false;

            if ((char.IsLetter(currentChar) || (isVisible && char.IsLetterOrDigit(currentChar))) || languageService.CanTriggerIntellisense(currentChar, previousChar))
            {
                result = true;
            }

            return result;
        }

        private bool IsLanguageSpecificTriggerChar(char currentChar, char previousChar)
        {
            return languageService.CanTriggerIntellisense(currentChar, previousChar);
        }

        private bool IsSearchChar(char currentChar)
        {
            return languageService.IntellisenseSearchCharacters.Contains(currentChar);
        }

        private bool IsCompletionChar(char currentChar)
        {
            return languageService.IntellisenseCompleteCharacters.Contains(currentChar);
        }

        public IntellisenseManager(AvaloniaEdit.TextEditor editor, IIntellisenseControl intellisenseControl, ICompletionAssistant completionAssistant, ILanguageService languageService, ISourceFile file)
        {
            intellisenseJobRunner = new JobRunner();
            intellisenseQueryRunner = new JobRunner(1);

            Task.Factory.StartNew(() => { intellisenseJobRunner.RunLoop(new CancellationToken()); });
            Task.Factory.StartNew(() => { intellisenseQueryRunner.RunLoop(new CancellationToken()); });

            this.intellisenseControl = intellisenseControl;
            this.completionAssistant = completionAssistant;
            this.languageService = languageService;
            this.file = file;
            this.editor = editor;

            this.editor.LostFocus += Editor_LostFocus;
            _hidden = true;
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
            CloseIntellisense();

            completionAssistant?.Close();
        }

        private void SetCompletionData(CodeCompletionResults completionData)
        {
            unfilteredCompletions.Clear();

            foreach (var result in completionData.Completions)
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

            // todo clear hidden?
        }

        private void OpenIntellisense(char currentChar, char previousChar, int caretIndex)
        {
            _justOpened = true;

            if (caretIndex > 1)
            {
                if (IsLanguageSpecificTriggerChar(currentChar, previousChar))
                {
                    intellisenseStartedAt = caretIndex;
                }
                else
                {
                    intellisenseStartedAt = TextUtilities.GetNextCaretPosition(editor.Document, caretIndex, LogicalDirection.Backward, CaretPositioningMode.WordStart);
                }
            }
            else
            {
                intellisenseStartedAt = 1;
            }

            UpdateFilter(caretIndex);
        }

        private void CloseIntellisense()
        {
            currentFilter = string.Empty;

            if (editor != null)
            {
                intellisenseStartedAt = editor.CaretOffset;
            }

            intellisenseControl.SelectedCompletion = noSelectedCompletion;
            _hidden = true;
            intellisenseControl.IsVisible = false;
        }

        private void UpdateFilter(int caretIndex)
        {
            if (caretIndex > intellisenseStartedAt && intellisenseStartedAt > 0)
            {
                currentFilter = editor.Document.GetText(intellisenseStartedAt, caretIndex - intellisenseStartedAt).Replace(".", string.Empty);
            }
            else
            {
                currentFilter = string.Empty;
            }

            CompletionDataViewModel suggestion = null;

            var filteredResults = unfilteredCompletions as IEnumerable<CompletionDataViewModel>;

            if (currentFilter != string.Empty)
            {
                filteredResults = unfilteredCompletions.Where(c => c != null && c.Title.ToLower().Contains(currentFilter.ToLower()));

                IEnumerable<CompletionDataViewModel> newSelectedCompletions = null;

                // try find exact match case sensitive
                newSelectedCompletions = filteredResults.Where(s => s.Title.StartsWith(currentFilter));

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

                    intellisenseControl.CompletionData = list;
                    intellisenseControl.SelectedCompletion = suggestion;

                    _hidden = false;
                    if (!_requestingData)
                    {
                        intellisenseControl.IsVisible = true;
                    }
                }
            }
            else
            {
                intellisenseControl.SelectedCompletion = noSelectedCompletion;
            }
        }

        private bool DoComplete(bool includeLastChar)
        {
            int caretIndex = -1;

            caretIndex = editor.CaretOffset;

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

                    editor.Document.BeginUpdate();

                    if (caretIndex - intellisenseStartedAt - offset >= 0 && intellisenseControl.SelectedCompletion != null)
                    {
                        editor.Document.Replace(intellisenseStartedAt, caretIndex - intellisenseStartedAt - offset,
                                intellisenseControl.SelectedCompletion.Title);

                        caretIndex = intellisenseStartedAt + intellisenseControl.SelectedCompletion.Title.Length + offset;

                        editor.CaretOffset = caretIndex;
                    }

                    editor.Document.EndUpdate();
                }
            }

            CloseIntellisense();

            return result;
        }

        public void SetCursor(int index, int line, int column, List<UnsavedFile> unsavedFiles, bool invokeOnRunner = true)
        {
            if (!intellisenseControl.IsVisible)
            {
                _requestingData = true;
                intellisenseQueryRunner.InvokeAsync(() =>
                {
                    if (invokeOnRunner)
                    {
                        intellisenseJobRunner.InvokeAsync(async () =>
                        {
                            var result = await languageService.CodeCompleteAtAsync(file, index, line, column, unsavedFiles);
                            SetCompletionData(result);
                        }).Wait();
                    }
                    else
                    {
                        intellisenseJobRunner.InvokeAsync(() =>
                        {
                            var task = languageService.CodeCompleteAtAsync(file, index, line, column, unsavedFiles);
                            task.Wait();
                            SetCompletionData(task.Result);
                        }).Wait();
                    }

                    Dispatcher.UIThread.InvokeAsync(() =>
                    {
                        _requestingData = false;
                        intellisenseControl.IsVisible = !_hidden;
                    });
                });
            }
        }

        public void OnTextInput(TextInputEventArgs e, int caretIndex, int line, int column)
        {
            if (e.Source == editor.TextArea)
            {
                if (e.Text.Length == 1)
                {
                    char currentChar = e.Text[0];
                    char previousChar = '\0';

                    if (IsCompletionChar(currentChar))
                    {
                        DoComplete(true);
                    }

                    if (completionAssistant.IsVisible)
                    {
                        if (caretIndex < completionAssistant.CurrentSignatureHelp.Offset)
                        {
                            completionAssistant.PopMethod();
                        }

                        if (completionAssistant.CurrentSignatureHelp != null)
                        {
                            int index = 0;
                            int level = 0;
                            int offset = completionAssistant.CurrentSignatureHelp.Offset;

                            while (offset < caretIndex)
                            {
                                var curChar = '\0';

                                curChar = editor.Document.GetCharAt(offset++);

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

                    if (currentChar.IsWhiteSpace() || IsSearchChar(currentChar))
                    {
                        SetCursor(caretIndex, line, column, Standard.CodeEditor.CodeEditor.UnsavedFiles.ToList(), false);
                    }

                    previousChar = editor.Document.GetCharAt(caretIndex - 2);

                    if (IsTriggerChar(currentChar, previousChar, !_hidden))
                    {
                        if (_hidden)
                        {
                            OpenIntellisense(currentChar, previousChar, caretIndex);
                        }
                        else if (caretIndex > intellisenseStartedAt)
                        {
                            UpdateFilter(caretIndex);
                        }
                        else
                        {
                            CloseIntellisense();
                            SetCursor(caretIndex, line, column, Standard.CodeEditor.CodeEditor.UnsavedFiles.ToList(), false);
                        }
                    }

                    Dispatcher.UIThread.InvokeAsync(async () =>
                    {
                        if (currentChar == '(' && (completionAssistant.CurrentSignatureHelp == null || completionAssistant.CurrentSignatureHelp.Offset != caretIndex))
                        {
                            string currentWord = string.Empty;

                            char behindBehindCaretChar = '\0';

                            behindBehindCaretChar = editor.Document.GetCharAt(caretIndex - 2);

                            if (behindBehindCaretChar.IsWhiteSpace() && behindBehindCaretChar != '\0')
                            {
                                currentWord = editor.GetPreviousWordAtIndex(caretIndex - 1);
                            }
                            else
                            {
                                currentWord = editor.GetWordAtIndex(caretIndex - 1);
                            }

                            SignatureHelp signatureHelp = null;

                            await intellisenseJobRunner.InvokeAsync(() =>
                            {
                                var task = languageService.SignatureHelp(file, Standard.CodeEditor.CodeEditor.UnsavedFiles.FirstOrDefault(), Standard.CodeEditor.CodeEditor.UnsavedFiles.ToList(), line, column, caretIndex, currentWord);
                                task.Wait();

                                signatureHelp = task.Result;
                            });

                            if (signatureHelp != null)
                            {
                                completionAssistant.PushMethod(signatureHelp);
                            }
                        }
                        else if (currentChar == ')')
                        {
                            completionAssistant.PopMethod();
                        }
                    });
                }
            }
        }

        public void OnKeyDown(KeyEventArgs e, int caretIndex, int line, int column)
        {
            if (e.Source == editor.TextArea)
            {
                capturedOnKeyDown = e.Key;

                if (!_hidden)
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
                                UpdateFilter(caretIndex - 1);
                            }
                            break;

                        case Key.Enter:
                            DoComplete(false);

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
                        completionAssistant.Close();
                    }
                    else if (!_hidden)
                    {
                        CloseIntellisense();
                    }
                }
            }
        }

        public void OnKeyUp(KeyEventArgs e, int caretIndex, int line, int column)
        {
            if (e.Source == editor.TextArea)
            {
                if (!_justOpened && !_hidden && caretIndex <= intellisenseStartedAt && e.Key != Key.LeftShift && e.Key != Key.RightShift && e.Key != Key.Up && e.Key != Key.Down)
                {
                    CloseIntellisense();

                    SetCursor(caretIndex, line, column, Standard.CodeEditor.CodeEditor.UnsavedFiles.ToList(), false);
                }

                _justOpened = false;

                if (e.Key == Key.Enter)
                {
                    SetCursor(caretIndex, line, column, Standard.CodeEditor.CodeEditor.UnsavedFiles.ToList(), false);
                }
            }
        }
    }
}