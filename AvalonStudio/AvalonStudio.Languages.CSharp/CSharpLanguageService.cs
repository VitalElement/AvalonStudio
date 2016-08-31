namespace AvalonStudio.Languages.CSharp
{
    using AvalonStudio.Languages;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using AvalonStudio.Extensibility.Languages.CompletionAssistance;
    using AvalonStudio.Projects;
    using TextEditor.Document;
    using TextEditor;
    using TextEditor.Indentation;
    using TextEditor.Rendering;
    using CPlusPlus.Rendering;
    using CPlusPlus;
    using OmniSharp;
    using Avalonia.Input;
    using System.Runtime.CompilerServices;
    using System.IO;
    using Avalonia.Interactivity;
    using Extensibility.Threading;
    using System.Threading;

    public class CSharpLanguageService : ILanguageService
    {
        private static readonly ConditionalWeakTable<ISourceFile, CSharpDataAssociation> dataAssociations =
            new ConditionalWeakTable<ISourceFile, CSharpDataAssociation>();

        private readonly JobRunner intellisenseJobRunner;

        public CSharpLanguageService()
        {
            IndentationStrategy = new CppIndentationStrategy();
            intellisenseJobRunner = new JobRunner();

            Task.Factory.StartNew(() => { intellisenseJobRunner.RunLoop(new CancellationToken()); });
        }

        public Type BaseTemplateType
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public IIndentationStrategy IndentationStrategy
        {
            get;
        }

        public string Title
        {
            get
            {
                return "C# (OmniSharp)";
            }
        }

        public bool CanHandle(ISourceFile file)
        {
            var result = false;

            switch (Path.GetExtension(file.Location))
            {
                case ".cs":
                    result = true;
                    break;
            }            

            return result;
        }

        CursorKind FromOmniSharpKind(object kind)
        {
            if (kind != null)
            {
                //switch (kind)
                //{
                //    case "method":
                //        return CursorKind.CXXMethod;
                //}
            }

            Console.WriteLine($"dont understand omnisharp: {kind}");
            return CursorKind.FirstInvalid;
        }

        public async Task<List<CodeCompletionData>> CodeCompleteAtAsync(ISourceFile sourceFile, int line, int column, List<UnsavedFile> unsavedFiles)
        {
            var result = new List<CodeCompletionData>();
            AutoCompleteOmniSharpRequest request = new AutoCompleteOmniSharpRequest();

            request.FileName = sourceFile.File;

            request.Buffer = unsavedFiles.FirstOrDefault()?.Contents;
            request.Line = line;
            request.Column = column;

            var dataAssociation = GetAssociatedData(sourceFile);

            var response = await dataAssociation.OmniSharpServer.SendRequest(request);

            if (response != null)
            {
                foreach(var completion in response)
                {
                    var newCompletion = new CodeCompletionData()
                    {
                        Suggestion = completion.CompletionText,
                        Priority = 1,
                        Hint = completion.DisplayText,
                        BriefComment = completion.Description,
                        //Kind = FromOmniSharpKind(completion.Kind)
                    };

                    result.Add(newCompletion);
                }
                    
            }

            return result;
        }

        public int Comment(TextDocument textDocument, ISegment segment, int caret = -1, bool format = true)
        {
            throw new NotImplementedException();
        }

        public int Format(TextDocument textDocument, uint offset, uint length, int cursor)
        {
            throw new NotImplementedException();
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
            return null;
            //throw new NotImplementedException();
        }

        public Task<List<Symbol>> GetSymbolsAsync(ISourceFile file, List<UnsavedFile> unsavedFiles, string name)
        {
            return null;
            //throw new NotImplementedException();
        }

        public void RegisterSourceFile(IIntellisenseControl intellisenseControl, ICompletionAdviceControl completionAdviceControl, ICompletionAssistant completionAssistant, TextEditor editor, ISourceFile file, TextDocument textDocument)
        {
            CSharpDataAssociation association = null;

            if (dataAssociations.TryGetValue(file, out association))
            {
                throw new Exception("Source file already registered with language service.");
            }

            association = new CSharpDataAssociation(textDocument);
            association.OmniSharpServer = new OmniSharpServer();
            association.OmniSharpServer.StartAsync(file.Project.Solution.CurrentDirectory).Wait();

            dataAssociations.Add(file, association);

            association.IntellisenseManager = new CSharpIntellisenseManager(this, intellisenseControl, completionAdviceControl, completionAssistant, file, editor);

            association.TunneledKeyUpHandler = async (sender, e) =>
            {
                await intellisenseJobRunner.InvokeAsync(() => { association.IntellisenseManager.OnKeyUp(e).Wait(); });
            };

            association.TunneledKeyDownHandler = async (sender, e) =>
            {
                association.IntellisenseManager.OnKeyDown(e);

                await intellisenseJobRunner.InvokeAsync(() => { association.IntellisenseManager.CompleteOnKeyDown(e).Wait(); });
            };

            association.KeyUpHandler = (sender, e) =>
            {
                if (editor.TextDocument == textDocument)
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
            };

            editor.AddHandler(InputElement.KeyDownEvent, association.TunneledKeyDownHandler, RoutingStrategies.Tunnel);
            editor.AddHandler(InputElement.KeyUpEvent, association.TunneledKeyUpHandler, RoutingStrategies.Tunnel);
            editor.AddHandler(InputElement.KeyUpEvent, association.KeyUpHandler, RoutingStrategies.Tunnel);

            editor.TextInput += association.TextInputHandler;
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
                case "punctuation":
                    return HighlightType.Punctuation;

                case "identifier":
                    return HighlightType.Identifier;

                case "keyword":
                    return HighlightType.Keyword;

                case "class name":
                    return HighlightType.UserType;

                case "operator":
                    return HighlightType.Comment;

                default:
                    Console.WriteLine($"Dont understand omnisharp {omniSharpHighlightType}");
                    return HighlightType.None;
            }
        }            

        public async Task<CodeAnalysisResults> RunCodeAnalysisAsync(ISourceFile file, List<UnsavedFile> unsavedFiles, Func<bool> interruptRequested)
        {
            var result = new CodeAnalysisResults();

            HighlightOmniSharpRequest request = new HighlightOmniSharpRequest();
            request.FileName = file.File;

            request.Buffer = unsavedFiles.FirstOrDefault()?.Contents;

            var dataAssociation = GetAssociatedData(file);

            var response = await dataAssociation.OmniSharpServer.SendRequest(request);             

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

        public int UnComment(TextDocument textDocument, ISegment segment, int caret = -1, bool format = true)
        {
            throw new NotImplementedException();
        }

        public void UnregisterSourceFile(TextEditor editor, ISourceFile file)
        {
            //throw new NotImplementedException();
        }
    }

    internal class CSharpDataAssociation
    {
        public CSharpDataAssociation(TextDocument textDocument)
        {
            BackgroundRenderers = new List<IBackgroundRenderer>();
            DocumentLineTransformers = new List<IDocumentLineTransformer>();

            TextColorizer = new TextColoringTransformer(textDocument);
            TextMarkerService = new TextMarkerService(textDocument);

            BackgroundRenderers.Add(new BracketMatchingBackgroundRenderer());
            BackgroundRenderers.Add(TextMarkerService);

            DocumentLineTransformers.Add(TextColorizer);            
        }

        public OmniSharpServer OmniSharpServer { get; set; }
        public TextColoringTransformer TextColorizer { get; }
        public TextMarkerService TextMarkerService { get; }
        public List<IBackgroundRenderer> BackgroundRenderers { get; }
        public List<IDocumentLineTransformer> DocumentLineTransformers { get; }
        public EventHandler<KeyEventArgs> TunneledKeyUpHandler { get; set; }
        public EventHandler<KeyEventArgs> TunneledKeyDownHandler { get; set; }
        public EventHandler<KeyEventArgs> KeyUpHandler { get; set; }
        public EventHandler<KeyEventArgs> KeyDownHandler { get; set; }
        public EventHandler<TextInputEventArgs> TextInputHandler { get; set; }
        public CSharpIntellisenseManager IntellisenseManager { get; set; }
    }
}
