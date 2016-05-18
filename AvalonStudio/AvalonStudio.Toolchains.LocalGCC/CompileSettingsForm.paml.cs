using Avalonia.Controls;
using Avalonia;
using Avalonia.Markup.Xaml;

namespace AvalonStudio.Toolchains.LocalGCC
{
    public class CompileSettingsForm : TabItem
    {
        public CompileSettingsForm()
        {
            this.InitializeComponent();
        }

        private void InitializeComponent()
        {
            Avalonia.Markup.Xaml.AvaloniaXamlLoader.Load(this);
        }
    }
}
