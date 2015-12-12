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

        private bool IsIntellisenseKey(Key key)
        {
            bool result = false;

            result = (key > Key.A && key < Key.Z);

            if(!result)
            {
                switch(key)
                {
                    case Key.Back:
                        result = true;
                        break;
                }
            }

            return result;
        }


        private string currentFilter = string.Empty;

        public void OnTextInput(TextInputEventArgs e)
        {            
            currentFilter += e.Text.ToLower();
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

        System.Diagnostics.Stopwatch sw = new System.Diagnostics.Stopwatch();

        public async void OnKeyUp(KeyEventArgs e)
        {
            Workspace.This.Console.Clear();
             
            if (IsIntellisenseKey(e.Key))
            {
                if (!IsVisible)
                {
                    var caret = editorViewModel.CaretTextLocation;

                    Workspace.This.Console.WriteLine("Starting Completion request");
                    sw.Reset();
                    sw.Start();
                    await editor.DoCompletionRequestAsync(caret.Line, caret.Column, currentFilter);
                    sw.Stop();
                    Workspace.This.Console.WriteLine(sw.ElapsedMilliseconds.ToString());

                    Workspace.This.Console.WriteLine("Generating Completion List");
                    sw.Reset();
                    sw.Start();                    
                    await Task.Factory.StartNew(() =>
                    {
                        unfilteredCompletions.Clear();

                        foreach (var result in editor.CodeCompletionResults.Completions)
                        {
                            if (result.Suggestion.ToLower().Contains(currentFilter))
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
                    sw.Stop();
                    Workspace.This.Console.WriteLine(sw.ElapsedMilliseconds.ToString());

                    Workspace.This.Console.WriteLine("Setting Completion List");
                    sw.Reset();
                    sw.Start();
                    Model = unfilteredCompletions;
                    IsVisible = true;
                    sw.Stop();
                    Workspace.This.Console.WriteLine(sw.ElapsedMilliseconds.ToString());
                }
                else if (currentFilter == string.Empty)
                {
                    IsVisible = false;
                }
                else
                {
                    List<CompletionDataViewModel> newFiltered = null;

                    Workspace.This.Console.WriteLine("Starting Completion filter");
                    sw.Reset();
                    sw.Start();
                    await Task.Factory.StartNew(() =>
                    {
                        newFiltered = unfilteredCompletions.Where((c) => c.Title.ToLower().Contains(currentFilter)).ToList();
                    });
                    sw.Stop();
                    Workspace.This.Console.WriteLine(sw.ElapsedMilliseconds.ToString());

                    Workspace.This.Console.WriteLine("Setting Completion List");
                    sw.Reset();
                    sw.Start();
                    Model = newFiltered;
                    IsVisible = true;
                    sw.Stop();
                    Workspace.This.Console.WriteLine(sw.ElapsedMilliseconds.ToString());
                }

                Workspace.This.Console.WriteLine("Finding selection");
                sw.Reset();
                sw.Start();
                if (currentFilter != string.Empty)
                {
                    var newSelectedCompletions = Model.Where((s) => s.Title.ToLower().StartsWith(currentFilter));

                    if (newSelectedCompletions.Count() == 0)
                    {
                        SelectedCompletion = noSelectedCompletion;
                    }
                    else
                    {
                        var newSelectedCompletion = Model.FirstOrDefault((s) => s.Title.StartsWith(currentFilter));

                        if (newSelectedCompletion != null)
                        {
                            SelectedCompletion = newSelectedCompletion;
                        }
                        else
                        {
                            // Todo find the closest string... if not too expensive.
                            SelectedCompletion = newSelectedCompletions.FirstOrDefault();
                        }
                    }
                }
                else
                {
                    SelectedCompletion = noSelectedCompletion;
                }
                sw.Stop();
                Workspace.This.Console.WriteLine(sw.ElapsedMilliseconds.ToString());
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
