using Perspex.Controls;
using Perspex.Markup.Xaml;

namespace AvalonStudio.Controls
{
    public class NewItemDialog : UserControl
    {
        public NewItemDialog()
        {
            this.InitializeComponent();
        }

        private void InitializeComponent()
        {
            PerspexXamlLoader.Load(this);
        }
    }
}
