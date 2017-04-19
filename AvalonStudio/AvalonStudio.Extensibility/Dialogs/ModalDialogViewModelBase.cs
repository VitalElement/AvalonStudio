using ReactiveUI;
using System;
using System.Threading.Tasks;

namespace AvalonStudio.Extensibility.Dialogs
{
    public class ModalDialogViewModelBase : ReactiveObject
    {
        private bool cancelButtonVisible;

        private bool isVisible;

        private bool okayButtonVisible;

        private string title;

        private TaskCompletionSource<bool> dialogCloseCompletionSource;

        public ModalDialogViewModelBase(string title, bool okayButton = true, bool cancelButton = true)
        {
            OKButtonVisible = okayButton;
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
            get { return okayButtonVisible; }
            set { this.RaiseAndSetIfChanged(ref okayButtonVisible, value); }
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
}