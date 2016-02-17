using Perspex.Controls;
using Perspex.Markup.Xaml;

namespace AvalonStudio.Projects.CPlusPlus
{
    public class IncludePathSettingsForm : TabItem
    {
        public IncludePathSettingsForm()
        {
            this.InitializeComponent();
        }

        private void InitializeComponent()
        {
            PerspexXamlLoader.Load(this);
        }
    }
}
