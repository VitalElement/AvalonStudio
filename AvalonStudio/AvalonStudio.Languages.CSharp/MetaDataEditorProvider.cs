using AvalonStudio.Documents;
using AvalonStudio.Extensibility.Editor;
using AvalonStudio.Projects;
using AvalonStudio.Shell;

namespace AvalonStudio.Languages.CSharp
{
    [ExportEditorProvider]
    internal class MetaDataEditorProvider : IEditorProvider
    {
        public bool CanEdit(ISourceFile file)
        {
            return file.FilePath.StartsWith("$metadata");
        }

        public IFileDocumentTabViewModel CreateViewModel(ISourceFile file)
        {
            return new TextEditorViewModel(file) { IsReadOnly = true };
        }
    }
}
