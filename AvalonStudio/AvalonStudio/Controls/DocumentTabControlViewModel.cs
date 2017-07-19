using Avalonia.Media;
using Avalonia.Threading;
using AvalonStudio.Documents;
using AvalonStudio.MVVM;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Threading.Tasks;

namespace AvalonStudio.Controls
{
    public class DocumentTabControlViewModel : ViewModel
    {
        private ObservableCollection<IDocumentTabViewModel> documents;        

        private bool _seperatorVisible;
        private IDocumentTabViewModel selectedDocument;

        public DocumentTabControlViewModel()
        {
            Documents = new ObservableCollection<IDocumentTabViewModel>();            
        }        

        public void InvalidateSeperatorVisibility()
        {
            if(Documents.Where(d=>d.IsVisible).Count() > 0)
            {
                SeperatorVisible = true;
            }
            else
            {
                SeperatorVisible = false;
            }
        }

        public bool SeperatorVisible
        {
            get { return _seperatorVisible; }
            set { this.RaiseAndSetIfChanged(ref _seperatorVisible, value); }
        }

        public ObservableCollection<IDocumentTabViewModel> Documents
        {
            get { return documents; }
            set { this.RaiseAndSetIfChanged(ref documents, value); }
        }

        public IDocumentTabViewModel SelectedDocument
        {
            get
            {
                return selectedDocument;
            }
            set
            {
                selectedDocument = value;

                this.RaisePropertyChanged(nameof(SelectedDocument));

                if (value is IEditor editor)
                {
                    editor.TriggerCodeAnalysis();
                }
            }
        }

        public EditorViewModel TemporaryDocument { get; set; }
    }
}