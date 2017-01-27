using AvalonStudio.Extensibility.Languages.CompletionAssistance;
using AvalonStudio.Projects;
using AvalonStudio.Projects.TypeScript;
using AvalonStudio.TextEditor.Indentation;
using AvalonStudio.TextEditor.Rendering;
using IridiumJS.Runtime;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using TSBridge;
using TSBridge.Ast;
using TSBridge.Ast.Statements;

namespace AvalonStudio.Languages.TypeScript
{
    public class TypeScriptLanguageService : ILanguageService
    {
        private TypeScriptContext _tsContext;

        private static readonly ConditionalWeakTable<ISourceFile, TypeScriptDataAssociation> dataAssociations =
            new ConditionalWeakTable<ISourceFile, TypeScriptDataAssociation>();

        public Type BaseTemplateType => typeof(BlankTypeScriptProjectTemplate);

        public IEnumerable<char> IntellisenseTriggerCharacters { get { return new[] { '.', '>', ':' }; } }

        public IEnumerable<char> IntellisenseSearchCharacters { get { return new[] { '(', ')', '.', ':', '-', '>', ';' }; } }

        public IEnumerable<char> IntellisenseCompleteCharacters { get { return new[] { '.', ':', ';', '-', ' ', '(', '=', '+', '*', '/', '%', '|', '&', '!', '^' }; } }

        private SemaphoreSlim analysisThreadSemaphore = new SemaphoreSlim(1);

#if DEBUG
        private static string LogFilePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "AvalonStudio", "Diagnostics", $"{nameof(TypeScriptLanguageService)}.log");

        private static StreamWriter LogFileWriter;
#endif

        public TypeScriptLanguageService()
        {
#if DEBUG
            try
            {
                Directory.CreateDirectory(Path.GetDirectoryName(LogFilePath));
                LogFileWriter = new StreamWriter(System.IO.File.OpenWrite(LogFilePath));
            }
            catch (IOException) // Maybe another instance is running. Anyway, this isn't really needed.
            {
            }
#endif
        }

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

        public async Task<CodeCompletionResults> CodeCompleteAtAsync(ISourceFile sourceFile, int index, int line, int column, List<UnsavedFile> unsavedFiles, string filter = "")
        {
            //Get position in text
            var currentUnsavedFile = unsavedFiles.FirstOrDefault(f => f.FileName == sourceFile.FilePath);
            var currentFileConts = currentUnsavedFile.Contents;
            var lines = currentFileConts.Split('\n');
            var caretPosition = 0;
            for (int i = 0; i < line; i++)
            {
                caretPosition += lines[i].Length;
            }
            caretPosition += column;

            var completionDataList = await CodeCompleteAtAsync(sourceFile, caretPosition, unsavedFiles, filter);
            return new CodeCompletionResults
            {
                Completions = completionDataList,
                Contexts = CompletionContext.Unknown // TODO: ???
            };
        }

        private async Task<List<CodeCompletionData>> CodeCompleteAtAsync(ISourceFile sourceFile, int index, List<UnsavedFile> unsavedFiles, string filter = "")
        {
            var currentUnsavedFile = unsavedFiles.FirstOrDefault(f => f.FileName == sourceFile.FilePath);
            var currentFileConts = currentUnsavedFile?.Contents ?? System.IO.File.ReadAllText(sourceFile.FilePath);
            var caretPosition = index;
            var lsCompletions = await _tsContext.GetCompletionsAtPositionAsync(sourceFile.FilePath, caretPosition);

            var editorCompletions = lsCompletions.Entries.Select(cc =>
            {
                var ccData = new CodeCompletionData
                {
                    Kind = ConvertCodeCompletionKind(cc.Kind),
                    Hint = cc.KindModifiers,
                    BriefComment = cc.Name,
                    Suggestion = cc.Name
                };
                return ccData;
            }).ToList();

            return editorCompletions;
        }

        private CodeCompletionKind ConvertCodeCompletionKind(string kind)
        {
            switch (kind)
            {
                case "class":
                    return CodeCompletionKind.Class;

                case "var":
                    return CodeCompletionKind.Variable;

                case "keyword":
                    return CodeCompletionKind.Keyword;

                default:
                    return CodeCompletionKind.None;
            }
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
            return -1;
        }

        public IList<IBackgroundRenderer> GetBackgroundRenderers(ISourceFile file)
        {
            var associatedData = GetAssociatedData(file);

            return associatedData.BackgroundRenderers;
        }

        public IList<IDocumentLineTransformer> GetDocumentLineTransformers(ISourceFile file)
        {
            var associatedData = GetAssociatedData(file);

            return associatedData.DocumentLineTransformers;
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
            _tsContext = _tsContext ?? (file.Project as TypeScriptProject).TypeScriptContext;
            _tsContext.OpenFile(file.FilePath, System.IO.File.ReadAllText(file.FilePath));

            TypeScriptDataAssociation association = null;

            if (dataAssociations.TryGetValue(file, out association))
            {
                throw new InvalidOperationException("Source file already registered with language service.");
            }

            association = new TypeScriptDataAssociation(textDocument);
            dataAssociations.Add(file, association);
        }

        public async Task<CodeAnalysisResults> RunCodeAnalysisAsync(ISourceFile sourceFile, List<UnsavedFile> unsavedFiles, Func<bool> interruptRequested)
        {
            var result = new CodeAnalysisResults();

            var dataAssociation = GetAssociatedData(sourceFile);

            var currentUnsavedFile = unsavedFiles.FirstOrDefault(f => f.FileName == sourceFile.FilePath);
            var currentFileConts = currentUnsavedFile?.Contents ?? System.IO.File.ReadAllText(sourceFile.FilePath);
            var currentFileName = currentUnsavedFile?.FileName ?? sourceFile.FilePath;
            TypeScriptSyntaxTree tsSyntaxTree;
            // Only one analyzer at a time; the JS engine is single-threaded. TODO: Workaround with multiple JS engines
            await analysisThreadSemaphore.WaitAsync();
            try
            {
                tsSyntaxTree = await _tsContext.BuildAstAsync(currentFileName, currentFileConts);
            }
            catch (JavaScriptException)
            {
                return new CodeAnalysisResults
                {
                    Diagnostics = new TextEditor.Document.TextSegmentCollection<Diagnostic>
                    {
                        new Diagnostic
                        {
                            Project = sourceFile.Project,
                            Line = 1,
                            Spelling = "Code analysis language service call failed.",
                            StartOffset = 0,
                            File = sourceFile.Name,
                            Level = DiagnosticLevel.Error,
                        }
                    }
                };
            }
            finally
            {
                analysisThreadSemaphore.Release();
            }

#if DEBUG
            var syntaxTreeJsonDebug = Newtonsoft.Json.JsonConvert.SerializeObject(tsSyntaxTree);
#endif

            // Highlighting
            foreach (var rootStatement in tsSyntaxTree.Statements)
            {
                var highlightData = new OffsetSyntaxHighlightingData();
                int startPos = 0, endPos = 0;
                HighlightType highlightType = HighlightType.None;
                switch (rootStatement.Kind)
                {
                    case SyntaxKind.ClassDeclaration:
                        var classDeclaration = rootStatement as ClassDeclaration;
                        startPos = classDeclaration.Name.Position;
                        endPos = classDeclaration.Name.End;
                        highlightType = HighlightType.ClassName;
                        break;

                    case SyntaxKind.FunctionDeclaration:
                        var functionDeclaration = rootStatement as FunctionDeclaration;
                        startPos = functionDeclaration.Name.Position;
                        endPos = functionDeclaration.Name.End;
                        highlightType = HighlightType.Identifier;
                        break;
                }

                highlightData.Start = startPos;
                highlightData.Length = endPos - startPos;
                highlightData.Type = highlightType;
                result.SyntaxHighlightingData.Add(highlightData);
            }
            dataAssociation.TextMarkerService.Clear();

            // Diagnostics

            // Language service has diagnostics
            foreach (var tsDiagnostic in tsSyntaxTree.ParseDiagnostics)
            {
                // Convert diagnostics
                result.Diagnostics.Add(new Diagnostic
                {
                    Project = sourceFile.Project,
                    Line = GetLineNumber(currentFileConts, tsDiagnostic.Start), // TODO
                    StartOffset = tsDiagnostic.Start,
                    EndOffset = tsDiagnostic.Start + tsDiagnostic.Length,
                    Spelling = tsDiagnostic.MessageText,
                    Level = tsDiagnostic.Category == TSBridge.Ast.Diagnostics.Diagnostic.DiagnosticCategory.Error ? DiagnosticLevel.Error : DiagnosticLevel.Warning
                });
            }

            result.Diagnostics.Add(new Diagnostic
            {
                Project = sourceFile.Project,
                Line = 1,
                Spelling = "Code analysis for TypeScript is experimental and unstable. Use with caution.",
                StartOffset = 0,
                File = sourceFile.Name,
                Level = DiagnosticLevel.Warning,
            });

            dataAssociation.TextColorizer.SetTransformations(result.SyntaxHighlightingData);

            return result;
        }

        private int GetLineNumber(string document, int offset)
        {
            return document.Take(offset).Count(x => x == '\n') + 1;
        }

        public async Task<SignatureHelp> SignatureHelp(ISourceFile file, UnsavedFile buffer, List<UnsavedFile> unsavedFiles, int line, int column, int offset, string methodName)
        {
            //STUB!
            //return new SignatureHelp();
            return null;
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

        private TypeScriptDataAssociation GetAssociatedData(ISourceFile sourceFile)
        {
            TypeScriptDataAssociation result = null;

            if (!dataAssociations.TryGetValue(sourceFile, out result))
            {
                throw new Exception("Tried to parse file that has not been registered with the language service.");
            }

            return result;
        }

        public void UnregisterSourceFile(TextEditor.TextEditor editor, ISourceFile file)
        {
            _tsContext.RemoveFile(file.FilePath);
            dataAssociations.Remove(file);
        }

        public async Task PrepareLanguageServiceAsync()
        {
            await _tsContext.LoadComponentsAsync();
        }
    }
}