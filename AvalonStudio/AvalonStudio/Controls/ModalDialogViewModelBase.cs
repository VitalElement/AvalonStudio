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

            visible = false;
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

        private bool visible;
        public bool Visible
        {
            get { return visible; }
            set { this.RaiseAndSetIfChanged(ref visible, value); }
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

    public abstract class ModalDialogReactiveObject<T> : ModalDialogViewModelBase
    {
        public ModalDialogReactiveObject(T model, string title) : base(title)
        {
            this.Model = model;
        }

        public T Model { get; private set; }
    }
}
