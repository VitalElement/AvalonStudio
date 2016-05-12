namespace AvalonStudio.Controls
{
    using Avalonia;
    using Avalonia.Controls;
    using Avalonia.Controls.Presenters;
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;

    public class CachedContentPresenter : ContentPresenter
    {
        private static IDictionary<Type, Func<Control>> Factory;
        private IDictionary<Type, Control> Cache;

        static CachedContentPresenter()
        {
            Factory = new Dictionary<Type, Func<Control>>();

            // Views
            Factory.Add(typeof(EditorViewModel), () => new Editor());
        }

        public CachedContentPresenter()
        {
            Cache = new Dictionary<Type, Control>();

            this.GetObservable(DataContextProperty).Subscribe((value) =>
            {
                Debug.Print($"CachedContentPresenter DataContext Changed: {value}");
                SetContent(value);
            });
        }

        private void SetContent(object value)
        {
            Control control = null;
            object target = value;
            if (target != null)
            {
                control = GetControl(target.GetType());
            }
            this.Content = control;
        }

        private Control CreateControl(Type type)
        {
            Func<Control> createInstance;
            Factory.TryGetValue(type, out createInstance);
            if (createInstance != null)
            {
                var sw = Stopwatch.StartNew();
                var instance = createInstance();
                sw.Stop();
                Debug.Print($"CreateInstance: {type} in {sw.Elapsed.TotalMilliseconds}ms.");
                return instance;
            }
            Debug.Print($"Not Registered: {type}");
            return null;
        }

        private Control GetControl(Type type)
        {
            Control control;
            Cache.TryGetValue(type, out control);
            if (control == null)
            {
                control = CreateControl(type);
                if (control != null)
                {
                    Debug.Print($"New: {type} -> {control}");
                    Cache.Add(type, control);
                    return control;
                }
                Debug.Print($"Failed to create control for type: {type}");
                return null;
            }
            Debug.Print($"Cached: {type} -> {control}");
            return control;
        }
    }

}
