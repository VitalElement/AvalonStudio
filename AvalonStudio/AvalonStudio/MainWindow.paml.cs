namespace AvalonStudio
{
    using Controls;
    using Extensibility;
    using Extensibility.Commands;
    using Avalonia;
    using Avalonia.Controls;
    using Avalonia.Input;

    public class MainWindow : MetroWindow
    {
        public MainWindow()
        {
            this.InitializeComponent();

            DataContext = ShellViewModel.Instance;

            IoC.Get<ICommandKeyGestureService>().BindKeyGestures(this);
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
           Avalonia.Markup.Xaml.AvaloniaXamlLoader.Load(this);
        }        
    }
}
