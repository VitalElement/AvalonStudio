using ReactiveUI;
using System;
using System.Reactive.Linq;

namespace AvalonStudio.MVVM
{
    public abstract class ToolViewModel : ViewModel
    {
        private bool _isVisible;
        
        private string _title;

        protected ToolViewModel(string title)
        {
            _isVisible = true;

            IsVisibleObservable = this.ObservableForProperty(x => x.IsVisible).Select(x => x.Value);

            _title = title;
        }

        public IObservable<bool> IsVisibleObservable { get; }

        public bool IsVisible
        {
            get { return _isVisible; }
            set { this.RaiseAndSetIfChanged(ref _isVisible, value); }
        }

        public abstract Location DefaultLocation { get; }

        public string Title
        {
            get { return _title; }
            set { this.RaiseAndSetIfChanged(ref _title, value); }
        }
    }
}