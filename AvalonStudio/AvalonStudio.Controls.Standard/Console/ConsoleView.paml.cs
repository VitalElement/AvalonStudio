namespace AvalonStudio.Controls.Standard.Console
{
    using Perspex.Controls;
    using Perspex;
    
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
