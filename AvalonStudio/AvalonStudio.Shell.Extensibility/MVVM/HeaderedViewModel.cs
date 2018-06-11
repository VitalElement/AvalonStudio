namespace AvalonStudio.MVVM
{
    public abstract class HeaderedViewModel : HeaderedViewModel<object>
    {
        protected HeaderedViewModel(string header) : base(header, null)
        {
        }
    }
}