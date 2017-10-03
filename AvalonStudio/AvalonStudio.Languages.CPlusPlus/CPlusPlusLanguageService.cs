using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Media;
using AvaloniaEdit.Document;
using AvaloniaEdit.Indentation;
using AvaloniaEdit.Indentation.CSharp;
using AvaloniaEdit.Rendering;
using AvalonStudio.CodeEditor;
using AvalonStudio.Extensibility.Editor;
using AvalonStudio.Extensibility.Languages;
using AvalonStudio.Extensibility.Languages.CompletionAssistance;
using AvalonStudio.Extensibility.Threading;
using AvalonStudio.Platforms;
using AvalonStudio.Projects;
using AvalonStudio.Projects.CPlusPlus;
using AvalonStudio.Projects.Standard;
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

        private Dictionary<string, Func<string, string>> _snippetCodeGenerators;
        private Dictionary<string, Func<int, int, int, string>> _snippetDynamicVars;

        public CPlusPlusLanguageService()
        {
            clangAccessJobRunner = new JobRunner();

            Task.Factory.StartNew(() => { clangAccessJobRunner.RunLoop(new CancellationToken()); });

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

        public IIndentationStrategy IndentationStrategy { get; private set; }

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

        public IEnumerable<char> IntellisenseTriggerCharacters => new[]
        {
            '.', '>', '#'
        };

        public IEnumerable<char> IntellisenseSearchCharacters => new[]
        {
            '(', ')', '.', ':', '-', '<', '>', '[', ']', ';', '"', '#', ','
        };

        public IEnumerable<char> IntellisenseCompleteCharacters => new[]
        {
            ',', '.', ':', ';', '-', ' ', '(', ')', '[', ']', '<', '>', '=', '+', '*', '/', '%', '|', '&', '!', '^'
        };

        public bool IsValidIdentifierCharacter (char data)
        {
            return char.IsLetterOrDigit(data) || data == '_';
        }        

        public IDictionary<string, Func<string, string>> SnippetCodeGenerators => _snippetCodeGenerators;

        public IDictionary<string, Func<int, int, int, string>> SnippetDynamicVariables => _snippetDynamicVars;

        public string LanguageId => "cpp";

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

            Console.WriteLine($"dont understand{kind.ToString()}");
            return CodeCompletionKind.None;
        }

        public async Task<CodeCompletionResults> CodeCompleteAtAsync(ISourceFile file, int index, int line, int column,
            List<UnsavedFile> unsavedFiles, char lastChar, string filter)
        {
            var clangUnsavedFiles = new List<ClangUnsavedFile>();

            foreach (var unsavedFile in unsavedFiles)
            {
                clangUnsavedFiles.Add(new ClangUnsavedFile(unsavedFile.FileName, unsavedFile.Contents));
            }

            var result = new CodeCompletionResults();

            await clangAccessJobRunner.InvokeAsync(() =>
            {
                var translationUnit = GetAndParseTranslationUnit(file, clangUnsavedFiles);

                if (translationUnit != null)
                {
                    var completionResults = translationUnit.CodeCompleteAt(file.Location, line, column, clangUnsavedFiles.ToArray(),
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

                        var hint = string.Empty;

                        if (codeCompletion.CompletionString.Availability == AvailabilityKind.Available || codeCompletion.CompletionString.Availability == AvailabilityKind.Deprecated)
                        {
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
                                var completion = new CodeCompletionData
                                {
                                    Suggestion = typedText,
                                    Priority = codeCompletion.CompletionString.Priority,
                                    Kind = FromClangKind(codeCompletion.CursorKind),
                                    Hint = hint,
                                    BriefComment = codeCompletion.CompletionString.BriefComment
                                };

                                result.Completions.Add(completion);

                                if (completion.Kind == CodeCompletionKind.OverloadCandidate)
                                {
                                    Console.WriteLine("TODO Implement overload candidate.");
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
                var highlightData = new OffsetSyntaxHighlightingData();
                highlightData.Start = token.Extent.Start.FileLocation.Offset;
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

        private void GenerateDiagnostics(IEnumerable<ClangDiagnostic> clangDiagnostics, ClangTranslationUnit translationUnit, IProject project, TextSegmentCollection<Diagnostic> result)
        {
            foreach (var diagnostic in clangDiagnostics)
            {
                if (diagnostic.Location.IsFromMainFile)
                {
                    var diag = new Diagnostic
                    {
                        Project = project,
                        StartOffset = diagnostic.Location.FileLocation.Offset,
                        Line = diagnostic.Location.FileLocation.Line,
                        Spelling = diagnostic.Spelling,
                        File = diagnostic.Location.FileLocation.File.FileName,
                        Level = (DiagnosticLevel)diagnostic.Severity
                    };

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

        public async Task<CodeAnalysisResults> RunCodeAnalysisAsync(ISourceFile file, List<UnsavedFile> unsavedFiles,
            Func<bool> interruptRequested)
        {
            var result = new CodeAnalysisResults();

            var dataAssociation = GetAssociatedData(file);

            var clangUnsavedFiles = new List<ClangUnsavedFile>();

            clangUnsavedFiles.AddRange(unsavedFiles.Select(f => new ClangUnsavedFile(f.FileName, f.Contents)));

            await clangAccessJobRunner.InvokeAsync(() =>
            {
                try
                {
                    var translationUnit = GetAndParseTranslationUnit(file, clangUnsavedFiles);

                    if (translationUnit != null)
                    {
                        if (file != null && translationUnit != null)
                        {
                            ScanTokens(translationUnit, result.SyntaxHighlightingData);

                            GenerateHighlightData(translationUnit.GetCursor(), result.SyntaxHighlightingData, result.IndexItems);
                        }

                        GenerateDiagnostics(translationUnit.DiagnosticSet.Items, translationUnit, file.Project, result.Diagnostics);
                    }
                }
                catch (Exception e)
                {
                }
            });

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

        public void RegisterSourceFile(AvaloniaEdit.TextEditor editor, ISourceFile file, TextDocument doc)
        {
            if (dataAssociations.TryGetValue(file, out CPlusPlusDataAssociation association))
            {
                throw new Exception("Source file already registered with language service.");
            }

            IndentationStrategy = new CSharpIndentationStrategy(editor.Options);

            association = new CPlusPlusDataAssociation();
            dataAssociations.Add(file, association);

            association.TextInputHandler = (sender, e) =>
            {
                if (editor.Document == doc)
                {
                    switch (e.Text)
                    {
                        case "}":
                        case ";":
                            if (IndentationStrategy != null)
                            {
                                var line = editor.Document.GetLineByNumber(editor.TextArea.Caret.Line);
                                // use indentation strategy only if the line is not read-only
                                IndentationStrategy.IndentLine(editor.Document, line);
                            }

                            OpenBracket(editor, editor.Document, e.Text);
                            CloseBracket(editor, editor.Document, e.Text);

                            editor.CaretOffset = Format(file, editor.Document, 0, (uint)editor.Document.TextLength, editor.CaretOffset);
                            break;

                        case "{":
                            if (IndentationStrategy != null)
                            {
                                var line = editor.Document.GetLineByNumber(editor.TextArea.Caret.Line);
                                // use indentation strategy only if the line is not read-only
                                IndentationStrategy.IndentLine(editor.Document, line);
                            }

                            OpenBracket(editor, editor.Document, e.Text);
                            CloseBracket(editor, editor.Document, e.Text);

                            var lineCount = editor.Document.LineCount;
                            var offset = Format(file, editor.Document, 0, (uint)editor.Document.TextLength, editor.CaretOffset);

                            // suggests clang format didnt do anything, so we can assume not moving to new line.
                            if (lineCount != editor.Document.LineCount)
                            {
                                if (offset <= editor.Document.TextLength)
                                {
                                    var newLine = editor.Document.GetLineByOffset(offset);
                                    editor.CaretOffset = newLine.PreviousLine.EndOffset;
                                }
                            }
                            else
                            {
                                editor.CaretOffset = offset;
                            }
                            break;

                        default:
                            OpenBracket(editor, editor.Document, e.Text);
                            CloseBracket(editor, editor.Document, e.Text);
                            break;
                    }
                }
            };

            association.BeforeTextInputHandler = (sender, e) =>
            {
                switch (e.Text)
                {
                    case "\n":
                    case "\r\n":
                        var nextChar = ' ';

                        if (editor.CaretOffset != editor.Document.TextLength)
                        {
                            nextChar = editor.Document.GetCharAt(editor.CaretOffset);
                        }

                        if (nextChar == '}')
                        {
                            var newline = TextUtilities.GetNewLineFromDocument(editor.Document, editor.TextArea.Caret.Line);
                            editor.Document.Insert(editor.CaretOffset, newline);

                            editor.Document.TrimTrailingWhiteSpace(editor.TextArea.Caret.Line - 1);

                            var line = editor.Document.GetLineByNumber(editor.TextArea.Caret.Line);
                            // use indentation strategy only if the line is not read-only
                            IndentationStrategy.IndentLine(editor.Document, line);

                            editor.CaretOffset -= newline.Length;
                        }
                        break;
                }
            };

            editor.TextArea.TextEntered += association.TextInputHandler;
            editor.TextArea.TextEntering += association.BeforeTextInputHandler;
        }

        public void UnregisterSourceFile(AvaloniaEdit.TextEditor editor, ISourceFile file)
        {
            var association = GetAssociatedData(file);

            editor.TextArea.TextEntered -= association.TextInputHandler;
            editor.TextArea.TextEntering -= association.BeforeTextInputHandler;

            var tu = association.TranslationUnit;

            clangAccessJobRunner.InvokeAsync(() =>
            {
                tu?.Dispose();
            });

            dataAssociations.Remove(file);
        }

        public int Format(ISourceFile file, TextDocument textDocument, uint offset, uint length, int cursor)
        {
            bool replaceCursor = cursor >= 0 ? true : false;

            if (!replaceCursor)
            {
                cursor = 0;
            }

            var replacements = ClangFormat.FormatXml(file.Location, textDocument.Text, offset, length, (uint)cursor);

            if(replacements != null)
            {
                return ApplyReplacements(textDocument, cursor, replacements, replaceCursor);
            }

            return cursor;
        }

        public async Task<Symbol> GetSymbolAsync(ISourceFile file, List<UnsavedFile> unsavedFiles, int offset)
        {
            Symbol result = null;
            var associatedData = GetAssociatedData(file);

            var clangUnsavedFiles = new List<ClangUnsavedFile>();

            foreach (var unsavedFile in unsavedFiles)
            {
                clangUnsavedFiles.Add(new ClangUnsavedFile(unsavedFile.FileName, unsavedFile.Contents));
            }

            await clangAccessJobRunner.InvokeAsync(() =>
            {
                var tu = GetAndParseTranslationUnit(file, clangUnsavedFiles);

                if (tu != null)
                {
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
                }
            });

            return result;
        }

        public async Task<List<Symbol>> GetSymbolsAsync(ISourceFile file, List<UnsavedFile> unsavedFiles, string name)
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
                    var translationUnit = GetAndParseTranslationUnit(file, clangUnsavedFiles);

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

        public int Comment(ISourceFile file, TextDocument textDocument, int firstLine, int endLine, int caret = -1, bool format = true)
        {
            var result = caret;

            textDocument.BeginUpdate();

            for (int line = firstLine; line <= endLine; line++)
            {
                textDocument.Insert(textDocument.GetLineByNumber(line).Offset, "//");
            }

            textDocument.EndUpdate();

            if (format)
            {
                var startOffset = textDocument.GetLineByNumber(firstLine).Offset;
                var endOffset = textDocument.GetLineByNumber(endLine).EndOffset;
                result = Format(file, textDocument, (uint)startOffset, (uint)(endOffset - startOffset), caret);
            }

            return result;
        }

        public int UnComment(ISourceFile file, TextDocument textDocument, int firstLine, int endLine, int caret = -1, bool format = true)
        {
            var result = caret;

            textDocument.BeginUpdate();

            for (int line = firstLine; line <= endLine; line++)
            {
                var docLine = textDocument.GetLineByNumber(line);
                var index = textDocument.GetText(docLine).IndexOf("//");

                if (index >= 0)
                {
                    textDocument.Replace(docLine.Offset + index, 2, string.Empty);
                }
            }

            textDocument.EndUpdate();

            if (format)
            {
                var startOffset = textDocument.GetLineByNumber(firstLine).Offset;
                var endOffset = textDocument.GetLineByNumber(endLine).EndOffset;
                result = Format(file, textDocument, (uint)startOffset, (uint)(endOffset - startOffset), caret);
            }

            return result;
        }

        private void AddArguments(List<string> list, IEnumerable<string> arguments)
        {
            foreach (var argument in arguments)
            {
                AddArgument(list, argument);
            }
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
                    AddArguments(args, toolchainIncludes.Select(s => $"-isystem{s}"));
                }

                // toolchain includes
                // This code is same as in toolchain, get compiler arguments... does this need a refactor, or toolchain get passed in? Clang take GCC compatible arguments.
                // perhaps this language service has its own clang tool chain, to generate compiler arguments from project configuration?

                // Referenced includes
                var referencedIncludes = project.GetReferencedIncludes();

                AddArguments(args, referencedIncludes.Select(s => $"-I{s}"));

                // global includes
                var globalIncludes = superProject.GetGlobalIncludes();

                AddArguments(args, globalIncludes.Select(s => $"-I{s}"));

                // includes
                AddArguments(args, project.Includes.Select(s => $"-I{Path.Combine(project.CurrentDirectory, s.Value)}"));

                var referencedDefines = project.GetReferencedDefines();

                AddArguments(args, referencedDefines.Select(s => $"-D{s}"));

                // global includes
                var globalDefines = superProject.GetGlobalDefines();

                AddArguments(args, globalDefines.Select(s => $"-D{s}"));

                AddArguments(args, project.Defines.Select(s => $"-D{s}"));

                switch (file.Extension)
                {
                    case ".c":
                        {
                            AddArguments(args, superProject.CCompilerArguments);
                        }
                        break;

                    case ".cpp":
                        {
                            AddArguments(args, superProject.CppCompilerArguments);
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

        private CPlusPlusDataAssociation GetAssociatedData(ISourceFile sourceFile)
        {
            CPlusPlusDataAssociation result = null;

            dataAssociations.TryGetValue(sourceFile, out result);

            return result;
        }

        private ClangTranslationUnit GetAndParseTranslationUnit(ISourceFile sourceFile, List<ClangUnsavedFile> unsavedFiles)
        {
            var dataAssociation = GetAssociatedData(sourceFile);

            if (dataAssociation != null)
            {
                if (dataAssociation.TranslationUnit == null)
                {
                    dataAssociation.TranslationUnit = GenerateTranslationUnit(sourceFile, unsavedFiles);
                }

                // Always do a reparse, as a workaround for some issues in libclang 3.7.1
                dataAssociation.TranslationUnit.Reparse(unsavedFiles.ToArray(), ReparseTranslationUnitFlags.None);

                return dataAssociation.TranslationUnit;
            }
            return null;
        }

        private void OpenBracket(AvaloniaEdit.TextEditor editor, TextDocument document, string text)
        {
            if (text[0].IsOpenBracketChar() && editor.CaretOffset <= document.TextLength && editor.CaretOffset > 0)
            {
                var nextChar = ' ';

                if (editor.CaretOffset != document.TextLength)
                {
                    nextChar = document.GetCharAt(editor.CaretOffset);
                }

                var location = document.GetLocation(editor.CaretOffset);

                if (char.IsWhiteSpace(nextChar) || nextChar.IsCloseBracketChar())
                {
                    if (text[0] == '{')
                    {
                        var offset = editor.CaretOffset;

                        document.Insert(editor.CaretOffset, " " + text[0].GetCloseBracketChar().ToString() + " ");

                        if (IndentationStrategy != null)
                        {
                            var line = editor.Document.GetLineByNumber(editor.TextArea.Caret.Line);
                            // use indentation strategy only if the line is not read-only
                            IndentationStrategy.IndentLine(editor.Document, line);
                        }


                        editor.CaretOffset = offset + 1;
                    }
                    else
                    {
                        var offset = editor.CaretOffset;

                        document.Insert(editor.CaretOffset, text[0].GetCloseBracketChar().ToString());

                        editor.CaretOffset = offset;
                    }
                }
            }
        }

        private void CloseBracket(AvaloniaEdit.TextEditor editor, TextDocument document, string text)
        {
            if (text[0].IsCloseBracketChar() && editor.CaretOffset < document.TextLength && editor.CaretOffset > 0)
            {
                var offset = editor.CaretOffset;

                while (offset < document.TextLength)
                {
                    var currentChar = document.GetCharAt(offset);

                    if (currentChar == text[0])
                    {
                        document.Replace(offset, 1, string.Empty);
                        break;
                    }
                    else if (!currentChar.IsWhiteSpace())
                    {
                        break;
                    }

                    offset++;
                }
            }
        }

        public static int ApplyReplacements(TextDocument document, int cursor, XDocument replacements, bool replaceCursor = true)
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

                        document.Replace(offsetChange + offset, replacementLength, element.Value);

                        offsetChange += element.Value.Length - replacementLength;
                        break;
                }
            }

            document.EndUpdate();

            return replaceCursor ? cursor : -1;
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

        public void BeforeActivation()
        {
        }

        public void Activation()
        {
        }
    }
}