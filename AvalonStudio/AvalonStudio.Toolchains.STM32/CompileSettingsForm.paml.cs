using Avalonia.Controls;
using Avalonia;

namespace AvalonStudio.Toolchains.STM32
{
    public class CompileSettingsFormView : UserControl
    {
        public CompileSettingsFormView()
        {
            this.InitializeComponent();
        }

        private void InitializeComponent()
        {
            Avalonia.Markup.Xaml.AvaloniaXamlLoader.Load(this);
        }
    }
}
