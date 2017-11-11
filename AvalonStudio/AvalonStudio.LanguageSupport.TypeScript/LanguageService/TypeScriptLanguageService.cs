using AvaloniaEdit.Document;
using AvaloniaEdit.Indentation;
using AvaloniaEdit.Rendering;
using AvalonStudio.Editor;
using AvalonStudio.Extensibility.Languages.CompletionAssistance;
using AvalonStudio.Languages;
using AvalonStudio.LanguageSupport.TypeScript.Projects;
using AvalonStudio.Projects;
using IridiumJS.Runtime;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
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
        private TypeScriptContext _typeScriptContext;

        private static readonly ConditionalWeakTable<ISourceFile, TypeScriptDataAssociation> dataAssociations =
            new ConditionalWeakTable<ISourceFile, TypeScriptDataAssociation>();

        public Type BaseTemplateType => typeof(BlankTypeScriptProjectTemplate);

        public IEnumerable<ICodeEditorInputHelper> InputHelpers => null;

        public bool CanTriggerIntellisense(char currentChar, char previousChar)
        {
            return IntellisenseTriggerCharacters.Contains(currentChar);
        }

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

        public bool IsValidIdentifierCharacter(char data)
        {
            return char.IsLetterOrDigit(data) || data == '_';
        }

        private SemaphoreSlim analysisThreadSemaphore = new SemaphoreSlim(1);

        // https://github.com/Microsoft/TypeScript/issues/2536
        private static readonly Regex KeywordPattern =
            new Regex(
                @"\b(break|case|catch|class|constructor|const|continue|debugger|default|delete|do|else|enum|export|extends|false|finally|for|function|interface|if|import|in|instanceof|new|null|return|super|switch|this|throw|true|try|typeof|var|void|while|with|as|implements|let|package|private|protected|public|static|yield|symbol|type|from|of|any|boolean|declare|get|module|require|number|set|string)",
                RegexOptions.Compiled);

        private static readonly Regex LineCommentPattern =
            new Regex(@"//(.*?)\r?\n",
                RegexOptions.Compiled);

        private static readonly Regex BlockCommentPattern =
            new Regex(
                @"(//[\t|\s|\w|\d|\.]*[\r\n|\n])|([\s|\t]*/\*[\t|\s|\w|\W|\d|\.|\r|\n]*\*/)|(\<[!%][ \r\n\t]*(--([^\-]|[\r\n]|-[^\-])*--[ \r\n\t%]*)\>)",
                RegexOptions.Compiled);

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

        public IDictionary<string, Func<string, string>> SnippetCodeGenerators => null;

        public IDictionary<string, Func<int, int, int, string>> SnippetDynamicVariables => null;

        public string LanguageId => "ts";

        public IObservable<TextSegmentCollection<Diagnostic>> Diagnostics => throw new NotImplementedException();

        public bool CanHandle(ISourceFile file)
        {
            var result = false;

            switch (Path.GetExtension(file.Location))
            {
                case ".ts":
                    result = true;
                    break;
            }

            return result;
        }

        public async Task<CodeCompletionResults> CodeCompleteAtAsync(ISourceFile sourceFile, int index, int line,
            int column, List<UnsavedFile> unsavedFiles, char previousChar, string filter = "")
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
            var completions = await _typeScriptContext.GetCompletionsAtPositionAsync(sourceFile.FilePath, caretPosition);

            var editorCompletions = completions.Entries.Select(cc =>
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
                    return CodeCompletionKind.ClassPublic;

                case "var":
                    return CodeCompletionKind.Variable;

                case "keyword":
                    return CodeCompletionKind.Keyword;

                default:
                    return CodeCompletionKind.None;
            }
        }

        public int Format(ISourceFile file, TextDocument textDocument, uint offset, uint length, int cursor)
        {
            //STUB!
            return -1;
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

        public void RegisterSourceFile(AvaloniaEdit.TextEditor editor, ISourceFile file,
            TextDocument textDocument)
        {
            _typeScriptContext = _typeScriptContext ?? ((TypeScriptProject)file.Project).TypeScriptContext;
            _typeScriptContext.OpenFile(file.FilePath, File.ReadAllText(file.FilePath));

            TypeScriptDataAssociation association = null;

            if (dataAssociations.TryGetValue(file, out association))
            {
                throw new InvalidOperationException("Source file already registered with language service.");
            }

            association = new TypeScriptDataAssociation();
            dataAssociations.Add(file, association);
        }

        public async Task<CodeAnalysisResults> RunCodeAnalysisAsync(ISourceFile file, TextDocument document,
            List<UnsavedFile> unsavedFiles, Func<bool> interruptRequested)
        {
            var result = new CodeAnalysisResults();

            var dataAssociation = GetAssociatedData(file);

            var currentUnsavedFile = unsavedFiles.FirstOrDefault(f => f.FileName == file.FilePath);
            var currentFileConts = currentUnsavedFile?.Contents ?? File.ReadAllText(file.FilePath);
            var currentFileName = currentUnsavedFile?.FileName ?? file.FilePath;
            TypeScriptSyntaxTree syntaxTree;
            // Only one analyzer at a time; the JS engine is single-threaded. TODO: Workaround with multiple JS engines
            await analysisThreadSemaphore.WaitAsync();
            try
            {
                syntaxTree = await _typeScriptContext.BuildAstAsync(currentFileName, currentFileConts);
            }
            catch (JavaScriptException)
            {
                result.Diagnostics = new TextSegmentCollection<Diagnostic>
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
                };

                return new CodeAnalysisResults
                {
                };
            }
            finally
            {
                analysisThreadSemaphore.Release();
            }

#if DEBUG
            var syntaxTreeJsonDebug = JsonConvert.SerializeObject(syntaxTree);
#endif

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
            foreach (var rootStatement in syntaxTree.Statements)
            {
                HighlightNode(rootStatement, result);
            }

            // Diagnostics
            var diagnostics = new TextSegmentCollection<Diagnostic>(document);
            // Language service has diagnostics
            foreach (var diagnostic in syntaxTree.ParseDiagnostics)
            {
                // Convert diagnostics
                diagnostics.Add(new Diagnostic
                {
                    Project = file.Project,
                    Line = GetLineNumber(currentFileConts, diagnostic.Start), // TODO
                    StartOffset = diagnostic.Start,
                    EndOffset = diagnostic.Start + diagnostic.Length,
                    Spelling = diagnostic.MessageText,
                    Level = diagnostic.Category == TSBridge.Ast.Diagnostics.Diagnostic.DiagnosticCategory.Error
                        ? DiagnosticLevel.Error
                        : DiagnosticLevel.Warning
                });
            }

            diagnostics.Add(new Diagnostic
            {
                Project = file.Project,
                Line = 1,
                Spelling = "Code analysis for TypeScript is experimental and unstable. Use with caution.",
                StartOffset = 0,
                File = file.Name,
                Level = DiagnosticLevel.Warning,
            });

            return result;
        }

        private void HighlightNode(INode node, CodeAnalysisResults result)
        {
            var highlightData = new OffsetSyntaxHighlightingData();
            int startPos = 0, endPos = 0;

            // This will add highlighting data for declarations, but it will not show up because it is set to highlight as None
            if (node is IDeclaration)
            {
                var declaration = (IDeclaration)node;
                startPos = declaration.Position;
                endPos = declaration.End;
                if (startPos >= 0 && endPos > 0)
                {
                    if (node is VariableDeclaration)
                    {
                        // For variables, we only want to highlight the name, not the expression
                        startPos = ((VariableDeclaration)node).Name.Position;
                        endPos = ((VariableDeclaration)node).Name.End;
                    }
                    else if (node is ClassDeclaration)
                    {
                        // For classes, we only want to highlight the name, not the entire contents
                        startPos = ((ClassDeclaration)node).Name.Position;
                        endPos = ((ClassDeclaration)node).Name.End;
                    }
                    else if (node is FunctionDeclaration)
                    {
                        // For functions, we only want to highlight the name, not the body
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

        public int Comment(ISourceFile file, TextDocument textDocument, int firstLine, int endLine, int caret = -1, bool format = true)
        {
            var result = caret;

            textDocument.BeginUpdate();

            for (int line = firstLine; line <= endLine; line++)
            {
                textDocument.Insert(textDocument.GetLineByNumber(line).Offset, "//");
            }

            if (format)
            {
                var startOffset = textDocument.GetLineByNumber(firstLine).Offset;
                var endOffset = textDocument.GetLineByNumber(endLine).EndOffset;
                result = Format(file, textDocument, (uint)startOffset, (uint)(endOffset - startOffset), caret);
            }

            textDocument.EndUpdate();

            return result;
        }

        public int UnComment(ISourceFile file, TextDocument textDocument, int firstLine, int endLine, int caret = -1, bool format = true)
        {
            var result = caret;

            textDocument.BeginUpdate();

            for (int line = firstLine; line <= endLine; line++)
            {
                var docLine = textDocument.GetLineByNumber(firstLine);
                var index = textDocument.GetText(docLine).IndexOf("//");

                if (index >= 0)
                {
                    textDocument.Replace(docLine.Offset + index, 2, string.Empty);
                }
            }

            if (format)
            {
                var startOffset = textDocument.GetLineByNumber(firstLine).Offset;
                var endOffset = textDocument.GetLineByNumber(endLine).EndOffset;
                result = Format(file, textDocument, (uint)startOffset, (uint)(endOffset - startOffset), caret);
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

        public void UnregisterSourceFile(AvaloniaEdit.TextEditor editor, ISourceFile file)
        {
            _typeScriptContext.RemoveFile(file.FilePath);
            dataAssociations.Remove(file);
        }

        public async Task PrepareLanguageServiceAsync()
        {
            await _typeScriptContext.LoadComponentsAsync();
        }

        public virtual void BeforeActivation()
        {
        }

        public virtual void Activation()
        {
        }
    }
}