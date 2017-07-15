using Avalonia.Media;
using Avalonia.Threading;
using AvalonStudio.Documents;
using AvalonStudio.MVVM;
using ReactiveUI;
using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Threading.Tasks;

namespace AvalonStudio.Controls
{
    public class DocumentTabControlViewModel : ViewModel
    {
        private ObservableCollection<IDocumentTabViewModel> documents;        

        private IDocumentTabViewModel selectedDocument;

        public DocumentTabControlViewModel()
        {
            Documents = new ObservableCollection<IDocumentTabViewModel>();
            Documents.CollectionChanged += (sender, e) =>
            {
                if (e.Action == NotifyCollectionChangedAction.Remove)
                {
                    Dispatcher.UIThread.InvokeAsync(async () =>
                    {
                        await Task.Delay(25);
                        GC.Collect();
                    });
                }
            };
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
                    editor.Focus();
                    editor.TriggerCodeAnalysis();
                }
            }
        }

        public EditorViewModel TemporaryDocument { get; set; }
    }
}