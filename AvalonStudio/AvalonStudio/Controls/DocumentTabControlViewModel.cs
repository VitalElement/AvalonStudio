using Avalonia.Threading;
using AvalonStudio.Documents;
using AvalonStudio.MVVM;
using ReactiveUI;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace AvalonStudio.Controls
{
    public class DocumentTabControlViewModel : ViewModel
    {
        private ObservableCollection<IDocumentTabViewModel> documents;

        private bool _seperatorVisible;
        private IDocumentTabViewModel _selectedDocument;
        private IDocumentTabViewModel _temporaryDocument;

        public DocumentTabControlViewModel()
        {
            Documents = new ObservableCollection<IDocumentTabViewModel>();
        }

        public void InvalidateSeperatorVisibility()
        {
            if (Documents.Count() > 0)
            {
                SeperatorVisible = true;
            }
            else
            {
                SeperatorVisible = false;
            }
        }

        public void OpenDocument(IDocumentTabViewModel document, bool temporary)
        {
            if (document == null)
            {
                return;
            }

            if (Documents.Contains(document))
            {
                SelectedDocument = document;
            }
            else
            {
                if (TemporaryDocument != null)
                {
                    CloseDocument(TemporaryDocument);
                }

                Documents.Add(document);
                SelectedDocument = document;

                if (temporary)
                {
                    TemporaryDocument = document;
                }
            }

            InvalidateSeperatorVisibility();
        }

        public void CloseDocument(IDocumentTabViewModel document)
        {
            if (!Documents.Contains(document))
            {
                return;
            }

            IDocumentTabViewModel newSelectedTab = SelectedDocument;

            if (SelectedDocument == document)
            {
                var index = Documents.IndexOf(document);
                var current = index;

                bool foundTab = false;

                while (!foundTab)
                {
                    index++;

                    if (index < Documents.Count)
                    {
                        foundTab = true;
                        newSelectedTab = Documents[index];
                        break;
                    }
                    else
                    {
                        break;
                    }
                }

                index = current;

                while (!foundTab)
                {
                    index--;

                    if (index >= 0)
                    {
                        foundTab = true;
                        newSelectedTab = Documents[index];
                        break;
                    }
                    else if (index != current)
                    {
                        break;
                    }
                }

                if (foundTab)
                {
                    SelectedDocument = newSelectedTab;
                }
                else
                {
                    SelectedDocument = null;
                }
            }

            if (TemporaryDocument == document)
            {
                TemporaryDocument = null;
            }

            Documents.Remove(document);

            Dispatcher.UIThread.InvokeAsync(async () =>
            {
                await Task.Delay(25);
                GC.Collect();
            });

            InvalidateSeperatorVisibility();
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
                return _selectedDocument;
            }
            set
            {
                this.RaiseAndSetIfChanged(ref _selectedDocument, value);

                if (value is IEditor editor)
                {
                    editor.TriggerCodeAnalysis();
                }
            }
        }

        public IDocumentTabViewModel TemporaryDocument
        {
            get { return _temporaryDocument; }
            set
            {
                this.RaiseAndSetIfChanged(ref _temporaryDocument, value);
            }
        }

    }
}