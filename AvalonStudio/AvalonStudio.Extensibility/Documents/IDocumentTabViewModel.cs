using AvalonStudio.Projects;

namespace AvalonStudio.Controls
{
    public interface IFileDocumentTabViewModel : IDocumentTabViewModel
    {
        ISourceFile SourceFile { get; }

        bool IsDirty { get; set; }

        void Save();
    }

    public interface IDocumentTabViewModel
    {
        string Title { get; set; }

        void Close();
    }
}