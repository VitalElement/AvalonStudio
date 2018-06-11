namespace AvalonStudio.MVVM
{
    using ReactiveUI;

    public abstract class ToolViewModel<T> : ToolViewModel
    {
        private T _model;

        protected ToolViewModel(T model)
        {
            _model = model;
        }

        public new T Model
        {
            get { return _model; }
            set { this.RaiseAndSetIfChanged(ref _model, value); }
        }
    }
}
