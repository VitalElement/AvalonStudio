using AvaloniaEdit.Indentation;
using AvaloniaEdit.Indentation.CSharp;
using AvalonStudio.CodeEditor;
using AvalonStudio.Controls;
using AvalonStudio.Documents;
using AvalonStudio.Editor;
using AvalonStudio.Extensibility;
using AvalonStudio.Extensibility.Editor;
using AvalonStudio.Extensibility.Languages;
using AvalonStudio.Extensibility.Languages.CompletionAssistance;
using AvalonStudio.Extensibility.Threading;
using AvalonStudio.Platforms;
using AvalonStudio.Projects;
using AvalonStudio.Projects.Standard;
using AvalonStudio.Utils;
using NClang;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace AvalonStudio.Languages.CPlusPlus
{
    internal class CPlusPlusLanguageService : ILanguageService
    {
        private static readonly ClangIndex index = ClangService.CreateIndex();

        private ClangTranslationUnit _translationUnit;
        private JobRunner clangAccessJobRunner;
        private CancellationTokenSource _runnerCanellationSource;

        private Dictionary<string, Func<string, string>> _snippetCodeGenerators;
        private Dictionary<string, Func<int, int, int, string>> _snippetDynamicVars;
        private ITextEditor _editor;

        public CPlusPlusLanguageService()
        {
            _runnerCanellationSource = new CancellationTokenSource();
            _snippetCodeGenerators = new Dictionary<string, Func<string, string>>();
            _snippetDynamicVars = new Dictionary<string, Func<int, int, int, string>>();

            _snippetCodeGenerators.Add("ToFieldName", (propertyName) =>
            {
                if (string.IsNullOrEmpty(propertyName))
                    return propertyName;
                string newName = Char.ToLower(propertyName[0]) + propertyName.Substring(1);
                if (newName == propertyName)
                    return "_" + newName;
                else
                    return newName;
            });

            _snippetCodeGenerators.Add("Dereference", (accessReference) =>
            {
                switch (accessReference)
                {
                    case "&":
                        return "*";

                    default:
                        return "";
                }
            });

            _snippetCodeGenerators.Add("ToStorageReference", (accessReference) =>
            {
                switch (accessReference)
                {
                    case "&":
                    case "*":
                        return "*";

                    default:
                        return "";
                }
            });

            _snippetDynamicVars.Add("ClassName", (offset, line, column) => null);
        }

        public bool CanTriggerIntellisense(char currentChar, char previousChar)
        {
            bool result = false;

            if (IntellisenseTriggerCharacters.Contains(currentChar))
            {
                result = true;
            }
            else if (currentChar == ':' && previousChar == ':')
            {
                result = true;
            }

            return result;
        }

        public IEnumerable<char> IntellisenseTriggerCharacters { get; } = new[]
        {
            '.', '>', '#'
        };

        public IEnumerable<char> IntellisenseSearchCharacters { get; } = new[]
        {
            '(', ')', '.', ':', '-', '<', '>', '[', ']', ';', '"', '#', ','
        };

        public IEnumerable<char> IntellisenseCompleteCharacters { get; } = new[]
        {
            ',', '.', ':', ';', '-', ' ', '(', ')', '[', ']', '<', '>', '=', '+', '*', '/', '%', '|', '&', '!', '^'
        };

        public bool IsValidIdentifierCharacter(char data)
        {
            return char.IsLetterOrDigit(data) || data == '_';
        }

        public IDictionary<string, Func<string, string>> SnippetCodeGenerators => _snippetCodeGenerators;

        public IDictionary<string, Func<int, int, int, string>> SnippetDynamicVariables => _snippetDynamicVars;

        public string LanguageId => "cpp";

        public IEnumerable<ITextEditorInputHelper> InputHelpers { get; }
            = new ITextEditorInputHelper[] { new AutoBrackedInputHelper(), new CBasedLanguageIndentationInputHelper() };

        private CodeCompletionKind FromClangKind(NClang.CursorKind kind)
        {
            switch (kind)
            {
                case NClang.CursorKind.FunctionDeclaration:
                case NClang.CursorKind.CXXMethod:
                case NClang.CursorKind.Constructor:
                case NClang.CursorKind.Destructor:
                case NClang.CursorKind.FunctionTemplate:
                case NClang.CursorKind.ClassTemplate:
                    return CodeCompletionKind.MethodPublic;

                case NClang.CursorKind.ClassDeclaration:
                    return CodeCompletionKind.ClassPublic;

                case NClang.CursorKind.StructDeclaration:
                    return CodeCompletionKind.StructurePublic;

                case NClang.CursorKind.MacroDefinition:
                    return CodeCompletionKind.Macro;

                case NClang.CursorKind.NotImplemented:
                case NClang.CursorKind.TypedefDeclaration:
                    return CodeCompletionKind.Keyword;

                case NClang.CursorKind.EnumDeclaration:
                    return CodeCompletionKind.EnumPublic;

                case NClang.CursorKind.EnumConstantDeclaration:
                    return CodeCompletionKind.EnumMemberPublic;

                case NClang.CursorKind.VarDeclaration:
                    return CodeCompletionKind.Variable;

                case NClang.CursorKind.Namespace:
                    return CodeCompletionKind.NamespacePublic;

                case NClang.CursorKind.ParmDeclaration:
                    return CodeCompletionKind.FieldPublic;

                case NClang.CursorKind.FieldDeclaration:
                    return CodeCompletionKind.Parameter;

                case NClang.CursorKind.OverloadCandidate:
                    return CodeCompletionKind.OverloadCandidate;
            }

            //Console.WriteLine($"dont understand{kind.ToString()}");
            return CodeCompletionKind.None;
        }

        public async Task<CodeCompletionResults> CodeCompleteAtAsync(int index, int line, int column,
            IEnumerable<UnsavedFile> unsavedFiles, char lastChar, string filter)
        {
            var clangUnsavedFiles = new List<ClangUnsavedFile>();

            foreach (var unsavedFile in unsavedFiles)
            {
                clangUnsavedFiles.Add(new ClangUnsavedFile(unsavedFile.FileName, unsavedFile.Contents));
            }

            var result = new CodeCompletionResults();

            await clangAccessJobRunner.InvokeAsync(() =>
            {
                var translationUnit = GetAndParseTranslationUnit(clangUnsavedFiles);

                if (translationUnit != null)
                {
                    var completionResults = translationUnit.CodeCompleteAt(_editor.SourceFile.Location, line, column, clangUnsavedFiles.ToArray(),
                        CodeCompleteFlags.IncludeBriefComments | CodeCompleteFlags.IncludeMacros | CodeCompleteFlags.IncludeCodePatterns);
                    completionResults.Sort();

                    result.Contexts = (CompletionContext)completionResults.Contexts;

                    if (result.Contexts == CompletionContext.Unexposed && lastChar == ':')
                    {
                        result.Contexts = CompletionContext.AnyType; // special case Class::<- here static class member access.
                    }

                    foreach (var codeCompletion in completionResults.Results)
                    {
                        var typedText = string.Empty;

                        if (codeCompletion.CompletionString.Availability == AvailabilityKind.Available || codeCompletion.CompletionString.Availability == AvailabilityKind.Deprecated)
                        {
                            foreach (var chunk in codeCompletion.CompletionString.Chunks)
                            {
                                if (chunk.Kind == CompletionChunkKind.TypedText)
                                {
                                    typedText = chunk.Text;
                                }

                                // TODO construct chunks into replacement text.

                                // i.e. do should insert do {} while();

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
                                }
                            }

                            if (filter == string.Empty || typedText.StartsWith(filter))
                            {
                                var completion = new CodeCompletionData(typedText, typedText, typedText)
                                {
                                    Priority = (int)codeCompletion.CompletionString.Priority,
                                    Kind = FromClangKind(codeCompletion.CursorKind),
                                    BriefComment = codeCompletion.CompletionString.BriefComment
                                };

                                result.Completions.Add(completion);

                                if (completion.Kind == CodeCompletionKind.OverloadCandidate)
                                {
                                    //Console.WriteLine("TODO Implement overload candidate.");
                                }
                            }
                        }
                    }

                    completionResults.Dispose();
                }
            });

            return result;
        }

        private OffsetSyntaxHighlightingData CreateOffsetData(NClang.ClangCursor cursor, NClang.ClangCursor parent)
        {
            HighlightType highlightKind = HighlightType.Literal;

            bool useSpellingLocation = false;

            switch (cursor.Kind)
            {
                case NClang.CursorKind.StringLiteral:
                case NClang.CursorKind.CharacterLiteral:
                    break;

                case NClang.CursorKind.IntegerLiteral:
                case NClang.CursorKind.FloatingLiteral:
                case NClang.CursorKind.ImaginaryLiteral:
                    highlightKind = HighlightType.NumericLiteral;
                    break;

                case NClang.CursorKind.Constructor:
                case NClang.CursorKind.Destructor:
                case NClang.CursorKind.TypedefDeclaration:
                case NClang.CursorKind.ClassDeclaration:
                case NClang.CursorKind.TemplateReference:
                    useSpellingLocation = true;
                    highlightKind = HighlightType.ClassName;
                    break;

                case NClang.CursorKind.EnumDeclaration:
                case NClang.CursorKind.UnionDeclaration:
                    useSpellingLocation = true;
                    highlightKind = HighlightType.EnumTypeName;
                    break;

                case NClang.CursorKind.TemplateTypeParameter:
                    useSpellingLocation = true;
                    highlightKind = HighlightType.InterfaceName;
                    break;

                case NClang.CursorKind.TypeReference:
                    if (parent.Kind == NClang.CursorKind.CXXBaseSpecifier)
                    {
                        highlightKind = HighlightType.ClassName;
                        useSpellingLocation = true;
                    }
                    else if (cursor.CursorType.Kind == NClang.TypeKind.Enum)
                    {
                        highlightKind = HighlightType.EnumTypeName;
                    }
                    else if (cursor.CursorType.Kind == NClang.TypeKind.Record && cursor.Spelling.StartsWith("union"))
                    {
                        highlightKind = HighlightType.EnumTypeName;
                    }
                    else
                    {
                        highlightKind = HighlightType.ClassName;
                    }
                    break;

                case NClang.CursorKind.CXXMethod:
                case NClang.CursorKind.FunctionDeclaration:
                    useSpellingLocation = true;
                    highlightKind = HighlightType.CallExpression;
                    break;

                case NClang.CursorKind.FirstExpression:
                    if (parent.Kind == NClang.CursorKind.CallExpression && cursor.CursorType.Kind == NClang.TypeKind.Pointer && cursor.CursorType.PointeeType.Kind == NClang.TypeKind.FunctionProto)
                    {
                        useSpellingLocation = true;
                        highlightKind = HighlightType.CallExpression;
                    }
                    else
                    {
                        return null;
                    }
                    break;

                case NClang.CursorKind.MemberReferenceExpression:
                    if (parent.Kind == NClang.CursorKind.CallExpression && cursor.CursorType.Kind == NClang.TypeKind.Pointer && cursor.CursorType.PointeeType.Kind == NClang.TypeKind.FunctionProto)
                    {
                        useSpellingLocation = true;
                        highlightKind = HighlightType.CallExpression;
                    }
                    else if (parent.Kind == NClang.CursorKind.CallExpression && cursor.CursorType.Kind == NClang.TypeKind.Unexposed)
                    {
                        useSpellingLocation = true;
                        highlightKind = HighlightType.CallExpression;
                    }
                    else
                    {
                        return null;
                    }
                    break;

                default:
                    return null;
            }

            if (highlightKind == HighlightType.ClassName)
            {
                string spelling = cursor.Spelling;

                if (cursor.Kind == NClang.CursorKind.TypeReference && parent.Kind == NClang.CursorKind.CXXBaseSpecifier)
                {
                    spelling = cursor.Spelling.Replace("class ", string.Empty);
                }
                if (spelling.Length > 1 && spelling.StartsWith("I") && char.IsUpper(spelling[1]))
                {
                    highlightKind = HighlightType.InterfaceName;
                }
            }

            if (useSpellingLocation)
            {
                if (cursor.Kind == NClang.CursorKind.TypeReference && parent.Kind == NClang.CursorKind.CXXBaseSpecifier && cursor.Spelling.StartsWith("class"))
                {
                    return new OffsetSyntaxHighlightingData()
                    {
                        Start = cursor.Location.SpellingLocation.Offset,
                        Length = cursor.Spelling.Length - 5, // Because spelling includes keyword "class"
                        Type = highlightKind
                    };
                }
                else if ((cursor.Kind == NClang.CursorKind.Destructor || cursor.Kind == NClang.CursorKind.Constructor) && parent.Kind == NClang.CursorKind.ClassTemplate)
                {
                    return new OffsetSyntaxHighlightingData()
                    {
                        Start = cursor.Location.SpellingLocation.Offset,
                        Length = cursor.Spelling.Length, // TODO select only the name...
                        Type = highlightKind
                    };
                }
                else
                {
                    return new OffsetSyntaxHighlightingData()
                    {
                        Start = cursor.Location.SpellingLocation.Offset,
                        Length = cursor.Spelling.Length,
                        Type = highlightKind
                    };
                }
            }
            else
            {
                return new OffsetSyntaxHighlightingData()
                {
                    Start = cursor.CursorExtent.Start.FileLocation.Offset,
                    Length = cursor.CursorExtent.End.FileLocation.Offset - cursor.CursorExtent.Start.FileLocation.Offset,
                    Type = highlightKind
                };
            }
        }

        private void ScanTokens(NClang.ClangTranslationUnit tu, SyntaxHighlightDataList result)
        {
            var tokens = tu.Tokenize(tu.GetCursor().CursorExtent);

            foreach (var token in tokens.Tokens)
            {
                var highlightData = new OffsetSyntaxHighlightingData
                {
                    Start = token.Extent.Start.FileLocation.Offset
                };

                highlightData.Length = token.Extent.End.FileLocation.Offset - highlightData.Start;

                switch (token.Kind)
                {
                    case TokenKind.Comment:
                        highlightData.Type = HighlightType.Comment;
                        result.Add(highlightData);
                        break;

                    case TokenKind.Keyword:
                        highlightData.Type = HighlightType.Keyword;
                        result.Add(highlightData);
                        break;
                }
            }
        }

        private void GenerateHighlightData(ClangCursor cursor, SyntaxHighlightDataList highlightList, List<IndexEntry> result)
        {
            cursor.VisitChildren((current, parent, ptr) =>
            {
                if (current.Location.IsFromMainFile)
                {
                    var highlight = CreateOffsetData(current, parent);

                    if (highlight != null)
                    {
                        highlightList.Add(highlight);
                    }

                    switch (current.Kind)
                    {
                        case NClang.CursorKind.CompoundStatement:
                        case NClang.CursorKind.ClassDeclaration:
                        case NClang.CursorKind.Namespace:
                            result.Add(new IndexEntry(current.Spelling, current.CursorExtent.Start.FileLocation.Offset,
                            current.CursorExtent.End.FileLocation.Offset, (CursorKind)current.Kind));
                            break;
                    }

                    return ChildVisitResult.Recurse;
                }

                if (current.Location.IsInSystemHeader)
                {
                    return ChildVisitResult.Continue;
                }

                return ChildVisitResult.Recurse;
            }, IntPtr.Zero);
        }

        private void GenerateDiagnostics(IEnumerable<ClangDiagnostic> clangDiagnostics, ClangTranslationUnit translationUnit, ISourceFile file, List<Diagnostic> result)
        {
            foreach (var diagnostic in clangDiagnostics)
            {
                if (diagnostic.Location.IsFromMainFile)
                {
                    var diag = new Diagnostic(
                        diagnostic.Location.FileLocation.Offset,
                        0,
                        file.Project.Name,
                        diagnostic.Location.FileLocation.File.FileName,
                        diagnostic.Location.FileLocation.Line,
                        diagnostic.Spelling,
                        "",
                        (DiagnosticLevel)diagnostic.Severity,
                        DiagnosticCategory.Compiler);

                    var cursor = translationUnit.GetCursor(diagnostic.Location);

                    var tokens = translationUnit.Tokenize(cursor.CursorExtent);

                    foreach (var token in tokens.Tokens)
                    {
                        if (token.Location == diagnostic.Location)
                        {
                            diag.EndOffset = diag.StartOffset + token.Spelling.Length;
                        }
                    }

                    result.Add(diag);
                    tokens.Dispose();
                }
            }
        }

        public async Task<CodeAnalysisResults> RunCodeAnalysisAsync(IEnumerable<UnsavedFile> unsavedFiles,
            Func<bool> interruptRequested)
        {
            var result = new CodeAnalysisResults();

            var clangUnsavedFiles = new List<ClangUnsavedFile>();

            clangUnsavedFiles.AddRange(unsavedFiles.Select(f => new ClangUnsavedFile(f.FileName, f.Contents)));

            var diagnostics = new List<Diagnostic>();

            await clangAccessJobRunner.InvokeAsync(() =>
            {
                try
                {
                    var translationUnit = GetAndParseTranslationUnit(clangUnsavedFiles);

                    if (translationUnit != null)
                    {
                        if (_editor.SourceFile != null && translationUnit != null)
                        {
                            ScanTokens(translationUnit, result.SyntaxHighlightingData);

                            GenerateHighlightData(translationUnit.GetCursor(), result.SyntaxHighlightingData, result.IndexItems);
                        }

                        GenerateDiagnostics(translationUnit.DiagnosticSet.Items, translationUnit, _editor.SourceFile, diagnostics);
                    }
                }
                catch (Exception)
                {
                }
            });

            var errorList = IoC.Get<IErrorList>();
            errorList.Remove((this, _editor.SourceFile));
            errorList.Create((this, _editor.SourceFile), _editor.SourceFile.FilePath, DiagnosticSourceKind.Analysis, diagnostics.ToImmutableArray());

            return result;
        }

        public void RegisterEditor(ITextEditor editor)
        {
            _editor = editor;

            if (clangAccessJobRunner == null)
            {
                clangAccessJobRunner = new JobRunner();

                Task.Factory.StartNew(() => { clangAccessJobRunner.RunLoop(_runnerCanellationSource.Token); });
            }
        }

        public void UnregisterEditor()
        {
            clangAccessJobRunner.InvokeAsync(() =>
            {
                _translationUnit?.Dispose();
                _runnerCanellationSource.Cancel();
            });
        }

        public int Format(uint offset, uint length, int cursor)
        {
            bool replaceCursor = cursor >= 0 ? true : false;

            if (!replaceCursor)
            {
                cursor = 0;
            }

            var replacements = ClangFormat.FormatXml(_editor.SourceFile.Location, _editor.Document.Text, offset, length, (uint)cursor);

            if (replacements != null)
            {
                return ApplyReplacements(_editor.Document, cursor, replacements, replaceCursor);
            }

            return cursor;
        }

        public async Task<QuickInfoResult> QuickInfo(IEnumerable<UnsavedFile> unsavedFiles, int offset)
        {
            StyledText styledText = null;

            var clangUnsavedFiles = new List<ClangUnsavedFile>();

            foreach (var unsavedFile in unsavedFiles)
            {
                clangUnsavedFiles.Add(new ClangUnsavedFile(unsavedFile.FileName, unsavedFile.Contents));
            }

            await clangAccessJobRunner.InvokeAsync(() =>
            {
                var tu = GetAndParseTranslationUnit(clangUnsavedFiles);

                if (tu != null)
                {
                    var cursor = tu.GetCursor(tu.GetLocationForOffset(tu.GetFile(_editor.SourceFile.FilePath), offset));

                    switch (cursor.Kind)
                    {
                        case NClang.CursorKind.MemberReferenceExpression:
                        case NClang.CursorKind.DeclarationReferenceExpression:
                        case NClang.CursorKind.CallExpression:
                        case NClang.CursorKind.TypeReference:
                            styledText = InfoTextFromCursor(cursor.Referenced);
                            break;

                        case NClang.CursorKind.NoDeclarationFound:
                            break;

                        default:
                            styledText = InfoTextFromCursor(cursor);
                            break;
                    }
                }
            });

            return styledText == null ? null : new QuickInfoResult(styledText);
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

                        var arg = new ParameterSymbol
                        {
                            IsBuiltInType = IsBuiltInType(argument.CursorType),
                            Name = argument.Spelling,

                            TypeDescription = argument.CursorType.Spelling
                        };
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

                                if (paragraph != null)
                                {
                                    if (isVarArgs != null)
                                    {
                                        result.Arguments.Last().Comment = paragraph.Value;
                                    }
                                    else
                                    {
                                        var inx = argument.Element("Index");

                                        if (inx != null)
                                        {
                                            // This happens when documentation for an argument was left in, but the argument no longer exists.
                                            var index = int.Parse(inx.Value);

                                            result.Arguments[index].Comment = paragraph.Value;
                                        }
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

        public async Task<List<Symbol>> GetSymbolsAsync(IEnumerable<UnsavedFile> unsavedFiles, string name)
        {
            var results = new List<Symbol>();

            if (name != string.Empty)
            {
                var clangUnsavedFiles = new List<ClangUnsavedFile>();

                foreach (var unsavedFile in unsavedFiles)
                {
                    clangUnsavedFiles.Add(new ClangUnsavedFile(unsavedFile.FileName, unsavedFile.Contents));
                }

                await clangAccessJobRunner.InvokeAsync(() =>
                {
                    var translationUnit = GetAndParseTranslationUnit(clangUnsavedFiles);

                    if (translationUnit != null)
                    {
                        var cursors = FindFunctions(translationUnit.GetCursor(), name);

                        foreach (var cursor in cursors)
                        {
                            results.Add(SymbolFromClangCursor(cursor));
                        }
                    }
                });
            }

            return results;
        }

        public int Comment(int firstLine, int endLine, int caret = -1, bool format = true)
        {
            var result = caret;
            var textDocument = _editor.Document;

            using (textDocument.RunUpdate())
            {
                for (int line = firstLine; line <= endLine; line++)
                {
                    textDocument.Insert(textDocument.GetLineByNumber(line).Offset, "//");
                }

                if (format)
                {
                    var startOffset = textDocument.GetLineByNumber(firstLine).Offset;
                    var endOffset = textDocument.GetLineByNumber(endLine).EndOffset;
                    result = Format((uint)startOffset, (uint)(endOffset - startOffset), caret);
                }
            }
            return result;
        }

        public int UnComment(int firstLine, int endLine, int caret = -1, bool format = true)
        {
            var result = caret;

            var textDocument = _editor.Document;

            using (textDocument.RunUpdate())
            {
                for (int line = firstLine; line <= endLine; line++)
                {
                    var docLine = textDocument.GetLineByNumber(line);
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
                    result = Format((uint)startOffset, (uint)(endOffset - startOffset), caret);
                }
            }

            return result;
        }

        private void AddArguments(List<string> list, IEnumerable<string> arguments)
        {
            if (list != null && arguments != null)
            {
                foreach (var argument in arguments)
                {
                    AddArgument(list, argument);
                }
            }
        }

        private void AddArgument(List<string> list, string argument)
        {
            if (list != null)
            {
                if (!list.Contains(argument))
                {
                    list.Add(argument);
                }
            }
        }

        private ClangTranslationUnit GenerateTranslationUnit(List<ClangUnsavedFile> unsavedFiles)
        {
            ClangTranslationUnit result = null;

            var file = _editor.SourceFile;

            if (System.IO.File.Exists(file.Location))
            {
                var args = new List<string>();

                if (!(file.Project.Solution.StartupProject is IStandardProject superProject))
                {
                    superProject = file.Project as IStandardProject;
                }

                var project = file.Project as IStandardProject;

                var toolchainIncludes = superProject?.ToolChain?.GetToolchainIncludes(file);

                if (toolchainIncludes != null)
                {
                    AddArguments(args, toolchainIncludes.Select(s => $"-isystem{s}"));
                }

                // toolchain includes
                // This code is same as in toolchain, get compiler arguments... does this need a refactor, or toolchain get passed in? Clang take GCC compatible arguments.
                // perhaps this language service has its own clang tool chain, to generate compiler arguments from project configuration?

                // Referenced includes
                var referencedIncludes = project.GetReferencedIncludes();

                AddArguments(args, referencedIncludes.Select(s => $"-I{s}"));

                // global includes
                var globalIncludes = superProject?.GetGlobalIncludes();

                AddArguments(args, globalIncludes?.Select(s => $"-I{s}"));

                // includes
                AddArguments(args, project.Includes.Select(s => $"-I{Path.Combine(project.CurrentDirectory, s.Value)}"));

                var referencedDefines = project.GetReferencedDefines();

                AddArguments(args, referencedDefines.Select(s => $"-D{s}"));

                // global includes
                var globalDefines = superProject?.GetGlobalDefines();

                AddArguments(args, globalDefines?.Select(s => $"-D{s}"));

                AddArguments(args, project.Defines.Select(s => $"-D{s}"));

                switch (file.Extension)
                {
                    case ".cc":
                    case ".c":
                        {
                            AddArguments(args, superProject?.CCompilerArguments);
                        }
                        break;

                    case ".cpp":
                        {
                            AddArguments(args, superProject?.CppCompilerArguments);
                        }
                        break;
                }

                // TODO do we mark files as class header? CAn clang auto detect this?
                //if (file.Language == Language.Cpp)
                {
                    args.Add("-xc++");
                    args.Add("-std=c++14");
                    args.Add("-D__STDC__"); // This is needed to ensure inbuilt functions are appropriately prototyped.
                }

                args.Add("-Wunused-variable");

                var translationUnitFlags =
                    TranslationUnitFlags.IncludeBriefCommentsInCodeCompletion |
                    TranslationUnitFlags.PrecompiledPreamble |
                    TranslationUnitFlags.CacheCompletionResults |
                    TranslationUnitFlags.Incomplete;

                result = index.ParseTranslationUnit(file.Location, args.ToArray(), unsavedFiles.ToArray(), translationUnitFlags);
            }

            if (result == null)
            {
                throw new Exception("Error generating translation unit.");
            }

            return result;
        }

        private ClangTranslationUnit GetAndParseTranslationUnit(List<ClangUnsavedFile> unsavedFiles)
        {
            if (_translationUnit == null)
            {
                _translationUnit = GenerateTranslationUnit(unsavedFiles);
            }

            // Always do a reparse, as a workaround for some issues in libclang 3.7.1
            _translationUnit.Reparse(unsavedFiles.ToArray(), ReparseTranslationUnitFlags.None);

            return _translationUnit;
        }

        public static int ApplyReplacements(ITextDocument document, int cursor, XDocument replacements, bool replaceCursor = true)
        {
            var elements = replacements.Elements().First().Elements();

            using (document.RunUpdate())
            {
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

                            document.Replace(offsetChange + offset, replacementLength, element.Value);

                            offsetChange += element.Value.Length - replacementLength;
                            break;
                    }
                }
            }

            return replaceCursor ? cursor : -1;
        }

        private static StyledText InfoTextFromCursor(ClangCursor cursor)
        {
            var result = StyledText.Create();

            string name = "";
            CursorKind kind = (CursorKind)cursor.Kind;

            switch (cursor.Kind)
            {
                case NClang.CursorKind.CXXAccessSpecifier:
                    name = "(Access Specifier) " + cursor.CxxAccessSpecifier;
                    break;

                default:
                    name = cursor.Spelling;
                    break;
            }

            var theme = ColorScheme.CurrentColorScheme;

            if (cursor.Kind == NClang.CursorKind.VarDeclaration)
            {
                switch (cursor.Linkage)
                {
                    case NClang.LinkageKind.NoLinkage:
                        result.Append("(local variable) ");
                        break;

                    case NClang.LinkageKind.Internal:
                        result.Append("(static variable) ");
                        break;

                    case NClang.LinkageKind.External:
                        result.Append("(global variable) ");
                        break;
                }
            }

            switch (cursor.CxxAccessSpecifier)
            {
                case CXXAccessSpecifier.Private:
                    result.Append("(private) ");
                    break;

                case CXXAccessSpecifier.Protected:
                    result.Append("(protected) ");
                    break;

                case CXXAccessSpecifier.Public:
                    result.Append("(public) ");
                    break;
            }

            if (cursor.ResultType != null)
            {
                result.Append(cursor.ResultType.Spelling + " ", IsBuiltInType(cursor.ResultType) ? theme.Keyword : theme.Type);
            }
            else if (cursor.CursorType != null)
            {
                switch (kind)
                {
                    case CursorKind.ClassDeclaration:
                    case CursorKind.CXXThisExpression:
                        result.Append("class ", theme.Keyword);
                        break;

                    case CursorKind.Namespace:
                        result.Append("namespace ", theme.Keyword);
                        break;

                    case CursorKind.TypedefDeclaration:
                        result.Append("typedef ", theme.Keyword);
                        break;

                    case CursorKind.EnumDeclaration:
                        result.Append("enum ", theme.Keyword);
                        break;

                    case CursorKind.StructDeclaration:
                        result.Append("struct ", theme.Keyword);
                        break;

                    case CursorKind.UnionDeclaration:
                        result.Append("union ", theme.Keyword);
                        break;
                }

                result.Append(cursor.CursorType.Spelling + " ", IsBuiltInType(cursor.ResultType) ? theme.Keyword : theme.Type);
            }

            switch (kind)
            {
                case CursorKind.UnionDeclaration:
                case CursorKind.TypedefDeclaration:
                case CursorKind.StructDeclaration:
                case CursorKind.ClassDeclaration:
                case CursorKind.CXXThisExpression:
                case CursorKind.Namespace:
                case CursorKind.EnumDeclaration:
                    break;

                default:
                    result.Append(name);
                    break;
            }

            string parsedDocumentation = "";

            switch (kind)
            {
                case CursorKind.EnumConstantDeclaration:
                    result.Append(" = " + cursor.EnumConstantDeclUnsignedValue.ToString());
                    result.Append(" (0x" + cursor.EnumConstantDeclUnsignedValue.ToString("X") + ")");
                    break;

                case CursorKind.FunctionDeclaration:
                case CursorKind.CXXMethod:
                case CursorKind.Constructor:
                case CursorKind.Destructor:
                    result.Append(" (");

                    for (var i = 0; i < cursor.ArgumentCount; i++)
                    {
                        var argument = cursor.GetArgument(i);

                        result.Append(argument.CursorType.Spelling + " ", IsBuiltInType(argument.CursorType) ? theme.Keyword : theme.Type);

                        result.Append(argument.Spelling + (i == cursor.ArgumentCount - 1 ? "" : ", "));
                    }

                    if (cursor.IsVariadic)
                    {
                        result.Append(", ... variadic");
                    }

                    if (cursor.ArgumentCount == 0)
                    {
                        result.Append("void", theme.Keyword);
                    }

                    result.Append(")");

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

                                if (paragraph != null)
                                {
                                    if (isVarArgs != null)
                                    {
                                        parsedDocumentation += paragraph.Value + Environment.NewLine;
                                    }
                                    else
                                    {
                                        var inx = argument.Element("Index");

                                        if (inx != null)
                                        {
                                            parsedDocumentation += paragraph.Value + Environment.NewLine;
                                        }
                                    }
                                }
                            }
                        }
                    }
                    break;
            }

            if (cursor.BriefCommentText != null || !string.IsNullOrEmpty(parsedDocumentation))
            {
                result.AppendLine();

                if (cursor.BriefCommentText != null)
                {
                    result.AppendLine(cursor.BriefCommentText);
                }

                if (!string.IsNullOrEmpty(parsedDocumentation))
                {
                    result.Append(parsedDocumentation);
                }
            }

            return result;
        }

        private static Signature SignatureFromSymbol(Symbol symbol)
        {
            var result = new Signature
            {
                Name = symbol.Name,
                Description = symbol.BriefComment
            };

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

        public async Task<SignatureHelp> SignatureHelp(IEnumerable<UnsavedFile> unsavedFiles, int offset, string methodName)
        {
            SignatureHelp result = null;
            var clangUnsavedFiles = new List<ClangUnsavedFile>();

            foreach (var unsavedFile in unsavedFiles)
            {
                if (Platform.CompareFilePath(unsavedFile.FileName, _editor.SourceFile.Location) != 0)
                {
                    clangUnsavedFiles.Add(new ClangUnsavedFile(unsavedFile.FileName, unsavedFile.Contents));
                }
            }

            var symbols = await GetSymbolsAsync(unsavedFiles, methodName);

            if (symbols.Count > 0)
            {
                result = new SignatureHelp(offset - methodName.Length);

                foreach (var symbol in symbols)
                {
                    result.Signatures.Add(SignatureFromSymbol(symbol));
                }
            }

            return result;
        }

        public Task<GotoDefinitionInfo> GotoDefinition(int offset)
        {
            return Task.FromResult<GotoDefinitionInfo>(null);
        }

        public Task<IEnumerable<SymbolRenameInfo>> RenameSymbol(string renameTo)
        {
            return Task.FromResult<IEnumerable<SymbolRenameInfo>>(null);
        }

        public IEnumerable<IContextActionProvider> GetContextActionProviders()
        {
            return Enumerable.Empty<IContextActionProvider>();
        }
    }
}