using Avalonia.Controls;
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
}
