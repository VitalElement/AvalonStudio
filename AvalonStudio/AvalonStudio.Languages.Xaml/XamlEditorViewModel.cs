using AvalonStudio.Documents;
using AvalonStudio.Extensibility.Editor;
using AvalonStudio.Projects;
using ReactiveUI;
using System;

namespace AvalonStudio.Languages.Xaml
{
    public class XamlEditorViewModel : CodeEditorViewModel
    {
        public XamlEditorViewModel(ITextDocument document, ISourceFile file) : base(document, file)
        {
        }

        public string SourceText => Document.Text;

        public override void OnTextChanged()
        {
            base.OnTextChanged();

            this.RaisePropertyChanged(nameof(SourceText));
        }
    }
}
