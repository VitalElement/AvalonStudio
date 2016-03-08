using Perspex.Controls;
using Perspex.Markup.Xaml;

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
            PerspexXamlLoader.Load(this);
        }
    }
}
