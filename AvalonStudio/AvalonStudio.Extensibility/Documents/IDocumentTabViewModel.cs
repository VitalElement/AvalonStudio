using AvalonStudio.Projects;

namespace AvalonStudio.Documents
{
    public interface IFileDocumentTabViewModel : IDocumentTabViewModel
    {
        ISourceFile SourceFile { get; }

        IEditor Editor { get; }
        
        bool IsDirty { get; set; }        
    }

    public interface IDocumentTabViewModel
    {
        string Title { get; set; }

        void Close();
    }
}