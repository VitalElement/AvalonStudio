namespace AvalonStudio.Debugging
{
    using Avalonia.Media;
    using AvalonStudio.MVVM;
    using ReactiveUI;

    public class MemoryValueViewModel<T> : ViewModel
    {
        public MemoryValueViewModel(ulong address, T value, string formatString)
        {
            this.address = address;
            this.dataValue = value;
            this.formatString = formatString;
            this.valueForeground = Brush.Parse("#108930");
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
            get
            {
                return dataValue;
            }
            set
            {
                this.RaiseAndSetIfChanged(ref dataValue, value);
                this.RaisePropertyChanged(nameof(FormattedValue));
            }
        }

        private bool hasChanged;

        public bool HasChanged
        {
            get
            {
                return hasChanged;
            }
            set
            {
                this.RaiseAndSetIfChanged(ref hasChanged, value);

                if (hasChanged)
                {
                    ValueForeground = Brush.Parse("#FFF38B76");
                }
                else
                {
                    ValueForeground = Brush.Parse("#4EC9B0");
                }
            }
        }

        private IBrush valueForeground;

        public IBrush ValueForeground
        {
            get { return valueForeground; }
            set { this.RaiseAndSetIfChanged(ref valueForeground, value); }
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