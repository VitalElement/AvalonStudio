using AvalonStudio.Documents;
using AvalonStudio.Extensibility.Dialogs;
using Dock.Model;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AvalonStudio.Shell
{
    public interface IShell
    {
        IDocumentTabViewModel SelectedDocument { get; set; }

        ModalDialogViewModelBase ModalDialog { get; set; }        

        void AddDocument(IDocumentTabViewModel document, bool temporary = true);

        void RemoveDocument(IDocumentTabViewModel document);

        IReadOnlyList<IDocumentTabViewModel> Documents { get; }

        IDock Layout { get; }        
    }
}