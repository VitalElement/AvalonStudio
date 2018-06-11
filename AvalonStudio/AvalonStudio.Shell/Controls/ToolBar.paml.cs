using System;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace AvalonStudio.Shell.Controls
{
    public class ToolBar : UserControl
    {
        public ToolBar()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}