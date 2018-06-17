using Avalonia.Controls;
using AvalonStudio.Documents;
using AvalonStudio.Extensibility;
using AvalonStudio.MVVM;
using AvalonStudio.Shell;
using Dock.Model;
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

            Width = double.NaN;
            Height = double.NaN;
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

        /// <summary>
        /// Gets or sets view id.
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Gets or sets view context.
        /// </summary>
        public object Context { get; set; }

        /// <summary>
        /// Gets or sets view width.
        /// </summary>
        public double Width { get; set; }

        /// <summary>
        /// Gets or sets view height.
        /// </summary>
        public double Height { get; set; }

        /// <summary>
        /// Gets or sets view parent.
        /// </summary>
        /// <remarks>If parrent is <see cref="null"/> than view is root.</remarks>
        public IView Parent { get; set; }
    }
}