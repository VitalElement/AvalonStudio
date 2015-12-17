﻿namespace AvalonStudio.Controls
{
    using Models.Solutions;
    using Perspex.Threading;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Threading;
    using TextEditor.Document;
    using System.Threading.Tasks;
    using Languages;
    using Projects;
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

        private bool completionRequested = false;
        private TextLocation beginCompletionLocation = new TextLocation();
        private string filter;

        public async Task DoCompletionRequestAsync(int line, int column, string filter)
        {
            if (!completionRequested)
            {
                beginCompletionLocation = new TextLocation(line, column);
                this.filter = filter;
                completionRequested = true;

                DoCodeCompletionRequest();

                await endCompletionRequestSemaphore.WaitAsync();
            }
        }

        public event EventHandler<EventArgs> DocumentLoaded;
        public event EventHandler<EventArgs> TextChanged;

        public void OpenFile(ISourceFile file)
        {
            if (File.Exists(file.Location))
            {
                using (var fs = File.OpenText(file.Location))
                {
                    TextDocument = new TextDocument(fs.ReadToEnd());
                }
            }

            ShutdownBackgroundWorkers();

            // TODO use factory to create the correct language service.
            LanguageService = LanguageServices.Instance.GetLanguageService(file);

            StartBackgroundWorkers();

            sourceFile = file;

            DocumentLoaded(this, new EventArgs());

            TextDocument.TextChanged += TextDocument_TextChanged;

            if (textChangedSemaphore.CurrentCount == 0)
            {
                textChangedSemaphore.Release();
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

        public event EventHandler<EventArgs> CodeCompletionRequestCompleted;

        private CodeCompletionResults codeCompletionResults;
        public CodeCompletionResults CodeCompletionResults
        {
            get { return codeCompletionResults; }
            set
            {
                codeCompletionResults = value;

                //if(CodeCompletionRequestCompleted != null)
                //{
                //    CodeCompletionRequestCompleted(this, new EventArgs());
                //}
            }
        }


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
            }

            if (codeAnalysisThread != null && codeCompletionThread.IsAlive)
            {
                codeCompletionThread.Abort();
                codeCompletionThread.Join();
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
            completionRequestLock.EnterWriteLock();

            if (startCompletionRequestSemaphore.CurrentCount == 0)
            {
                startCompletionRequestSemaphore.Release();
            }
        }

        public void OnTextChanged(object param)
        {
            IsDirty = true;

            editorLock.ExitWriteLock();

            if (textChangedSemaphore.CurrentCount == 0)
            {
                textChangedSemaphore.Release();
            }
        }

        private void CodeCompletionThread()
        {
            try
            {
                while (true)
                {
                    startCompletionRequestSemaphore.Wait();

                    var results = LanguageService.CodeCompleteAt(sourceFile.Location, beginCompletionLocation.Line, beginCompletionLocation.Column, UnsavedFiles, filter);

                    Dispatcher.UIThread.InvokeAsync(() =>
                    {
                        CodeCompletionResults = new CodeCompletionResults() { Completions = results };
                        completionRequestLock.ExitWriteLock();
                        endCompletionRequestSemaphore.Release();
                    });

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
                        var result = LanguageService.RunCodeAnalysis(UnsavedFiles, () => editorLock.WaitingWriteCount > 0);

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

            }

        }
    }
}
