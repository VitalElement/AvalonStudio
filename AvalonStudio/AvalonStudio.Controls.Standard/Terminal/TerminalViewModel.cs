using AvalonStudio.Extensibility.Plugin;
using AvalonStudio.MVVM;
using System;

namespace AvalonStudio.Controls.Standard.Terminal
{
    class TerminalViewModel : ToolViewModel, IExtension
    {
        public override Location DefaultLocation => Location.BottomRight;

        public void Activation()
        {
        }

        public void BeforeActivation()
        {
        }
    }
}
