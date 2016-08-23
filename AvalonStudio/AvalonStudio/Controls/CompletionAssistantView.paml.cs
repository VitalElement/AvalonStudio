using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace AvalonStudio.Controls
{
    public class CompletionAssistantView : UserControl
    {
        public CompletionAssistantView()
        {
            this.InitializeComponent();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}
