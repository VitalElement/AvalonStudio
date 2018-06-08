using AvalonStudio.Documents;
using AvalonStudio.Extensibility.Dialogs;
using AvalonStudio.Extensibility.Editor;
using AvalonStudio.Projects;
using Dock.Model;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AvalonStudio.Shell
{
    public enum Perspective
    {
        Editor,
        Debug
    }

    public interface IShell
    {
        IDocumentTabViewModel SelectedDocument { get; set; }

        ModalDialogViewModelBase ModalDialog { get; set; }

        ColorScheme CurrentColorScheme { get; set; }

        void AddDocument(IDocumentTabViewModel document, bool temporary = true);

        void RemoveDocument(IDocumentTabViewModel document);

        IReadOnlyList<IDocumentTabViewModel> Documents { get; }

        IDock Layout { get; }

        bool DebugMode { get; }

        double GlobalZoomLevel { get; set; }
    }
}