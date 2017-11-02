using Avalonia.Controls;
using AvalonStudio.Projects;
using ReactiveUI;

namespace AvalonStudio.Controls
{
    public interface IFileDocumentTabViewModel : IDocumentTabViewModel
    {
        ISourceFile File { get; }
    }

    public interface IDocumentTabViewModel
    {
        bool IsDirty { get; set; }

        string Title { get; set; }

        ReactiveCommand CloseCommand { get; }

        bool IsTemporary { get; set; }

        bool IsVisible { get; set; }

        bool IsSelected { get; set; }

        Dock Dock { get; set; }

        void OnClose();
    }
}