namespace AvalonStudio.Controls
{
    using AvalonStudio.MVVM;
    using Avalonia.Media;
    using Avalonia.Threading;
    using ReactiveUI;
    using System;
    using System.Collections.ObjectModel;
    using System.Threading.Tasks;
    public class DocumentTabsViewModel : ViewModel
    {
        public DocumentTabsViewModel()
        {
            Documents = new ObservableCollection<EditorViewModel>();
            Documents.CollectionChanged += (sender, e) =>
            {
                if (e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Remove)
                {
                    Dispatcher.UIThread.InvokeAsync(async () =>
                    {
                        await Task.Delay(25);
                        GC.Collect();
                    });
                }
            };

            tabBrush = Brush.Parse("#007ACC");
            tabHighlightBrush = Brush.Parse("#1c97ea");

            temporaryTabBrush = Brush.Parse("#68217A");
            temporaryTabHighlighBrush = Brush.Parse("#B064AB");            
        } 

        private IBrush temporaryTabHighlighBrush;
        private IBrush temporaryTabBrush;
        private IBrush tabHighlightBrush;
        private IBrush tabBrush;
        private IBrush backgroundBrush;

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
            set
            {
                this.RaiseAndSetIfChanged(ref selectedDocument, value);

                // Dispatcher invoke is hack to make sure the Editor propery has been generated.
                Dispatcher.UIThread.InvokeAsync(() =>
                {
                    value?.Model.Editor?.Focus();
                });
                
                if(value == TemporaryDocument)
                {
                    TabBackgroundBrush = temporaryTabBrush;
                    HoverTabBackgroundBrush = temporaryTabHighlighBrush;
                }
                else
                {
                    TabBackgroundBrush = tabBackgroundBrush;
                    HoverTabBackgroundBrush = tabHighlightBrush;
                }
            }
        }

        private EditorViewModel temporaryDocument;
        public EditorViewModel TemporaryDocument
        {
            get { return temporaryDocument; }
            set { temporaryDocument = value; }
        }

        private IBrush tabBackgroundBrush;
        public IBrush TabBackgroundBrush
        {
            get { return tabBackgroundBrush; }
            set { this.RaiseAndSetIfChanged(ref tabBackgroundBrush, value); }
        }

        private IBrush hoverTabBackgroundBrush;
        public IBrush HoverTabBackgroundBrush
        {
            get { return hoverTabBackgroundBrush; }
            set { this.RaiseAndSetIfChanged(ref hoverTabBackgroundBrush, value); }
        }
    }
}
