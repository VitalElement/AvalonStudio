namespace AvalonStudio.Extensibility.Dialogs
{
    public abstract class ModalDialogReactiveObject<T> : ModalDialogViewModelBase
    {
        public ModalDialogReactiveObject(T model, string title) : base(title)
        {
            Model = model;
        }

        public T Model { get; private set; }
    }
}