namespace AvalonStudio.Controls.ViewModels
{
    using Perspex.MVVM;
    using System.Windows.Input;

    public abstract class ModalDialogViewModelBase : ViewModelBase
    {
        public ModalDialogViewModelBase(string title, bool okButton = true, bool cancelButton = true)
        {
            this.OKButtonVisible = okButton;
                this.CancelButtonVisible = cancelButton;

            this.visible = false;
            this.title = title;
            this.CancelCommand = new RoutingCommand((o) => { this.Close(); });
        }

        private bool cancelButtonVisible = false;
        public bool CancelButtonVisible
        {
            get { return cancelButtonVisible; }
            set { cancelButtonVisible = value; OnPropertyChanged(); }
        }

        private bool okButtonVisible = false;
        public bool OKButtonVisible
        {
            get { return okButtonVisible; }
            set { okButtonVisible = value; OnPropertyChanged(); }
        }

        public abstract ICommand OKCommand { get; protected set; }
        public ICommand CancelCommand { get; private set; }

        private string title;
        public string Title
        {
            get { return title; }
            private set { title = value; OnPropertyChanged(); }
        }

        private bool visible;
        public bool Visible
        {
            get { return visible; }
            set { visible = value; OnPropertyChanged(); }
        }

        public void ShowDialog()
        {
            Workspace.This.HideWhenModalVisibility = false;
            this.Visible = true;
        }

        public void Close()
        {
            this.Visible = false;
            Workspace.This.HideWhenModalVisibility = true;
        }
    }

    public abstract class ModalDialogViewModelBase<T> : ModalDialogViewModelBase
    {
        public ModalDialogViewModelBase(T model, string title) : base(title)
        {
            this.Model = model;
        }

        public T Model { get; private set; }
    }
}
