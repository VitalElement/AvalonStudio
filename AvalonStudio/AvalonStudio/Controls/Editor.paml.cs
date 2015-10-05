using Perspex.Controls;
using Perspex.Markup.Xaml;

namespace AvalonStudio.Controls
{
    public class Editor : UserControl
    {
        public Editor()
        {
            this.InitializeComponent();

            var editor = ControlExtensions.FindControl<TextBox>(this, "editor");
        }

        private void InitializeComponent()
        {
            PerspexXamlLoader.Load(this);
        }
    }
}
