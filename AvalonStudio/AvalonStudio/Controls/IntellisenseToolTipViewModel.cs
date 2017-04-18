using AvalonStudio.MVVM;
using ReactiveUI;
using System.Collections.ObjectModel;

namespace AvalonStudio.Controls
{
    public class IntellisenseToolTipViewModel : ViewModel
    {
        private ObservableCollection<string> completionAdvices;

        private int selectedIndex;

        private bool visible;

        public IntellisenseToolTipViewModel()
        {
            CompletionAdvices = new ObservableCollection<string>();
        }

        public int SelectedIndex
        {
            get
            {
                return selectedIndex;
            }
            set
            {
                if (value >= 0 && value < CompletionAdvices.Count)
                {
                    selectedIndex = value;

                    this.RaisePropertyChanged(nameof(SelectedAdvice));
                    this.RaisePropertyChanged(nameof(ListPositionVisibility));
                    this.RaisePropertyChanged(nameof(ListPositionText));
                }
            }
        }

        public string SelectedAdvice
        {
            get
            {
                if (CompletionAdvices.Count > 0)
                {
                    return CompletionAdvices[SelectedIndex];
                }
                return string.Empty;
            }
        }

        public ObservableCollection<string> CompletionAdvices
        {
            get { return completionAdvices; }
            set { this.RaiseAndSetIfChanged(ref completionAdvices, value); }
        }

        public string ListPositionText
        {
            get { return string.Format("{0} of {1}", SelectedIndex + 1, CompletionAdvices.Count); }
        }

        public bool ListPositionVisibility
        {
            get
            {
                if (CompletionAdvices.Count > 1)
                {
                    return true;
                }
                return false;
            }
        }

        public bool Visible
        {
            get { return visible; }
            set { this.RaiseAndSetIfChanged(ref visible, value); }
        }
    }
}