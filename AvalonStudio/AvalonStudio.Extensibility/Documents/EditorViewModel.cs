using AvalonStudio.Projects;
using ReactiveUI;

namespace AvalonStudio.Controls
{
    public abstract class EditorViewModel : DocumentTabViewModel, IFileDocumentTabViewModel
    {
        private bool _isDirty;
        private ISourceFile _sourceFile;

        public EditorViewModel(ISourceFile file)
        {
            _sourceFile = file;
        }

        public bool IsDirty
        {
            get => _isDirty;
            set
            {
                this.RaiseAndSetIfChanged(ref _isDirty, value);

                if (value && IsTemporary)
                {
                    IsTemporary = false;
                }
            }
        }

        public ISourceFile SourceFile
        {
            get { return _sourceFile; }
            set { this.RaiseAndSetIfChanged(ref _sourceFile, value); }
        }
    }
}
