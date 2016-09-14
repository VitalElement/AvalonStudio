using Avalonia.Input;
using Avalonia.Threading;
using AvalonStudio.Extensibility;
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
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AvalonStudio.Controls
{
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

        private readonly char[] searchChars = { '(', ')', '.', ':', '-', '>', ';' };
        private readonly char[] completionChars = { '.', ':', ';', '-', ' ', '(', '=', '+', '*', '/', '%', '|', '&', '!', '^' };
        private readonly char[] triggerChars = { '.', '>', ':' };


        private bool IsTriggerChar(char currentChar)
        {
            bool result = false;

            if (char.IsLetter(currentChar) || triggerChars.Contains(currentChar))
            {
                result = true;
            }

            return result;
        }

        private bool IsLanguageSpecificTriggerChar(char currentChar)
        {
            return triggerChars.Contains(currentChar);
        }

        private bool IsSearchChar(char currentChar)
        {
            return searchChars.Contains(currentChar);
        }

        private bool IsCompletionChar(char currentChar)
        {
            return completionChars.Contains(currentChar);
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
        }

        public void Dispose()
        {
            editor = null;
        }

        ~IntellisenseManager()
        {
            editor = null;
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

            UpdateFilter(caretIndex);
        }

        private void CloseIntellisense()
        {
            intellisenseStartedAt = editor.CaretIndex;
            currentFilter = string.Empty;

            Dispatcher.UIThread.InvokeAsync(() =>
            {
                intellisenseControl.SelectedCompletion = noSelectedCompletion;
                intellisenseControl.IsVisible = false;
            });
        }

        private void UpdateFilter(int caretIndex)
        {
            currentFilter = editor.TextDocument.GetText(intellisenseStartedAt, caretIndex - intellisenseStartedAt).Replace(".", string.Empty);

            CompletionDataViewModel suggestion = null;

            var filteredResults = unfilteredCompletions as IEnumerable<CompletionDataViewModel>;

            if (currentFilter != string.Empty)
            {
                filteredResults = unfilteredCompletions.Where(c => c.Title.ToLower().Contains(currentFilter.ToLower()));

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
                    });

                    Dispatcher.UIThread.InvokeAsync(() => intellisenseControl.IsVisible = true);

                    Dispatcher.UIThread.InvokeTaskAsync(() =>
                    {
                        intellisenseControl.SelectedCompletion = null;
                    });

                    Dispatcher.UIThread.InvokeTaskAsync(() =>
                    {
                        intellisenseControl.SelectedCompletion = suggestion;
                    });
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

                    editor.TextDocument.BeginUpdate();

                    editor.TextDocument.Replace(intellisenseStartedAt, caretIndex - intellisenseStartedAt - offset,
                            intellisenseControl.SelectedCompletion.Title);

                    caretIndex = intellisenseStartedAt + intellisenseControl.SelectedCompletion.Title.Length + offset;

                    editor.CaretIndex = caretIndex;

                    editor.TextDocument.EndUpdate();

                    CloseIntellisense();
                }
            }

            return result;
        }

        public Task SetCursor(int index, int line, int column, List<UnsavedFile> unsavedFiles)
        {
            return intellisenseJobRunner.InvokeAsync(() =>
            {
                if (!isProcessingKey)
                {
                    if (intellisenseControl.IsVisible)
                    {
                        CloseIntellisense();
                    }

                    var codeCompleteTask = languageService.CodeCompleteAtAsync(file, line, column, unsavedFiles);
                    codeCompleteTask.Wait();
                    SetCompletionData(codeCompleteTask.Result);
                }
            });
        }

        public async void OnTextInput(TextInputEventArgs e, int caretIndex, int line, int column)
        {
            if (e.Text.Length == 1)
            {
                char currentChar = e.Text[0];

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
                            var curChar = editor.TextDocument.GetCharAt(offset++);

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

                    char behindBehindCaretChar = editor.TextDocument.GetCharAt(caretIndex - 2);

                    if (behindBehindCaretChar.IsWhiteSpace() && behindBehindCaretChar != '\0')
                    {
                        currentWord = editor.GetPreviousWordAtIndex(editor.CaretIndex - 1);
                    }
                    else
                    {
                        currentWord = editor.GetWordAtIndex(editor.CaretIndex - 1);
                    }
                    
                    var signatureHelp = await languageService.SignatureHelp(file, EditorModel.UnsavedFiles.FirstOrDefault(), EditorModel.UnsavedFiles, line, column, editor.CaretIndex, currentWord);

                    if (signatureHelp != null)
                    {
                        completionAssistant.PushMethod(signatureHelp);
                    }
                }
                else if (currentChar == ')')
                {
                    completionAssistant.PopMethod();
                }

                if (IsCompletionChar(currentChar))
                {
                    DoComplete(true);
                }

                if (currentChar.IsWhiteSpace() || IsSearchChar(currentChar))
                {
                    await SetCursor(caretIndex, line, column, EditorModel.UnsavedFiles);
                }

                if (IsTriggerChar(currentChar) || IsLanguageSpecificTriggerChar(currentChar))
                {
                    await intellisenseJobRunner.InvokeAsync(async () =>
                    {
                        await Dispatcher.UIThread.InvokeTaskAsync(async () =>
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
                                await SetCursor(caretIndex, line, column, EditorModel.UnsavedFiles);
                            }

                            isProcessingKey = intellisenseControl.IsVisible;
                        });

                    });
                }
            }
        }

        public async void OnKeyDown(KeyEventArgs e, int caretIndex, int line, int column)
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
                }

                if (capturedOnKeyDown == Key.Enter)
                {
                    DoComplete(false);
                    e.Handled = true;
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
                else if (intellisenseControl.IsVisible)
                {
                    await intellisenseJobRunner.InvokeAsync(() => CloseIntellisense());
                }
            }

            if (!intellisenseControl.IsVisible)
            {
                await SetCursor(caretIndex, line, column, EditorModel.UnsavedFiles);
            }            
        }

        public void OnKeyUp(KeyEventArgs e, int caretIndex, int line, int column)
        {
            isProcessingKey = false;

            if (intellisenseControl.IsVisible && caretIndex < intellisenseStartedAt)
            {
                CloseIntellisense();

                SetCursor(caretIndex, line, column, EditorModel.UnsavedFiles);
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
