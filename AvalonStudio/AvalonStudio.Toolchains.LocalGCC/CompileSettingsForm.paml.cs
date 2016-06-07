using Avalonia.Controls;
using Avalonia;
using Avalonia.Markup.Xaml;

namespace AvalonStudio.Toolchains.LocalGCC
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
