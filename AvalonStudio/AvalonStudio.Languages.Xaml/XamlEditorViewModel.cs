using Avalonia.Controls;
using AvalonStudio.Controls;
using AvalonStudio.Extensibility.Editor;
using AvalonStudio.Projects;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Text;

namespace AvalonStudio.Languages.Xaml
{
    public class XamlEditorViewModel : TextEditorViewModel
    {
        public XamlEditorViewModel(ISourceFile file) : base(file)
        {
        }
    }
}
