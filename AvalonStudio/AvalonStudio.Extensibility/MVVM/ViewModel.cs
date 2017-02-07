using System.Composition;
using ReactiveUI;
using System;
using System.Reactive.Linq;

namespace AvalonStudio.MVVM
{
    public interface IActivatable
    {
        void Activate();
    }

    public enum Location
    {
        Left,
        Right,
        Bottom,
        BottomRight,
        RightBottom,
        RightMiddle,
        RightTop,
        MiddleTop,
    }


    //[InheritedExport(typeof(ToolViewModel))]
    public abstract class ToolViewModel : ViewModel
    {
        private bool _isVisible;

        // TODO This should use ToolControl
        private string _title;

        protected ToolViewModel()
        {
            _isVisible = true;

            IsVisibleObservable = this.ObservableForProperty(x => x.IsVisible).Select(x => x.Value);
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

    public abstract class ToolViewModel<T> : ToolViewModel
    {
        private T _model;

        protected ToolViewModel(T model)
        {
            _model = model;
        }

        public new T Model
        {
            get { return _model; }
            set { this.RaiseAndSetIfChanged(ref _model, value); }
        }
    }

    public abstract class ViewModel : ViewModel<object>
    {
        protected ViewModel() : base(null)
        {
        }
    }

    public abstract class HeaderedViewModel : HeaderedViewModel<object>
    {
        protected HeaderedViewModel(string header) : base(header, null)
        {
        }
    }

    public abstract class HeaderedViewModel<T> : ViewModel<T>
    {
        protected HeaderedViewModel(string header, T model) : base(model)
        {
            Title = header;
        }

        public string Title { get; private set; }
    }

    public abstract class ViewModel<T> : ReactiveObject
    {
        private T _model;

        protected ViewModel(T model)
        {
            _model = model;
        }

        public T Model
        {
            get { return _model; }
            set
            {
                this.RaiseAndSetIfChanged(ref _model, value);
                Invalidate();
            }
        }

        public void Invalidate()
        {
            this.RaisePropertyChanged("");
        }
    }
}