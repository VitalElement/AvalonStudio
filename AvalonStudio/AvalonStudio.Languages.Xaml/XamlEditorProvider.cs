using AvalonStudio.Documents;
using AvalonStudio.Extensibility;
using AvalonStudio.Extensibility.Studio;
using AvalonStudio.Projects;
using AvalonStudio.Shell;
using System.Threading.Tasks;

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

        public async Task<ITextDocumentTabViewModel> CreateViewModel(ISourceFile file)
        {
            return new XamlEditorViewModel(await IoC.Get<IStudio>().CreateDocumentAsync(file.FilePath), file);
        }
    }
}
