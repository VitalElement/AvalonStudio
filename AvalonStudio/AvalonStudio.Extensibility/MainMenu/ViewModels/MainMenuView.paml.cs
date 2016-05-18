namespace AvalonStudio.Extensibility.MainMenu.ViewModels
{
    using Avalonia;
    using Avalonia.Controls;
    using Avalonia.Markup.Xaml;
    public class MainMenuView : UserControl
    {
        public MainMenuView()
        {
            this.InitializeComponent();
        }

        private void InitializeComponent()
        {
            Avalonia.Markup.Xaml.AvaloniaXamlLoader.Load(this);
        }
    }
}
