namespace AvalonStudio.MVVM
{
    using ReactiveUI;

    public abstract class ViewModel<T> : ReactiveObject
    {
        private T _model;

        protected ViewModel(T model)
        {
            _model = model;
        }

        public T Model
        {
            get
            {
                return _model;
            }
            set
            {
                this.RaiseAndSetIfChanged(ref _model, value);
                Invalidate();
            }
        }

        public void Invalidate()
        {
            this.RaisePropertyChanged("");
        }
    }
}
