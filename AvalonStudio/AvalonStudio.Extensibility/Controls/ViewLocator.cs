using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Presenters;

namespace AvalonStudio.Controls
{
    public static class View
    {
        static View()
        {
            ViewProperty.Changed.AddClassHandler<ContentPresenter>((presenter, args) =>
            {
                var text = "null";

                if(args.NewValue != null)
                {
                    text = args.NewValue.GetType().FullName;
                }

                presenter.Content = new TextBlock { Text = text };
            });
        }

        public static readonly AttachedProperty<object> ViewProperty = AvaloniaProperty
            .RegisterAttached<ContentPresenter, object>("View", typeof(View));

        public static void SetView(ContentPresenter control, object value)
        {
            control.SetValue(ViewProperty, value);
        }

        public static object GetView(ContentPresenter control)
        {
            return control.GetValue(ViewProperty);
        }
    }
}
