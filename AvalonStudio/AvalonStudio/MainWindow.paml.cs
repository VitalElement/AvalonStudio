namespace AvalonStudio
{
    using Controls;
    using Perspex;
    using Perspex.Controls;
    using Perspex.Input;
    using Perspex;

    public class MainWindow : MetroWindow
    {
        public MainWindow()
        {
            this.InitializeComponent();

            DataContext = WorkspaceViewModel.Instance;

            this.AttachDevTools();
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            if(DataContext != null && DataContext is WorkspaceViewModel)
            {
                (DataContext as WorkspaceViewModel).OnKeyDown(e);
            }
        }

        private void InitializeComponent()
        {
            this.LoadFromXaml();
        }        
    }
}
