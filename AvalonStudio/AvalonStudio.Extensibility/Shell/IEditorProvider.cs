using AvalonStudio.Documents;
using AvalonStudio.Projects;

namespace AvalonStudio.Shell
{
    public interface IEditorProvider
    {
        bool CanEdit(ISourceFile file);
        ITextDocumentTabViewModel CreateViewModel(ISourceFile file, ITextDocument document);
    }
}
