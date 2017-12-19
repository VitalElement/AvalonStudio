using AvalonStudio.Documents;
using AvalonStudio.Extensibility.Editor;
using AvalonStudio.Projects;
using System;
using System.Collections.Generic;
using System.Text;

namespace AvalonStudio.Languages.CSharp
{
    class MetaDataEditorProvider : IEditorProvider
    {
        public void Activation()
        {
            
        }

        public void BeforeActivation()
        {
            
        }

        public bool CanEdit(ISourceFile file)
        {
            return file.FilePath.StartsWith("$metadata");
        }

        public IFileDocumentTabViewModel CreateViewModel(ISourceFile file)
        {
            return new TextEditorViewModel(file);
        }
    }
}
