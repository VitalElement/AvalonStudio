namespace AvalonStudio.Controls
{
    using System.Threading;
    using Models.Solutions;
    using System;
    using Perspex.Threading;
    using TextEditor.Document;
    using System.IO;
    using Models.LanguageServices;
    using System.Collections.Generic;
    public class EditorModel
    {
        private ReaderWriterLockSlim editorLock;
        private Thread codeAnalysisThread;
        private SemaphoreSlim textChangedSemaphore;
        private ProjectFile projectFile;

        public EditorModel()
        {
            editorLock = new ReaderWriterLockSlim();
            textChangedSemaphore = new SemaphoreSlim(0, 1);
            UnsavedFiles = new List<UnsavedFile>();

            codeAnalysisThread = new Thread(new ThreadStart(CodeAnalysisThread));
            codeAnalysisThread.Start();
        }

        public event EventHandler<EventArgs> DocumentLoaded;

        public void OpenFile (ProjectFile file)
        {
            projectFile = file;
            Document = new Document(file);
            TextDocument = Document.TextDocument;

            DocumentLoaded(this, new EventArgs());
            
            TextDocument.TextChanged += TextDocument_TextChanged;

            if (textChangedSemaphore.CurrentCount == 0)
            {
                textChangedSemaphore.Release();
            }
        }

        private UnsavedFile unsavedFile = null;
        private void TextDocument_TextChanged(object sender, EventArgs e)
        {
            if(unsavedFile != null)
            {
                UnsavedFiles.Remove(unsavedFile);
            }

            unsavedFile = new UnsavedFile(projectFile.Location, TextDocument.Text);

            UnsavedFiles.Add(unsavedFile);
            IsDirty = true;
        }

        public static List<UnsavedFile> UnsavedFiles = new List<UnsavedFile>();
        public TextDocument TextDocument { get; set; }
        public Document Document { get; set; }

        public void Shutdown()
        {
            codeAnalysisThread.Abort();
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
        

        public void OnTextChanged(object param)
        {
            IsDirty = true;

            editorLock.ExitWriteLock();

            if (textChangedSemaphore.CurrentCount == 0)
            {
                textChangedSemaphore.Release();
            }
        }

        private void CodeAnalysisThread()
        {
            try
            {
                while (true)
                {
                    textChangedSemaphore.Wait();

                    editorLock.EnterReadLock();

                    if (Document != null)
                    {
                        var result = Document.LanguageService.RunCodeAnalysis(UnsavedFiles, () => editorLock.WaitingWriteCount > 0);

                        Dispatcher.UIThread.InvokeAsync(() =>
                        {
                            Document.SyntaxHighlightingData = result;
                        });
                    }

                    editorLock.ExitReadLock();
                }
            }
            catch (ThreadAbortException)
            {

            }

        }
    }
}
