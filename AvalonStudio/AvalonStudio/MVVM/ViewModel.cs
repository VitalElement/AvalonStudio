namespace AvalonStudio.MVVM
{
    using ReactiveUI;

    public abstract class ViewModel : ViewModel<object>
    {
        public ViewModel(object model) : base(model)
        {

        }
    }

    public abstract class ViewModel<T> : ReactiveObject
    {
        public ViewModel(T model)
        {
            this.model = model;
        }

        private T model;
        public T Model
        {
            get { return model; }
            set { this.RaiseAndSetIfChanged(ref model, value); }
        }
    }
}
