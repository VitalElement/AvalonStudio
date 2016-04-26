namespace AvalonStudio.Controls.TeamExplorer
{
    using Perspex.Controls;
    using Perspex;
    
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
