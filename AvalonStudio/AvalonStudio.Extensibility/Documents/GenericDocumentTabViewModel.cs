using Avalonia.Controls;
using AvalonStudio.Extensibility;
using AvalonStudio.MVVM;
using AvalonStudio.Shell;
using ReactiveUI;
using System;

namespace AvalonStudio.Controls
{
    public class DocumentTabViewModel<T> : ViewModel<T>, IDocumentTabViewModel where T : class
    {
        public DocumentTabViewModel(T model) : base(model)
        {
            CloseCommand = ReactiveCommand.Create(() =>
            {
                IoC.Get<IShell>().RemoveDocument(this);

                OnClose();
            });

            IsVisible = true;
        }

        private Dock dock;

        public Dock Dock
        {
            get { return dock; }
            set { this.RaiseAndSetIfChanged(ref dock, value); }
        }

        public ReactiveCommand CloseCommand { get; protected set; }

        private string title;

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

        private bool isDirty;

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

        private bool _isTemporary;

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

        private bool _isHidden;

        public bool IsVisible
        {
            get { return _isHidden; }
            set { this.RaiseAndSetIfChanged(ref _isHidden, value); }
        }

        public virtual void OnClose()
        {
        }
    }
}