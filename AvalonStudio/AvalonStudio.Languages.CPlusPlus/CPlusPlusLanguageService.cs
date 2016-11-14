using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Media;
using AvalonStudio.Extensibility.Languages;
using AvalonStudio.Extensibility.Languages.CompletionAssistance;
using AvalonStudio.Extensibility.Threading;
using AvalonStudio.Languages.CPlusPlus.Rendering;
using AvalonStudio.Platforms;
using AvalonStudio.Projects;
using AvalonStudio.Projects.Standard;
using AvalonStudio.TextEditor.Document;
using AvalonStudio.TextEditor.Indentation;
using AvalonStudio.TextEditor.Rendering;
using AvalonStudio.Utils;
using NClang;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace AvalonStudio.Languages.CPlusPlus
{
    public class CPlusPlusLanguageService : ILanguageService
    {
        private static readonly ClangIndex index = ClangService.CreateIndex();

        private static readonly ConditionalWeakTable<ISourceFile, CPlusPlusDataAssociation> dataAssociations =
            new ConditionalWeakTable<ISourceFile, CPlusPlusDataAssociation>();

        private readonly JobRunner clangAccessJobRunner;

        private readonly JobRunner intellisenseJobRunner;

        public CPlusPlusLanguageService()
        {
            IndentationStrategy = new CppIndentationStrategy();
            intellisenseJobRunner = new JobRunner();
            clangAccessJobRunner = new JobRunner();

            Task.Factory.StartNew(() => { intellisenseJobRunner.RunLoop(new CancellationToken()); });

            Task.Factory.StartNew(() => { clangAccessJobRunner.RunLoop(new CancellationToken()); });
        }

        public IProjectTemplate EmptyProjectTemplate
        {
            get { throw new NotImplementedException(); }
        }

        public string Title
        {
            get { return "C/C++"; }
        }

        public Type BaseTemplateType
        {
            get { return typeof(BlankCPlusPlusLanguageTemplate); }
        }

        public IIndentationStrategy IndentationStrategy { get; }        

        public IEnumerable<char> IntellisenseTriggerCharacters { get { return new []{ '.', '>', ':' }; } }

        public IEnumerable<char> IntellisenseSearchCharacters { get { return new[] { '(', ')', '.', ':', '-', '<', '>', '[', ']', ';', '"', '#' }; } }

        public IEnumerable<char> IntellisenseCompleteCharacters { get { return new[] { '.', ':', ';', '-', ' ', '(', ')', '[', ']', '<', '>', '=', '+', '*', '/', '%', '|', '&', '!', '^' }; } }

        CodeCompletionKind FromClangKind(NClang.CursorKind kind)
        {
            switch (kind)
            {
                case NClang.CursorKind.FunctionDeclaration:
                case NClang.CursorKind.CXXMethod:
                case NClang.CursorKind.Constructor:
                case NClang.CursorKind.Destructor:
                case NClang.CursorKind.FunctionTemplate:
                case NClang.CursorKind.ClassTemplate:
                    return CodeCompletionKind.Method;

                case NClang.CursorKind.ClassDeclaration:
                    return CodeCompletionKind.Class;

                case NClang.CursorKind.StructDeclaration:
                    return CodeCompletionKind.Struct;

                case NClang.CursorKind.MacroDefinition:
                    return CodeCompletionKind.Macro;

                case NClang.CursorKind.NotImplemented:
                case NClang.CursorKind.TypedefDeclaration:
                    return CodeCompletionKind.Keyword;

                case NClang.CursorKind.EnumDeclaration:
                    return CodeCompletionKind.Enum;

                case NClang.CursorKind.EnumConstantDeclaration:
                    return CodeCompletionKind.EnumConstant;

                case NClang.CursorKind.VarDeclaration:
                    return CodeCompletionKind.Variable;

                case NClang.CursorKind.Namespace:
                    return CodeCompletionKind.Namespace;

                case NClang.CursorKind.ParmDeclaration:
                    return CodeCompletionKind.Field;

                case NClang.CursorKind.FieldDeclaration:
                    return CodeCompletionKind.Parameter;
            }

            Console.WriteLine($"dont understand{kind.ToString()}");
            return CodeCompletionKind.None;
        }

        public async Task<List<CodeCompletionData>> CodeCompleteAtAsync(ISourceFile file, int line, int column,
            List<UnsavedFile> unsavedFiles, string filter)
        {
            var clangUnsavedFiles = new List<ClangUnsavedFile>();

            foreach (var unsavedFile in unsavedFiles)
            {
                clangUnsavedFiles.Add(new ClangUnsavedFile(unsavedFile.FileName, unsavedFile.Contents));
            }

            var result = new List<CodeCompletionData>();

            await clangAccessJobRunner.InvokeAsync(() =>
            {
                var translationUnit = GetAndParseTranslationUnit(file, clangUnsavedFiles);

                var completionResults = translationUnit.CodeCompleteAt(file.Location, line, column, clangUnsavedFiles.ToArray(),
                    CodeCompleteFlags.IncludeBriefComments | CodeCompleteFlags.IncludeMacros | CodeCompleteFlags.IncludeCodePatterns);
                completionResults.Sort();

                foreach (var codeCompletion in completionResults.Results)
                {
                    var typedText = string.Empty;

                    var hint = string.Empty;

                    foreach (var chunk in codeCompletion.CompletionString.Chunks)
                    {
                        if (chunk.Kind == CompletionChunkKind.TypedText)
                        {
                            typedText = chunk.Text;
                        }

                        hint += chunk.Text;

                        switch (chunk.Kind)
                        {
                            case CompletionChunkKind.LeftParen:
                            case CompletionChunkKind.LeftAngle:
                            case CompletionChunkKind.LeftBrace:
                            case CompletionChunkKind.LeftBracket:
                            case CompletionChunkKind.RightAngle:
                            case CompletionChunkKind.RightBrace:
                            case CompletionChunkKind.RightBracket:
                            case CompletionChunkKind.RightParen:
                            case CompletionChunkKind.Placeholder:
                            case CompletionChunkKind.Comma:
                                break;

                            default:
                                hint += " ";
                                break;
                        }
                    }

                    if (filter == string.Empty || typedText.StartsWith(filter))
                    {
                        result.Add(new CodeCompletionData
                        {
                            Suggestion = typedText,
                            Priority = codeCompletion.CompletionString.Priority,
                            Kind = FromClangKind(codeCompletion.CursorKind),
                            Hint = hint,
                            BriefComment = codeCompletion.CompletionString.BriefComment
                        });
                    }
                }

                completionResults.Dispose();
            });

            return result;
        }

        public async Task<CodeAnalysisResults> RunCodeAnalysisAsync(ISourceFile file, List<UnsavedFile> unsavedFiles,
            Func<bool> interruptRequested)
        {
            var result = new CodeAnalysisResults();

            var dataAssociation = GetAssociatedData(file);

            var clangUnsavedFiles = new List<ClangUnsavedFile>();

            foreach (var unsavedFile in unsavedFiles)
            {
                clangUnsavedFiles.Add(new ClangUnsavedFile(unsavedFile.FileName, unsavedFile.Contents));
            }

            await clangAccessJobRunner.InvokeAsync(() =>
            {
                var translationUnit = GetAndParseTranslationUnit(file, clangUnsavedFiles);

                if (file != null)
                {
                    var callbacks = new ClangIndexerCallbacks();

                    callbacks.IndexDeclaration += (handle, e) =>
                    {
                        if (e.Cursor.Spelling != null && e.Location.SourceLocation.IsFromMainFile)
                        {
                            switch (e.Cursor.Kind)
                            {
                                case NClang.CursorKind.FunctionDeclaration:
                                case NClang.CursorKind.CXXMethod:
                                case NClang.CursorKind.Constructor:
                                case NClang.CursorKind.Destructor:
                                case NClang.CursorKind.VarDeclaration:
                                case NClang.CursorKind.ParmDeclaration:
                                case NClang.CursorKind.StructDeclaration:
                                case NClang.CursorKind.ClassDeclaration:
                                case NClang.CursorKind.TypedefDeclaration:
                                case NClang.CursorKind.ClassTemplate:
                                case NClang.CursorKind.EnumDeclaration:
                                case NClang.CursorKind.UnionDeclaration:
                                    result.IndexItems.Add(new IndexEntry(e.Cursor.Spelling, e.Cursor.CursorExtent.Start.FileLocation.Offset,
                                        e.Cursor.CursorExtent.End.FileLocation.Offset, (CursorKind)e.Cursor.Kind));
                                    break;
                            }


                            switch (e.Cursor.Kind)
                            {
                                case NClang.CursorKind.StructDeclaration:
                                case NClang.CursorKind.ClassDeclaration:
                                case NClang.CursorKind.TypedefDeclaration:
                                case NClang.CursorKind.ClassTemplate:
                                case NClang.CursorKind.EnumDeclaration:
                                case NClang.CursorKind.UnionDeclaration:
                                case NClang.CursorKind.CXXBaseSpecifier:
                                    result.SyntaxHighlightingData.Add(new OffsetSyntaxHighlightingData
                                    {
                                        Start = e.Cursor.CursorExtent.Start.FileLocation.Offset,
                                        Length = e.Cursor.CursorExtent.End.FileLocation.Offset - e.Cursor.CursorExtent.Start.FileLocation.Offset,
                                        Type = HighlightType.ClassName
                                    });
                                    break;
                            }
                        }
                    };

                    callbacks.IndexEntityReference += (handle, e) =>
                    {
                        if (e.Cursor.Spelling != null && e.Location.SourceLocation.IsFromMainFile)
                        {
                            switch (e.Cursor.Kind)
                            {
                                case NClang.CursorKind.TypeReference:
                                case NClang.CursorKind.CXXBaseSpecifier:
                                case NClang.CursorKind.TemplateReference:
                                    result.SyntaxHighlightingData.Add(new OffsetSyntaxHighlightingData
                                    {
                                        Start = e.Cursor.CursorExtent.Start.FileLocation.Offset,
                                        Length = e.Cursor.CursorExtent.End.FileLocation.Offset - e.Cursor.CursorExtent.Start.FileLocation.Offset,
                                        Type = HighlightType.ClassName
                                    });
                                    break;
                            }
                        }
                    };

                    if (translationUnit != null)
                    {
                        var tokens = translationUnit.Tokenize(translationUnit.GetCursor().CursorExtent);
                        //var annotatedTokens = tokens.Annotate();           //TODO see if this can provide us with additional data.

                        foreach (var token in tokens.Tokens)
                        {
                            var highlightData = new OffsetSyntaxHighlightingData();
                            highlightData.Start = token.Extent.Start.FileLocation.Offset;
                            highlightData.Length = token.Extent.End.FileLocation.Offset - highlightData.Start;


                            switch (token.Kind)
                            {
                                case TokenKind.Comment:
                                    highlightData.Type = HighlightType.Comment;
                                    break;

                                case TokenKind.Identifier:
                                    highlightData.Type = HighlightType.Identifier;
                                    break;

                                case TokenKind.Punctuation:
                                    highlightData.Type = HighlightType.Punctuation;
                                    break;

                                case TokenKind.Keyword:
                                    highlightData.Type = HighlightType.Keyword;
                                    break;

                                case TokenKind.Literal:
                                    highlightData.Type = HighlightType.Literal;
                                    break;
                            }

                            result.SyntaxHighlightingData.Add(highlightData);
                        }

                        var indexAction = index.CreateIndexAction();
                        indexAction.IndexTranslationUnit(IntPtr.Zero, new[] { callbacks }, IndexOptionFlags.SkipParsedBodiesInSession,
                            translationUnit);
                        indexAction.Dispose();
                    }
                }

                dataAssociation.TextMarkerService.Clear();

                var diags = translationUnit.DiagnosticSet.Items;

                foreach (var diagnostic in diags)
                {
                    if (diagnostic.Location.IsFromMainFile)
                    {
                        var diag = new Diagnostic
                        {
                            Project = file.Project,
                            StartOffset = diagnostic.Location.FileLocation.Offset,
                            Line = diagnostic.Location.FileLocation.Line,
                            Spelling = diagnostic.Spelling,
                            File = diagnostic.Location.FileLocation.File.FileName,
                            Level = (DiagnosticLevel)diagnostic.Severity
                        };


                        var cursor = translationUnit.GetCursor(diagnostic.Location);
                        var tokens = translationUnit.Tokenize(cursor.CursorExtent);

                        foreach(var token in tokens.Tokens)
                        {
                            if(token.Location == diagnostic.Location)
                            {
                                diag.EndOffset = diag.StartOffset + token.Spelling.Length;
                            }
                        }

                        result.Diagnostics.Add(diag);
                        tokens.Dispose();

                        Color markerColor;

                        switch (diag.Level)
                        {
                            case DiagnosticLevel.Error:
                            case DiagnosticLevel.Fatal:
                                markerColor = Color.FromRgb(253, 45, 45);
                                break;

                            case DiagnosticLevel.Warning:
                                markerColor = Color.FromRgb(255, 207, 40);
                                break;

                            default:
                                markerColor = Color.FromRgb(0, 42, 74);
                                break;
                        }

                        dataAssociation.TextMarkerService.Create(diag.StartOffset, diag.Length, diag.Spelling, markerColor);
                    }
                }
            });

            dataAssociation.TextColorizer.SetTransformations(result.SyntaxHighlightingData);

            return result;
        }

        public bool CanHandle(ISourceFile file)
        {
            var result = false;

            switch (Path.GetExtension(file.Location))
            {
                case ".h":
                case ".cpp":
                case ".hpp":
                case ".c":
                    result = true;
                    break;
            }

            if (result)
            {
                if (!(file.Project is IStandardProject))
                {
                    result = false;
                }
            }

            return result;
        }

        public void RegisterSourceFile(IIntellisenseControl intellisense, ICompletionAssistant completionAssistant,
            TextEditor.TextEditor editor, ISourceFile file, TextDocument doc)
        {
            CPlusPlusDataAssociation association = null;

            if (dataAssociations.TryGetValue(file, out association))
            {
                throw new Exception("Source file already registered with language service.");
            }

            association = new CPlusPlusDataAssociation(doc);
            dataAssociations.Add(file, association);

            association.KeyUpHandler = (sender, e) =>
            {
                if (editor.TextDocument == doc)
                {
                    switch (e.Key)
                    {
                        case Key.Return:
                            {
                                if (editor.CaretIndex >= 0 && editor.CaretIndex < editor.TextDocument.TextLength)
                                {
                                    if (editor.TextDocument.GetCharAt(editor.CaretIndex) == '}')
                                    {
                                        editor.TextDocument.Insert(editor.CaretIndex, Environment.NewLine);
                                        editor.CaretIndex--;

                                        var currentLine = editor.TextDocument.GetLineByOffset(editor.CaretIndex);

                                        editor.CaretIndex = IndentationStrategy.IndentLine(editor.TextDocument, currentLine, editor.CaretIndex);
                                        editor.CaretIndex = IndentationStrategy.IndentLine(editor.TextDocument, currentLine.NextLine.NextLine,
                                            editor.CaretIndex);
                                        editor.CaretIndex = IndentationStrategy.IndentLine(editor.TextDocument, currentLine.NextLine, editor.CaretIndex);
                                    }

                                    var newCaret = IndentationStrategy.IndentLine(editor.TextDocument,
                                        editor.TextDocument.GetLineByOffset(editor.CaretIndex), editor.CaretIndex);

                                    editor.CaretIndex = newCaret;
                                }
                            }
                            break;
                    }
                }
            };

            association.TextInputHandler = (sender, e) =>
            {
                if (editor.TextDocument == doc)
                {
                    OpenBracket(editor, editor.TextDocument, e.Text);
                    CloseBracket(editor, editor.TextDocument, e.Text);

                    switch (e.Text)
                    {
                        case "}":
                        case ";":
                            editor.CaretIndex = Format(editor.TextDocument, 0, (uint)editor.TextDocument.TextLength, editor.CaretIndex);
                            break;

                        case "{":
                            var lineCount = editor.TextDocument.LineCount;
                            var offset = Format(editor.TextDocument, 0, (uint)editor.TextDocument.TextLength, editor.CaretIndex);

                            // suggests clang format didnt do anything, so we can assume not moving to new line.
                            if (lineCount != editor.TextDocument.LineCount)
                            {
                                if (offset <= editor.TextDocument.TextLength)
                                {
                                    var newLine = editor.TextDocument.GetLineByOffset(offset);
                                    editor.CaretIndex = newLine.PreviousLine.EndOffset;
                                }
                            }
                            else
                            {
                                editor.CaretIndex = offset;
                            }
                            break;
                    }
                }
            };

            editor.AddHandler(InputElement.KeyUpEvent, association.KeyUpHandler, RoutingStrategies.Tunnel);

            editor.TextInput += association.TextInputHandler;
        }

        public IList<IDocumentLineTransformer> GetDocumentLineTransformers(ISourceFile file)
        {
            var associatedData = GetAssociatedData(file);

            return associatedData.DocumentLineTransformers;
        }

        public IList<IBackgroundRenderer> GetBackgroundRenderers(ISourceFile file)
        {
            var associatedData = GetAssociatedData(file);

            return associatedData.BackgroundRenderers;
        }

        public void UnregisterSourceFile(TextEditor.TextEditor editor, ISourceFile file)
        {
            var association = GetAssociatedData(file);

            editor.RemoveHandler(InputElement.KeyUpEvent, association.KeyUpHandler);

            editor.TextInput -= association.TextInputHandler;

            association.TranslationUnit?.Dispose();
            dataAssociations.Remove(file);
        }

        public int Format(TextDocument textDocument, uint offset, uint length, int cursor)
        {
            var replacements = ClangFormat.FormatXml(textDocument.Text, offset, length, (uint)cursor,
                ClangFormatSettings.Default);

            return ApplyReplacements(textDocument, cursor, replacements);
        }

        public async Task<Symbol> GetSymbolAsync(ISourceFile file, List<UnsavedFile> unsavedFiles, int offset)
        {
            Symbol result = null;
            var associatedData = GetAssociatedData(file);

            await clangAccessJobRunner.InvokeAsync(() =>
            {
                var tu = associatedData.TranslationUnit;
                var cursor = tu.GetCursor(tu.GetLocationForOffset(tu.GetFile(file.FilePath), offset));

                switch (cursor.Kind)
                {
                    case NClang.CursorKind.MemberReferenceExpression:
                    case NClang.CursorKind.DeclarationReferenceExpression:
                    case NClang.CursorKind.CallExpression:
                    case NClang.CursorKind.TypeReference:
                        cursor = cursor.Referenced;
                        break;
                }

                result = SymbolFromClangCursor(cursor);
            });

            return result;
        }

        public async Task<List<Symbol>> GetSymbolsAsync(ISourceFile file, List<UnsavedFile> unsavedFiles, string name)
        {
            var results = new List<Symbol>();

            if (name != string.Empty)
            {
                await clangAccessJobRunner.InvokeAsync(() =>
                {
                    var translationUnit = GetAndParseTranslationUnit(file, new List<ClangUnsavedFile>());

                    var cursors = FindFunctions(translationUnit.GetCursor(), name);

                    foreach (var cursor in cursors)
                    {
                        results.Add(SymbolFromClangCursor(cursor));
                    }
                });
            }

            return results;
        }

        public int Comment(TextDocument textDocument, ISegment segment, int caret = -1, bool format = true)
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


        public int UnComment(TextDocument textDocument, ISegment segment, int caret = -1, bool format = true)
        {
            var result = caret;

            var lines = VisualLineGeometryBuilder.GetLinesForSegmentInDocument(textDocument, segment);

            textDocument.BeginUpdate();

            foreach (var line in lines)
            {
                var index = textDocument.GetText(line).IndexOf("//");

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

        private void AddArgument(List<string> list, string argument)
        {
            if (!list.Contains(argument))
            {
                list.Add(argument);
            }
        }

        private ClangTranslationUnit GenerateTranslationUnit(ISourceFile file, List<ClangUnsavedFile> unsavedFiles)
        {
            ClangTranslationUnit result = null;

            if (System.IO.File.Exists(file.Location))
            {
                var args = new List<string>();

                var superProject = file.Project.Solution.StartupProject as IStandardProject;
                var project = file.Project as IStandardProject;

                var toolchainIncludes = superProject.ToolChain?.GetToolchainIncludes(file);

                if (toolchainIncludes != null)
                {
                    foreach (var include in toolchainIncludes)
                    {
                        AddArgument(args, string.Format("-isystem{0}", include));
                    }
                }

                // toolchain includes
                // This code is same as in toolchain, get compiler arguments... does this need a refactor, or toolchain get passed in? Clang take GCC compatible arguments.
                // perhaps this language service has its own clang tool chain, to generate compiler arguments from project configuration?


                // Referenced includes
                var referencedIncludes = project.GetReferencedIncludes();

                foreach (var include in referencedIncludes)
                {
                    AddArgument(args, string.Format("-I{0}", include));
                }

                // global includes
                var globalIncludes = superProject.GetGlobalIncludes();

                foreach (var include in globalIncludes)
                {
                    AddArgument(args, string.Format("-I{0}", include));
                }

                // public includes
                foreach (var include in project.PublicIncludes)
                {
                    AddArgument(args, string.Format("-I{0}", include));
                }

                // includes
                foreach (var include in project.Includes)
                {
                    AddArgument(args, string.Format("-I{0}", Path.Combine(project.CurrentDirectory, include.Value)));
                }

                var referencedDefines = project.GetReferencedDefines();
                foreach (var define in referencedDefines)
                {
                    AddArgument(args, string.Format("-D{0}", define));
                }

                // global includes
                var globalDefines = superProject.GetGlobalDefines();

                foreach (var define in globalDefines)
                {
                    AddArgument(args, string.Format("-D{0}", define));
                }

                foreach (var define in project.Defines)
                {
                    AddArgument(args, string.Format("-D{0}", define));
                }

                //foreach (var arg in superProject.ToolChainArguments)
                //{
                //    args.Add(string.Format("{0}", arg));
                //}

                //foreach (var arg in superProject.CompilerArguments)
                //{
                //    args.Add(string.Format("{0}", arg));
                //}

                switch (file.Extension)
                {
                    case ".c":
                        {
                            foreach (var arg in superProject.CCompilerArguments)
                            {
                                args.Add(string.Format("{0}", arg));
                            }
                        }
                        break;

                    case ".cpp":
                        {
                            foreach (var arg in superProject.CppCompilerArguments)
                            {
                                args.Add(string.Format("{0}", arg));
                            }
                        }
                        break;
                }

                // TODO do we mark files as class header? CAn clang auto detect this?
                //if (file.Language == Language.Cpp)
                {
                    args.Add("-xc++");
                    args.Add("-std=c++14");
                }

                args.Add("-Wunused-variable");

                result = index.ParseTranslationUnit(file.Location, args.ToArray(), unsavedFiles.ToArray(),
                    TranslationUnitFlags.IncludeBriefCommentsInCodeCompletion | TranslationUnitFlags.PrecompiledPreamble |
                    TranslationUnitFlags.CacheCompletionResults | TranslationUnitFlags.Incomplete);
            }

            if (result == null)
            {
                throw new Exception("Error generating translation unit.");
            }

            return result;
        }

        private CPlusPlusDataAssociation GetAssociatedData(ISourceFile sourceFile)
        {
            CPlusPlusDataAssociation result = null;

            if (!dataAssociations.TryGetValue(sourceFile, out result))
            {
                throw new Exception("Tried to parse file that has not been registered with the language service.");
            }

            return result;
        }

        private ClangTranslationUnit GetAndParseTranslationUnit(ISourceFile sourceFile, List<ClangUnsavedFile> unsavedFiles)
        {
            var dataAssociation = GetAssociatedData(sourceFile);

            if (dataAssociation.TranslationUnit == null)
            {
                dataAssociation.TranslationUnit = GenerateTranslationUnit(sourceFile, unsavedFiles);
            }

            // Always do a reparse, as a workaround for some issues in libclang 3.7.1
            dataAssociation.TranslationUnit.Reparse(unsavedFiles.ToArray(), ReparseTranslationUnitFlags.None);

            return dataAssociation.TranslationUnit;
        }

        private void OpenBracket(TextEditor.TextEditor editor, TextDocument document, string text)
        {
            if (text[0].IsOpenBracketChar() && editor.CaretIndex <= document.TextLength && editor.CaretIndex > 0)
            {
                var nextChar = ' ';

                if (editor.CaretIndex != document.TextLength)
                {
                    document.GetCharAt(editor.CaretIndex);
                }

                if (char.IsWhiteSpace(nextChar) || nextChar.IsCloseBracketChar())
                {
                    document.Insert(editor.CaretIndex, text[0].GetCloseBracketChar().ToString());
                }
            }
        }

        private void CloseBracket(TextEditor.TextEditor editor, TextDocument document, string text)
        {
            if (text[0].IsCloseBracketChar() && editor.CaretIndex < document.TextLength && editor.CaretIndex > 0)
            {
                if (document.GetCharAt(editor.CaretIndex) == text[0])
                {
                    document.Replace(editor.CaretIndex - 1, 1, string.Empty);
                }
            }
        }

        public static int ApplyReplacements(TextDocument document, int cursor, XDocument replacements)
        {
            var elements = replacements.Elements().First().Elements();

            document.BeginUpdate();

            var offsetChange = 0;
            foreach (var element in elements)
            {
                switch (element.Name.LocalName)
                {
                    case "cursor":
                        cursor = Convert.ToInt32(element.Value);
                        break;

                    case "replacement":
                        var offset = -1;
                        var replacementLength = -1;
                        var attributes = element.Attributes();

                        foreach (var attribute in attributes)
                        {
                            switch (attribute.Name.LocalName)
                            {
                                case "offset":
                                    offset = Convert.ToInt32(attribute.Value);
                                    break;

                                case "length":
                                    replacementLength = Convert.ToInt32(attribute.Value);
                                    break;
                            }
                        }

                        if (offset >= document.TextLength)
                        {
                            //document.Insert(offset, element.Value);
                        }
                        if (offset + replacementLength > document.TextLength)
                        {
                            //document.Replace(offset, document.TextLength - offset, element.Value);
                        }
                        else
                        {
                            document.Replace(offsetChange + offset, replacementLength, element.Value);
                        }

                        offsetChange += element.Value.Length - replacementLength;
                        break;
                }
            }

            document.EndUpdate();

            return cursor;
        }

        private static Symbol SymbolFromClangCursor(ClangCursor cursor)
        {
            var result = new Symbol();

            switch (cursor.Kind)
            {
                case NClang.CursorKind.CXXAccessSpecifier:
                    result.Name = "(Access Specifier) " + cursor.CxxAccessSpecifier;
                    break;

                default:
                    result.Name = cursor.Spelling;
                    break;
            }

            result.Kind = (CursorKind)cursor.Kind;
            result.BriefComment = cursor.BriefCommentText;
            result.TypeDescription = cursor.CursorType?.Spelling;
            result.EnumDescription = cursor.EnumConstantDeclValue.ToString();
            result.Definition = cursor.Definition.DisplayName;
            result.Linkage = (LinkageKind)cursor.Linkage;
            result.IsBuiltInType = IsBuiltInType(cursor.CursorType);
            result.SymbolType = cursor.CursorType?.Spelling.Replace(" &", "&").Replace(" *", "*") + " ";
            result.ResultType = cursor.ResultType?.Spelling;
            result.Arguments = new List<ParameterSymbol>();
            result.Access = (AccessType)cursor.CxxAccessSpecifier;
            result.IsVariadic = cursor.IsVariadic;

            switch (result.Kind)
            {
                case CursorKind.FunctionDeclaration:
                case CursorKind.CXXMethod:
                case CursorKind.Constructor:
                case CursorKind.Destructor:
                    for (var i = 0; i < cursor.ArgumentCount; i++)
                    {
                        var argument = cursor.GetArgument(i);

                        var arg = new ParameterSymbol();
                        arg.IsBuiltInType = IsBuiltInType(argument.CursorType);
                        arg.Name = argument.Spelling;

                        arg.TypeDescription = argument.CursorType.Spelling;
                        result.Arguments.Add(arg);
                    }

                    if (cursor.IsVariadic)
                    {
                        result.Arguments.Last().Name += ", ";
                        result.Arguments.Add(new ParameterSymbol { Name = "... variadic" });
                    }

                    if (cursor.ParsedComment.FullCommentAsXml != null)
                    {
                        var documentation = XDocument.Parse(cursor.ParsedComment.FullCommentAsXml);

                        var function = documentation.Element("Function");

                        var parameters = function.Element("Parameters");

                        if (parameters != null)
                        {
                            var arguments = parameters.Elements("Parameter");

                            foreach (var argument in arguments)
                            {
                                var isVarArgs = argument.Element("IsVarArg");

                                var discussion = argument.Element("Discussion");

                                var paragraph = discussion.Element("Para");

                                if (isVarArgs != null)
                                {
                                    result.Arguments.Last().Comment = paragraph.Value;
                                }
                                else
                                {
                                    var inx = argument.Element("Index");

                                    if (inx != null)    // This happens when documentation for an argument was left in, but the argument no longer exists.
                                    {
                                        var index = int.Parse(inx.Value);

                                        result.Arguments[index].Comment = paragraph.Value;
                                    }
                                }
                            }
                        }
                    }

                    if (result.Arguments.Count == 0)
                    {
                        result.Arguments.Add(new ParameterSymbol { Name = "void" });
                    }
                    break;
            }

            return result;
        }

        private static Signature SignatureFromSymbol(Symbol symbol)
        {
            var result = new Signature();

            result.Name = symbol.Name;
            result.Description = symbol.BriefComment;

            if (symbol.IsBuiltInType)
            {
                result.BuiltInReturnType = symbol.ResultType;
            }
            else
            {
                result.ReturnType = symbol.ResultType;
            }

            foreach (var param in symbol.Arguments)
            {
                var newParam = new Parameter();

                if (param.IsBuiltInType)
                {
                    newParam.BuiltInType = param.TypeDescription;
                }
                else
                {
                    newParam.Type = param.TypeDescription;
                }

                newParam.Name = param.Name;
                newParam.Documentation = param.Comment;

                result.Parameters.Add(newParam);
            }

            return result;
        }

        private static bool IsBuiltInType(ClangType cursor)
        {
            var result = false;

            if (cursor != null && cursor.Kind >= TypeKind.FirstBuiltin && cursor.Kind <= TypeKind.LastBuiltin)
            {
                return true;
            }

            return result;
        }

        private bool CursorIsValidDeclaration(ClangCursor c)
        {
            var result = false;

            if ((c.Kind == NClang.CursorKind.FunctionDeclaration) || c.Kind == NClang.CursorKind.CXXMethod ||
                c.Kind == NClang.CursorKind.Constructor || c.Kind == NClang.CursorKind.Destructor ||
                c.Kind == NClang.CursorKind.FunctionDeclaration)
            {
                result = true;
            }

            return result;
        }

        private List<ClangCursor> FindFunctions(ClangCursor head, string name)
        {
            var result = new List<ClangCursor>();

            if (name != string.Empty)
            {
                foreach (var c in head.GetChildren())
                {
                    if (c.Spelling == name)
                    {
                        if (CursorIsValidDeclaration(c))
                        {
                            if (!result.Any(cc => cc.DisplayName == c.DisplayName))
                            {
                                result.Add(c);
                            }
                        }
                    }

                    result.AddRange(FindFunctions(c, name));
                }
            }

            return result;
        }

        public async Task<SignatureHelp> SignatureHelp(ISourceFile file, UnsavedFile buffer, List<UnsavedFile> unsavedFiles, int line, int column, int offset, string methodName)
        {
            SignatureHelp result = null;
            var clangUnsavedFiles = new List<ClangUnsavedFile>();

            unsavedFiles.Add(buffer);

            foreach (var unsavedFile in unsavedFiles)
            {
                if (Platform.CompareFilePath(unsavedFile.FileName, buffer.FileName) != 0)
                {
                    clangUnsavedFiles.Add(new ClangUnsavedFile(unsavedFile.FileName, unsavedFile.Contents));
                }
            }

            var symbols = await GetSymbolsAsync(file, unsavedFiles, methodName);

            if (symbols.Count > 0)
            {
                result = new SignatureHelp();
                result.Offset = offset;

                foreach (var symbol in symbols)
                {
                    result.Signatures.Add(SignatureFromSymbol(symbol));
                }
            }

            return result;
        }
    }

    internal class CPlusPlusDataAssociation
    {
        public CPlusPlusDataAssociation(TextDocument textDocument)
        {
            BackgroundRenderers = new List<IBackgroundRenderer>();
            DocumentLineTransformers = new List<IDocumentLineTransformer>();

            TextColorizer = new TextColoringTransformer(textDocument);
            TextMarkerService = new TextMarkerService(textDocument);

            BackgroundRenderers.Add(new BracketMatchingBackgroundRenderer());
            BackgroundRenderers.Add(TextMarkerService);

            DocumentLineTransformers.Add(TextColorizer);
            DocumentLineTransformers.Add(new DefineTextLineTransformer());
            DocumentLineTransformers.Add(new PragmaMarkTextLineTransformer());
            DocumentLineTransformers.Add(new IncludeTextLineTransformer());
        }

        public ClangTranslationUnit TranslationUnit { get; set; }
        public TextColoringTransformer TextColorizer { get; }
        public TextMarkerService TextMarkerService { get; }
        public List<IBackgroundRenderer> BackgroundRenderers { get; }
        public List<IDocumentLineTransformer> DocumentLineTransformers { get; }        
        public EventHandler<KeyEventArgs> KeyUpHandler { get; set; }
        public EventHandler<TextInputEventArgs> TextInputHandler { get; set; }
    }
}