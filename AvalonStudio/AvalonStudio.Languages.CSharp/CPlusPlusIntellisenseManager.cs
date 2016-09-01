using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Avalonia.Input;
using Avalonia.Threading;
using AvalonStudio.Languages.ViewModels;
using AvalonStudio.Projects;
using AvalonStudio.TextEditor.Document;
using AvalonStudio.Utils;
using AvalonStudio.Extensibility;
using AvalonStudio.Extensibility.Languages.CompletionAssistance;

namespace AvalonStudio.Languages.CSharp
{
	public class CompletionAdviceState
	{
		public int SelectedIndex { get; set; }
		public int BracketOpenedAt { get; set; }
		public List<Symbol> Symbols { get; set; }
	}

	internal class CSharpIntellisenseManager
	{
		private readonly ICompletionAdviceControl completionAdviceControl;
		private readonly Stack<CompletionAdviceState> completionAdviceStack;
		private CompletionAdviceState currentAdvice;
		private string currentFilter = string.Empty;
		private readonly TextEditor.TextEditor editor;

		private readonly ISourceFile file;
		private readonly IIntellisenseControl intellisenseControl;
        private readonly ICompletionAssistant completionAssistant;
		private int intellisenseStartedAt;
        private int intellisenseOpenedAt;
        private int intellisenseEndsAt;
		private readonly ILanguageService languageService;

		private readonly CompletionDataViewModel noSelectedCompletion = new CompletionDataViewModel(null);

		private readonly List<CompletionDataViewModel> unfilteredCompletions = new List<CompletionDataViewModel>();

		public CSharpIntellisenseManager(ILanguageService languageService, IIntellisenseControl intellisenseControl,
			ICompletionAdviceControl completionAdviceControl, ICompletionAssistant completionAssistant, ISourceFile file, TextEditor.TextEditor editor)
		{
			this.languageService = languageService;
			this.intellisenseControl = intellisenseControl;
			this.completionAdviceControl = completionAdviceControl;
            this.completionAssistant = completionAssistant;
			this.file = file;
			this.editor = editor;

			completionAdviceStack = new Stack<CompletionAdviceState>();
		}

		private bool IsIntellisenseOpenKey(KeyEventArgs e)
		{
			var result = false;

			result = (e.Key >= Key.D0 && e.Key <= Key.D9 && e.Modifiers == InputModifiers.None) ||
			         (e.Key >= Key.A && e.Key <= Key.Z) || (e.Key >= Key.NumPad0 && e.Key <= Key.NumPad9) || (e.Key == Key.Oem1);

			return result;
		}

		private bool IsIntellisenseFilterModificationKey(KeyEventArgs e)
		{
			var result = false;

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
			var result = false;

			if (e.Modifiers == InputModifiers.None || (e.Modifiers == InputModifiers.Shift && e.Key == Key.End))
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
			var result = false;

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
                    case Key.Escape:
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
			var result = false;

			if (e.Key == Key.OemPeriod)
			{
				result = true;
			}

			return result;
		}

		public void OnKeyDown(KeyEventArgs e)
		{
            if(e.Modifiers == InputModifiers.None)
            {                
                if (intellisenseControl.IsVisible)
                {
                    if (!IsCompletionKey(e))
                    {
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

                if (completionAssistant.IsVisible)
                {
                    if (!e.Handled)
                    {
                        switch (e.Key)
                        {
                            case Key.Down:
                                {
                                    completionAssistant.IncrementOverloadIndex();
                                    e.Handled = true;
                                }
                                break;

                            case Key.Up:
                                {
                                    completionAssistant.DecrementOverloadIndex();
                                    e.Handled = true;
                                }
                                break;
                        }
                    }
                }                
            }
		}

		private async Task<bool> DoComplete(bool includeLastChar)
		{
			var caretIndex = editor.CaretIndex;

			var result = false;

			if (intellisenseControl.CompletionData.Count > 0 && intellisenseControl.SelectedCompletion != noSelectedCompletion && intellisenseControl.SelectedCompletion != null)
			{
				var offset = 0;

				if (includeLastChar)
				{
					offset = 1;
				}

				result = true;

                if (intellisenseStartedAt < caretIndex)
                {
                    await Dispatcher.UIThread.InvokeTaskAsync(() =>
                    {
                        editor.TextDocument.BeginUpdate();

                        if (caretIndex < (intellisenseEndsAt + (caretIndex - intellisenseOpenedAt)))
                        {                            
                            editor.TextDocument.Replace(intellisenseStartedAt, caretIndex - intellisenseStartedAt - offset,
                                intellisenseControl.SelectedCompletion.Title);

                            caretIndex = intellisenseStartedAt + intellisenseControl.SelectedCompletion.Title.Length + offset;

                            editor.CaretIndex = caretIndex;
                            // Allow user to press Ctrl + Z to undo overwrite.
                            editor.TextDocument.EndUpdate();
                            editor.TextDocument.BeginUpdate();


                            editor.TextDocument.Replace(caretIndex, intellisenseEndsAt - intellisenseOpenedAt, string.Empty);                            
                        }
                        else
                        {
                            editor.TextDocument.Replace(intellisenseStartedAt, caretIndex - intellisenseStartedAt - offset,
                                intellisenseControl.SelectedCompletion.Title);

                            caretIndex = intellisenseStartedAt + intellisenseControl.SelectedCompletion.Title.Length + offset;
                        }

                        editor.CaretIndex = caretIndex;

                        editor.TextDocument.EndUpdate();

                        Close();
                    });
                }
			}

			return result;
		}

		public async Task CompleteOnKeyDown(KeyEventArgs e)
		{
			var caretIndex = editor.CaretIndex;

			var behindCaretChar = '\0';

			if (caretIndex > 0)
			{
				await Dispatcher.UIThread.InvokeTaskAsync(() =>
				{
					if (caretIndex < editor.TextDocument.TextLength)
					{
						behindCaretChar = editor.TextDocument.GetCharAt(caretIndex - 1);
					}
				});
			}

			if (behindCaretChar == '(')
			{
				var word = string.Empty;
				await Dispatcher.UIThread.InvokeTaskAsync(() => { word = editor.GetWordAtIndex(caretIndex - 1); });

                List<Symbol> symbols = null;// = await languageService.GetSymbolsAsync(file, new List<UnsavedFile>(), word);

                int line = -1;
                int column = -1;
                string text = string.Empty;

                await Dispatcher.UIThread.InvokeTaskAsync(() =>
                {
                    text = editor.TextDocument.Text;
                    var location = editor.TextDocument.GetLocation(caretIndex);
                    line = location.Line;
                    column = location.Column;
                });

                var symbol = await languageService.SignatureHelp(file, new UnsavedFile(file.File, text), new List<UnsavedFile>(), line, column);

				if (symbols.Count() > 0)
				{
					var adviceState = new CompletionAdviceState();
					adviceState.BracketOpenedAt = caretIndex;
					adviceState.Symbols = symbols;

					if (currentAdvice != null)
					{
						completionAdviceStack.Push(currentAdvice);
					}

					currentAdvice = adviceState;

					await Dispatcher.UIThread.InvokeTaskAsync(() =>
					{
						completionAdviceControl.Count = currentAdvice.Symbols.Count;
						completionAdviceControl.SelectedIndex = 0;
						completionAdviceControl.Symbol = currentAdvice.Symbols[currentAdvice.SelectedIndex];
						completionAdviceControl.IsVisible = true;
					});
				}
			}

			if (intellisenseControl.IsVisible)
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
			var caretIndex = editor.CaretIndex;

			if (intellisenseControl.IsVisible)
			{
				var behindCaretChar = '\0';
				var behindBehindCaretChar = '\0';

				if (caretIndex > 0)
				{
					behindCaretChar = editor.TextDocument.GetCharAt(caretIndex - 1);
				}

				if (caretIndex > 1)
				{
					behindBehindCaretChar = editor.TextDocument.GetCharAt(caretIndex - 2);
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

        public async Task CompletionAssistantOnKeyUp (char behindCaretChar, char behindBehindCaretChar)
        {
            if (completionAssistant.IsVisible)
            {
                var caret = editor.CaretIndex;

                await Dispatcher.UIThread.InvokeTaskAsync(() =>
                {
                    if (caret < completionAssistant.CurrentMethodInfo.Offset)
                    {
                        completionAssistant.PopMethod();
                    }

                    if (completionAssistant.CurrentMethodInfo != null)
                    {
                        int index = 0;
                        int level = 0;
                        int offset = completionAssistant.CurrentMethodInfo.Offset;

                        while (offset < caret)
                        {
                            var currentChar = editor.TextDocument.GetCharAt(offset++);

                            switch(currentChar)
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

                        completionAssistant.SetArgumentIndex(index);
                    }
                });
            }

            if (behindCaretChar == '(' && (completionAssistant.CurrentMethodInfo == null ||completionAssistant.CurrentMethodInfo.Offset != editor.CaretIndex))
            {
                string currentWord = string.Empty;

                await Dispatcher.UIThread.InvokeTaskAsync(async () =>
                {
                    if (behindBehindCaretChar.IsWhiteSpace() && behindBehindCaretChar != '\0')
                    {
                        currentWord = editor.GetPreviousWordAtIndex(editor.CaretIndex - 1);
                    }
                    else
                    {
                        currentWord = editor.GetWordAtIndex(editor.CaretIndex - 1);
                    }

                    var text = editor.TextDocument.Text;
                    var location = editor.TextDocument.GetLocation(editor.CaretIndex);
                    int line = location.Line;
                    int column = location.Column;

                    var symbol = await languageService.SignatureHelp(file, new UnsavedFile(file.File, text), new List<UnsavedFile>(), line, column);
                    List<Symbol> symbols = new List<Symbol>();// await languageService.GetSymbolsAsync(file, new List<UnsavedFile>(), currentWord);

                    if (symbols.Count > 0)
                    {
                        completionAssistant.PushMethod(new MethodInfo(symbols, editor.CaretIndex));
                    }
                });
            }
            else if (behindCaretChar == ')')
            {
                await Dispatcher.UIThread.InvokeTaskAsync(() =>
                {
                    completionAssistant.PopMethod();
                });
            }
        }


		public async Task OnKeyUp(KeyEventArgs e)
		{
            var isVisible = intellisenseControl.IsVisible;
            var caretIndex = editor.CaretIndex;

            var caretChar = '\0';
            var behindCaretChar = '\0';
            var behindBehindCaretChar = '\0';

            await Dispatcher.UIThread.InvokeTaskAsync(() =>
            {
                if (caretIndex > 1 && caretIndex < editor.TextDocument.TextLength)
                {
                    behindBehindCaretChar = editor.TextDocument.GetCharAt(caretIndex - 2);
                }

                if (caretIndex > 0 && caretIndex < editor.TextDocument.TextLength)
                {
                    caretChar = editor.TextDocument.GetCharAt(caretIndex);
                }

                if (caretIndex > 0 && caretIndex <= editor.TextDocument.TextLength)
                {
                    behindCaretChar = editor.TextDocument.GetCharAt(caretIndex - 1);
                }
            });

            await CompletionAssistantOnKeyUp(behindCaretChar, behindBehindCaretChar);

            if (e.Key == Key.Escape && e.Modifiers == InputModifiers.None)
            {
                await Dispatcher.UIThread.InvokeTaskAsync(() =>
                {
                    if (completionAssistant.IsVisible)
                    {
                        completionAssistant.Close();
                    }
                    else if (intellisenseControl.IsVisible)
                    {
                        Close();
                    }
                });
            }

            if (IsIntellisenseKey(e))
            { 				
				if (caretIndex <= intellisenseStartedAt)
				{
					await Dispatcher.UIThread.InvokeTaskAsync(() => { Close(); });
					return;
				}

				if (IsIntellisenseResetKey(e))
				{
					isVisible = false; // We dont actually want to hide, so use backing field.
					//currentFilter = string.Empty;
				}

				await Dispatcher.UIThread.InvokeTaskAsync(async () => { await CompleteOnKeyUp(); });

				IEnumerable<CompletionDataViewModel> filteredResults = null;

				if (!intellisenseControl.IsVisible && (IsIntellisenseOpenKey(e) || IsIntellisenseResetKey(e)))
				{
					var caret = new TextLocation();

                    var behindStartChar = '\0';                   					

					if (behindCaretChar == ':' && behindBehindCaretChar == ':')
					{
						intellisenseStartedAt = caretIndex;
                        intellisenseEndsAt = intellisenseStartedAt;
                        intellisenseOpenedAt = intellisenseStartedAt;
                    }
					else if (behindCaretChar == '>' || behindBehindCaretChar == ':' || behindBehindCaretChar == '>')
					{
						intellisenseStartedAt = caretIndex - 1;
                        intellisenseEndsAt = intellisenseStartedAt;
                        intellisenseOpenedAt = intellisenseStartedAt;
                    }
					else
					{
                        await
                            Dispatcher.UIThread.InvokeTaskAsync(
                                () =>
                                {
                                    if (caretIndex > 1)
                                    {
                                        intellisenseOpenedAt = caretIndex - 1;

                                        intellisenseStartedAt = TextUtilities.GetNextCaretPosition(editor.TextDocument, caretIndex,
                                            TextUtilities.LogicalDirection.Backward, TextUtilities.CaretPositioningMode.WordStart);
                                    }
                                    else
                                    {
                                        intellisenseOpenedAt = 1;
                                        intellisenseStartedAt = 1;
                                    }

                                    behindStartChar = editor.TextDocument.GetCharAt(intellisenseStartedAt - 1);

                                    if ((behindStartChar.IsWhiteSpace() || !behindStartChar.IsSymbolChar()) && (caretChar.IsWhiteSpace() || !caretChar.IsSymbolChar()))
                                    {
                                        intellisenseEndsAt = intellisenseStartedAt;
                                    }
                                    else
                                    {
                                        intellisenseEndsAt = TextUtilities.GetNextCaretPosition(editor.TextDocument, caretIndex,
                                            TextUtilities.LogicalDirection.Forward, TextUtilities.CaretPositioningMode.WordBorder) - 1;
                                    }
                                });
					}

					if (IsIntellisenseResetKey(e))
					{
						intellisenseStartedAt++;
                        intellisenseOpenedAt++;
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
                                    currentCompletion.Overloads++;
                                }
							}
						}
					}

					filteredResults = unfilteredCompletions;
				}
				else
				{
                    await Dispatcher.UIThread.InvokeTaskAsync(
                    () =>
                    {
                        if (intellisenseStartedAt != -1 && caretIndex > intellisenseStartedAt)
                        {
                            currentFilter = editor.TextDocument.GetText(intellisenseStartedAt, caretIndex - intellisenseStartedAt);
                        }
                        else
                        {
                            currentFilter = string.Empty;
                        }
                    });
					
					filteredResults = unfilteredCompletions.Where(c => c.Title.ToLower().Contains(currentFilter.ToLower()));
				}

				CompletionDataViewModel suggestion = null;

				if (currentFilter != string.Empty)
				{
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
						await Dispatcher.UIThread.InvokeTaskAsync(() => { Close(); });
					}
					else
					{
						var list = filteredResults.ToList();

						await Dispatcher.UIThread.InvokeTaskAsync(() =>
						{
							intellisenseControl.CompletionData = list;

                            intellisenseControl.SelectedCompletion = null;
							intellisenseControl.SelectedCompletion = suggestion;

							intellisenseControl.IsVisible = true;
						});
					}
				}
				else
				{
					await Dispatcher.UIThread.InvokeTaskAsync(() => { Close(); });
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

				await Dispatcher.UIThread.InvokeTaskAsync(() => { Close(); });
			}
		}
	}
}