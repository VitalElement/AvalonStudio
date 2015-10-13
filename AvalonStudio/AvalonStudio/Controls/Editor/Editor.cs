namespace AvalonStudio.Controls
{
    using System.Threading;

    public class EditorModel
    {
        private ReaderWriterLockSlim editorLock;
        private Thread codeAnalysisThread;

        public EditorModel ()
        {
            editorLock = new ReaderWriterLockSlim();

            codeAnalysisThread = new Thread(new ThreadStart(CodeAnalysisThread));
            codeAnalysisThread.Start();
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

            Workspace.This.Console.WriteLine("Write lock aquired.");
        }

        public void OnTextChanged(object param)
        {
            editorLock.ExitWriteLock();
            Workspace.This.Console.WriteLine("Write lock freed.");
        }

        private void CodeAnalysisThread()
        {
            Thread.Sleep(1000);

            while (true)
            {
                if (editorLock.TryEnterReadLock(40))
                {
                    Workspace.This.Console.WriteLine("Read Lock aquired");

                    while (editorLock.IsReadLockHeld)
                    {
                        Thread.Sleep(40);       // Work done here.

                        if (editorLock.WaitingWriteCount > 0)
                        {
                            editorLock.ExitReadLock();
                            break;
                        }
                    }
                }                

                Workspace.This.Console.WriteLine("Read Lock released.");

                Thread.Sleep(100); // Allow UI update before aquiring lock.
            }
        }
    }
}
