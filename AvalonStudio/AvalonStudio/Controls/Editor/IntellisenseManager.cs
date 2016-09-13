using Avalonia.Input;
using AvalonStudio.Extensibility;
using AvalonStudio.Languages;
using AvalonStudio.Languages.ViewModels;
using AvalonStudio.Projects;
using AvalonStudio.TextEditor.Document;
using AvalonStudio.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AvalonStudio.Controls
{
    class IntellisenseManager
    {
        private ILanguageService languageService;
        private ISourceFile file;
        private IIntellisenseControl intellisenseControl;
        private TextEditor.TextEditor editor;
        private bool isProcessingKey;
        private int intellisenseStartedAt;
        private string currentFilter = string.Empty;
        private readonly CompletionDataViewModel noSelectedCompletion = new CompletionDataViewModel(null);
        private readonly List<CompletionDataViewModel> unfilteredCompletions = new List<CompletionDataViewModel>();
        TaskCompletionSource<List<CompletionDataViewModel>> completionDataReady;


        public IntellisenseManager(TextEditor.TextEditor editor, IIntellisenseControl intellisenseControl, ILanguageService languageService, ISourceFile file)
        {
            this.intellisenseControl = intellisenseControl;
            this.languageService = languageService;
            this.file = file;
            this.editor = editor;
        }

        public async void SetCursor(int index, int line, int column, List<UnsavedFile> unsavedFiles)
        {
            if (!isProcessingKey)
            {
                if (intellisenseControl.IsVisible)
                {
                    CloseIntellisense();
                }

                if (completionDataReady != null)
                {
                    await completionDataReady.Task;
                }

                completionDataReady = new TaskCompletionSource<List<CompletionDataViewModel>>();
                await SetCompletionData(await languageService.CodeCompleteAtAsync(file, line, column, unsavedFiles));

                IoC.Get<IConsole>().WriteLine("Updated Completion Data");
            }
        }

        private async Task SetCompletionData(List<CodeCompletionData> completionData)
        {
            TaskCompletionSource<List<CompletionDataViewModel>> completionDataCompiled = new TaskCompletionSource<List<CompletionDataViewModel>>();

            await Task.Factory.StartNew(() =>
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

                completionDataCompiled.SetResult(unfilteredCompletions);
                completionDataReady.SetResult(unfilteredCompletions);
            });

            intellisenseControl.CompletionData = await completionDataCompiled.Task;
        }

        private async Task OpenIntellisense(Key key, int caretIndex)
        {
            if(completionDataReady!=null)
            {
                await completionDataReady.Task;
            }

            if (caretIndex > 1)
            {
                if (key.IsLanguageSpecificTriggerKey())
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
            intellisenseControl.SelectedCompletion = noSelectedCompletion;
            intellisenseStartedAt = editor.CaretIndex;
            intellisenseControl.IsVisible = false;
            currentFilter = string.Empty;
        }

        private void UpdateFilter(int caretIndex)
        {
            currentFilter = editor.TextDocument.GetText(intellisenseStartedAt, caretIndex - intellisenseStartedAt).Replace(".",string.Empty);

            IoC.Get<IConsole>().WriteLine($"Filter: {currentFilter}, caret: {caretIndex}");
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

                    intellisenseControl.IsVisible = true;

                    intellisenseControl.CompletionData = list;

                    intellisenseControl.SelectedCompletion = null;
                    intellisenseControl.SelectedCompletion = suggestion;
                }
            }
            else
            {
                CloseIntellisense();
            }
        }

        private bool DoComplete()
        {
            var caretIndex = editor.CaretIndex;

            var result = false;

            if (intellisenseControl.CompletionData.Count > 0 && intellisenseControl.SelectedCompletion != noSelectedCompletion && intellisenseControl.SelectedCompletion != null)
            {
                result = true;

                if (intellisenseStartedAt <= caretIndex)
                {
                    editor.TextDocument.BeginUpdate();

                    editor.TextDocument.Replace(intellisenseStartedAt, caretIndex - intellisenseStartedAt,
                            intellisenseControl.SelectedCompletion.Title);

                    caretIndex = intellisenseStartedAt + intellisenseControl.SelectedCompletion.Title.Length ;

                    editor.CaretIndex = caretIndex;

                    editor.TextDocument.EndUpdate();

                    CloseIntellisense();
                }
            }

            return result;
        }

        public void OnKeyDown(KeyEventArgs e, int caretIndex, int line, int column)
        {
            if (!e.Key.IsNavigationKey() && !e.Key.IsSearchKey() && !e.Key.IsCompletionKey() || (intellisenseControl.IsVisible && e.Key == Key.Back))
            {
                isProcessingKey = true;

                switch (e.Key)
                {
                    case Key.Escape:
                        CloseIntellisense();
                        break;
                }
            }
            else
            {
                if(intellisenseControl.IsVisible)
                {
                    if (e.Key.IsCompletionKey())
                    {
                        if (e.Key == Key.Enter)
                        {
                            DoComplete();
                            e.Handled = true;
                        }
                        else
                        {
                            DoComplete();
                        }
                    }

                    switch (e.Key)
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
                }
            }

            IoC.Get<IConsole>().WriteLine("OnKeyDown");
        }

        public async void OnKeyUp(KeyEventArgs e, int caretIndex, int line, int column)
        {
            isProcessingKey = false;

            if (e.Key.IsTriggerKey() || e.Key.IsLanguageSpecificTriggerKey())
            {
                if (!intellisenseControl.IsVisible)
                {
                    await OpenIntellisense(e.Key, caretIndex);
                }
                else if (caretIndex > intellisenseStartedAt)
                {
                    UpdateFilter(caretIndex);

                    intellisenseControl.IsVisible = true;
                }
                else
                {
                    CloseIntellisense();
                    SetCursor(caretIndex, line, column, EditorModel.UnsavedFiles);
                }
            }

            IoC.Get<IConsole>().WriteLine("OnKeyUp");
        }
    }

    static class KeyExtensions
    {
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

            if (!key.IsNavigationKey() && !key.IsCompletionKey())
            {
                result = true;
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

        public static bool IsSearchKey (this Key key)
        {
            bool result = false;

            if (!key.IsNavigationKey() && !key.IsTriggerKey())
            {
                switch (key)
                {
                    case Key.OemPeriod:
                    case Key.Space:
                        result = true;
                        break;
                }
            }
            
            return result;
        }

        public static bool IsCompletionKey(this Key key)
        {
            bool result = false;

            switch(key)
            {
                case Key.Enter:
                case Key.Space:
                case Key.OemPeriod:
                    result = true;
                    break;
            }

            return result;
        }
    }
}
