using AvalonStudio.Documents;
using AvalonStudio.Projects;
using System.Threading.Tasks;

namespace AvalonStudio.Shell
{
    public interface IEditorProvider
    {
        bool CanEdit(ISourceFile file);
        Task<ITextDocumentTabViewModel> CreateViewModel(ISourceFile file);
    }
}
