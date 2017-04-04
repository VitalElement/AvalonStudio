using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using AvalonStudio.Extensibility.Languages.CompletionAssistance;
using AvalonStudio.Projects;
using AvalonStudio.Languages;
using AvalonStudio.LanguageSupport.TypeScript.Projects;
using AvalonStudio.TextEditor.Document;
using AvalonStudio.TextEditor.Indentation;
using AvalonStudio.TextEditor.Rendering;
using IridiumJS.Runtime;
using Newtonsoft.Json;
using TSBridge;
using TSBridge.Ast;
using TSBridge.Ast.Statements;
using TSBridge.Ast.SubNodes.ClassElements;
using TSBridge.Ast.SubNodes.Declarations;
using File = System.IO.File;

namespace AvalonStudio.LanguageSupport.TypeScript.LanguageService
{
    public class TypeScriptLanguageService : ILanguageService
    {
        private TypeScriptContext _tsContext;

        private static readonly ConditionalWeakTable<ISourceFile, TypeScriptDataAssociation> dataAssociations =
            new ConditionalWeakTable<ISourceFile, TypeScriptDataAssociation>();

        public Type BaseTemplateType => typeof(BlankTypeScriptProjectTemplate);

        public IEnumerable<char> IntellisenseTriggerCharacters
        {
            get { return new[] { '.', '>', ':' }; }
        }

        public IEnumerable<char> IntellisenseSearchCharacters
        {
            get { return new[] { '(', ')', '.', ':', '-', '>', ';' }; }
        }

        public IEnumerable<char> IntellisenseCompleteCharacters
        {
            get { return new[] { '.', ':', ';', '-', ' ', '(', '=', '+', '*', '/', '%', '|', '&', '!', '^' }; }
        }

        private SemaphoreSlim analysisThreadSemaphore = new SemaphoreSlim(1);

        // https://github.com/Microsoft/TypeScript/issues/2536
        private static Regex KeywordPattern =
            new Regex(
                @"\b(break|case|catch|class|constructor|const|continue|debugger|default|delete|do|else|enum|export|extends|false|finally|for|function|interface|if|import|in|instanceof|new|null|return|super|switch|this|throw|true|try|typeof|var|void|while|with|as|implements|let|package|private|protected|public|static|yield|symbol|type|from|of|any|boolean|declare|get|module|require|number|set|string)",
                RegexOptions.Compiled);

        private static Regex LineCommentPattern =
            new Regex(@"//(.*?)\r?\n",
                RegexOptions.Compiled);

        private static Regex BlockCommentPattern =
            new Regex(
                @"(//[\t|\s|\w|\d|\.]*[\r\n|\n])|([\s|\t]*/\*[\t|\s|\w|\W|\d|\.|\r|\n]*\*/)|(\<[!%][ \r\n\t]*(--([^\-]|[\r\n]|-[^\-])*--[ \r\n\t%]*)\>)",
                RegexOptions.Compiled);

#if DEBUG

        private static StreamWriter LogFileWriter;
#endif

        public TypeScriptLanguageService()
        {
#if DEBUG
            try
            {
                //Directory.CreateDirectory(Path.GetDirectoryName(LogFilePath));
                //LogFileWriter = new StreamWriter(File.OpenWrite(LogFilePath));
            }
            catch (IOException) // Maybe another instance is running. Anyway, this isn't really needed.
            {
            }
#endif
        }

        public IIndentationStrategy IndentationStrategy { get; }

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

        public async Task<CodeCompletionResults> CodeCompleteAtAsync(ISourceFile sourceFile, int index, int line,
            int column, List<UnsavedFile> unsavedFiles, string filter = "")
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

        private async Task<List<CodeCompletionData>> CodeCompleteAtAsync(ISourceFile sourceFile, int index,
            List<UnsavedFile> unsavedFiles, string filter = "")
        {
            var currentUnsavedFile = unsavedFiles.FirstOrDefault(f => f.FileName == sourceFile.FilePath);
            var currentFileConts = currentUnsavedFile?.Contents ?? File.ReadAllText(sourceFile.FilePath);
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
                })
                .ToList();

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

        public int Comment(TextDocument textDocument, ISegment segment,
            int caret = -1, bool format = true)
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

        public int Format(TextDocument textDocument, uint offset, uint length, int cursor)
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

        public Task<Symbol> GetSymbolAsync(ISourceFile file, List<UnsavedFile> unsavedFiles, int offset)
        {
            //STUB!
            return Task.FromResult(new Symbol());
        }

        public Task<List<Symbol>> GetSymbolsAsync(ISourceFile file, List<UnsavedFile> unsavedFiles, string name)
        {
            //STUB!
            return Task.FromResult(new List<Symbol>());
        }

        public void RegisterSourceFile(IIntellisenseControl intellisenseControl,
            ICompletionAssistant completionAssistant, TextEditor.TextEditor editor, ISourceFile file,
            TextDocument textDocument)
        {
            _tsContext = _tsContext ?? ((TypeScriptProject)file.Project).TypeScriptContext;
            _tsContext.OpenFile(file.FilePath, File.ReadAllText(file.FilePath));

            TypeScriptDataAssociation association = null;

            if (dataAssociations.TryGetValue(file, out association))
            {
                throw new InvalidOperationException("Source file already registered with language service.");
            }

            association = new TypeScriptDataAssociation(textDocument);
            dataAssociations.Add(file, association);
        }

        public async Task<CodeAnalysisResults> RunCodeAnalysisAsync(ISourceFile file,
            List<UnsavedFile> unsavedFiles, Func<bool> interruptRequested)
        {
            var result = new CodeAnalysisResults();

            var dataAssociation = GetAssociatedData(file);

            var currentUnsavedFile = unsavedFiles.FirstOrDefault(f => f.FileName == file.FilePath);
            var currentFileConts = currentUnsavedFile?.Contents ?? File.ReadAllText(file.FilePath);
            var currentFileName = currentUnsavedFile?.FileName ?? file.FilePath;
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
                    Diagnostics = new TextSegmentCollection<Diagnostic>
                    {
                        new Diagnostic
                        {
                            Project = file.Project,
                            Line = 1,
                            Spelling = "Code analysis language service call failed.",
                            StartOffset = 0,
                            File = file.Name,
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
            var syntaxTreeJsonDebug = JsonConvert.SerializeObject(tsSyntaxTree);
#endif

            // Highlighting

            // Highlight comments

            var lineCommentMatches = LineCommentPattern.Matches(currentFileConts);
            foreach (Match commentMatch in lineCommentMatches)
            {
                result.SyntaxHighlightingData.Add(new OffsetSyntaxHighlightingData
                {
                    Start = commentMatch.Index,
                    Length = commentMatch.Length,
                    Type = HighlightType.Comment
                });
            }

            var blockCommentMatches = BlockCommentPattern.Matches(currentFileConts);
            foreach (Match commentMatch in blockCommentMatches)
            {
                result.SyntaxHighlightingData.Add(new OffsetSyntaxHighlightingData
                {
                    Start = commentMatch.Index,
                    Length = commentMatch.Length,
                    Type = HighlightType.Comment
                });
            }

            // Highlight keywords
            var keywordMatches = KeywordPattern.Matches(currentFileConts);
            foreach (Match keywordMatch in keywordMatches)
            {
                result.SyntaxHighlightingData.Add(new OffsetSyntaxHighlightingData
                {
                    Start = keywordMatch.Index,
                    Length = keywordMatch.Length,
                    Type = HighlightType.Keyword
                });
            }

            // Recursively highlight and analyze from parse tree
            foreach (var rootStatement in tsSyntaxTree.Statements)
            {
                HighlightNode(rootStatement, result);
            }

            // Clean up previous highlighting
            dataAssociation.TextMarkerService.Clear();

            // Diagnostics

            // Language service has diagnostics
            foreach (var tsDiagnostic in tsSyntaxTree.ParseDiagnostics)
            {
                // Convert diagnostics
                result.Diagnostics.Add(new Diagnostic
                {
                    Project = file.Project,
                    Line = GetLineNumber(currentFileConts, tsDiagnostic.Start), // TODO
                    StartOffset = tsDiagnostic.Start,
                    EndOffset = tsDiagnostic.Start + tsDiagnostic.Length,
                    Spelling = tsDiagnostic.MessageText,
                    Level = tsDiagnostic.Category == TSBridge.Ast.Diagnostics.Diagnostic.DiagnosticCategory.Error
                        ? DiagnosticLevel.Error
                        : DiagnosticLevel.Warning
                });
            }

            result.Diagnostics.Add(new Diagnostic
            {
                Project = file.Project,
                Line = 1,
                Spelling = "Code analysis for TypeScript is experimental and unstable. Use with caution.",
                StartOffset = 0,
                File = file.Name,
                Level = DiagnosticLevel.Warning,
            });

            dataAssociation.TextColorizer.SetTransformations(result.SyntaxHighlightingData);

            return result;
        }

        private void HighlightNode(INode node, CodeAnalysisResults result)
        {
            var highlightData = new OffsetSyntaxHighlightingData();
            int startPos = 0, endPos = 0;

            // This will add highlighting data for declarations, but it will not show up because it is set to highlight as None
            if (node is IDeclaration)
            {
                var declaration = ((IDeclaration)node);
                startPos = declaration.Position;
                endPos = declaration.End;
                if (startPos >= 0 && endPos > 0)
                {
                    if (node is VariableDeclaration
                    ) // For variables, we only want to highlight the name, not the expression
                    {
                        startPos = ((VariableDeclaration)node).Name.Position;
                        endPos = ((VariableDeclaration)node).Name.End;
                    }
                    else if (node is ClassDeclaration
                    ) // For classes, we only want to highlight the name, not the entire contents
                    {
                        startPos = ((ClassDeclaration)node).Name.Position;
                        endPos = ((ClassDeclaration)node).Name.End;
                    }
                    else if (node is FunctionDeclaration
                    ) // For functions, we only want to highlight the name, not the body
                    {
                        startPos = ((FunctionDeclaration)node).Name.Position;
                        endPos = ((FunctionDeclaration)node).Name.End;
                    }

                    highlightData.Start = startPos;
                    highlightData.Length = endPos - startPos;
                    result.SyntaxHighlightingData.Add(highlightData);
                }
            }
            // Originally, we meant to highlight CallExpression
            // We are leaving this out because the language service
            // marks the entire call, including the parameters, as part
            // of the call. This ends up looking weird.
            //else if (node is ExpressionStatement)
            //{
            //    var expressionNode = ((ExpressionStatement)node).Expression;
            //    // TODO: Highlight expressions
            //    if (expressionNode.Kind == SyntaxKind.CallExpression)
            //    {
            //        startPos = expressionNode.Position;
            //        endPos = expressionNode.End;
            //        highlightData.Type = HighlightType.CallExpression;
            //    }
            //    highlightData.Start = startPos;
            //    highlightData.Length = endPos - startPos;
            //    result.SyntaxHighlightingData.Add(highlightData);
            //}

            // This section will adjust highlight data, and set the highlight type
            switch (node.Kind)
            {
                case SyntaxKind.ClassDeclaration:
                    var classDeclaration = node as ClassDeclaration;
                    highlightData.Type = HighlightType.ClassName;
                    foreach (var member in classDeclaration.Members)
                    {
                        HighlightNode(member, result);
                    }
                    break;

                case SyntaxKind.MethodDeclaration:
                    var methodDeclaration = node as MethodDeclaration;
                    highlightData.Type = HighlightType.Identifier;
                    // TODO: Child nodes
                    foreach (var statement in methodDeclaration.Body.Statements)
                    {
                        HighlightNode(statement, result);
                    }
                    break;

                case SyntaxKind.Constructor:
                    highlightData.Type = HighlightType.Identifier;
                    break;

                case SyntaxKind.VariableStatement:
                    var variableStatement = node as VariableStatement;
                    foreach (var declaration in variableStatement.DeclarationList.Declarations)
                    {
                        HighlightNode(declaration, result);
                    }
                    break;

                case SyntaxKind.VariableDeclaration:
                    highlightData.Type = HighlightType.Identifier;
                    break;

                default:
                    // lol
                    break;
            }
        }

        private int GetLineNumber(string document, int offset)
        {
            return document.Take(offset).Count(x => x == '\n') + 1;
        }

        public Task<SignatureHelp> SignatureHelp(ISourceFile file, UnsavedFile buffer,
            List<UnsavedFile> unsavedFiles, int line, int column, int offset, string methodName)
        {
            //STUB!
            //return new SignatureHelp();
            return Task.FromResult<SignatureHelp>(null);
        }

        public int UnComment(TextDocument textDocument, ISegment segment,
            int caret = -1, bool format = true)
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

        public virtual void BeforeActivation()
        {
        }

        public virtual void Activation()
        {
        }
    }
}