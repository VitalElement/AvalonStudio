namespace AvalonStudio.Controls
{
    using AvalonStudio.MVVM;
    using Perspex.Media;
    using ReactiveUI;
    using System.Collections.ObjectModel;

    public class DocumentTabsViewModel : ViewModel
    {
        public DocumentTabsViewModel()
        {
            Documents = new ObservableCollection<EditorViewModel>();
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
                value?.Model.Editor?.Focus();

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
