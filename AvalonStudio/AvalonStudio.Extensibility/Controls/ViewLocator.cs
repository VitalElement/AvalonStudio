using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Presenters;
using Avalonia.Controls.Templates;
using System;

namespace AvalonStudio.Controls
{
    public class ViewLocatorDataTemplate : IDataTemplate
    {
        public bool SupportsRecycling => throw new System.NotImplementedException();

        public IControl Build(object data)
        {
            var name = data.GetType().FullName.Replace("ViewModel", "View");
            var type = Type.GetType(name);

            if (type != null)
            {
                return (Control)Activator.CreateInstance(type);
            }
            else
            {
                return new TextBlock { Text = name };
            }
        }

        public bool Match(object data)
        {
            return true;
        }
    }

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
