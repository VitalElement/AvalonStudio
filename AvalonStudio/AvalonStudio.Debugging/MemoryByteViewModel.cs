namespace AvalonStudio.Debugging
{
    using AvalonStudio.MVVM;
    using ReactiveUI;

    public class MemoryValueViewModel<T> : ViewModel
    {
        public MemoryValueViewModel(ulong address, T value, string formatString)
        {
            this.address = address;
            this.dataValue = value;
            this.formatString = formatString;
        }

        private string formatString = string.Empty;

        private ulong address;
        public ulong Address
        {
            get { return address; }
            set { this.RaiseAndSetIfChanged(ref address, value); }
        }

        private T dataValue;
        public T Value
        {
            get { return dataValue; }
            set { this.RaiseAndSetIfChanged(ref dataValue, value); this.RaisePropertyChanged(nameof(FormattedValue)); }
        }

        private bool hasChanged;
        public bool HasChanged
        {
            get { return hasChanged; }
            set { this.RaiseAndSetIfChanged(ref hasChanged, value); }
        }


        public string FormattedValue
        {
            get
            {
                return string.Format(formatString, dataValue);
            }
        } 
    }
}
