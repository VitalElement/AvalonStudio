using AvalonStudio.Extensibility.Languages.CompletionAssistance;
using AvalonStudio.Projects;
using AvalonStudio.Projects.TypeScript;
using AvalonStudio.TextEditor.Indentation;
using AvalonStudio.TextEditor.Rendering;
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

        public async Task<List<CodeCompletionData>> CodeCompleteAtAsync(ISourceFile sourceFile, int line, int column, List<UnsavedFile> unsavedFiles, string filter = "")
        {
            //STUB!
            return new List<CodeCompletionData>();
        }

        public int Comment(TextEditor.Document.TextDocument textDocument, TextEditor.Document.ISegment segment, int caret = -1, bool format = true)
        {
            var result = caret;

            var lines = VisualLineGeometryBuilder.GetLinesForSegmentInDocument(textDocument, segment);

            textDocument.BeginUpdate();

            foreach (var line in lines)
            {
                textDocument.Insert(line.Offset, "//");
            }

            if (format)
            {
                result = Format(textDocument, (uint)segment.Offset, (uint)segment.Length, caret);
            }

            textDocument.EndUpdate();

            return result;
        }

        public int Format(TextEditor.Document.TextDocument textDocument, uint offset, uint length, int cursor)
        {
            //STUB!
            throw new NotImplementedException();
        }

        public IList<IBackgroundRenderer> GetBackgroundRenderers(ISourceFile file)
        {
            //STUB!
            return new List<IBackgroundRenderer>();
        }

        public IList<IDocumentLineTransformer> GetDocumentLineTransformers(ISourceFile file)
        {
            //STUB!
            return new List<IDocumentLineTransformer>();
        }

        public async Task<Symbol> GetSymbolAsync(ISourceFile file, List<UnsavedFile> unsavedFiles, int offset)
        {
            //STUB!
            return new Symbol();
        }

        public async Task<List<Symbol>> GetSymbolsAsync(ISourceFile file, List<UnsavedFile> unsavedFiles, string name)
        {
            //STUB!
            return new List<Symbol>();
        }

        public void RegisterSourceFile(IIntellisenseControl intellisenseControl, ICompletionAssistant completionAssistant, TextEditor.TextEditor editor, ISourceFile file, TextEditor.Document.TextDocument textDocument)
        {
            //throw new NotImplementedException();
        }

        public async Task<CodeAnalysisResults> RunCodeAnalysisAsync(ISourceFile file, List<UnsavedFile> unsavedFiles, Func<bool> interruptRequested)
        {
            //throw new NotImplementedException();
            return new CodeAnalysisResults
            {
                Diagnostics = new TextEditor.Document.TextSegmentCollection<Diagnostic>
                {
                    new Diagnostic
                    {
                        Project = file.Project,
                        Line = 1,
                        Spelling = "Code analysis is not yet supported for TypeScript. Use with caution.",
                        StartOffset = 0,
                        File = file.Name,
                        Level = DiagnosticLevel.Warning,
                    }
                }
            };
        }

        public async Task<SignatureHelp> SignatureHelp(ISourceFile file, UnsavedFile buffer, List<UnsavedFile> unsavedFiles, int line, int column, int offset, string methodName)
        {
            //STUB!
            return new SignatureHelp();
        }

        public int UnComment(TextEditor.Document.TextDocument textDocument, TextEditor.Document.ISegment segment, int caret = -1, bool format = true)
        {
            var result = caret;

            var lines = VisualLineGeometryBuilder.GetLinesForSegmentInDocument(textDocument, segment);

            textDocument.BeginUpdate();

            foreach (var line in lines)
            {
                var index = textDocument.GetText(line).IndexOf("//", StringComparison.Ordinal);

                if (index >= 0)
                {
                    textDocument.Replace(line.Offset + index, 2, string.Empty);
                }
            }

            if (format)
            {
                result = Format(textDocument, (uint)segment.Offset, (uint)segment.Length, caret);
            }

            textDocument.EndUpdate();

            return result;
        }

        public void UnregisterSourceFile(TextEditor.TextEditor editor, ISourceFile file)
        {
            //throw new NotImplementedException();
        }
    }
}