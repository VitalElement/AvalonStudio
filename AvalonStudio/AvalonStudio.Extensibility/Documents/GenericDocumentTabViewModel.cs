using Avalonia.Controls;
using AvalonStudio.Extensibility;
using AvalonStudio.MVVM;
using AvalonStudio.Projects;
using AvalonStudio.Shell;
using ReactiveUI;

namespace AvalonStudio.Controls
{
    public abstract class DocumentTabViewModel<T> : ViewModel<T>, IDocumentTabViewModel where T : class
    {
        private Dock dock;
        private string title;
        private bool isDirty;
        private bool _isTemporary;
        private bool _isHidden;
        private bool _isSelected;

        public DocumentTabViewModel(T model) : base(model)
        {
            CloseCommand = ReactiveCommand.Create(() =>
            {
                IoC.Get<IShell>().RemoveDocument(this);
            });

            IsVisible = true;
        }

        public Dock Dock
        {
            get { return dock; }
            set { this.RaiseAndSetIfChanged(ref dock, value); }
        }

        public ReactiveCommand CloseCommand { get; protected set; }

        public string Title
        {
            get
            {
                if (IsDirty)
                {
                    return title + "*";
                }
                else
                {
                    return title;
                }
            }
            set
            {
                this.RaiseAndSetIfChanged(ref title, value);
            }
        }

        public bool IsDirty
        {
            get
            {
                return isDirty;
            }
            set
            {
                this.RaiseAndSetIfChanged(ref isDirty, value);
                this.RaisePropertyChanged(nameof(Title));
            }
        }

        public bool IsTemporary
        {
            get
            {
                return _isTemporary;
            }
            set
            {
                this.RaiseAndSetIfChanged(ref _isTemporary, value);
            }
        }

        public bool IsVisible
        {
            get { return _isHidden; }
            set { this.RaiseAndSetIfChanged(ref _isHidden, value); }
        }

        public bool IsSelected
        {
            get { return _isSelected; }
            set { this.RaiseAndSetIfChanged(ref _isSelected, value); }
        }

        public virtual void OnClose()
        {
        }
    }
}