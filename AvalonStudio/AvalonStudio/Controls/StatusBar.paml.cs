namespace AvalonStudio.Controls
{
    using Avalonia;
    using Avalonia.Controls;

    public class StatusBar : UserControl
    {
        public StatusBar()
        {
            this.InitializeComponent();            
        }

        private void InitializeComponent()
        {
            this.LoadFromXaml();
        }
    }
}
