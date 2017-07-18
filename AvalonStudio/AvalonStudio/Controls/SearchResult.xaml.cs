using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using System.Reactive.Disposables;
using System;
using Avalonia;
using System.Reactive.Linq;
using Avalonia.Controls.Primitives;

namespace AvalonStudio.Controls
{
    public class SearchResult : UserControl
    {
        public SearchResult()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}