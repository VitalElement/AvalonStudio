using AvalonStudio.Documents;
using AvalonStudio.Projects;
using AvalonStudio.Shell;

namespace AvalonStudio.Languages.Xaml
{
    [ExportEditorProvider]
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
