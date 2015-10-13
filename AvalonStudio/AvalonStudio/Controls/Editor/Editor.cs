namespace AvalonStudio.Controls
{
    using System.Threading;
    using Models.LanguageServices;
    using Models.LanguageServices.CPlusPlus;

    public class EditorModel
    {
        private ReaderWriterLockSlim editorLock;
        private Thread codeAnalysisThread;
        private ILanguageService languageService;
        private SemaphoreSlim textChangedSemaphore;

        public EditorModel()
        {
            editorLock = new ReaderWriterLockSlim();
            textChangedSemaphore = new SemaphoreSlim(0, 1);

            codeAnalysisThread = new Thread(new ThreadStart(CodeAnalysisThread));
            codeAnalysisThread.Start();

            languageService = new CPlusPlusLanguageService();
        }

        private string text;
        public string Text
        {
            get { return text; }
            set { text = value; }
        }


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

                    languageService.RunCodeAnalysis(() => editorLock.WaitingWriteCount > 0);

                    editorLock.ExitReadLock();
                }
            }
            catch (ThreadAbortException)
            {

            }

        }
    }
}
