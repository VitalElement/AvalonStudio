using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AvalonStudio.Projects;
using AvalonStudio.Languages.CPlusPlus;
using AvalonStudio.TextEditor.Indentation;
using System.IO;
using AvalonStudio.TextEditor.Rendering;

namespace AvalonStudio.Languages.Verilog
{
    public class VerilogLanguageService : ILanguageService
    {
        public VerilogLanguageService()
        {
            indentationStrategy = new CppIndentationStrategy();
        }
        public Type BaseTemplateType
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        private IIndentationStrategy indentationStrategy;
        public AvalonStudio.TextEditor.Indentation.IIndentationStrategy IndentationStrategy
        {
            get
            {
                return indentationStrategy;
            }
        }

        public string Title
        {
            get
            {
                return "Verilog";
            }
        }

        public bool CanHandle(ISourceFile file)
        {
            bool result = false;

            switch (Path.GetExtension(file.Location))
            {
                case ".v":
                
                    result = true;
                    break;
            }

            return result;
        }

        public List<CodeCompletionData> CodeCompleteAt(ISourceFile sourceFile, int line, int column, List<UnsavedFile> unsavedFiles, string filter)
        {
            return new List<CodeCompletionData>();
        }

        public int Comment(AvalonStudio.TextEditor.Document.TextDocument textDocument, AvalonStudio.TextEditor.Document.ISegment segment, int caret = -1, bool format = true)
        {
            return caret;
        }

        public int Format(AvalonStudio.TextEditor.Document.TextDocument textDocument, uint offset, uint length, int cursor)
        {
            return (int)offset;
        }

        public IList<AvalonStudio.TextEditor.Rendering.IBackgroundRenderer> GetBackgroundRenderers(ISourceFile file)
        {
            return new List<IBackgroundRenderer>();
        }

        public IList<AvalonStudio.TextEditor.Rendering.IDocumentLineTransformer> GetDocumentLineTransformers(ISourceFile file)
        {
            return new List<IDocumentLineTransformer>();
        }

        public Symbol GetSymbol(ISourceFile file, List<UnsavedFile> unsavedFiles, string name)
        {
            return null;
        }

        public Symbol GetSymbol(ISourceFile file, List<UnsavedFile> unsavedFiles, int offset)
        {
            return null;
        }

        public void RegisterSourceFile(IIntellisenseControl intellisenseControl, AvalonStudio.TextEditor.TextEditor editor, ISourceFile file, AvalonStudio.TextEditor.Document.TextDocument textDocument)
        {
            
        }

        public CodeAnalysisResults RunCodeAnalysis(ISourceFile file, List<UnsavedFile> unsavedFiles, Func<bool> interruptRequested)
        {
            return new CodeAnalysisResults();
        }

        public int UnComment(AvalonStudio.TextEditor.Document.TextDocument textDocument, AvalonStudio.TextEditor.Document.ISegment segment, int caret = -1, bool format = true)
        {
            return caret;
        }

        public void UnregisterSourceFile(AvalonStudio.TextEditor.TextEditor editor, ISourceFile file)
        {
            
        }
    }
}
