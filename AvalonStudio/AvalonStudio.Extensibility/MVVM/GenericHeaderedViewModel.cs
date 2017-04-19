namespace AvalonStudio.MVVM
{
    public abstract class HeaderedViewModel<T> : ViewModel<T>
    {
        protected HeaderedViewModel(string header, T model) : base(model)
        {
            Title = header;
        }

        public string Title { get; private set; }
    }
}
