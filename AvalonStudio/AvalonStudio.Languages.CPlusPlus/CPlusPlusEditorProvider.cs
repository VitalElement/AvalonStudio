using AvalonStudio.Documents;
using AvalonStudio.Extensibility;
using AvalonStudio.Extensibility.Editor;
using AvalonStudio.Extensibility.Studio;
using AvalonStudio.Projects;
using AvalonStudio.Shell;
using System.IO;
using System.Threading.Tasks;

namespace AvalonStudio.Languages.CPlusPlus
{
    [ExportEditorProvider]
    internal class CPlusPlusEditorPropvider : IEditorProvider
    {
        public bool CanEdit(ISourceFile file)
        {
            switch (Path.GetExtension(file.FilePath))
            {
                case ".h":
                case ".c":
                case ".cc":
                case ".cpp":
                case ".hpp":
                    return true;
            }

            return false;
        }

        public async Task<ITextDocumentTabViewModel> CreateViewModel(ISourceFile file)
        {
            return new CodeEditorViewModel(await IoC.Get<IStudio>().CreateDocumentAsync(file.FilePath), file);
        }
    }
}
