using AvalonStudio.Documents;
using AvalonStudio.Projects;

namespace AvalonStudio.Shell
{
    public interface IEditorProvider
    {
        bool CanEdit(ISourceFile file);
        IFileDocumentTabViewModel CreateViewModel(ISourceFile file);
    }
}
