using AvalonStudio.Projects;
using Dock.Model.Controls;
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

    public interface IDocumentTabViewModel : IDocumentTab
    {
        void Close();
    }
}