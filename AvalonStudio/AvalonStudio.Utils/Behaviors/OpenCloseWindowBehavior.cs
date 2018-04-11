using Avalonia;
using Avalonia.Controls;
using Avalonia.Data;
using Avalonia.Xaml.Interactivity;
using System;
using System.Reactive.Disposables;

namespace AvalonStudio.Utils.Behaviors
{
    class OpenCloseWindowBehavior : Behavior<Control>
    {
        private CompositeDisposable _disposables;
        private Window _currentWindow;

        protected override void OnAttached()
        {
            base.OnAttached();

            _disposables = new CompositeDisposable {
            this.GetObservable(IsOpenProperty).Subscribe(open =>
            {
                if(open && _currentWindow == null)
                {
                    if(WindowType != null)
                    {
                        _currentWindow = Activator.CreateInstance(WindowType) as Window;
                    }
                    else
                    {
                        _currentWindow = Activator.CreateInstance<Window>();
                    }

                    if (DataContext == null)
                    {
                        _currentWindow.DataContext = AssociatedObject.DataContext;
                    }
                    else
                    {
                        _currentWindow.DataContext = DataContext;
                    }

                    _currentWindow.Show();
                }
                else
                {
                    _currentWindow?.Close();
                    _currentWindow = null;
                }
            })};
        }

        protected override void OnDetaching()
        {
            _disposables.Dispose();

            base.OnDetaching();
        }

        public static readonly AvaloniaProperty<Type> WindowTypeProperty =
            AvaloniaProperty.Register<OpenCloseWindowBehavior, Type>(nameof(WindowType), defaultBindingMode: BindingMode.TwoWay);

        public Type WindowType
        {
            get => GetValue(WindowTypeProperty);
            set => SetValue(WindowTypeProperty, value);
        }

        public static readonly StyledProperty<bool> IsOpenProperty =
            AvaloniaProperty.Register<OpenCloseWindowBehavior, bool>(nameof(IsOpen), defaultBindingMode: BindingMode.TwoWay);

        public bool IsOpen
        {
            get => GetValue(IsOpenProperty);
            set => SetValue(IsOpenProperty, value);
        }

        public static readonly StyledProperty<object> DataContextProperty =
            AvaloniaProperty.Register<OpenCloseWindowBehavior, object>(nameof(DataContext), defaultBindingMode: BindingMode.TwoWay);

        public object DataContext
        {
            get => GetValue(DataContextProperty);
            set => SetValue(DataContextProperty, value);
        }
    }
}
