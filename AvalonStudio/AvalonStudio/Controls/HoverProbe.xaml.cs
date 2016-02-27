namespace AvalonStudio.Controls
{
    using Perspex.Controls;
    using Perspex.Markup.Xaml;

    public class HoverProbe : UserControl
    {
        public HoverProbe()
        {
            this.InitializeComponent();
        }

        private void InitializeComponent()
        {
            PerspexXamlLoader.Load(this);
        }
    }
}
