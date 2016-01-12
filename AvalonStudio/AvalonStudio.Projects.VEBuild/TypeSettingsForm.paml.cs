using Perspex.Controls;
using Perspex.Markup.Xaml;

namespace AvalonStudio.Projects.VEBuild
{
    public class TypeSettingsForm : TabItem
    {
        public TypeSettingsForm()
        {
            this.InitializeComponent();
        }

        private void InitializeComponent()
        {
            PerspexXamlLoader.Load(this);
        }
    }
}
