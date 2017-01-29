using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Avalonia.Threading;
using AvalonStudio.Extensibility;
using AvalonStudio.Extensibility.Threading;
using AvalonStudio.Languages;
using AvalonStudio.Projects;
using AvalonStudio.Shell;
using AvalonStudio.TextEditor.Document;
using AvalonStudio.Extensibility.Languages.CompletionAssistance;
using AvalonStudio.Utils;
using System.Reactive.Subjects;
using System.Reactive.Linq;

namespace AvalonStudio.Controls
{
    [Export(typeof(EditorModel))]
    public class EditorModel : IDisposable
    {
        public static List<UnsavedFile> _unsavedFiles;

        public static List<UnsavedFile> UnsavedFiles
        {
            get
            {
                if (_unsavedFiles == null)
                {
                    _unsavedFiles = new List<UnsavedFile>();
                }

                return _unsavedFiles;
            }
        }


        private CancellationTokenSource cancellationSource;

        private CodeAnalysisResults codeAnalysisResults;
        private readonly JobRunner codeAnalysisRunner;

        private readonly IShell shell;

        public EditorModel()
        {
            shell = IoC.Get<IShell>();

            codeAnalysisRunner = new JobRunner();
            TextDocument = new TextDocument();

            AnalysisTriggerEvents.Select(_ => Observable.Timer(TimeSpan.FromMilliseconds(500))
            .SelectMany(o => DoCodeAnalysisAsync())).Switch().Subscribe(_ => { });

            //AnalysisTriggerEvents.Throttle(TimeSpan.FromMilliseconds(500)).Subscribe(async _ =>
            //{
            //    await DoCodeAnalysisAsync();
            //});
        }

        public TextEditor.TextEditor Editor { get; set; }

        public ISourceFile ProjectFile { get; private set; }

        public TextDocument TextDocument { get; set; }
        public string Title { get; set; }

        public CodeAnalysisResults CodeAnalysisResults
        {
            get { return codeAnalysisResults; }
            set
            {
                codeAnalysisResults = value;

                CodeAnalysisCompleted?.Invoke(this, new EventArgs());
            }
        }


        public ILanguageService LanguageService { get; set; }

        public bool IsDirty { get; set; }


        public void Dispose()
        {
            Editor = null;
            TextDocument.TextChanged -= TextDocument_TextChanged;
        }

        ~EditorModel()
        {
        }

        public async Task<CodeCompletionResults> DoCompletionRequestAsync(int index, int line, int column)
        {
            CodeCompletionResults results = null;

            var completions = await LanguageService.CodeCompleteAtAsync(ProjectFile, index, line, column, UnsavedFiles.ToList());
            results = completions;

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

            UnsavedFile unsavedFile = null;

            lock (UnsavedFiles)
            {
                unsavedFile = UnsavedFiles.BinarySearch(ProjectFile.Location);
            }

            if (unsavedFile != null)
            {
                lock (UnsavedFiles)
                {
                    UnsavedFiles.Remove(unsavedFile);
                }
            }

            if (LanguageService != null && ProjectFile != null)
            {
                LanguageService.UnregisterSourceFile(Editor, ProjectFile);
            }

            TextDocument.TextChanged -= TextDocument_TextChanged;
        }

        public void RegisterLanguageService(IIntellisenseControl intellisenseControl,
            ICompletionAssistant completionAssistant)
        {
            UnRegisterLanguageService();

            LanguageService = shell.LanguageServices.FirstOrDefault(o => o.CanHandle(ProjectFile));

            if (LanguageService != null)
            {
                ShellViewModel.Instance.StatusBar.Language = LanguageService.Title;

                LanguageService.RegisterSourceFile(intellisenseControl, completionAssistant, Editor, ProjectFile, TextDocument);
            }
            else
            {
                LanguageService = null;
                ShellViewModel.Instance.StatusBar.Language = "Text";
            }

            IsDirty = false;

            StartBackgroundWorkers();

            TextDocument.TextChanged += TextDocument_TextChanged;

            OnBeforeTextChanged(null);

            DoCodeAnalysisAsync().GetAwaiter();
        }

        public void OpenFile(ISourceFile file, IIntellisenseControl intellisense,
            ICompletionAssistant completionAssistant)
        {
            if (ProjectFile != file)
            {
                if (System.IO.File.Exists(file.Location))
                {
                    using (var fs = System.IO.File.OpenText(file.Location))
                    {
                        TextDocument = new TextDocument(fs.ReadToEnd());
                        TextDocument.FileName = file.Location;
                    }

                    ProjectFile = file;

                    RegisterLanguageService(intellisense, completionAssistant);

                    DocumentLoaded?.Invoke(this, new EventArgs());
                }
            }
        }

        public void Save()
        {
            if (ProjectFile != null && TextDocument != null && IsDirty)
            {
                System.IO.File.WriteAllText(ProjectFile.Location, TextDocument.Text);
                IsDirty = false;

                lock (UnsavedFiles)
                {
                    var unsavedFile = UnsavedFiles.BinarySearch(ProjectFile.Location);

                    if (unsavedFile != null)
                    {
                        UnsavedFiles.Remove(unsavedFile);
                    }
                }
            }
        }

        private void TextDocument_TextChanged(object sender, EventArgs e)
        {
            UnsavedFile unsavedFile = null;

            lock (UnsavedFiles)
            {
                unsavedFile = UnsavedFiles.BinarySearch(ProjectFile.Location);
            }

            if (unsavedFile == null)
            {
                lock (UnsavedFiles)
                {
                    UnsavedFiles.InsertSorted(new UnsavedFile(ProjectFile.Location, TextDocument.Text));
                }
            }
            else
            {
                unsavedFile.Contents = TextDocument.Text;
            }

            IsDirty = true;

            TextChanged?.Invoke(this, new EventArgs());
        }

        public event EventHandler<EventArgs> CodeAnalysisCompleted;

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

        private Subject<bool> AnalysisTriggerEvents = new Subject<bool>();

        private async Task<bool> DoCodeAnalysisAsync()
        {
            await codeAnalysisRunner.InvokeAsync(async () =>
            {
                if (LanguageService != null)
                {
                    // TODO allow interruption.
                    var result = await LanguageService.RunCodeAnalysisAsync(ProjectFile, UnsavedFiles.ToList(), () => false);

                    Dispatcher.UIThread.InvokeAsync(() => { CodeAnalysisResults = result; });
                }
            });

            return true;
        }

        /// <summary>
        ///     Write lock must be held before calling this.
        /// </summary>
        public void TriggerCodeAnalysis()
        {
            AnalysisTriggerEvents.OnNext(true);
        }

        public void OnTextChanged(object param)
        {
            IsDirty = true;

            TriggerCodeAnalysis();
        }
    }
}