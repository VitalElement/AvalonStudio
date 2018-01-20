using AvalonStudio.Controls;
using AvalonStudio.Documents;
using AvalonStudio.Extensibility.Plugin;
using AvalonStudio.Projects;

namespace AvalonStudio.Extensibility.Editor
{
    public interface IEditorProvider : IExtension
    {
        bool CanEdit(ISourceFile file);

        IFileDocumentTabViewModel CreateViewModel(ISourceFile file);
    }
}
