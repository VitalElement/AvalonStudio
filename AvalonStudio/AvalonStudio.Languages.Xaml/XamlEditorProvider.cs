using AvalonStudio.Controls;
using AvalonStudio.Extensibility.Editor;
using AvalonStudio.Projects;

namespace AvalonStudio.Languages.Xaml
{
    public class XamlEditorProvider : IEditorProvider
    {
        public void Activation()
        {
        }

        public void BeforeActivation()
        {
        }

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
