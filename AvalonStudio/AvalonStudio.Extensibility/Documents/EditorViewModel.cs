using AvalonStudio.Projects;
using ReactiveUI;
using System;

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

        ~EditorViewModel()
        {
            Console.WriteLine("Dispose VM");
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
        
        public override void Save()
        {
            IsDirty = false;
        }

        public ISourceFile SourceFile
        {
            get { return _sourceFile; }
            set { this.RaiseAndSetIfChanged(ref _sourceFile, value); }
        }        
    }
}
