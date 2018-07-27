using AvalonStudio.Documents;
using AvalonStudio.Extensibility.Editor;
using AvalonStudio.Projects;
using AvalonStudio.Shell;
using System.IO;

namespace AvalonStudio.Languages.CPlusPlus
{
    [ExportEditorProvider]
    internal class CPlusPlusEditorPropvider : IEditorProvider
    {
        public bool CanEdit(ISourceFile file)
        {
            switch (Path.GetExtension(file.FilePath))
            {
                case ".h":
                case ".c":
                case ".cpp":
                case ".hpp":
                    return true;
            }

            return false;
        }

        public ITextDocumentTabViewModel CreateViewModel(ISourceFile file, ITextDocument document)
        {
            return new CodeEditorViewModel(document, file);
        }
    }
}
