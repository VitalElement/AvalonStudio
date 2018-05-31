using Avalonia.Controls;
using AvalonStudio.Documents;
using AvalonStudio.Extensibility;
using AvalonStudio.MVVM;
using AvalonStudio.Shell;
using ReactiveUI;

namespace AvalonStudio.Controls
{
    public abstract class DocumentTabViewModel<T> : ViewModel<T>, IDocumentTabViewModel where T : class
    {
        private Avalonia.Controls.Dock dock;
        private string title;
        private bool _isTemporary;
        private bool _isHidden;
        private bool _isSelected;

        public DocumentTabViewModel(T model) : base(model)
        {
            Dock = Avalonia.Controls.Dock.Left;

            IsVisible = true;
        }

        public Avalonia.Controls.Dock Dock
        {
            get { return dock; }
            set { this.RaiseAndSetIfChanged(ref dock, value); }
        }

        public string Title
        {
            get => title;
            set
            {
                this.RaiseAndSetIfChanged(ref title, value);
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
                if (value)
                {
                    Dock = Avalonia.Controls.Dock.Right;
                }
                else
                {
                    Dock = Avalonia.Controls.Dock.Left;
                }

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

        public virtual void Close()
        {
            IoC.Get<IShell>().RemoveDocument(this);
        }
    }
}