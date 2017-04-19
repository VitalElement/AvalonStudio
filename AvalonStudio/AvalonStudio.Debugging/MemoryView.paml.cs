namespace AvalonStudio.Debugging
{
    using Avalonia.Controls;
    using Avalonia.Markup.Xaml;

    /// <summary>
    /// Interaction logic for MemoryView.xaml
    /// </summary>
    public partial class MemoryView : UserControl
    {
        public MemoryView()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}