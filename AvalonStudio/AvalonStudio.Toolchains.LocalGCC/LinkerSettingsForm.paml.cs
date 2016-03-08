using Perspex.Controls;
using Perspex.Markup.Xaml;

namespace AvalonStudio.Toolchains.LocalGCC
{
    public class LinkerSettingsForm : TabItem
    {
        public LinkerSettingsForm()
        {
            this.InitializeComponent();
        }

        private void InitializeComponent()
        {
            PerspexXamlLoader.Load(this);
        }
    }
}
