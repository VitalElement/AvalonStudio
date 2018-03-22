using AvalonStudio.Extensibility.Plugin;
using AvalonStudio.MVVM;
using System;

namespace AvalonStudio.Controls.Standard.Terminal
{
    public class TerminalViewModel : ToolViewModel, IExtension
    {
        public TerminalViewModel() : base("Terminal")
        {            
        }

        public override Location DefaultLocation => Location.BottomRight;

        public void Activation()
        {
        }

        public void BeforeActivation()
        {
        }
    }
}
