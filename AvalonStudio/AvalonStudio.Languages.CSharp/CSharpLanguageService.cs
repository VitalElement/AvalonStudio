using Avalonia.Media;
using Avalonia.Threading;
using AvaloniaEdit.Indentation;
using AvaloniaEdit.Indentation.CSharp;
using AvalonStudio.CodeEditor;
using AvalonStudio.Controls;
using AvalonStudio.Documents;
using AvalonStudio.Editor;
using AvalonStudio.Extensibility.Editor;
using AvalonStudio.Extensibility.Languages.CompletionAssistance;
using AvalonStudio.Projects;
using AvalonStudio.Projects.OmniSharp.Roslyn;
using AvalonStudio.Projects.OmniSharp.Roslyn.Diagnostics;
using AvalonStudio.Projects.OmniSharp.Roslyn.Editor;
using AvalonStudio.Utils;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Classification;
using Microsoft.CodeAnalysis.Completion;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.DocumentationComments;
using Microsoft.CodeAnalysis.FindSymbols;
using Microsoft.CodeAnalysis.Formatting;
using Microsoft.CodeAnalysis.LanguageServices;
using Microsoft.CodeAnalysis.Rename;
using Microsoft.CodeAnalysis.Shared.Extensions;
using Microsoft.CodeAnalysis.Text;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.IO;
using System.Linq;
using System.Reactive.Subjects;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AvalonStudio.Languages.CSharp
{
    static class TaggedTextUtil
    {
        public static void AppendTaggedText(this StyledText markup, ColorScheme theme, IEnumerable<TaggedText> text)
        {
            foreach (var part in text)
            {
                if (part.Tag == TextTags.LineBreak)
                {
                    markup.AppendLine();
                    continue;
                }

                markup.Append(part.Text, part.Tag != TextTags.Text ? GetThemeColor(theme, part.Tag) : null);
            }
        }

        public static void AppendTaggedText(this StyledText markup, ColorScheme theme, IEnumerable<TaggedText> text, int col, int maxColumn)
        {
            foreach (var part in text)
            {
                if (part.Tag == TextTags.LineBreak)
                {
                    markup.AppendLine();
                    col = 0;
                    continue;
                }
                if (maxColumn >= 0 && col + part.Text.Length > maxColumn)
                {
                    markup.AppendLine(part.Text, part.Tag != TextTags.Text ? GetThemeColor(theme, part.Tag) : null);
                    //AppendAndBreakText(markup, part.Text, col, maxColumn);
                    col = 0;
                }
                else
                {
                    markup.Append(part.Text);
                    var lineBreak = part.Text.LastIndexOfAny(new[] { '\n', '\r' });
                    if (lineBreak >= 0)
                    {
                        col += part.Text.Length - lineBreak;
                    }
                    else
                    {
                        col += part.Text.Length;
                    }
                }
            }
        }

        static void AppendAndBreakText(StringBuilder markup, string text, int col, int maxColumn)
        {
            var idx = maxColumn - col > 0 && maxColumn - col < text.Length ? text.IndexOf(' ', maxColumn - col) : -1;
            if (idx < 0)
            {
                markup.Append(text);
            }
            else
            {
                markup.Append(text.Substring(0, idx));
                if (idx + 1 >= text.Length)
                    return;
                markup.AppendLine();
                AppendAndBreakText(markup, text.Substring(idx + 1), 0, maxColumn);
            }
        }

        static IBrush GetThemeColor(ColorScheme theme, string tag)
        {
            switch (tag)
            {
                case TextTags.Keyword:
                    return theme.Keyword;
                case TextTags.Class:
                    return theme.Type;
                case TextTags.Delegate:
                    return theme.DelegateName;
                case TextTags.Enum:
                    return theme.EnumType;
                case TextTags.Interface:
                    return theme.InterfaceType;
                case TextTags.Module:
                    return theme.Type;
                case TextTags.Struct:
                    return theme.StructName;
                case TextTags.TypeParameter:
                    return theme.Type;

                case TextTags.Alias:
                case TextTags.Assembly:
                case TextTags.Field:
                case TextTags.ErrorType:
                case TextTags.Event:
                case TextTags.Label:
                case TextTags.Local:
                case TextTags.Method:
                case TextTags.Namespace:
                case TextTags.Parameter:
                case TextTags.Property:
                case TextTags.RangeVariable:
                    return null;

                case TextTags.NumericLiteral:
                    return theme.NumericLiteral;

                case TextTags.StringLiteral:
                    return theme.Literal;

                case TextTags.Space:
                case TextTags.LineBreak:
                    return null;

                case TextTags.Operator:
                    return theme.Operator;

                case TextTags.Punctuation:
                    return theme.Punctuation;

                case TextTags.AnonymousTypeIndicator:
                case TextTags.Text:
                    return null;

                default:
                    Console.WriteLine("Warning unexpected text tag: " + tag);
                    return null;
            }
        }
    }


    [ExportLanguageService(ContentCapabilities.CSharp)]
    internal class CSharpLanguageService : ILanguageService
    {
        private static readonly ConditionalWeakTable<IEditor, CSharpDataAssociation> dataAssociations =
            new ConditionalWeakTable<IEditor, CSharpDataAssociation>();

        private Dictionary<string, Func<string, string>> _snippetCodeGenerators;
        private Dictionary<string, Func<int, int, int, string>> _snippetDynamicVars;
        private MetadataHelper _metadataHelper;

        private static readonly Microsoft.CodeAnalysis.SymbolDisplayFormat DefaultFormat = Microsoft.CodeAnalysis.SymbolDisplayFormat.MinimallyQualifiedFormat.
            WithGlobalNamespaceStyle(Microsoft.CodeAnalysis.SymbolDisplayGlobalNamespaceStyle.Omitted).
            WithMiscellaneousOptions(Microsoft.CodeAnalysis.SymbolDisplayMiscellaneousOptions.None).
            WithMiscellaneousOptions(Microsoft.CodeAnalysis.SymbolDisplayMiscellaneousOptions.EscapeKeywordIdentifiers);

        private static readonly Microsoft.CodeAnalysis.SymbolDisplayFormat FullyQualifiedFormat = Microsoft.CodeAnalysis.SymbolDisplayFormat.FullyQualifiedFormat.
            WithGlobalNamespaceStyle(Microsoft.CodeAnalysis.SymbolDisplayGlobalNamespaceStyle.Omitted).
            WithMiscellaneousOptions(Microsoft.CodeAnalysis.SymbolDisplayMiscellaneousOptions.None).
            WithMiscellaneousOptions(Microsoft.CodeAnalysis.SymbolDisplayMiscellaneousOptions.EscapeKeywordIdentifiers);



        public event EventHandler<DiagnosticsUpdatedEventArgs> DiagnosticsUpdated;

        public CSharpLanguageService()
        {
            _snippetCodeGenerators = new Dictionary<string, Func<string, string>>();
            _snippetDynamicVars = new Dictionary<string, Func<int, int, int, string>>();

            _snippetCodeGenerators.Add("ToFieldName", (propertyName) =>
            {
                if (string.IsNullOrEmpty(propertyName))
                    return propertyName;
                string newName = Char.ToLower(propertyName[0]) + propertyName.Substring(1);
                return "_" + newName;
            });

            _metadataHelper = new MetadataHelper(new AssemblyLoader());
        }

        public IEnumerable<ICodeEditorInputHelper> InputHelpers => null;

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
            '.', '<', ':'
        };

        public IEnumerable<char> IntellisenseSearchCharacters => new[]
        {
            '(', ')', '.', ':', '-', '>', ';', '<'
        };

        public IEnumerable<char> IntellisenseCompleteCharacters => new[]
        {
            '.', ':', ';', '-', ' ', '(', '=', '+', '*', '/', '%', '|', '&', '!', '^'
        };

        public bool IsValidIdentifierCharacter(char data)
        {
            return char.IsLetterOrDigit(data) || data == '_';
        }

        public IIndentationStrategy IndentationStrategy
        {
            get; private set;
        }

        public IDictionary<string, Func<string, string>> SnippetCodeGenerators => _snippetCodeGenerators;

        public IDictionary<string, Func<int, int, int, string>> SnippetDynamicVariables => _snippetDynamicVars;

        public string LanguageId => "cs";

        public IObservable<SyntaxHighlightDataList> AdditionalHighlightingData { get; } = new Subject<SyntaxHighlightDataList>();

        public bool CanHandle(IEditor editor)
        {
            var result = false;

            switch (Path.GetExtension(editor.SourceFile.Location))
            {
                case ".cs":
                    result = true;
                    break;
            }

            return result;
        }

        private CodeCompletionKind FromOmniSharpKind(string kind)
        {
            var roslynKind = (Microsoft.CodeAnalysis.SymbolKind)int.Parse(kind);

            switch (roslynKind)
            {
                case Microsoft.CodeAnalysis.SymbolKind.NamedType:
                    return CodeCompletionKind.ClassPublic;

                case Microsoft.CodeAnalysis.SymbolKind.Parameter:
                    return CodeCompletionKind.Parameter;

                case Microsoft.CodeAnalysis.SymbolKind.Property:
                    return CodeCompletionKind.PropertyPublic;

                case Microsoft.CodeAnalysis.SymbolKind.Method:
                    return CodeCompletionKind.MethodPublic;

                case Microsoft.CodeAnalysis.SymbolKind.Event:
                    return CodeCompletionKind.EventPublic;

                case Microsoft.CodeAnalysis.SymbolKind.Namespace:
                    return CodeCompletionKind.NamespacePublic;

                case Microsoft.CodeAnalysis.SymbolKind.Local:
                    return CodeCompletionKind.Variable;

                case Microsoft.CodeAnalysis.SymbolKind.Field:
                    return CodeCompletionKind.FieldPublic;
            }

            Console.WriteLine($"dont understand omnisharp: {kind}");
            return CodeCompletionKind.None;
        }

        private static CompletionTrigger GetCompletionTrigger(char? triggerChar)
        {
            return triggerChar != null
                ? CompletionTrigger.CreateInsertionTrigger(triggerChar.Value)
                : CompletionTrigger.Invoke;
        }

        public async Task<CodeCompletionResults> CodeCompleteAtAsync(IEditor editor, int index, int line, int column, List<UnsavedFile> unsavedFiles, char previousChar, string filter)
        {
            if (editor.SourceFile is MetaDataFile)
            {
                return null;
            }

            var result = new CodeCompletionResults();

            var dataAssociation = GetAssociatedData(editor);

            var workspace = RoslynWorkspace.GetWorkspace(dataAssociation.Solution);
            var document = workspace.GetDocument(editor.SourceFile);
            var semanticModel = await document.GetSemanticModelAsync();

            var completionService = CompletionService.GetService(document);
            var completionTrigger = GetCompletionTrigger(previousChar);
            var data = await completionService.GetCompletionsAsync(document, index, completionTrigger);

            if (data != null)
            {
                var recommendedSymbols = await Microsoft.CodeAnalysis.Recommendations.Recommender.GetRecommendedSymbolsAtPositionAsync(semanticModel, index, workspace);
                foreach (var completion in data.Items)
                {
                    var insertionText = completion.FilterText;

                    if (completion.Properties.ContainsKey("InsertionText"))
                    {
                        insertionText = completion.Properties["InsertionText"];
                    }

                    var selectionBehavior = Languages.CompletionItemSelectionBehavior.Default;
                    int priority = 0;

                    if (completion.Rules.SelectionBehavior != Microsoft.CodeAnalysis.Completion.CompletionItemSelectionBehavior.Default)
                    {
                        selectionBehavior = (Languages.CompletionItemSelectionBehavior)completion.Rules.SelectionBehavior;
                        priority = completion.Rules.MatchPriority;
                    }

                    if (completion.Properties.ContainsKey("Provider") && completion.Properties["Provider"] == "Microsoft.CodeAnalysis.CSharp.Completion.Providers.SymbolCompletionProvider")
                    {
                        var symbols = recommendedSymbols.Where(x => x.Name == completion.Properties["SymbolName"] && (int)x.Kind == int.Parse(completion.Properties["SymbolKind"])).Distinct();

                        if (symbols != null && symbols.Any())
                        {
                            foreach (var symbol in symbols)
                            {
                                if (symbol != null)
                                {
                                    var newCompletion = new CodeCompletionData(symbol.Name, insertionText, null, selectionBehavior, priority);

                                    if (completion.Properties.ContainsKey("SymbolKind"))
                                    {
                                        newCompletion.Kind = FromOmniSharpKind(completion.Properties["SymbolKind"]);
                                    }

                                    var xmlDocumentation = symbol.GetDocumentationCommentXml();

                                    if (xmlDocumentation != string.Empty)
                                    {
                                        var docComment = DocumentationComment.From(xmlDocumentation, Environment.NewLine);
                                        newCompletion.BriefComment = docComment.SummaryText;
                                    }

                                    result.Completions.Add(newCompletion);
                                }
                            }
                        }
                    }
                    else
                    {
                        var newCompletion = new CodeCompletionData(completion.DisplayText, insertionText, null, selectionBehavior, priority);

                        if (completion.Properties.ContainsKey("SymbolKind"))
                        {
                            newCompletion.Kind = FromOmniSharpKind(completion.Properties["SymbolKind"]);
                        }

                        result.Completions.Add(newCompletion);
                    }
                }

                result.Contexts = Languages.CompletionContext.AnyType;
            }

            return result;
        }

        public int Format(IEditor editor, uint offset, uint length, int cursor)
        {
            if (editor.SourceFile is MetaDataFile)
            {
                return cursor;
            }

            var dataAssociation = GetAssociatedData(editor);

            var document = RoslynWorkspace.GetWorkspace(dataAssociation.Solution).GetDocument(editor.SourceFile);
            var formattedDocument = Formatter.FormatAsync(document).GetAwaiter().GetResult();

            RoslynWorkspace.GetWorkspace(dataAssociation.Solution).TryApplyChanges(formattedDocument.Project.Solution);

            return -1;
        }

        public async Task<GotoDefinitionInfo> GotoDefinition(IEditor editor, int offset)
        {
            var dataAssociation = GetAssociatedData(editor);

            var document = GetDocument(dataAssociation, editor.SourceFile);

            var semanticModel = await document.GetSemanticModelAsync();

            var symbol = await SymbolFinder.FindSymbolAtPositionAsync(semanticModel, editor.CaretOffset, RoslynWorkspace.GetWorkspace(dataAssociation.Solution));

            if (symbol != null && !(symbol is Microsoft.CodeAnalysis.INamespaceSymbol))
            {
                // for partial methods, pick the one with body
                if (symbol is Microsoft.CodeAnalysis.IMethodSymbol method)
                {
                    symbol = method.PartialImplementationPart ?? symbol;
                }

                var location = symbol.Locations.First();

                if (location.IsInSource)
                {
                    var lineSpan = symbol.Locations.First().GetMappedLineSpan();
                    return new GotoDefinitionInfo
                    {
                        FileName = lineSpan.Path,
                        Line = lineSpan.StartLinePosition.Line + 1,
                        Column = lineSpan.StartLinePosition.Character + 1
                    };
                }
                else if (location.IsInMetadata)
                {
                    var timeout = 5000;
                    var cancellationSource = new CancellationTokenSource(TimeSpan.FromMilliseconds(timeout));
                    var (metadataDocument, _) = await _metadataHelper.GetAndAddDocumentFromMetadata(document.Project, symbol, cancellationSource.Token);
                    if (metadataDocument != null)
                    {
                        cancellationSource = new CancellationTokenSource(TimeSpan.FromMilliseconds(timeout));

                        var metadataLocation = await _metadataHelper.GetSymbolLocationFromMetadata(symbol, metadataDocument, cancellationSource.Token);
                        var lineSpan = metadataLocation.GetMappedLineSpan();

                        var metaDataFile = new MetaDataFile(editor.SourceFile.Project, metadataDocument, metadataDocument.Name, _metadataHelper.GetSymbolName(symbol));

                        return new GotoDefinitionInfo
                        {
                            FileName = lineSpan.Path,
                            Line = lineSpan.StartLinePosition.Line + 1,
                            Column = lineSpan.StartLinePosition.Character + 1,
                            MetaDataFile = metaDataFile
                        };
                    }
                }
            }

            return null;
        }

        private Microsoft.CodeAnalysis.Document GetDocument(CSharpDataAssociation dataAssociation, ISourceFile file, RoslynWorkspace workspace = null)
        {
            if (file is MetaDataFile metaDataFile)
            {
                return metaDataFile.Document;
            }
            else
            {
                if (workspace == null)
                {
                    workspace = RoslynWorkspace.GetWorkspace(dataAssociation.Solution);
                }

                return workspace.GetDocument(file);
            }
        }

        static SyntaxNode GetBestFitResolveableNode(SyntaxNode node)
        {
            // case constructor name : new Foo (); 'Foo' only resolves to the type not to the constructor
            if (node.Parent.IsKind(SyntaxKind.ObjectCreationExpression))
            {
                var oce = (ObjectCreationExpressionSyntax)node.Parent;

                if (oce.Type == node)
                    return oce;

            }

            return node;
        }

        public async Task<QuickInfoResult> QuickInfo(IEditor editor, List<UnsavedFile> unsavedFiles, int offset)
        {
            var dataAssociation = GetAssociatedData(editor);

            var workspace = RoslynWorkspace.GetWorkspace(dataAssociation.Solution);

            var document = GetDocument(dataAssociation, editor.SourceFile, workspace);

            var semanticModel = await document.GetSemanticModelAsync();

            var descriptionService = workspace.Services.GetLanguageServices(semanticModel.Language).GetService<ISymbolDisplayService>();

            var root = semanticModel.SyntaxTree.GetRoot(CancellationToken.None);

            SyntaxToken syntaxToken;

            try
            {
                syntaxToken = root.FindToken(offset);
            }
            catch (ArgumentOutOfRangeException)
            {
                return null;
            }

            if (!syntaxToken.Span.IntersectsWith(offset))

                return null;

            var node = GetBestFitResolveableNode(syntaxToken.Parent);

            var symbolInfo = semanticModel.GetSymbolInfo(node, CancellationToken.None);

            var symbol = symbolInfo.Symbol ?? semanticModel.GetDeclaredSymbol(node, CancellationToken.None);

            if (symbol != null)
            {
                var sections = await descriptionService.ToDescriptionGroupsAsync(workspace, semanticModel, offset, new[] { symbol }.AsImmutable(), default(CancellationToken)).ConfigureAwait(false);

                ImmutableArray<TaggedText> parts;

                var styledText = StyledText.Create();
                var theme = ColorScheme.CurrentColorScheme;


                if (sections.TryGetValue(SymbolDescriptionGroups.MainDescription, out parts))
                {
                    TaggedTextUtil.AppendTaggedText(styledText, theme, parts);
                }

                // if generating quick info for an attribute, bind to the class instead of the constructor
                if (symbol.ContainingType?.IsAttribute() == true)
                {
                    symbol = symbol.ContainingType;
                }

                var formatter = workspace.Services.GetLanguageServices(semanticModel.Language).GetService<IDocumentationCommentFormattingService>();
                var documentation = symbol.GetDocumentationParts(semanticModel, offset, formatter, CancellationToken.None);

                if (documentation != null && documentation.Any())
                {
                    styledText.AppendLine();
                    TaggedTextUtil.AppendTaggedText(styledText, theme, documentation);
                }

                if (sections.TryGetValue(SymbolDescriptionGroups.AnonymousTypes, out parts))
                {
                    if (!parts.IsDefaultOrEmpty)
                    {
                        styledText.AppendLine();
                        TaggedTextUtil.AppendTaggedText(styledText, theme, parts);
                    }
                }

                if (sections.TryGetValue(SymbolDescriptionGroups.AwaitableUsageText, out parts))
                {
                    if (!parts.IsDefaultOrEmpty)
                    {
                        styledText.AppendLine();
                        TaggedTextUtil.AppendTaggedText(styledText, theme, parts);
                    }
                }

                if (sections.TryGetValue(SymbolDescriptionGroups.Exceptions, out parts))
                {
                    if (!parts.IsDefaultOrEmpty)
                    {
                        styledText.AppendLine();
                        TaggedTextUtil.AppendTaggedText(styledText, theme, parts);
                    }
                }

                if (sections.TryGetValue(SymbolDescriptionGroups.Captures, out parts))
                {
                    if (!parts.IsDefaultOrEmpty)
                    {
                        styledText.AppendLine();
                        TaggedTextUtil.AppendTaggedText(styledText, theme, parts);
                    }
                }

                return new QuickInfoResult(styledText);
            }

            return null;
        }

        public Task<List<Symbol>> GetSymbolsAsync(IEditor editor, List<UnsavedFile> unsavedFiles, string name)
        {
            return null;
            //throw new NotImplementedException();
        }

        private void OpenBracket(IEditor editor, ITextDocument document, string text)
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
                            editor.IndentLine(editor.Line);
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

        private void CloseBracket(IEditor editor, ITextDocument document, string text)
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

        public void RegisterSourceFile(IEditor editor)
        {
            if (dataAssociations.TryGetValue(editor, out CSharpDataAssociation association))
            {
                throw new Exception("Source file already registered with language service.");
            }

            IndentationStrategy = new CSharpIndentationStrategy(new AvaloniaEdit.TextEditorOptions { ConvertTabsToSpaces = true });

            association = new CSharpDataAssociation
            {
                Solution = editor.SourceFile.Project.Solution
            };

            dataAssociations.Add(editor, association);

            if (!(editor.SourceFile is MetaDataFile))
            {
                var avaloniaEditTextContainer = new AvalonEditTextContainer(editor.Document) { Editor = editor };

                RoslynWorkspace.GetWorkspace(association.Solution).OpenDocument(editor.SourceFile, avaloniaEditTextContainer, (diagnostics) =>
                {
                    var dataAssociation = GetAssociatedData(editor);

                    var results = new List<Diagnostic>();

                    var fadedCode = new SyntaxHighlightDataList();

                    foreach (var diagnostic in diagnostics.Diagnostics)
                    {
                        if (diagnostic.CustomTags.Contains("Unnecessary"))
                        {
                            fadedCode.Add(new OffsetSyntaxHighlightingData
                            {
                                Offset = diagnostic.TextSpan.Start,
                                Length = diagnostic.TextSpan.Length,
                                Type = HighlightType.Unnecessary
                            });
                        }
                        else
                        {
                            results.Add(FromRoslynDiagnostic(diagnostic, editor.SourceFile.Location, editor.SourceFile.Project));
                        }
                    }

                    DiagnosticsUpdated?.Invoke(this, new DiagnosticsUpdatedEventArgs(diagnostics.Id, editor.SourceFile, (DiagnosticsUpdatedKind)diagnostics.Kind, results.ToImmutableArray(), fadedCode));
                });

                association.TextInputHandler = (sender, e) =>
                {
                    switch (e.Text)
                    {
                        case "}":
                        case ";":
                            editor.IndentLine(editor.Line);
                            break;

                        case "{":
                            if (IndentationStrategy != null)
                            {
                                editor.IndentLine(editor.Line);
                            }
                            break;
                    }

                    OpenBracket(editor, editor.Document, e.Text);
                    CloseBracket(editor, editor.Document, e.Text);
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
                                var newline = "\r\n"; // TextUtilities.GetNewLineFromDocument(editor.Document, editor.TextArea.Caret.Line);
                                editor.Document.Insert(editor.CaretOffset, newline);

                                editor.Document.TrimTrailingWhiteSpace(editor.Line - 1);

                                editor.IndentLine(editor.Line);

                                editor.CaretOffset -= newline.Length;
                            }
                            break;
                    }
                };

                editor.TextEntered += association.TextInputHandler;
                editor.TextEntering += association.BeforeTextInputHandler;
            }
        }

        private CSharpDataAssociation GetAssociatedData(IEditor editor)
        {
            if (!dataAssociations.TryGetValue(editor, out CSharpDataAssociation result))
            {
                throw new Exception("Tried to parse file that has not been registered with the language service.");
            }

            return result;
        }

        HighlightType FromRoslynType(string type)
        {
            var result = HighlightType.None;

            switch (type)
            {
                case "preprocessor keyword":
                    return HighlightType.PreProcessor;

                case "preprocessor text":
                    return HighlightType.PreProcessorText;

                case "keyword":
                    result = HighlightType.Keyword;
                    break;

                case "identifier":
                    result = HighlightType.Identifier;
                    break;

                case "punctuation":
                    result = HighlightType.Punctuation;
                    break;

                case "class name":
                    result = HighlightType.ClassName;
                    break;

                case "interface name":
                    result = HighlightType.InterfaceName;
                    break;

                case "enum name":
                    return HighlightType.EnumTypeName;

                case "struct name":
                    result = HighlightType.StructName;
                    break;

                case "number":
                    result = HighlightType.NumericLiteral;
                    break;

                case "string":
                    result = HighlightType.Literal;
                    break;

                case "operator":
                    result = HighlightType.Operator;
                    break;

                case "xml doc comment - text":
                case "xml doc comment - delimiter":
                case "xml doc comment - name":
                case "xml doc comment - attribute name":
                case "xml doc comment - attribute quotes":
                case "comment":
                    result = HighlightType.Comment;
                    break;

                case "delegate name":
                    result = HighlightType.DelegateName;
                    break;

                case "excluded code":
                    result = HighlightType.None;
                    break;

                default:
                    Console.WriteLine($"Dont understand {type}");
                    break;
            }

            return result;
        }

        private Diagnostic FromRoslynDiagnostic(DiagnosticData diagnostic, string fileName, IProject project)
        {
            DiagnosticCategory category = DiagnosticCategory.Compiler;

            if (diagnostic.Category == Microsoft.CodeAnalysis.Diagnostics.DiagnosticCategory.Style)
            {
                category = DiagnosticCategory.Style;
            }
            else if (diagnostic.Category == Microsoft.CodeAnalysis.Diagnostics.DiagnosticCategory.EditAndContinue)
            {
                category = DiagnosticCategory.EditAndContinue;
            }

            var result = new Diagnostic(
                diagnostic.TextSpan.Start,
                diagnostic.TextSpan.Length,
                project,
                fileName,
                diagnostic.DataLocation.MappedStartLine,
                diagnostic.Message,
                (DiagnosticLevel)diagnostic.Severity,
                category);

            return result;
        }

        public async Task<CodeAnalysisResults> RunCodeAnalysisAsync(IEditor editor, List<UnsavedFile> unsavedFiles, Func<bool> interruptRequested)
        {
            var result = new CodeAnalysisResults();

            var textLength = 0;

            await Dispatcher.UIThread.InvokeAsync(() => { textLength = editor.Document.TextLength; });

            var dataAssociation = GetAssociatedData(editor);

            var document = GetDocument(dataAssociation, editor.SourceFile);

            if (document == null)
            {
                return result;
            }

            try
            {
                var highlightData = await Classifier.GetClassifiedSpansAsync(document, new TextSpan(0, textLength));

                foreach (var span in highlightData)
                {
                    var type = FromRoslynType(span.ClassificationType);

                    switch (type)
                    {
                        case HighlightType.DelegateName:
                        case HighlightType.EnumTypeName:
                        case HighlightType.StructName:
                        case HighlightType.InterfaceName:
                        case HighlightType.ClassName:
                            result.SyntaxHighlightingData.Add(new OffsetSyntaxHighlightingData { Offset = span.TextSpan.Start, Length = span.TextSpan.Length, Type = type });
                            break;
                    }
                }
            }
            catch (NullReferenceException)
            {
            }

            result.IndexItems = await IndexBuilder.Compute(document);

            return result;
        }

        public int Comment(IEditor editor, int firstLine, int endLine, int caret = -1, bool format = true)
        {
            var result = caret;
            var textDocument = editor.Document;

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
                    result = Format(editor, (uint)startOffset, (uint)(endOffset - startOffset), caret);
                }
            }
            return result;
        }

        public int UnComment(IEditor editor, int firstLine, int endLine, int caret = -1, bool format = true)
        {
            var result = caret;

            var textDocument = editor.Document;

            using (textDocument.RunUpdate())
            {
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
                    result = Format(editor, (uint)startOffset, (uint)(endOffset - startOffset), caret);
                }
            }

            return result;
        }

        public void UnregisterSourceFile(IEditor editor)
        {
            var association = GetAssociatedData(editor);

            editor.TextEntered -= association.TextInputHandler;
            editor.TextEntering -= association.BeforeTextInputHandler;

            if (!(editor.SourceFile is MetaDataFile))
            {
                RoslynWorkspace.GetWorkspace(association.Solution).CloseDocument(editor.SourceFile);
            }

            association.Solution = null;
            dataAssociations.Remove(editor);
        }

        private async Task<InvocationContext> GetInvocation(Microsoft.CodeAnalysis.Document document, int offset)
        {
            var sourceText = await document.GetTextAsync();
            var position = offset;
            var tree = await document.GetSyntaxTreeAsync();
            var root = await tree.GetRootAsync();
            var node = root.FindToken(position).Parent;

            // Walk up until we find a node that we're interested in.
            while (node != null)
            {
                if (node is InvocationExpressionSyntax invocation && invocation.ArgumentList != null && invocation.ArgumentList.Span.Contains(position))
                {
                    var semanticModel = await document.GetSemanticModelAsync();
                    return new InvocationContext(semanticModel, position, invocation.Expression, invocation.ArgumentList, invocation.IsInStaticContext());
                }

                if (node is ObjectCreationExpressionSyntax objectCreation && objectCreation.ArgumentList != null && objectCreation.ArgumentList.Span.Contains(position))
                {
                    var semanticModel = await document.GetSemanticModelAsync();
                    return new InvocationContext(semanticModel, position, objectCreation, objectCreation.ArgumentList, objectCreation.IsInStaticContext());
                }

                if (node is AttributeSyntax attributeSyntax && attributeSyntax.ArgumentList != null && attributeSyntax.ArgumentList.Span.Contains(position))
                {
                    var semanticModel = await document.GetSemanticModelAsync();
                    return new InvocationContext(semanticModel, position, attributeSyntax, attributeSyntax.ArgumentList, attributeSyntax.IsInStaticContext());
                }

                node = node.Parent;
            }

            return null;
        }

        public async Task<SignatureHelp> SignatureHelp(IEditor editor, List<UnsavedFile> unsavedFiles, int offset, string methodName)
        {
            var dataAssociation = GetAssociatedData(editor);

            var workspace = RoslynWorkspace.GetWorkspace(dataAssociation.Solution);

            var document = workspace.GetDocument(editor.SourceFile);

            var invocation = await GetInvocation(document, offset);

            if (invocation != null)
            {
                return invocation.BuildSignatureHelp();
            }

            return null;
        }

        public async Task<IEnumerable<SymbolRenameInfo>> RenameSymbol(IEditor editor, string renameTo = "")
        {
            if (editor.SourceFile is MetaDataFile)
            {
                return null;
            }

            var dataAssociation = GetAssociatedData(editor);

            var workspace = RoslynWorkspace.GetWorkspace(dataAssociation.Solution);

            var document = GetDocument(dataAssociation, editor.SourceFile, workspace);

            if (document != null)
            {
                var sourceText = await document.GetTextAsync();

                var symbol = await SymbolFinder.FindSymbolAtPositionAsync(document, editor.CaretOffset);
                var solution = workspace.CurrentSolution;

                if (symbol != null)
                {
                    if (renameTo == string.Empty)
                    {
                        renameTo = "Test" + symbol.Name + "1";
                    }

                    try
                    {
                        solution = await Renamer.RenameSymbolAsync(solution, symbol, renameTo, workspace.Options);
                    }
                    catch (ArgumentException e)
                    {
                        return null;
                    }
                }

                var changes = new Dictionary<string, SymbolRenameInfo>();
                var solutionChanges = solution.GetChanges(workspace.CurrentSolution);

                foreach (var projectChange in solutionChanges.GetProjectChanges())
                {
                    foreach (var changedDocumentId in projectChange.GetChangedDocuments())
                    {
                        var changedDocument = solution.GetDocument(changedDocumentId);

                        if (!changes.TryGetValue(changedDocument.FilePath, out var modifiedFileResponse))
                        {
                            modifiedFileResponse = new SymbolRenameInfo(changedDocument.FilePath);
                            changes[changedDocument.FilePath] = modifiedFileResponse;
                        }

                        var originalDocument = workspace.CurrentSolution.GetDocument(changedDocumentId);
                        var linePositionSpanTextChanges = await TextChanges.GetAsync(changedDocument, originalDocument);

                        modifiedFileResponse.Changes = modifiedFileResponse.Changes != null
                            ? modifiedFileResponse.Changes.Union(linePositionSpanTextChanges)
                            : linePositionSpanTextChanges;

                    }
                }

                return changes.Values;
            }

            return null;
        }

        public IEnumerable<IContextActionProvider> GetContextActionProviders(IEditor editor)
        {
            var dataAssociation = GetAssociatedData(editor);

            var workspace = RoslynWorkspace.GetWorkspace(dataAssociation.Solution);

            return new List<IContextActionProvider>
            {
                new RoslynContextActionProvider(workspace)
            };
        }
    }
}