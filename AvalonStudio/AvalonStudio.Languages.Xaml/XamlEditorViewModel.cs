using AvalonStudio.Documents;
using AvalonStudio.Extensibility.Editor;
using AvalonStudio.Projects;
using ReactiveUI;
using System;

namespace AvalonStudio.Languages.Xaml
{
    public class XamlEditorViewModel : TextEditorViewModel, IFileDocumentTabViewModel
    {
        public XamlEditorViewModel(ISourceFile file) : base(file)
        {
        }
    }
}
