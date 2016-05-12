namespace AvalonStudio.Controls.Standard.Console
{
    using Avalonia.Controls;
    using Avalonia;
    
    public class ConsoleView : UserControl
    {
        public ConsoleView()
        {
            this.InitializeComponent();            
        }        

        private void InitializeComponent()
        {
            this.LoadFromXaml();
        }
    }
}
