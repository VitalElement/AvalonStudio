using AvalonStudio.Extensibility.Languages.CompletionAssistance;
using AvalonStudio.Projects;
using AvalonStudio.Projects.TypeScript;
using AvalonStudio.TextEditor.Indentation;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace AvalonStudio.Languages.TypeScript
{
    public class TypeScriptLanguageService : ILanguageService
    {
        public Type BaseTemplateType => typeof(BlankTypeScriptProjectTemplate);

        public IEnumerable<char> IntellisenseTriggerCharacters { get { return new[] { '.', '>', ':' }; } }

        public IEnumerable<char> IntellisenseSearchCharacters { get { return new[] { '(', ')', '.', ':', '-', '>', ';' }; } }

        public IEnumerable<char> IntellisenseCompleteCharacters { get { return new[] { '.', ':', ';', '-', ' ', '(', '=', '+', '*', '/', '%', '|', '&', '!', '^' }; } }

        public IIndentationStrategy IndentationStrategy
        {
            get;
        }

        public string Title => "TypeScript";

        public bool CanHandle(ISourceFile file)
        {
            var result = false;

            switch (Path.GetExtension(file.Location))
            {
                case ".ts":
                    result = true;
                    break;
            }

            //if (!(file.Project.Solution is TypeScriptSolution))
            //{
            //    result = false;
            //}

            return result;
        }

        public Task<List<CodeCompletionData>> CodeCompleteAtAsync(ISourceFile sourceFile, int line, int column, List<UnsavedFile> unsavedFiles, string filter = "")
        {
            throw new NotImplementedException();
        }

        public int Comment(AvalonStudio.TextEditor.Document.TextDocument textDocument, AvalonStudio.TextEditor.Document.ISegment segment, int caret = -1, bool format = true)
        {
            throw new NotImplementedException();
        }

        public int Format(AvalonStudio.TextEditor.Document.TextDocument textDocument, uint offset, uint length, int cursor)
        {
            throw new NotImplementedException();
        }

        public IList<AvalonStudio.TextEditor.Rendering.IBackgroundRenderer> GetBackgroundRenderers(ISourceFile file)
        {
            throw new NotImplementedException();
        }

        public IList<AvalonStudio.TextEditor.Rendering.IDocumentLineTransformer> GetDocumentLineTransformers(ISourceFile file)
        {
            throw new NotImplementedException();
        }

        public Task<Symbol> GetSymbolAsync(ISourceFile file, List<UnsavedFile> unsavedFiles, int offset)
        {
            throw new NotImplementedException();
        }

        public Task<List<Symbol>> GetSymbolsAsync(ISourceFile file, List<UnsavedFile> unsavedFiles, string name)
        {
            throw new NotImplementedException();
        }

        public void RegisterSourceFile(IIntellisenseControl intellisenseControl, ICompletionAssistant completionAssistant, AvalonStudio.TextEditor.TextEditor editor, ISourceFile file, AvalonStudio.TextEditor.Document.TextDocument textDocument)
        {
            throw new NotImplementedException();
        }

        public Task<CodeAnalysisResults> RunCodeAnalysisAsync(ISourceFile file, List<UnsavedFile> unsavedFiles, Func<bool> interruptRequested)
        {
            throw new NotImplementedException();
        }

        public Task<SignatureHelp> SignatureHelp(ISourceFile file, UnsavedFile buffer, List<UnsavedFile> unsavedFiles, int line, int column, int offset, string methodName)
        {
            throw new NotImplementedException();
        }

        public int UnComment(AvalonStudio.TextEditor.Document.TextDocument textDocument, AvalonStudio.TextEditor.Document.ISegment segment, int caret = -1, bool format = true)
        {
            throw new NotImplementedException();
        }

        public void UnregisterSourceFile(AvalonStudio.TextEditor.TextEditor editor, ISourceFile file)
        {
            throw new NotImplementedException();
        }
    }
}