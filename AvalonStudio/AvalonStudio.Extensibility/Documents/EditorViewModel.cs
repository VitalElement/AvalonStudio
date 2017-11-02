using AvalonStudio.Projects;
using ReactiveUI;

namespace AvalonStudio.Controls
{
    public abstract class EditorViewModel : DocumentTabViewModel
    {
        private bool _isDirty;
        private ISourceFile _sourceFile;

        public bool IsDirty
        {
            get
            {
                return _isDirty;
            }
            set
            {
                this.RaiseAndSetIfChanged(ref _isDirty, value);
            }
        }

        public ISourceFile SourceFile
        {
            get { return _sourceFile; }
            set { this.RaiseAndSetIfChanged(ref _sourceFile, value); }
        }
    }
}
