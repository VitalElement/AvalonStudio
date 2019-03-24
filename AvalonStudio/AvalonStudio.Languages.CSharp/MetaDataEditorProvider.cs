using AvalonStudio.Controls.Editor;
using AvalonStudio.Documents;
using AvalonStudio.Extensibility;
using AvalonStudio.Extensibility.Editor;
using AvalonStudio.Extensibility.Studio;
using AvalonStudio.Projects;
using AvalonStudio.Shell;
using System;
using System.IO;
using System.Threading.Tasks;

namespace AvalonStudio.Languages.CSharp
{
    [ExportEditorProvider]
    internal class CsharpEditorProvider : IEditorProvider
    {
        public bool CanEdit(ISourceFile file)
        {
            if(file is MetaDataFile)
            {
                return false;
            }

            return Path.GetExtension(file.FilePath) == ".cs";
        }

        public async Task<ITextDocumentTabViewModel> CreateViewModel(ISourceFile file)
        {

            return new CodeEditorViewModel(await IoC.Get<IStudio>().CreateDocumentAsync(file.FilePath), file);
        }
    }

    [ExportEditorProvider]
    internal class MetaDataEditorProvider : IEditorProvider
    {
        public bool CanEdit(ISourceFile file)
        {
            return file.FilePath.StartsWith("$metadata");
        }

        public async Task<ITextDocumentTabViewModel> CreateViewModel(ISourceFile file)
        {
            if (file is MetaDataFile metaDataFile)
            {
                return new CodeEditorViewModel(AvalonStudioTextDocument.Create(await metaDataFile.GetTextAsync()), file) { IsReadOnly = true };
            }

            throw new NotSupportedException();
        }
    }
}
