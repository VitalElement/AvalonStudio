namespace AvalonStudio.Controls
{
    using Models.LanguageServices.CPlusPlus;
    using Models.LanguageServices;
    using Models.Solutions;
    using Perspex.Threading;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Threading;
    using TextEditor.Document;


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
        public event EventHandler<EventArgs> TextChanged;

        public void OpenFile(ProjectFile file)
        {
			if (File.Exists(file.Location))
            {
				using (var fs = File.OpenText(file.Location))
                {
                    TextDocument = new TextDocument(fs.ReadToEnd());
                }
            }

            // TODO use factory to create the correct language service.
            LanguageService = new CPlusPlusLanguageService(file.Project.Solution.NClangIndex, file);

            projectFile = file;

            DocumentLoaded(this, new EventArgs());

            TextDocument.TextChanged += TextDocument_TextChanged;

            if (textChangedSemaphore.CurrentCount == 0)
            {
                textChangedSemaphore.Release();
            }
        }

        public void Save ()
        {
            if (projectFile != null && TextDocument != null)
            {
                File.WriteAllText(projectFile.Location, TextDocument.Text);
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
                unsavedFile = new UnsavedFile(projectFile.Location, TextDocument.Text);

                UnsavedFiles.Add(unsavedFile);
            }
            else
            {
                unsavedFile.Contents = TextDocument.Text;
            }

            IsDirty = true;

            if(TextChanged != null)
            {
                TextChanged(this, new EventArgs());
            }
        }

        public ProjectFile ProjectFile
        {
            get
            {
                return projectFile;
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

                if(CodeAnalysisCompleted != null)
                {
                    CodeAnalysisCompleted(this, new EventArgs());
                }
            }
        }

        public ILanguageService LanguageService { get; set; }

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

                    if (LanguageService != null)
                    {
                        var result = LanguageService.RunCodeAnalysis(UnsavedFiles, () => editorLock.WaitingWriteCount > 0);

                        Dispatcher.UIThread.InvokeAsync(() =>
                        {
                            CodeAnalysisResults = result;
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
