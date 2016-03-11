namespace AvalonStudio
{
    using Controls;
    using Perspex.Controls;
    using Perspex.Input;
    using Perspex.Markup.Xaml;

    public class MainWindow : MetroWindow
    {
        public MainWindow()
        {
            this.InitializeComponent();

            App.AttachDevTools(this);
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
            PerspexXamlLoader.Load(this);
        }        
    }
}
