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
            if (Documents.Count() > 0)
            {
                SeperatorVisible = true;
            }
            else
            {
                SeperatorVisible = false;
            }
        }

        public void OpenDocument(IDocumentTabViewModel document)
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
                IDocumentTabViewModel documentToClose = null;

                if (TemporaryDocument != null)
                {
                    documentToClose = TemporaryDocument;
                }

                document.IsTemporary = true;
                Documents.Add(document);
                SelectedDocument = document;
                TemporaryDocument = document;
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
                return selectedDocument;
            }
            set
            {
                this.RaiseAndSetIfChanged(ref selectedDocument, value);

                if (value is IEditor editor)
                {
                    editor.Focus();
                    editor.TriggerCodeAnalysis();
                }
            }
        }

        public IDocumentTabViewModel TemporaryDocument { get; set; }
    }
}