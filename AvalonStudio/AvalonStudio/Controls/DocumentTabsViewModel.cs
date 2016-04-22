namespace AvalonStudio.Controls
{
    using AvalonStudio.MVVM;
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using ReactiveUI;

    public class DocumentTabsViewModel : ViewModel
    {
        public DocumentTabsViewModel()
        {
            Documents = new ObservableCollection<EditorViewModel>();
        }

        private ObservableCollection<EditorViewModel> documents;
        public ObservableCollection<EditorViewModel> Documents
        {
            get { return documents; }
            set { this.RaiseAndSetIfChanged(ref documents, value); }
        }

        private EditorViewModel selectedDocument;
        public EditorViewModel SelectedDocument
        {
            get { return selectedDocument; }
            set { this.RaiseAndSetIfChanged(ref selectedDocument, value); value?.Model.Editor?.Focus(); }
        }

        private EditorViewModel temporaryDocument;
        public EditorViewModel TemporaryDocument
        {
            get { return temporaryDocument; }
            set { temporaryDocument = value; }
        }

    }
}
