namespace AvalonStudio.Debugging
{
    using Perspex.Controls;
    using Perspex;

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
