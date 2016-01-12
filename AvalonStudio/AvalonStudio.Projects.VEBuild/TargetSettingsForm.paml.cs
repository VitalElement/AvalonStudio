using Perspex.Controls;
using Perspex.Markup.Xaml;

namespace AvalonStudio.Projects.VEBuild
{
    public class TargetSettingsForm : TabItem
    {
        public TargetSettingsForm()
        {
            this.InitializeComponent();
        }

        private void InitializeComponent()
        {
            PerspexXamlLoader.Load(this);
        }
    }
}
