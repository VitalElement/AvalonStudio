using Avalonia.Controls;
using AvalonStudio.MVVM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ReactiveUI;

namespace AvalonStudio.Controls
{
    public class DocumentTabViewModel<T> : ViewModel<T>, IDocumentTabViewModel where T : class
    {
        public DocumentTabViewModel(T model) : base(model)
        {
            
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

            set { this.RaiseAndSetIfChanged(ref title, value); }
        }

        private bool isDirty;
        public bool IsDirty
        {
            get { return isDirty; }
            set
            {
                this.RaiseAndSetIfChanged(ref isDirty, value);
                this.RaisePropertyChanged(nameof(Title));
            }
        }


    }

    public interface IDocumentTabViewModel
    {
        bool IsDirty { get; set; }

        string Title { get; set; }

        ReactiveCommand<object> CloseCommand { get; }

        Dock Dock { get; set; }
    }
}
