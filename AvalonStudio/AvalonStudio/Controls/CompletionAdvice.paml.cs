namespace AvalonStudio.Controls
{
    using Avalonia.Controls;
    using Avalonia.Markup.Xaml;

    public class CompletionAdvice : UserControl
    {
        public CompletionAdvice()
        {
            this.InitializeComponent();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}
