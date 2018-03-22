namespace AvalonStudio.MVVM
{
    using ReactiveUI;

    public abstract class ToolViewModel<T> : ToolViewModel
    {
        private T _model;

        protected ToolViewModel(string title, T model) : base(title)
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
