namespace AvalonStudio.Controls
{
    using Avalonia.Input;
    using Avalonia.Threading;
    using AvaloniaEdit.Document;
    using AvalonStudio.Controls.Standard.CodeEditor;
    using AvalonStudio.Controls.Standard.CodeEditor.Completion;
    using AvalonStudio.Controls.Standard.CodeEditor.Snippets;
    using AvalonStudio.Extensibility;
    using AvalonStudio.Extensibility.Languages.CompletionAssistance;
    using AvalonStudio.Extensibility.Threading;
    using AvalonStudio.Languages;
    using AvalonStudio.Projects;
    using AvalonStudio.Shell;
    using AvalonStudio.Utils;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;

    internal class IntellisenseManager : IDisposable
    {
        private IShell _shell;
        private IConsole _console;
        private readonly ILanguageService languageService;
        private readonly ISourceFile file;
        private readonly IIntellisenseControl intellisenseControl;
        private readonly ICompletionAssistant completionAssistant;
        private AvaloniaEdit.TextEditor editor;
        private IList<CodeSnippet> _snippets;
        private CancellationTokenSource _cancelRunners;

        private bool _requestingData;
        private bool _hidden; // i.e. can be technically open, but hidden awaiting completion data..
        private bool _justOpened;
        private string currentFilter = string.Empty;

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

            _cancelRunners = new CancellationTokenSource();

            Task.Factory.StartNew(() => { intellisenseJobRunner.RunLoop(_cancelRunners.Token); });
            Task.Factory.StartNew(() => { intellisenseQueryRunner.RunLoop(_cancelRunners.Token); });

            this.intellisenseControl = intellisenseControl;
            this.completionAssistant = completionAssistant;
            this.languageService = languageService;
            this.file = file;
            this.editor = editor;

            this.editor.LostFocus += Editor_LostFocus;
            _hidden = true;

            _shell = IoC.Get<IShell>();
            _console = IoC.Get<IConsole>();

            var snippetManager = IoC.Get<SnippetManager>();
            _snippets = snippetManager.GetSnippets(languageService, file.Project?.Solution, file.Project);
        }

        public bool IncludeSnippets { get; set; } = true;

        public void Dispose()
        {
            editor.LostFocus -= Editor_LostFocus;
            editor = null;

            _cancelRunners.Cancel();
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
            if (_shell.DebugMode)
            {
                _console.WriteLine(completionData.Contexts.ToString());
            }

            if (!completionData.Contexts.HasFlag(CompletionContext.NaturalLanguage) && (completionData.Contexts != CompletionContext.Unexposed || completionData.Contexts == CompletionContext.Unknown))
            {
                if (IncludeSnippets && !(completionData.Contexts.HasFlag(CompletionContext.ArrowMemberAccess) || completionData.Contexts.HasFlag(CompletionContext.DotMemberAccess)))
                {
                    InsertSnippets(completionData.Completions);
                }

                foreach (var result in completionData.Completions)
                {
                    CompletionDataViewModel currentCompletion = null;

                    currentCompletion = unfilteredCompletions.BinarySearch(c => c.Title, result.DisplayText);

                    if (currentCompletion == null)
                    {
                        unfilteredCompletions.Add(new CompletionDataViewModel(result));
                    }
                    else
                    {
                        currentCompletion.Overloads++;
                    }
                }
            }
        }

        private void OpenIntellisense(char currentChar, char previousChar, int caretIndex)
        {
            _justOpened = true;

            if (_hidden)
            {
                _hidden = false;

                if (_shell.DebugMode)
                {
                    _console.WriteLine($"Open Intellisense {caretIndex}");
                }

                UpdateFilter(caretIndex);
            }
            else
            {
                if (_shell.DebugMode)
                {
                    _console.WriteLine($"Reopen Intellisense {caretIndex}");
                }

                currentFilter = "";
            }
        }

        public void CloseIntellisense()
        {
          currentFilter = string.Empty;

            intellisenseControl.SelectedCompletion = null;
            _hidden = true;
            intellisenseControl.IsVisible = false;
        }

        private bool UpdateFilter(int caretIndex, bool allowVisiblityChanges = true)
        {
            bool result = false;

            if (!_requestingData)
            {
                if (_shell.DebugMode)
                {
                    _console.WriteLine("Filtering");
                }

                var wordStart = DocumentUtilities.FindPrevSymbolNameStart(editor.Document, caretIndex);

                if (wordStart >= 0)
                {
                    result = true;
                    currentFilter = editor.Document.GetText(wordStart, caretIndex - wordStart).Replace(".", string.Empty).Replace("->", string.Empty).Replace("::", string.Empty);

                    if(currentFilter.Any(s=>char.IsWhiteSpace(s)))
                    {
                        currentFilter = "";
                    }
                }
                else
                {
                    currentFilter = string.Empty;
                }

                if (_shell.DebugMode)
                {
                    _console.WriteLine($"Filter: {currentFilter}");
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
                        suggestion = null;
                    }
                    else
                    {
                        var newSelectedCompletion = newSelectedCompletions.FirstOrDefault();

                        suggestion = newSelectedCompletion;
                    }
                }
                else
                {
                    suggestion = null;
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

                        intellisenseControl.SelectedCompletion = null;
                        intellisenseControl.CompletionData = list;

                        Dispatcher.UIThread.InvokeAsync(() =>
                        {
                            intellisenseControl.SelectedCompletion = suggestion;
                        });

                        if (allowVisiblityChanges)
                        {
                            _hidden = false;

                            if (!_requestingData && !_hidden)
                            {
                                intellisenseControl.IsVisible = true;
                            }
                        }
                    }
                }
                else
                {
                    intellisenseControl.SelectedCompletion = null;
                }
            }

            return result;
        }

        private bool DoComplete(bool includeLastChar, int caretOffset = 0)
        {
            int caretIndex = -1;

            caretIndex = editor.CaretOffset;

            var result = false;
            bool hiddenOverride = false;

            if (intellisenseControl.CompletionData.Count > 0 && intellisenseControl.SelectedCompletion != null && intellisenseControl.SelectedCompletion != null)
            {
                result = true;

                var offset = 0;

                if (includeLastChar)
                {
                    offset = 1;
                }

                editor.Document.BeginUpdate();

                int wordStart = editor.Document.GetIntellisenseStartPosition(caretIndex + caretOffset, languageService.IsValidIdentifierCharacter);

                if (caretIndex - wordStart - offset >= 0 && intellisenseControl.SelectedCompletion != null)
                {
                    editor.Document.Replace(wordStart, caretIndex - wordStart - offset,
                            intellisenseControl.SelectedCompletion.Model.InsertionText);

                    var length = intellisenseControl.SelectedCompletion.Model.InsertionText.Length;

                    if(intellisenseControl.SelectedCompletion.Model.RecommendedCaretPosition.HasValue)
                    {
                        length = intellisenseControl.SelectedCompletion.Model.RecommendedCaretPosition.Value;
                    }

                    caretIndex = wordStart + length + offset;

                    editor.CaretOffset = caretIndex;

                    if (intellisenseControl.SelectedCompletion.Model.RecommendImmediateSuggestions)
                    {
                        hiddenOverride = true;
                    }
                }

                editor.Document.EndUpdate();
            }

            CloseIntellisense();

            _hidden = !hiddenOverride;

            var location = editor.Document.GetLocation(editor.CaretOffset);
            SetCursor(editor.CaretOffset, location.Line, location.Column, CodeEditor.UnsavedFiles.ToList());

            return result;
        }

        private void InsertSnippets(List<CodeCompletionData> sortedResults)
        {
            foreach (var snippet in _snippets)
            {
                var current = sortedResults.InsertSortedExclusive(new CodeCompletionData (snippet.Name, snippet.Name) { Kind = CodeCompletionKind.Snippet, BriefComment = snippet.Description });

                if (current != null && current.Kind != CodeCompletionKind.Snippet)
                {
                    current.Kind = CodeCompletionKind.Snippet;
                }
            }
        }

        public void SetCursor(int index, int line, int column, List<UnsavedFile> unsavedFiles)
        {
            if (!intellisenseControl.IsVisible)
            {
               unfilteredCompletions.Clear();

                if (_shell.DebugMode)
                {
                    _console.WriteLine("Set Cursor");
                }

                _requestingData = true;

                char previousChar = '\0';

                if (index >= 1)
                {
                    previousChar = editor.Document.GetCharAt(index - 1);
                }

                intellisenseQueryRunner.InvokeAsync(() =>
                {
                    CodeCompletionResults result = null;
                    intellisenseJobRunner.InvokeAsync(() =>
                    {
                        if (_shell.DebugMode)
                        {
                            _console.WriteLine($"Query Language Service {index}, {line}, {column}");
                        }

                        var task = languageService.CodeCompleteAtAsync(file, index, line, column, unsavedFiles, previousChar);
                        task.Wait();

                        result = task.Result;
                    }).Wait();

                    Dispatcher.UIThread.InvokeAsync(() =>
                    {
                        if (_shell.DebugMode)
                        {
                            _console.WriteLine($"Set Completion Data {_hidden}");
                        }

                        SetCompletionData(result);

                        _requestingData = false;

                        if (unfilteredCompletions.Count > 0)
                        {
                            UpdateFilter(editor.CaretOffset, false);
                            intellisenseControl.IsVisible = !_hidden;
                        }
                        else
                        {
                            _hidden = true;
                        }
                    });
                });
            }
            else
            {
                UpdateFilter(editor.CaretOffset, false);
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

                    if (intellisenseControl.IsVisible)
                    {
                        if (IsCompletionChar(currentChar))
                        {
                            if (currentChar.IsWhiteSpace())
                            {
                                DoComplete(true);
                            }
                            else
                            {
                                DoComplete(true, -1);
                            }
                        }
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
                        SetCursor(caretIndex, line, column, CodeEditor.UnsavedFiles.ToList());
                    }

                    if (caretIndex >= 2)
                    {
                        previousChar = editor.Document.GetCharAt(caretIndex - 2);
                    }

                    if (IsTriggerChar(currentChar, previousChar, !_hidden))
                    {
                        if (_hidden)
                        {
                            OpenIntellisense(currentChar, previousChar, caretIndex);
                        }
                        else
                        {
                            if(!UpdateFilter(caretIndex))
                            {
                               // CloseIntellisense();
                                SetCursor(caretIndex, line, column, CodeEditor.UnsavedFiles.ToList());
                            }
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
                            UpdateFilter(caretIndex - 1);
                            break;

                        case Key.Tab:
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
                        e.Handled = true;
                    }
                    else if (!_hidden)
                    {
                        CloseIntellisense();
                        e.Handled = true;
                    }
                }
            }
        }

        public void OnKeyUp(KeyEventArgs e, int caretIndex, int line, int column)
        {
            if (e.Source == editor.TextArea)
            {
                if (!_justOpened && !_hidden && currentFilter == "" && e.Key != Key.LeftShift && e.Key != Key.RightShift && e.Key != Key.Up && e.Key != Key.Down && e.Key != Key.Enter)
                {
                    CloseIntellisense();

                    SetCursor(caretIndex, line, column, CodeEditor.UnsavedFiles.ToList());
                }
                else if (e.Key == Key.Enter)
                {
                    SetCursor(caretIndex, line, column, CodeEditor.UnsavedFiles.ToList());
                }

                _justOpened = false;
            }
        }
    }
}