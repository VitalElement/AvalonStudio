using ReactiveUI;
using System;
using System.Threading.Tasks;

namespace AvalonStudio.Extensibility.Dialogs
{
    public class ModalDialogViewModelBase : ReactiveObject
    {
        private bool cancelButtonVisible;

        private bool isVisible;

        private bool okButtonVisible;

        private string title;

        private TaskCompletionSource<bool> dialogCloseCompletionSource;

        public ModalDialogViewModelBase(string title, bool okButton = true, bool cancelButton = true)
        {
            OKButtonVisible = okButton;
            CancelButtonVisible = cancelButton;

            isVisible = false;
            this.title = title;

            CancelCommand = ReactiveCommand.Create();
            CancelCommand.Subscribe(_ => { Close(false); });
        }

        public bool CancelButtonVisible
        {
            get { return cancelButtonVisible; }
            set { this.RaiseAndSetIfChanged(ref cancelButtonVisible, value); }
        }

        public bool OKButtonVisible
        {
            get { return okButtonVisible; }
            set { this.RaiseAndSetIfChanged(ref okButtonVisible, value); }
        }

        public virtual ReactiveCommand<object> OKCommand { get; protected set; }
        public ReactiveCommand<object> CancelCommand { get; }

        public string Title
        {
            get { return title; }
            private set { this.RaiseAndSetIfChanged(ref title, value); }
        }

        public bool IsVisible
        {
            get { return isVisible; }
            set { this.RaiseAndSetIfChanged(ref isVisible, value); }
        }

        public Task<bool> ShowDialog()
        {
            IsVisible = true;

            dialogCloseCompletionSource = new TaskCompletionSource<bool>();

            return dialogCloseCompletionSource.Task;
        }

        public void Close(bool success = true)
        {
            IsVisible = false;

            dialogCloseCompletionSource.SetResult(success);
        }
    }

    public abstract class ModalDialogReactiveObject<T> : ModalDialogViewModelBase
    {
        public ModalDialogReactiveObject(T model, string title) : base(title)
        {
            Model = model;
        }

        public T Model { get; private set; }
    }
}