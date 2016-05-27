namespace AvalonStudio.Controls
{
    using AvalonStudio.MVVM;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using ReactiveUI;
    using Avalonia;
    using Languages;
    using Avalonia.Input;
    using Avalonia.Threading;

    public class CompletionAdviceViewModel : ViewModel, ICompletionAdviceControl
    {
        public CompletionAdviceViewModel()
        {

        }

        private Thickness position;
        public Thickness Position
        {
            get { return position; }
            set { this.RaiseAndSetIfChanged(ref position, value); }
        }

        private bool isVisible;
        public bool IsVisible
        {
            get { return isVisible; }
            set { this.RaiseAndSetIfChanged(ref isVisible, value); }
        }

        private Symbol symbol;
        public Symbol Symbol
        {
            get { return symbol; }
            set
            {
                this.RaiseAndSetIfChanged(ref symbol, value);
                this.RaisePropertyChanged(nameof(ListPositionText));
                this.RaisePropertyChanged(nameof(ListPositionVisibility));
                this.RaisePropertyChanged(nameof(SelectedAdvice));
            }
        }

        public string SelectedAdvice
        {
            get
            {
                return symbol?.Name;
            }
        }

        public string ListPositionText
        {
            get
            {
                return string.Format("{0} of {1}", SelectedIndex + 1, Count);
            }
        }

        public bool ListPositionVisibility
        {
            get
            {
                if (Count > 1)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        private int selectedIndex;
        public int SelectedIndex
        {
            get { return selectedIndex; }
            set { this.RaiseAndSetIfChanged(ref selectedIndex, value); }
        }

        private int count;
        public int Count
        {
            get { return count; }
            set { this.RaiseAndSetIfChanged(ref count, value); }
        }
    }
}
