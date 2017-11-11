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
                if (TemporaryDocument != null && TemporaryDocument.IsTemporary)
                {
                    CloseDocument(TemporaryDocument);
                }

                document.IsVisible = true;
                Documents.Add(document);
                SelectedDocument = document;

                if (temporary)
                {
                    document.IsTemporary = true;
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
                        if (Documents[index].IsVisible)
                        {
                            foundTab = true;
                            newSelectedTab = Documents[index];
                            break;
                        }
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
                        if (Documents[index].IsVisible)
                        {
                            foundTab = true;
                            newSelectedTab = Documents[index];
                            break;
                        }
                    }
                    else
                    {
                        break;
                    }
                }
            }

            SelectedDocument = newSelectedTab;

            if (TemporaryDocument == document)
            {
                TemporaryDocument = null;
            }

            Documents.Remove(document);
            document.OnClose();

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
                if(_selectedDocument != null)
                {
                    _selectedDocument.IsSelected = false;
                }

                this.RaiseAndSetIfChanged(ref _selectedDocument, value);

                if (value is ICodeEditor editor)
                {
                    editor.TriggerCodeAnalysis();
                }

                if(_selectedDocument != null)
                {
                    value.IsSelected = true;
                }
            }
        }

        public IDocumentTabViewModel TemporaryDocument
        {
            get { return _temporaryDocument; }
            set
            {
                if(_temporaryDocument != null)
                {
                    _temporaryDocument.IsSelected = false;
                }

                this.RaiseAndSetIfChanged(ref _temporaryDocument, value);

                if (_temporaryDocument != null)
                {
                    _temporaryDocument.IsSelected = true;
                }
            }
        }

    }
}