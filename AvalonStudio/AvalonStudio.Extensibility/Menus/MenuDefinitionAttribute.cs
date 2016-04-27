using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AvalonStudio.Extensibility.Menus
{
    public class MenuDefinitionAttribute : ExportAttribute
    {
        public MenuDefinitionAttribute()
            : base(typeof(MenuDefinitionBase))
        {

        }
    }
}
