using Perspex.Controls;
using Perspex.Markup.Xaml;

namespace AvalonStudio.Controls
{
    public class StatusBar : UserControl
    {
        public StatusBar()
        {
            this.InitializeComponent();            
        }

        private void InitializeComponent()
        {
            PerspexXamlLoader.Load(this);
        }
    }
}
