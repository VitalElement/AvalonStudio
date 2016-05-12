namespace AvalonStudio.Extensibility.MainMenu.ViewModels
{    
    using Perspex;
    using Perspex.Controls;

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
