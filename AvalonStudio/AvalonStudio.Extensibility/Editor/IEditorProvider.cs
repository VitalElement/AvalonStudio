using AvalonStudio.Controls;
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
