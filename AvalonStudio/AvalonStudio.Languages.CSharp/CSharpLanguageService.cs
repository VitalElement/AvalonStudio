namespace AvalonStudio.Languages.CSharp
{
    using Avalonia.Media;
    using Avalonia.Threading;
    using AvaloniaEdit.Document;
    using AvaloniaEdit.Indentation;
    using AvaloniaEdit.Indentation.CSharp;
    using AvaloniaEdit.Rendering;
    using AvalonStudio.Extensibility.Editor;
    using AvalonStudio.Extensibility.Languages.CompletionAssistance;
    using AvalonStudio.Languages;
    using AvalonStudio.Projects;
    using AvalonStudio.Utils;
    using Microsoft.CodeAnalysis.Classification;
    using Microsoft.CodeAnalysis.Completion;
    using Microsoft.CodeAnalysis.Formatting;
    using Projects.OmniSharp;
    using RoslynPad.Editor.Windows;
    using RoslynPad.Roslyn.Diagnostics;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Runtime.CompilerServices;
    using System.Threading.Tasks;

    public class CSharpLanguageService : ILanguageService
    {
        private static readonly ConditionalWeakTable<ISourceFile, CSharpDataAssociation> dataAssociations =
            new ConditionalWeakTable<ISourceFile, CSharpDataAssociation>();

        private Dictionary<string, Func<string, string>> _snippetCodeGenerators;
        private Dictionary<string, Func<int, int, int, string>> _snippetDynamicVars;

        public CSharpLanguageService()
        {
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
        }

        public Type BaseTemplateType
        {
            get
            {
                return typeof(BlankOmniSharpProjectTemplate);
            }
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

        public IEnumerable<char> IntellisenseTriggerCharacters => new[]
        {
            '.', '>', ':'
        };

        public IEnumerable<char> IntellisenseSearchCharacters => new[]
        {
            '(', ')', '.', ':', '-', '>', ';'
        };

        public IEnumerable<char> IntellisenseCompleteCharacters => new[]
        {
            '.', ':', ';', '-', ' ', '(', '=', '+', '*', '/', '%', '|', '&', '!', '^'
        };

        public IIndentationStrategy IndentationStrategy
        {
            get; private set;
        }

        public string Title
        {
            get
            {
                return "C# (OmniSharp)";
            }
        }

        public IDictionary<string, Func<string, string>> SnippetCodeGenerators => _snippetCodeGenerators;

        public IDictionary<string, Func<int, int, int, string>> SnippetDynamicVariables => _snippetDynamicVars;

        public string LanguageId => "cs";

        public bool CanHandle(ISourceFile file)
        {
            var result = false;

            switch (Path.GetExtension(file.Location))
            {
                case ".cs":
                    result = true;
                    break;
            }

            if (!(file.Project.Solution is OmniSharpSolution))
            {
                result = false;
            }

            return result;
        }

        private CodeCompletionKind FromOmniSharpKind(string kind)
        {
            var roslynKind = (Microsoft.CodeAnalysis.SymbolKind)int.Parse(kind);

            switch (roslynKind)
            {
                case Microsoft.CodeAnalysis.SymbolKind.NamedType:
                    return CodeCompletionKind.Class;

                case Microsoft.CodeAnalysis.SymbolKind.Parameter:
                    return CodeCompletionKind.Parameter;

                case Microsoft.CodeAnalysis.SymbolKind.Property:
                    return CodeCompletionKind.Property;

                case Microsoft.CodeAnalysis.SymbolKind.Method:
                    return CodeCompletionKind.Method;

                case Microsoft.CodeAnalysis.SymbolKind.Event:
                    return CodeCompletionKind.Event;

                case Microsoft.CodeAnalysis.SymbolKind.Namespace:
                    return CodeCompletionKind.Namespace;

                case Microsoft.CodeAnalysis.SymbolKind.Local:
                    return CodeCompletionKind.Variable;

                case Microsoft.CodeAnalysis.SymbolKind.Field:
                    return CodeCompletionKind.Field;
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

        public async Task<CodeCompletionResults> CodeCompleteAtAsync(ISourceFile sourceFile, int index, int line, int column, List<UnsavedFile> unsavedFiles, char previousChar, string filter)
        {
            var result = new CodeCompletionResults();

            var dataAssociation = GetAssociatedData(sourceFile);

            var document = dataAssociation.Solution.Workspace.GetDocument(sourceFile);

            var completionService = CompletionService.GetService(document);
            var completionTrigger = GetCompletionTrigger(null);
            var data = await completionService.GetCompletionsAsync(
                document,
                index,
                completionTrigger
                );

            if (data != null)
            {
                foreach (var completion in data.Items)
                {
                    var newCompletion = new CodeCompletionData()
                    {
                        Suggestion = completion.FilterText,
                        Hint = completion.DisplayText
                    };

                    if (completion.Properties.ContainsKey("SymbolKind"))
                    {
                        newCompletion.Kind = FromOmniSharpKind(completion.Properties["SymbolKind"]);
                    }

                    result.Completions.Add(newCompletion);
                }

                result.Contexts = Languages.CompletionContext.AnyType;
            }


            /*var response = await dataAssociation.Solution.Server.AutoComplete(sourceFile.FilePath, unsavedFiles.FirstOrDefault()?.Contents, line, column);

            if (response != null)
            {
                foreach (var completion in response)
                {
                    var newCompletion = new CodeCompletionData()
                    {
                        Suggestion = completion.CompletionText,
                        Priority = 1,
                        Hint = completion.DisplayText,
                        BriefComment = completion.Description?.Split(new[] { '\n' }, StringSplitOptions.RemoveEmptyEntries).FirstOrDefault(),
                        Kind = FromOmniSharpKind(completion.Kind)
                    };

                    if (filter == string.Empty || completion.CompletionText.StartsWith(filter))
                    {
                        result.Completions.Add(newCompletion);
                    }
                }

                result.Contexts = CompletionContext.AnyType;
            }*/

            return result;
        }

        public int Format(ISourceFile file, TextDocument textDocument, uint offset, uint length, int cursor)
        {
            var dataAssociation = GetAssociatedData(file);

            var document = dataAssociation.Solution.Workspace.GetDocument(file);
            var formattedDocument = Formatter.FormatAsync(document).GetAwaiter().GetResult();

            dataAssociation.Solution.Workspace.TryApplyChanges(formattedDocument.Project.Solution);

            return -1;
        }

        public IList<IBackgroundRenderer> GetBackgroundRenderers(ISourceFile file)
        {
            var associatedData = GetAssociatedData(file);

            return associatedData.BackgroundRenderers;
        }

        public IList<IVisualLineTransformer> GetDocumentLineTransformers(ISourceFile file)
        {
            var associatedData = GetAssociatedData(file);

            return associatedData.DocumentLineTransformers;
        }

        public async Task<Symbol> GetSymbolAsync(ISourceFile file, List<UnsavedFile> unsavedFiles, int offset)
        {
            return null;
            //throw new NotImplementedException();
        }

        public Task<List<Symbol>> GetSymbolsAsync(ISourceFile file, List<UnsavedFile> unsavedFiles, string name)
        {
            return null;
            //throw new NotImplementedException();
        }

        private void OpenBracket(AvaloniaEdit.TextEditor editor, TextDocument document, string text)
        {
            if (text[0].IsOpenBracketChar() && editor.CaretOffset <= document.TextLength && editor.CaretOffset > 0)
            {
                var nextChar = ' ';

                if (editor.CaretOffset != document.TextLength)
                {
                    document.GetCharAt(editor.CaretOffset);
                }

                if (char.IsWhiteSpace(nextChar) || nextChar.IsCloseBracketChar())
                {
                    document.Insert(editor.CaretOffset, text[0].GetCloseBracketChar().ToString());
                }

                editor.CaretOffset--;
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

        public void RegisterSourceFile(AvaloniaEdit.TextEditor editor, ISourceFile file, TextDocument doc)
        {
            CSharpDataAssociation association = null;

            if (dataAssociations.TryGetValue(file, out association))
            {
                throw new Exception("Source file already registered with language service.");
            }

            IndentationStrategy = new CSharpIndentationStrategy(editor.Options);

            association = new CSharpDataAssociation(doc);
            association.Solution = file.Project.Solution as OmniSharpSolution; // CanHandle has checked this.

            var avaloniaEditTextContainer = new AvalonEditTextContainer(editor.Document) { Editor = editor };
            association.Solution.Workspace.OpenDocument(file, avaloniaEditTextContainer, (diagnostics) =>
            {
                var dataAssociation = GetAssociatedData(file);

                var results = new TextSegmentCollection<Diagnostic>();

                foreach (var diagnostic in diagnostics.Diagnostics)
                {
                    results.Add(FromRoslynDiagnostic(diagnostic, file.Location, file.Project));
                }

                dataAssociation.Diagnostics.OnNext(results);
            });

            dataAssociations.Add(file, association);

            association.TextInputHandler = (sender, e) =>
            {
                if (editor.Document == doc)
                {
                    editor.BeginChange();
                    OpenBracket(editor, editor.Document, e.Text);
                    CloseBracket(editor, editor.Document, e.Text);

                    switch (e.Text)
                    {
                        case "}":
                        case ";":
                            Format(file, editor.Document, 0, (uint)editor.Document.TextLength, editor.CaretOffset);
                            break;

                        case "{":
                            var lineCount = editor.Document.LineCount;
                            Format(file, editor.Document, 0, (uint)editor.Document.TextLength, editor.CaretOffset);
                            break;
                    }

                    editor.EndChange();
                }
            };

            editor.TextArea.TextEntered += association.TextInputHandler;
        }

        private CSharpDataAssociation GetAssociatedData(ISourceFile sourceFile)
        {
            CSharpDataAssociation result = null;

            if (!dataAssociations.TryGetValue(sourceFile, out result))
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
            var result = new Diagnostic
            {
                Spelling = diagnostic.Message,
                Level = (DiagnosticLevel)diagnostic.Severity,
                StartOffset = diagnostic.TextSpan.Start,
                Length = diagnostic.TextSpan.Length,
                File = fileName,
                Project = project,
            };

            return result;
        }

        public IObservable<TextSegmentCollection<Diagnostic>> ObserveDiagnostics(ISourceFile file)
        {
            return GetAssociatedData(file).Diagnostics;
        }

        public async Task<CodeAnalysisResults> RunCodeAnalysisAsync(ISourceFile file, TextDocument textDocument, List<UnsavedFile> unsavedFiles, Func<bool> interruptRequested)
        {
            var result = new CodeAnalysisResults();

            var textLength = 0;

            await Dispatcher.UIThread.InvokeTaskAsync(() => { textLength = textDocument.TextLength; });

            var dataAssociation = GetAssociatedData(file);

            var document = dataAssociation.Solution.Workspace.GetDocument(file);

            if (document == null)
            {
                return result;
            }

            var highlightData = await Classifier.GetClassifiedSpansAsync(document, new Microsoft.CodeAnalysis.Text.TextSpan(0, textLength));

            foreach (var span in highlightData)
            {
                result.SyntaxHighlightingData.Add(new OffsetSyntaxHighlightingData { Start = span.TextSpan.Start, Length = span.TextSpan.Length, Type = FromRoslynType(span.ClassificationType) });
            }

            dataAssociation.TextColorizer.SetTransformations(result.SyntaxHighlightingData);

            return result;
        }

        public int Comment(TextDocument textDocument, int firstLine, int endLine, int caret = -1, bool format = true)
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
                // result = Format(textDocument, (uint)startOffset, (uint)(endOffset - startOffset), caret);
            }

            textDocument.EndUpdate();
            return result;
        }

        public int UnComment(TextDocument textDocument, int firstLine, int endLine, int caret = -1, bool format = true)
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
                // result = Format(textDocument, (uint)startOffset, (uint)(endOffset - startOffset), caret);
            }

            textDocument.EndUpdate();

            return result;
        }

        public void UnregisterSourceFile(AvaloniaEdit.TextEditor editor, ISourceFile file)
        {
            var association = GetAssociatedData(file);

            editor.TextInput -= association.TextInputHandler;

            association.Solution.Workspace.CloseDocument(file);

            association.Solution = null;
            dataAssociations.Remove(file);
        }

        public async Task<SignatureHelp> SignatureHelp(ISourceFile file, UnsavedFile buffer, List<UnsavedFile> unsavedFiles, int line, int column, int offset, string methodName)
        {
            SignatureHelp result = null;

            /*var dataAssociation = GetAssociatedData(file);

            result = await dataAssociation.Solution.Server.SignatureHelp(file.FilePath, unsavedFiles.FirstOrDefault()?.Contents, line, column);

            if (result != null)
            {
                result.NormalizeSignatureData();

                result.Offset = offset;
            }*/

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