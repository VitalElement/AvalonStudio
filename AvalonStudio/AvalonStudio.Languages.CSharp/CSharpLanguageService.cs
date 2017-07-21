namespace AvalonStudio.Languages.CSharp
{
    using Avalonia.Input;
    using Avalonia.Interactivity;
    using AvaloniaEdit.Document;
    using AvaloniaEdit.Indentation;
    using AvaloniaEdit.Indentation.CSharp;
    using AvaloniaEdit.Rendering;
    using AvalonStudio.Extensibility.Languages.CompletionAssistance;
    using AvalonStudio.Languages;
    using AvalonStudio.Projects;
    using AvalonStudio.Utils;
    using Projects.OmniSharp;
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

        public CSharpLanguageService()
        {
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
            return IntellisenseSearchCharacters.Contains(currentChar);
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

        public IDictionary<string, Func<string, string>> SnippetCodeGenerators => null;

        public IDictionary<string, Func<int, int, int, string>> SnippetDynamicVariables => null;

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
            if (kind != null)
            {
                switch (kind)
                {
                    case "Method":
                        return CodeCompletionKind.Method;

                    case "Class":
                        return CodeCompletionKind.Class;

                    case "Struct":
                        return CodeCompletionKind.Struct;

                    case "Enum":
                        return CodeCompletionKind.Enum;

                    case "Delegate":
                        return CodeCompletionKind.Delegate;

                    case "Property":
                        return CodeCompletionKind.Property;

                    case "Event":
                        return CodeCompletionKind.Event;

                    case "Interface":
                        return CodeCompletionKind.Interface;

                    case "Keyword":
                        return CodeCompletionKind.Keyword;

                    case "Namespace":
                        return CodeCompletionKind.Namespace;

                    case "Field":
                        return CodeCompletionKind.Field;

                    case "Parameter":
                        return CodeCompletionKind.Parameter;

                    case "Local":
                        return CodeCompletionKind.Variable;
                }
            }

            Console.WriteLine($"dont understand omnisharp: {kind}");
            return CodeCompletionKind.None;
        }

        public async Task<CodeCompletionResults> CodeCompleteAtAsync(ISourceFile sourceFile, int index, int line, int column, List<UnsavedFile> unsavedFiles, string filter)
        {
            var result = new CodeCompletionResults();

            var dataAssociation = GetAssociatedData(sourceFile);

            var response = await dataAssociation.Solution.Server.AutoComplete(sourceFile.FilePath, unsavedFiles.FirstOrDefault()?.Contents, line, column);

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
                        result.Contexts = CompletionContext.Unknown;
                    }
                }
            }

            return result;
        }

        public int Format(TextDocument textDocument, uint offset, uint length, int cursor)
        {
            return cursor;
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
                            editor.CaretOffset = Format(editor.Document, 0, (uint)editor.Document.TextLength, editor.CaretOffset);
                            break;

                        case "{":
                            var lineCount = editor.Document.LineCount;
                            var offset = Format(editor.Document, 0, (uint)editor.Document.TextLength, editor.CaretOffset);

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

        public HighlightType ToAvalonHighlightType(string omniSharpHighlightType)
        {
            switch (omniSharpHighlightType)
            {
                case "operator":
                case "punctuation":
                    return HighlightType.Punctuation;

                case "identifier":
                    return HighlightType.Identifier;

                case "keyword":
                    return HighlightType.Keyword;

                case "class name":
                    return HighlightType.ClassName;

                case "struct name":
                    return HighlightType.StructName;

                case "comment":
                    return HighlightType.Comment;

                case "delegate name":
                case "interface name":
                case "enum name":
                    return HighlightType.Identifier;

                case "string":
                case "number":
                    return HighlightType.Literal;

                case "preprocessor keyword":
                    return HighlightType.PreProcessor;

                case "preprocessor text":
                    return HighlightType.PreProcessorText;

                default:
                    Console.WriteLine($"Dont understand omnisharp {omniSharpHighlightType}");
                    return HighlightType.None;
            }
        }

        public async Task<CodeAnalysisResults> RunCodeAnalysisAsync(ISourceFile file, List<UnsavedFile> unsavedFiles, Func<bool> interruptRequested)
        {
            var result = new CodeAnalysisResults();

            var dataAssociation = GetAssociatedData(file);

            var response = await dataAssociation.Solution.Server.Highlight(file.FilePath, unsavedFiles.FirstOrDefault()?.Contents);

            if (response != null)
            {
                foreach (var highlight in response.Highlights)
                {
                    result.SyntaxHighlightingData.Add(new LineColumnSyntaxHighlightingData
                    {
                        StartLine = highlight.StartLine,
                        EndLine = highlight.EndLine,
                        StartColumn = highlight.StartColumn,
                        EndColumn = highlight.EndColumn,
                        Type = ToAvalonHighlightType(highlight.Kind)
                    });
                }

                dataAssociation.TextColorizer.SetTransformations(result.SyntaxHighlightingData);
            }

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
                result = Format(textDocument, (uint)startOffset, (uint)(endOffset - startOffset), caret);
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
                result = Format(textDocument, (uint)startOffset, (uint)(endOffset - startOffset), caret);
            }

            textDocument.EndUpdate();

            return result;
        }

        public void UnregisterSourceFile(AvaloniaEdit.TextEditor editor, ISourceFile file)
        {
            var association = GetAssociatedData(file);

            editor.TextInput -= association.TextInputHandler;

            association.Solution = null;
            dataAssociations.Remove(file);
        }

        public async Task<SignatureHelp> SignatureHelp(ISourceFile file, UnsavedFile buffer, List<UnsavedFile> unsavedFiles, int line, int column, int offset, string methodName)
        {
            SignatureHelp result = null;

            var dataAssociation = GetAssociatedData(file);

            result = await dataAssociation.Solution.Server.SignatureHelp(file.FilePath, unsavedFiles.FirstOrDefault()?.Contents, line, column);

            if (result != null)
            {
                result.NormalizeSignatureData();

                result.Offset = offset;
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