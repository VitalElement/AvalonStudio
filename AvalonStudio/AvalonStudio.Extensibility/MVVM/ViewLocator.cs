using Avalonia.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace AvalonStudio.MVVM
{
    public class ViewLocator
    {

        public static IControl Build(object data)
        {   
            var name = data.GetType().FullName.Replace("ViewModel", "View");

            var assemblies = AppDomain.CurrentDomain.GetAssemblies().Where(a=>name.Contains(a.GetName().Name));
            
            Type type = null;

            foreach(var assembly in assemblies)
            {
                type = assembly.GetType(name);

                if(type != null)
                {
                    break;
                }
            }
            
            if (type != null)
            {
                return (Control)Activator.CreateInstance(type);
            }
            else
            {
                return new TextBlock { Text = data.GetType().FullName };
            }
        }
    }

}
