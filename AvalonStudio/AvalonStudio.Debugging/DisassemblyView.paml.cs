namespace AvalonStudio.Debugging
{
    using Avalonia.Controls;
    using Avalonia;

    public class DisassemblyView : UserControl
    {
        public DisassemblyView()
        {
            this.InitializeComponent();
        }

        private void InitializeComponent()
        {
            this.LoadFromXaml();
        }
    }
}
