namespace AvalonStudio.Controls
{
    using System.Threading;
    using Models.Solutions;
    using System;
    using Perspex.Threading;
    using TextEditor.Document;
    using System.IO;

    public class EditorModel
    {
        private ReaderWriterLockSlim editorLock;
        private Thread codeAnalysisThread;
        private SemaphoreSlim textChangedSemaphore;

        public EditorModel()
        {
            editorLock = new ReaderWriterLockSlim();
            textChangedSemaphore = new SemaphoreSlim(0, 1);

            codeAnalysisThread = new Thread(new ThreadStart(CodeAnalysisThread));
            codeAnalysisThread.Start();
        }

        public event EventHandler<EventArgs> DocumentLoaded;

        public void OpenFile (ProjectFile file)
        {
            if (File.Exists(file.Location))
            {
                using (var fs = File.OpenText(file.Location))
                {
                    TextDocument = new TextDocument(fs.ReadToEnd());
                }
            }

            Document = new Document(file);

            DocumentLoaded(this, new EventArgs());

            if (textChangedSemaphore.CurrentCount == 0)
            {
                textChangedSemaphore.Release();
            }
        }

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

        public void OnTextChanged(object param)
        {
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
                        var result = Document.LanguageService.RunCodeAnalysis(() => editorLock.WaitingWriteCount > 0);

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
