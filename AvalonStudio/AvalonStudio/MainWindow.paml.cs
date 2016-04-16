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

            DataContext = ShellViewModel.Instance;

            this.AttachDevTools();
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            if(DataContext != null && DataContext is ShellViewModel)
            {
                (DataContext as ShellViewModel).OnKeyDown(e);
            }
        }

        private void InitializeComponent()
        {
            this.LoadFromXaml();
        }        
    }
}
