namespace AvalonStudio.Controls
{
    using Avalonia.Threading;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.Composition;
    using System.IO;
    using System.Threading;
    using TextEditor.Document;
    using System.Threading.Tasks;
    using Languages;
    using Projects;
    using System.Linq;
    using Extensibility;
    using AvalonStudio.Extensibility.Threading;
    using TextEditor;
    using Shell;

    [Export(typeof(EditorModel))]
    public class EditorModel : IDisposable
    {
        private JobRunner codeAnalysisRunner;
        private ISourceFile sourceFile;
        private IShell shell;
        private TextEditor textEditor;

        public TextEditor Editor
        {
            get { return textEditor; }
            set { textEditor = value; }
        }


        public void Dispose()
        {
            Editor = null;
            TextDocument.TextChanged -= TextDocument_TextChanged;
        }

        public EditorModel()
        {
            shell = IoC.Get<IShell>();

            codeAnalysisRunner = new JobRunner();
            TextDocument = new TextDocument();
        }

        ~EditorModel()
        {
            System.Console.WriteLine(("Editor Model Destructed."));
        }

        public async Task<CodeCompletionResults> DoCompletionRequestAsync(int line, int column)
        {
            CodeCompletionResults results = null;

            var completions = await LanguageService.CodeCompleteAtAsync(sourceFile, line, column, UnsavedFiles);
            results = new CodeCompletionResults() { Completions = completions };

            return results;
        }

        public void ScrollToLine(int line)
        {
            Editor?.ScrollToLine(line);
        }

        public event EventHandler<EventArgs> DocumentLoaded;
        public event EventHandler<EventArgs> TextChanged;

        public void UnRegisterLanguageService()
        {
            ShutdownBackgroundWorkers();

            if (unsavedFile != null)
            {
                UnsavedFiles.Remove(unsavedFile);
                unsavedFile = null;
            }

            if (LanguageService != null && sourceFile != null)
            {
                LanguageService.UnregisterSourceFile(Editor, sourceFile);
            }
        }

        public async void RegisterLanguageService(IIntellisenseControl intellisenseControl, ICompletionAdviceControl completionAdviceControl)
        {
            UnRegisterLanguageService();

            try
            {
                LanguageService = shell.LanguageServices.Single((o) => o.CanHandle(sourceFile));

                ShellViewModel.Instance.StatusBar.Language = LanguageService.Title;

                LanguageService.RegisterSourceFile(intellisenseControl, completionAdviceControl, Editor, sourceFile, TextDocument);
            }
            catch (Exception e)
            {
                LanguageService = null;
                ShellViewModel.Instance.StatusBar.Language = "Text";
            }

            IsDirty = false;


            StartBackgroundWorkers();
            
            TextDocument.TextChanged += TextDocument_TextChanged;

            OnBeforeTextChanged(null);

            await TriggerCodeAnalysis();
        }

        public void OpenFile(ISourceFile file, IIntellisenseControl intellisense, ICompletionAdviceControl completionAdviceControl)
        {
            if (this.sourceFile != file)
            {
                if (File.Exists(file.Location))
                {
                    using (var fs = File.OpenText(file.Location))
                    {
                        TextDocument = new TextDocument(fs.ReadToEnd());
                        TextDocument.FileName = file.Location;
                    }

                    sourceFile = file;

                    RegisterLanguageService(intellisense, completionAdviceControl);

                    if (DocumentLoaded != null)
                    {
                        DocumentLoaded(this, new EventArgs());
                    }
                }
            }
        }

        public void Save()
        {
            if (sourceFile != null && TextDocument != null && IsDirty)
            {
                File.WriteAllText(sourceFile.Location, TextDocument.Text);
                IsDirty = false;

                if (unsavedFile != null)
                {
                    UnsavedFiles.Remove(unsavedFile);
                    unsavedFile = null;
                }
            }
        }

        private UnsavedFile unsavedFile = null;
        private void TextDocument_TextChanged(object sender, EventArgs e)
        {
            if (unsavedFile == null)
            {
                unsavedFile = new UnsavedFile(sourceFile.Location, TextDocument.Text);

                UnsavedFiles.Add(unsavedFile);
            }
            else
            {
                unsavedFile.Contents = TextDocument.Text;
            }

            IsDirty = true;

            if (TextChanged != null)
            {
                TextChanged(this, new EventArgs());
            }
        }

        public ISourceFile ProjectFile
        {
            get
            {
                return sourceFile;
            }
        }

        public static List<UnsavedFile> UnsavedFiles = new List<UnsavedFile>();
        public TextDocument TextDocument { get; set; }
        public string Title { get; set; }

        public event EventHandler<EventArgs> CodeAnalysisCompleted;

        private CodeAnalysisResults codeAnalysisResults;
        public CodeAnalysisResults CodeAnalysisResults
        {
            get { return codeAnalysisResults; }
            set
            {
                codeAnalysisResults = value;

                if (CodeAnalysisCompleted != null)
                {
                    CodeAnalysisCompleted(this, new EventArgs());
                }
            }
        }
        

        public ILanguageService LanguageService { get; set; }

        private CancellationTokenSource cancellationSource;
        private void StartBackgroundWorkers()
        {
            cancellationSource = new CancellationTokenSource();

            Task.Factory.StartNew(() =>
            {
                codeAnalysisRunner.RunLoop(cancellationSource.Token);
                cancellationSource = null;
            });
        }

        public void ShutdownBackgroundWorkers()
        {
            cancellationSource?.Cancel();
        }

        public void OnBeforeTextChanged(object param)
        {
            
        }

        private bool isDirty;
        public bool IsDirty
        {
            get { return isDirty; }
            set { isDirty = value; }
        }

        /// <summary>
        /// Write lock must be held before calling this.
        /// </summary>
        private async Task TriggerCodeAnalysis()
        {
            await codeAnalysisRunner.InvokeAsync(async () =>
            {
                if (LanguageService != null)
                {
                    // TODO allow interruption.
                    var result = await LanguageService.RunCodeAnalysisAsync(sourceFile, UnsavedFiles, () => false);

                    Dispatcher.UIThread.InvokeAsync(() =>
                    {
                        CodeAnalysisResults = result;
                    });
                }
            });         
        }

        public async void OnTextChanged(object param)
        {
            IsDirty = true;

            await TriggerCodeAnalysis();
        }

                
    }
}
