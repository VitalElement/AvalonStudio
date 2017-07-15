using Avalonia.Controls;
using ReactiveUI;

namespace AvalonStudio.Controls
{
    public interface IDocumentTabViewModel
    {
        bool IsDirty { get; set; }

        string Title { get; set; }

        ReactiveCommand<object> CloseCommand { get; }

        bool IsTemporary { get; set; }

        bool IsVisible { get; set; }

        Dock Dock { get; set; }

        void OnClose();
    }
}