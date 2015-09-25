namespace AvalonStudio.Controls
{
    using Perspex.Controls;
    using Perspex.Markup.Xaml;

    public class ToolBar : UserControl
    {
        public ToolBar()
        {
            this.InitializeComponent();            
        }

        private void InitializeComponent()
        {
            PerspexXamlLoader.Load(this);
        }
    }
}
