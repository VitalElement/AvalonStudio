using ReactiveUI;
using System;

namespace AvalonStudio.MVVM
{
    public class MutuallyExclusiveEnumeration<T> : ViewModel where T : struct, IComparable
    {
        public MutuallyExclusiveEnumeration()
        {
            isChecked = false;
        }

        public T Enumeration { get; set; }

        private bool isChecked;

        public bool IsChecked
        {
            get { return isChecked; }
            set { this.RaiseAndSetIfChanged(ref isChecked, value); }
        }

        public string Title { get; set; }
    }
}