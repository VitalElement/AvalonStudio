namespace AvalonStudio.Controls
{
    using Avalonia.Input;
    using Avalonia.Threading;
    using AvalonStudio.Controls.Editor;
    using AvalonStudio.Controls.Editor.Completion;
    using AvalonStudio.Controls.Editor.Snippets;
    using AvalonStudio.Documents;
    using AvalonStudio.Extensibility;
    using AvalonStudio.Extensibility.Languages.CompletionAssistance;
    using AvalonStudio.Extensibility.Studio;
    using AvalonStudio.Extensibility.Threading;
    using AvalonStudio.Languages;
    using AvalonStudio.Projects;
    using AvalonStudio.Utils;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;

    internal class IntellisenseManager : IDisposable
    {        
        private IStudio _studio;
        private IConsole _console;
        private readonly ILanguageService languageService;
        private readonly ISourceFile file;
        private readonly IIntellisenseControl intellisenseControl;
        private readonly ICompletionAssistant completionAssistant;
        private ITextEditor editor;
        private IList<CodeSnippet> _snippets;
        private CancellationTokenSource _cancelRunners;
        private Action<int> _onSetSignatureHelpPosition;

        private bool _requestingData;
        private bool _hidden; // i.e. can be technically open, but hidden awaiting completion data..
        private bool _justOpened;
        private string currentFilter = string.Empty;
        private int _lastIndex = -1;

        private readonly List<CompletionDataViewModel> unfilteredCompletions = new List<CompletionDataViewModel>();
        private CompletionDataViewModel recommendedCompletion;
        private int? _recommendedInsertionOffset;
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

        public IntellisenseManager(ITextEditor editor, IIntellisenseControl intellisenseControl, ICompletionAssistant completionAssistant, ILanguageService languageService, ISourceFile file, Action<int> onSetSignatureHelpPosition)
        {
            intellisenseJobRunner = new JobRunner();
            intellisenseQueryRunner = new JobRunner(1);

            _cancelRunners = new CancellationTokenSource();

            _onSetSignatureHelpPosition = onSetSignatureHelpPosition;

            Task.Factory.StartNew(() => { intellisenseJobRunner.RunLoop(_cancelRunners.Token); });
            Task.Factory.StartNew(() => { intellisenseQueryRunner.RunLoop(_cancelRunners.Token); });

            this.intellisenseControl = intellisenseControl;
            this.completionAssistant = completionAssistant;
            this.languageService = languageService;
            this.file = file;
            this.editor = editor;
            _hidden = true;

            _studio = IoC.Get<IStudio>();
            _console = IoC.Get<IConsole>();

            var snippetManager = IoC.Get<SnippetManager>();
            _snippets = snippetManager.GetSnippets(languageService, file.Project?.Solution, file.Project);
        }

        public bool IncludeSnippets { get; set; } = true;

        public void Dispose()
        {
            editor = null;

            _cancelRunners.Cancel();
        }

        ~IntellisenseManager()
        {
            editor = null;
        }

        private void SetCompletionData(CodeCompletionResults completionData)
        {
            if (_studio.DebugMode)
            {
                _console.WriteLine(completionData.Contexts.ToString());
            }

            if (completionData.StartOffset.HasValue)
            {
                _recommendedInsertionOffset = completionData.StartOffset;
            }
            else
            {
                _recommendedInsertionOffset = null;
            }

            if (!completionData.Contexts.HasFlag(CompletionContext.NaturalLanguage) && (completionData.Contexts != CompletionContext.Unexposed || completionData.Contexts == CompletionContext.Unknown))
            {
                if (IncludeSnippets && !(completionData.Contexts.HasFlag(CompletionContext.ArrowMemberAccess) || completionData.Contexts.HasFlag(CompletionContext.DotMemberAccess)))
                {
                    InsertSnippets(completionData.Completions);
                }

                recommendedCompletion = null;

                foreach (var result in completionData.Completions)
                {
                    CompletionDataViewModel currentCompletion = null;

                    currentCompletion = unfilteredCompletions.BinarySearch(c => c.FilterText, result.FilterText);

                    if (currentCompletion == null)
                    {
                        currentCompletion = new CompletionDataViewModel(result);
                        unfilteredCompletions.Add(currentCompletion);
                    }
                    else
                    {
                        currentCompletion.Overloads++;
                    }

                    if (result.SelectionBehavior != CompletionItemSelectionBehavior.Default)
                    {
                        if (recommendedCompletion == null || (recommendedCompletion != null && result.Priority > recommendedCompletion.Priority))
                        {
                            recommendedCompletion = currentCompletion;
                        }
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

                if (_studio.DebugMode)
                {
                    _console.WriteLine($"Open Intellisense {caretIndex}");
                }

                UpdateFilter(caretIndex);
            }
            else
            {
                if (_studio.DebugMode)
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
                if (_studio.DebugMode)
                {
                    _console.WriteLine("Filtering");
                }

                var wordStart = DocumentUtilities.FindPrevSymbolNameStart(editor.Document, caretIndex);

                if (wordStart >= 0)
                {
                    result = true;
                    currentFilter = editor.Document.GetText(wordStart, caretIndex - wordStart).Replace(".", string.Empty).Replace("->", string.Empty).Replace("::", string.Empty);

                    if (currentFilter.Any(s => char.IsWhiteSpace(s)))
                    {
                        currentFilter = "";
                    }
                }
                else
                {
                    currentFilter = string.Empty;
                }

                if (_studio.DebugMode)
                {
                    _console.WriteLine($"Filter: {currentFilter}");
                }

                CompletionDataViewModel suggestion = null;

                var filteredResults = unfilteredCompletions as IEnumerable<CompletionDataViewModel>;

                if (currentFilter != string.Empty)
                {
                    filteredResults = unfilteredCompletions.Where(c => c != null && c.FilterText.ToLower().Contains(currentFilter.ToLower()));

                    IEnumerable<CompletionDataViewModel> newSelectedCompletions = null;

                    // try find exact match case sensitive
                    newSelectedCompletions = filteredResults.Where(s => s.FilterText.StartsWith(currentFilter));

                    if (newSelectedCompletions.Count() == 0)
                    {
                        newSelectedCompletions = filteredResults.Where(s => s.FilterText.ToLower().StartsWith(currentFilter.ToLower()));
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
                    suggestion = recommendedCompletion;

                    if (suggestion != null)
                    {
                        _hidden = false;
                    }
                }

                if (filteredResults?.Count() > 0)
                {
                    if (filteredResults?.Count() == 1 && filteredResults.First().FilterText == currentFilter)
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

        private bool DoComplete(bool includeInputText, bool searchAndReopen, int caretOffset = 0, string inputText = "")
        {
            int caretIndex = -1;

            caretIndex = editor.Offset;

            var result = false;
            bool hiddenOverride = false;

            if (intellisenseControl.CompletionData.Count > 0 && intellisenseControl.SelectedCompletion != null && intellisenseControl.SelectedCompletion != null)
            {
                result = true;

                using (editor.Document.RunUpdate())
                {
                    int wordStart = _recommendedInsertionOffset.HasValue ? _recommendedInsertionOffset.Value : editor.Document.GetIntellisenseStartPosition(caretIndex + caretOffset, languageService.IsValidIdentifierCharacter);

                    if (caretIndex - wordStart >= 0 && intellisenseControl.SelectedCompletion != null)
                    {
                        if(intellisenseControl.SelectedCompletion.Model.InsertionText.Contains(inputText))
                        {
                            includeInputText = false;
                        }

                        editor.Document.Replace(wordStart, caretIndex - wordStart,
                                intellisenseControl.SelectedCompletion.Model.InsertionText);

                        var length = intellisenseControl.SelectedCompletion.Model.InsertionText.Length;

                        if (intellisenseControl.SelectedCompletion.Model.RecommendedCaretPosition.HasValue)
                        {
                            length = intellisenseControl.SelectedCompletion.Model.RecommendedCaretPosition.Value;
                        }

                        caretIndex = wordStart + length;

                        if(includeInputText && inputText != string.Empty)
                        {
                            editor.Document.Insert(caretIndex, inputText);
                            caretIndex += inputText.Length;
                        }

                        if (_studio.DebugMode)
                        {
                            _console.WriteLine($"Completed: {caretIndex}");
                        }

                        if (intellisenseControl.SelectedCompletion.Model.RecommendImmediateSuggestions || searchAndReopen)
                        {
                            hiddenOverride = true;
                            _justOpened = true;
                        }
                    }
                }

                editor.Offset = caretIndex;
            }

            CloseIntellisense();

            _hidden = !hiddenOverride;

            var location = editor.Document.GetLocation(editor.Offset);
            SetCursor(editor.Offset, location.Line, location.Column, CodeEditor.UnsavedFiles.ToList(), false);

            return result;
        }

        private void InsertSnippets(List<CodeCompletionData> sortedResults)
        {
            foreach (var snippet in _snippets)
            {
                var current = sortedResults.InsertSortedExclusive(new CodeCompletionData(snippet.Name, snippet.Name, snippet.Name) { Kind = CodeCompletionKind.Snippet, BriefComment = snippet.Description });

                if (current != null && current.Kind != CodeCompletionKind.Snippet)
                {
                    current.Kind = CodeCompletionKind.Snippet;
                }
            }
        }

        private async Task<bool> PushToSignatureHelp(string currentWord, int offset)
        {
            SignatureHelp signatureHelp = null;

            var unsavedFiles = CodeEditor.UnsavedFiles;

            await intellisenseJobRunner.InvokeAsync(() =>
            {
                var task = languageService.SignatureHelp(unsavedFiles, offset, currentWord);
                task.Wait();

                signatureHelp = task.Result;
            });

            if (signatureHelp != null)
            {
                _onSetSignatureHelpPosition(signatureHelp.Offset);
                completionAssistant.PushMethod(signatureHelp);

                return true;
            }

            return false;
        }

        private async Task UpdateActiveParameterAndVisibility(bool canTrigger = false)
        {
            if (completionAssistant.IsVisible)
            {
                if (completionAssistant.CurrentSignatureHelp != null)
                {
                    var indexStack = new Stack<int>();

                    int index = 0;
                    int level = -1;
                    int offset = completionAssistant.Stack.Last().Offset;

                    SignatureHelp currentHelpStack = null;

                    while (offset < editor.Offset)
                    {
                        var curChar = '\0';

                        curChar = editor.Document.GetCharAt(offset++);

                        switch (curChar)
                        {
                            case ',':
                                index++;
                                break;

                            case '(':
                                level++;

                                if ((level + 1) > completionAssistant.Stack.Count)
                                {
                                    if(!await PushToSignatureHelp("", offset))
                                    {
                                        level--;
                                        break;
                                    }
                                }

                                currentHelpStack = completionAssistant.Stack[completionAssistant.Stack.Count - level - 1];

                                if (level >= 0)
                                {
                                    indexStack.Push(index);
                                    index = 0;
                                }
                                break;

                            case ')':
                                if (indexStack.Count > 0)
                                {
                                    index = indexStack.Pop();
                                }

                                level--;

                                if (level >= 0)
                                {
                                    currentHelpStack = completionAssistant.Stack[completionAssistant.Stack.Count - level - 1];
                                }
                                else
                                {
                                    currentHelpStack = null;
                                }
                                break;
                        }
                    }

                    if(currentHelpStack != null)
                    {
                        _onSetSignatureHelpPosition(currentHelpStack.Offset);
                        completionAssistant.SelectStack(currentHelpStack);
                        completionAssistant.SetParameterIndex(index);
                    }
                    else
                    {
                        completionAssistant.Close();
                    }
                }
            }
            else if (canTrigger)
            {
                var currentChar = editor.Document.GetCharAt(editor.Offset - 1);

                if ((currentChar == '(' || (currentChar == ',') && (completionAssistant.CurrentSignatureHelp == null || completionAssistant.CurrentSignatureHelp.Offset != editor.Offset)))
                {
                    string currentWord = string.Empty;

                    char behindBehindCaretChar = '\0';

                    behindBehindCaretChar = editor.Document.GetCharAt(editor.Offset - 2);

                    if (behindBehindCaretChar.IsWhiteSpace() && behindBehindCaretChar != '\0')
                    {
                        currentWord = editor.Document.GetPreviousWordAtIndex(editor.Offset - 1);
                    }
                    else
                    {
                        currentWord = editor.Document.GetWordAtIndex(editor.Offset - 1);
                    }

                    await PushToSignatureHelp(currentWord, editor.Offset);
                }
                else if (currentChar == ')')
                {
                    completionAssistant.PopMethod();
                }
            }
        }

        public void SetCursor(int index, int line, int column, List<UnsavedFile> unsavedFiles, bool canUpdateSignature = true)
        {
            if (_lastIndex != index)
            {
                _lastIndex = index;

                if (canUpdateSignature)
                {
                    UpdateActiveParameterAndVisibility().GetAwaiter();
                }

                if (!intellisenseControl.IsVisible)
                {
                    unfilteredCompletions.Clear();

                    if (_studio.DebugMode)
                    {
                      _console.WriteLine($"Set Cursor {index}");

                        if(index != 270)
                        {

                        }
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
                            if (_studio.DebugMode)
                            {
                                _console.WriteLine($"Query Language Service {index}, {line}, {column}");
                            }

                            var task = languageService.CodeCompleteAtAsync(index, line, column, unsavedFiles, previousChar);
                            task.Wait();

                            result = task.Result;
                        }).Wait();

                        if (result != null)
                        {
                            Dispatcher.UIThread.InvokeAsync(() =>
                            {
                                if (_studio.DebugMode)
                                {
                                    _console.WriteLine($"Set Completion Data {_hidden}");
                                }

                                SetCompletionData(result);

                                _requestingData = false;

                                if (unfilteredCompletions.Count > 0 && _justOpened)
                                {
                                    if (editor != null)
                                    {
                                        UpdateFilter(editor.Offset, false);
                                        intellisenseControl.IsVisible = !_hidden;
                                    }
                                }
                                else
                                {
                                    _hidden = true;
                                }
                            });
                        }
                    });
                }
                else
                {
                    UpdateFilter(editor.Offset, false);
                }
            }
        }

        public void OnTextInput(TextInputEventArgs e, int caretIndex, int line, int column)
        {
            if (e.Text.Length == 1)
            {
                char currentChar = e.Text[0];
                char previousChar = '\0';

                if (caretIndex >= 2)
                {
                    previousChar = editor.Document.GetCharAt(caretIndex - 2);
                }

                if (intellisenseControl.IsVisible)
                {
                    if (IsCompletionChar(currentChar))
                    {
                        DoComplete(true, IsSearchChar(currentChar) && IsTriggerChar(currentChar, previousChar, !_hidden), - 1, e.Text);
                    }
                }

                if (completionAssistant.IsVisible)
                {
                    if (caretIndex < completionAssistant.CurrentSignatureHelp.Offset)
                    {
                        completionAssistant.PopMethod();
                    }
                }

                if (!_justOpened &&(currentChar.IsWhiteSpace() || IsSearchChar(currentChar)))
                {
                    SetCursor(caretIndex, line, column, CodeEditor.UnsavedFiles, false);
                }

                if (IsTriggerChar(currentChar, previousChar, !_hidden))
                {
                    if (_hidden)
                    {
                        OpenIntellisense(currentChar, previousChar, caretIndex);
                    }
                    else if (!_justOpened)
                    {
                        if (!UpdateFilter(caretIndex))
                        {
                            // CloseIntellisense();
                            SetCursor(caretIndex, line, column, CodeEditor.UnsavedFiles, false);
                        }
                    }
                }

                Dispatcher.UIThread.InvokeAsync(async () =>
                {
                    await UpdateActiveParameterAndVisibility(true);
                });
            }
        }

        public void OnKeyDown(KeyEventArgs e, int caretIndex, int line, int column)
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
                        DoComplete(false, false);
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

        public void OnKeyUp(KeyEventArgs e, int caretIndex, int line, int column)
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