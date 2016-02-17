using Perspex.Controls;
using Perspex.Markup.Xaml;

namespace AvalonStudio.Projects.CPlusPlus
{
    public class ReferencesSettingsForm : TabItem
    {
        public ReferencesSettingsForm()
        {
            this.InitializeComponent();
        }

        private void InitializeComponent()
        {
            PerspexXamlLoader.Load(this);
        }
    }
}
