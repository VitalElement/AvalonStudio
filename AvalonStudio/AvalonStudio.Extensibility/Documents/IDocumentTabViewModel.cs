using AvalonStudio.Projects;
using System.Threading.Tasks;

namespace AvalonStudio.Documents
{
    public interface IFileDocumentTabViewModel : IDocumentTabViewModel
    {
        ISourceFile SourceFile { get; }

        IEditor Editor { get; }
        
        bool IsDirty { get; set; }

        Task WaitForEditorToLoadAsync();
    }

    public interface IDocumentTabViewModel
    {
        string Title { get; set; }

        void Close();
    }
}