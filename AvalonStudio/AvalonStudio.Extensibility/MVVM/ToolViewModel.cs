using ReactiveUI;
using System;
using System.Reactive.Linq;

namespace AvalonStudio.MVVM
{
    public abstract class ToolViewModel : ViewModel
    {
        private bool _isVisible;

        private bool _isSelected;

        private string _title;

        protected ToolViewModel(string title)
        {
            _isVisible = true;

            IsVisibleObservable = this.ObservableForProperty(x => x.IsVisible).Select(x => x.Value);

            _title = title;
        }

        public Action OnSelect { get; set; }

        public IObservable<bool> IsVisibleObservable { get; }

        public bool IsVisible
        {
            get { return _isVisible; }
            set { this.RaiseAndSetIfChanged(ref _isVisible, value); }
        }        

        public bool IsSelected
        {
            get { return _isSelected; }
            set
            {
                _isSelected = value;

                this.RaisePropertyChanged();

                if(value && OnSelect != null)
                {
                    OnSelect();
                }
            }
        }

        public abstract Location DefaultLocation { get; }

        public string Title
        {
            get { return _title; }
            set { this.RaiseAndSetIfChanged(ref _title, value); }
        }
    }
}