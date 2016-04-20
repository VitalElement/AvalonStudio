namespace AvalonStudio.Controls
{
    using Perspex.Threading;
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
    using TextEditor;

    [Export(typeof(EditorModel))]
    public class EditorModel
    {
        private ReaderWriterLockSlim editorLock;
        private Thread codeAnalysisThread;
        private Thread codeCompletionThread;
        private SemaphoreSlim textChangedSemaphore;
        private SemaphoreSlim startCompletionRequestSemaphore;
        private SemaphoreSlim endCompletionRequestSemaphore;
        private ReaderWriterLockSlim completionRequestLock;
        private ISourceFile sourceFile;

        public TextEditor Editor { get; set; }

        public EditorModel()
        {
            editorLock = new ReaderWriterLockSlim();
            completionRequestLock = new ReaderWriterLockSlim();

            textChangedSemaphore = new SemaphoreSlim(0, 1);
            startCompletionRequestSemaphore = new SemaphoreSlim(0, 1);
            endCompletionRequestSemaphore = new SemaphoreSlim(0, 1);

            codeCompletionResults = new CodeCompletionResults();
            TextDocument = new TextDocument();
        }

        ~EditorModel()
        {
            ShutdownBackgroundWorkers();
        }

        private bool completionRequested = false;
        private TextLocation beginCompletionLocation = new TextLocation();
        private string filter;

        public async Task<CodeCompletionResults> DoCompletionRequestAsync(int line, int column, string filter)
        {
            if (!completionRequested)
            {
                beginCompletionLocation = new TextLocation(line, column);
                this.filter = filter;
                completionRequested = true;

                DoCodeCompletionRequest();
                await endCompletionRequestSemaphore.WaitAsync();

                return codeCompletionResults;
            }

            return null;
        }

        public void ScrollToLine(int line)
        {
            if (Editor != null)
            {
                Editor.ScrollToLine(line);
            }
        }

        public event EventHandler<EventArgs> DocumentLoaded;
        public event EventHandler<EventArgs> TextChanged;

        public void UnRegisterLanguageService ()
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

        public void RegisterLanguageService(IIntellisenseControl intellisenseControl)
        {
            UnRegisterLanguageService();

            try
            {
                LanguageService = Shell.Instance.LanguageServices.Single((o) => o.CanHandle(sourceFile));

                ShellViewModel.Instance.StatusBar.Language = LanguageService.Title;

                LanguageService.RegisterSourceFile(intellisenseControl, Editor, sourceFile, TextDocument);
            }
            catch
            {
                LanguageService = null;
            }

            IsDirty = false;


            StartBackgroundWorkers();

            DocumentLoaded(this, new EventArgs());

            TextDocument.TextChanged += TextDocument_TextChanged;

            OnBeforeTextChanged(null);

            TriggerCodeAnalysis();
        }

        public void OpenFile(ISourceFile file, IIntellisenseControl intellisense)
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

                    RegisterLanguageService(intellisense);
                }
            }
        }

        public void Save()
        {
            if (sourceFile != null && TextDocument != null)
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

        private CodeCompletionResults codeCompletionResults;

        public ILanguageService LanguageService { get; set; }

        private void StartBackgroundWorkers()
        {
            if (codeAnalysisThread != null && codeCompletionThread != null)
            {
                if (codeAnalysisThread.IsAlive || codeCompletionThread.IsAlive)
                {
                    throw new Exception("Worker threads already active.");
                }
            }

            codeAnalysisThread = new Thread(new ThreadStart(CodeAnalysisThread));
            codeAnalysisThread.Start();

            codeCompletionThread = new Thread(new ThreadStart(CodeCompletionThread));
            codeCompletionThread.Start();
        }

        public void ShutdownBackgroundWorkers()
        {
            if (codeAnalysisThread != null && codeAnalysisThread.IsAlive)
            {
                codeAnalysisThread.Abort();
                codeAnalysisThread.Join();
                codeAnalysisThread = null;
            }

            if (codeCompletionThread != null && codeCompletionThread.IsAlive)
            {
                codeCompletionThread.Abort();
                codeCompletionThread.Join();
                codeCompletionResults = null;
            }
        }

        public void OnBeforeTextChanged(object param)
        {
            if (!editorLock.IsWriteLockHeld)
            {
                editorLock.EnterWriteLock();
            }
        }

        private bool isDirty;
        public bool IsDirty
        {
            get { return isDirty; }
            set { isDirty = value; }
        }

        private void DoCodeCompletionRequest()
        {
            if (LanguageService != null)
            {
                completionRequestLock.EnterWriteLock();

                if (startCompletionRequestSemaphore.CurrentCount == 0)
                {
                    startCompletionRequestSemaphore.Release();
                }

                completionRequestLock.ExitWriteLock();
            }
        }

        /// <summary>
        /// Write lock must be held before calling this.
        /// </summary>
        private void TriggerCodeAnalysis()
        {
            editorLock.ExitWriteLock();

            if (textChangedSemaphore.CurrentCount == 0)
            {
                textChangedSemaphore.Release();
            }
        }

        public void OnTextChanged(object param)
        {
            IsDirty = true;

            TriggerCodeAnalysis();
        }

        private void CodeCompletionThread()
        {
            try
            {
                while (true)
                {
                    startCompletionRequestSemaphore.Wait();

                    if (LanguageService != null)
                    {
                        var results = LanguageService.CodeCompleteAt(sourceFile, beginCompletionLocation.Line, beginCompletionLocation.Column, UnsavedFiles, filter);

                        codeCompletionResults = new CodeCompletionResults() { Completions = results };

                        endCompletionRequestSemaphore.Release();
                    }
                    else
                    {
                        endCompletionRequestSemaphore.Release();
                    }


                    completionRequested = false;
                }
            }
            catch (ThreadAbortException)
            {

            }
        }

        private void CodeAnalysisThread()
        {
            try
            {
                while (true)
                {
                    textChangedSemaphore.Wait();

                    completionRequestLock.EnterWriteLock();
                    editorLock.EnterReadLock();

                    if (LanguageService != null)
                    {
                        var result = LanguageService.RunCodeAnalysis(sourceFile, UnsavedFiles, () => editorLock.WaitingWriteCount > 0);

                        Dispatcher.UIThread.InvokeAsync(() =>
                        {
                            CodeAnalysisResults = result;
                        });
                    }

                    editorLock.ExitReadLock();
                    completionRequestLock.ExitWriteLock();
                }
            }
            catch (ThreadAbortException)
            {
                if (completionRequestLock.IsWriteLockHeld)
                {
                    completionRequestLock.ExitWriteLock();
                }

                if (editorLock.IsReadLockHeld)
                {
                    editorLock.ExitReadLock();
                }
            }

        }
    }
}
