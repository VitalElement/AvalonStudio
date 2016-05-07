namespace AvalonStudio.Extensibility.MainToolBar
{
    using AvalonStudio.Extensibility.ToolBars;
    using Commands;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.Composition;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public static class ToolBarDefinitions
    {
        [Export]
        public static ToolBarDefinition MainToolBar = new ToolBarDefinition(0, "Main");
    }
}
