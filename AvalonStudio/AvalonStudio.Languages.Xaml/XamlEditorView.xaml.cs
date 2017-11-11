using System;
using Avalonia.Controls;
using Avalonia.Ide.CompletionEngine;
using Avalonia.Input;
using Avalonia.Markup.Xaml;
using AvaloniaEdit.Highlighting;

namespace AvalonStudio.Languages.Xaml
{
    public class XamlEditorView : UserControl
    {
        public XamlEditorView()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}