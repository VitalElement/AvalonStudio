using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using System;

namespace AvalonStudio.Controls
{
    public class SignatureHelpView : UserControl
    {
        public SignatureHelpView()
        {
            this.InitializeComponent();            
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}
