using AvalonStudio.Documents;
using AvalonStudio.Extensibility.Editor;
using AvalonStudio.Projects;
using System.Composition;

namespace AvalonStudio.Languages.Xaml
{
    [Export(typeof(IEditorProvider))]
    internal class XamlEditorProvider : IEditorProvider
    {
        public bool CanEdit(ISourceFile file)
        {
            bool result = false;

            switch(file.Extension.ToLower())
            {
                case ".xaml":
                case ".paml":
                    result = true;
                    break;
            }

            return result;
        }

        public IFileDocumentTabViewModel CreateViewModel(ISourceFile file)
        {
            return new XamlEditorViewModel(file);
        }
    }
}
