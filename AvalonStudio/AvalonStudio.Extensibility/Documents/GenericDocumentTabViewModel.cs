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
            CloseCommand = ReactiveCommand.Create();

            CloseCommand.Subscribe((o) =>
            {
                IoC.Get<IShell>().RemoveDocument(this);
            });
        }

        private Dock dock;
        public Dock Dock
        {
            get { return dock; }
            set { this.RaiseAndSetIfChanged(ref dock, value); }
        }

        public ReactiveCommand<object> CloseCommand { get; protected set; }

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


    }
}
