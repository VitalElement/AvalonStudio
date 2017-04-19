using System;
using System.Collections.Generic;
using System.Text;

namespace AvalonStudio.Extensibility.ToolBars
{
    public class ToolBarItemDefinition<TCommand> : ToolBarItemDefinition where TCommand : CommandDefinition
    {
        public ToolBarItemDefinition(ToolBarItemGroupDefinition group, int sortOrder, ToolBarItemDisplay display = ToolBarItemDisplay.IconOnly) : base(group, sortOrder, display)
        {
        }

        public override void Activation()
        {
            base.Activation();

            CommandDefinition = IoC.Get<TCommand>();
        }
    }
}
