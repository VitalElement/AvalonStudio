namespace AvalonStudio.Controls
{
    using Perspex;
    using Perspex.Controls;

    public class ToolBar : UserControl
    {
        public ToolBar()
        {
            this.InitializeComponent();            
        }

        private void InitializeComponent()
        {
            this.LoadFromXaml();
        }
    }
}
