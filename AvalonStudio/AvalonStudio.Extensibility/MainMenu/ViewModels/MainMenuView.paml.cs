namespace AvalonStudio.Extensibility.MainMenu.ViewModels
{    
    using Avalonia;
    using Avalonia.Controls;

    public class MainMenuView : UserControl
    {
        public MainMenuView()
        {
            this.InitializeComponent();
        }

        private void InitializeComponent()
        {
            this.LoadFromXaml();
        }
    }
}
