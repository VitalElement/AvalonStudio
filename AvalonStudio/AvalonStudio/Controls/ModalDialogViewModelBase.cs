namespace AvalonStudio.Controls.ViewModels
{
    using ReactiveUI;
    using System;

    public class ModalDialogViewModelBase : ReactiveObject
    {
        public ModalDialogViewModelBase(string title, bool okButton = true, bool cancelButton = true)
        {
            OKButtonVisible = okButton;
            CancelButtonVisible = cancelButton;

            isVisible = false;
            this.title = title;

            CancelCommand = ReactiveCommand.Create();
            CancelCommand.Subscribe(_ => { Close(); });
        }

        private bool cancelButtonVisible = false;
        public bool CancelButtonVisible
        {
            get { return cancelButtonVisible; }
            set { this.RaiseAndSetIfChanged(ref cancelButtonVisible, value); }
        }

        private bool okButtonVisible = false;
        public bool OKButtonVisible
        {
            get { return okButtonVisible; }
            set { this.RaiseAndSetIfChanged(ref okButtonVisible, value); }
        }

        public virtual ReactiveCommand<object> OKCommand { get; protected set; }
        public ReactiveCommand<object> CancelCommand { get; private set; }

        private string title;
        public string Title
        {
            get { return title; }
            private set { this.RaiseAndSetIfChanged(ref title, value); }
        }

        private bool isVisible;
        public bool IsVisible
        {
            get { return isVisible; }
            set { this.RaiseAndSetIfChanged(ref isVisible, value); }
        }

        public void ShowDialog()
        {
            ShellViewModel.Instance.HideWhenModalVisibility = false;
            this.IsVisible = true;
        }

        public void Close()
        {
            this.IsVisible = false;
            ShellViewModel.Instance.HideWhenModalVisibility = true;
        }
    }

    public abstract class ModalDialogReactiveObject<T> : ModalDialogViewModelBase
    {
        public ModalDialogReactiveObject(T model, string title) : base(title)
        {
            this.Model = model;
        }

        public T Model { get; private set; }
    }
}
