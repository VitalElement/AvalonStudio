using Perspex.Controls;
using Perspex.Markup.Xaml;

namespace AvalonStudio.Controls
{
    public class Intellisense : UserControl
    {
        public Intellisense()
        {
            this.InitializeComponent();
        }

        private void InitializeComponent()
        {
            PerspexXamlLoader.Load(this);
        }
    }
}
