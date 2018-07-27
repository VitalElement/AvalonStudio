using AvalonStudio.Controls.Standard.CodeEditor;
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

        public ITextDocumentTabViewModel CreateViewModel(ISourceFile file, ITextDocument document)
        {
            return new TextEditorViewModel(document, file) { IsReadOnly = true };
        }
    }
}
