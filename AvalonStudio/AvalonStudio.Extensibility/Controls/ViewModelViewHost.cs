namespace AvalonStudio.Controls
{
    using Avalonia;
    using Avalonia.Controls;
    using Avalonia.Controls.Presenters;
    using ReactiveUI;
    using Splat;
    using System;
    using System.Linq;
    using System.Reactive;
    using System.Reactive.Linq;
    using System.Reactive.Subjects;

    /// <summary>
    /// This content control will automatically load the View associated with
    /// the ViewModel property and display it. This control is very useful
    /// inside a DataTemplate to display the View associated with a ViewModel.
    /// </summary>
    public class ViewModelViewHost : ContentPresenter, IViewFor, IEnableLogger, IActivationForViewFetcher
    {
        /// <summary>
        /// The ViewModel to display
        /// </summary>
        public object ViewModel
        {
            get { return GetValue(ViewModelProperty); }
            set { SetValue(ViewModelProperty, value); }
        }

        public static readonly AvaloniaProperty ViewModelProperty = AvaloniaProperty.Register<ViewModelViewHost, object>(nameof(ViewModel), null, false, Avalonia.Data.BindingMode.OneWay, null, somethingChanged);            

        readonly Subject<Unit> updateViewModel = new Subject<Unit>();

        /// <summary>
        /// If no ViewModel is displayed, this content (i.e. a control) will be displayed.
        /// </summary>
        public object DefaultContent
        {
            get { return GetValue(DefaultContentProperty); }
            set { SetValue(DefaultContentProperty, value); }
        }

        public static readonly AvaloniaProperty DefaultContentProperty = AvaloniaProperty.Register<ViewModelViewHost, object>(nameof(DefaultContent), null, false, Avalonia.Data.BindingMode.OneWay, null, somethingChanged);


        public IObservable<string> ViewContractObservable
        {
            get { return (IObservable<string>)GetValue(ViewContractObservableProperty); }
            set { SetValue(ViewContractObservableProperty, value); }
        }

        public static readonly AvaloniaProperty ViewContractObservableProperty = AvaloniaProperty.Register<ViewModelViewHost, IObservable<string>>(nameof(ViewContractObservable), Observable.Return(default(string)));

        private string viewContract;

        public string ViewContract
        {
            get { return this.viewContract; }
            set { ViewContractObservable = Observable.Return(value); }
        }

        //public IViewLocator ViewLocator { get; set; }

        public ViewModelViewHost()
        {
            
        }

        protected override void OnDataContextChanged()
        {
            if (DataContext != null)
            {
                Content = AvalonStudio.MVVM.ViewLocator.Build(DataContext);
            }
        }

        static void somethingChanged(IAvaloniaObject dependencyObject, bool changed)
        {
            if (changed)
            {
                ((ViewModelViewHost)dependencyObject).updateViewModel.OnNext(Unit.Default);
            }
        }

        public int GetAffinityForView(Type view)
        {
            throw new NotImplementedException();
        }

        public IObservable<bool> GetActivationForView(IActivatable view)
        {
            throw new NotImplementedException();
        }
    }

}
