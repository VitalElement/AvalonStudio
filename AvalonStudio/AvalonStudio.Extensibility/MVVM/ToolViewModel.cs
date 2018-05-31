using Dock.Model;
using Dock.Model.Controls;
using ReactiveUI;
using System;
using System.Reactive.Linq;

namespace AvalonStudio.MVVM
{
    public abstract class ToolViewModel : ViewModel, IToolTab
    {
        private bool _isVisible;
        private bool _isSelected;

        // TODO This should use ToolControl
        private string _title;

        protected ToolViewModel()
        {
            _isVisible = true;

            IsVisibleObservable = this.ObservableForProperty(x => x.IsVisible).Select(x => x.Value);

            //Height = double.NaN;
            //Width = double.NaN;            
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

        /// <summary>
        /// Gets or sets view id.
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Gets or sets view context.
        /// </summary>
        public object Context { get; set; }

        /// <summary>
        /// Gets or sets view width.
        /// </summary>
        public double Width { get; set; }

        /// <summary>
        /// Gets or sets view height.
        /// </summary>
        public double Height { get; set; }

        /// <summary>
        /// Gets or sets view parent.
        /// </summary>
        /// <remarks>If parrent is <see cref="null"/> than view is root.</remarks>
        public IView Parent { get; set; }
    }
}