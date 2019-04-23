using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using System.Collections;

namespace AvalonStudio.Debugging
{
    public class DisassemblyView : UserControl
    {
        public DisassemblyView()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}