namespace AvalonStudio.Controls
{
    using AvalonStudio.MVVM;
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using ReactiveUI;

    public class IntellisenseToolTipViewModel : ViewModel
    {
        public IntellisenseToolTipViewModel()
        {
            CompletionAdvices = new ObservableCollection<string>();
        }

        private int selectedIndex;
        public int SelectedIndex
        {
            get { return selectedIndex; }
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
                else
                {
                    return string.Empty;
                }
            }
        }

        private ObservableCollection<string> completionAdvices;
        public ObservableCollection<string> CompletionAdvices
        {
            get { return completionAdvices; }
            set { this.RaiseAndSetIfChanged(ref completionAdvices, value); }
        }

        public string ListPositionText
        {
            get
            {
                return string.Format("{0} of {1}", SelectedIndex + 1, CompletionAdvices.Count);
            }
        }

        public bool ListPositionVisibility
        {
            get
            {
                if (CompletionAdvices.Count > 1)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        private bool visible;
        public bool Visible
        {
            get { return visible; }
            set { this.RaiseAndSetIfChanged(ref visible, value); }
        }
    }
}
