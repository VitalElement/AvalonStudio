namespace AvalonStudio.Controls.TeamExplorer
{
    using Avalonia.Controls;
    using Avalonia;
    
    public class TeamExplorerView : UserControl
    {
        public TeamExplorerView()
        {
            this.InitializeComponent();            
        }        

        private void InitializeComponent()
        {
            this.LoadFromXaml();
        }
    }
}
