using AvalonStudio.Documents;
using AvalonStudio.Projects;

namespace AvalonStudio.Extensibility.Editor
{
    public interface IEditorProvider
    {
        bool CanEdit(ISourceFile file);

        IFileDocumentTabViewModel CreateViewModel(ISourceFile file);
    }
}
