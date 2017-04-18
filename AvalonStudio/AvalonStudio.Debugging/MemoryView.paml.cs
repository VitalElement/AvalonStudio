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

        //private void lv_SelectionChanged(object sender, SelectionChangedEventArgs e)
        //{
        //    Selector selector = sender as Selector;

        //    if (selector is ListBox)
        //    {
        //        (selector as ListBox).ScrollIntoView(selector.SelectedItem);
        //    }
        //}

        //private void TextBox_KeyUp(object sender, KeyEventArgs e)
        //{
        //    if (e.Key == Key.Enter)
        //    {
        //        TextBox tBox = (TextBox)sender;
        //        DependencyProperty prop = TextBox.TextProperty;

        //        BindingExpression binding = BindingOperations.GetBindingExpression(tBox, prop);
        //        if (binding != null) { binding.UpdateSource(); }
        //    }

        //}

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}