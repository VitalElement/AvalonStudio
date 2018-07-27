using AvalonStudio.Controls.Standard.CodeEditor;
using AvalonStudio.Documents;
using AvalonStudio.Extensibility.Editor;
using AvalonStudio.Projects;
using AvalonStudio.Shell;
using System.IO;

namespace AvalonStudio.Languages.CSharp
{
    [ExportEditorProvider]
    internal class CsharpEditorProvider : IEditorProvider
    {
        public bool CanEdit(ISourceFile file)
        {
            return Path.GetExtension(file.FilePath) == ".cs";
        }

        public ITextDocumentTabViewModel CreateViewModel(ISourceFile file, ITextDocument document)
        {
            return new CodeEditorViewModel(document, file);
        }
    }

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
