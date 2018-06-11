using Avalonia.Controls;
using Avalonia.Controls.Templates;
using System;

namespace AvalonStudio.Controls
{
    public class ViewLocatorDataTemplate : IDataTemplate
    {
        public bool SupportsRecycling => false;

        public IControl Build(object data)
        {
            var name = data.GetType().FullName.Replace("ViewModel", "View");
            var type = Type.GetType(name);

            if (type != null)
            {
                if (typeof(Control).IsAssignableFrom(type))
                {
                    var constructor = type.GetConstructor(Type.EmptyTypes);

                    if (constructor != null)
                    {
                        return (Control)Activator.CreateInstance(type);
                    }
                }                
            }
            
            return new TextBlock { Text = name };            
        }

        public bool Match(object data)
        {
            return true;
        }
    }
}
