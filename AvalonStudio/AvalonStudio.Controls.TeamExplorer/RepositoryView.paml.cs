namespace AvalonStudio.Controls.TeamExplorer
{
    using Avalonia.Controls;
    using Avalonia;
    
    public class RepositoryView : UserControl
    {
        public RepositoryView()
        {
            this.InitializeComponent();            
        }        

        private void InitializeComponent()
        {
            this.LoadFromXaml();
        }
    }
}
