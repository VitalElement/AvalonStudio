using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using AvalonStudio.Extensibility.Languages.CompletionAssistance;
using AvalonStudio.Extensibility.Threading;
using AvalonStudio.Projects;
using AvalonStudio.Projects.DUB;
using AvalonStudio.TextEditor.Document;
using AvalonStudio.TextEditor.Indentation;
using AvalonStudio.TextEditor.Rendering;
using D_Parser.Dom;
using D_Parser.Dom.Expressions;
using D_Parser.Parser;
using Avalonia.Input;
using AvalonStudio.Languages.Highlighting;
using AvalonStudio.Platforms;
using D_Parser.Completion;
using D_Parser.Resolver;
using D_Parser.Misc;
using Avalonia.Interactivity;

namespace AvalonStudio.Languages.D
{

    class DCompletionDataGenerator : ICompletionDataGenerator
    {
        public void Add(INode Node)
        {
            Console.WriteLine(Node);
        }

        public void Add(byte Token)
        {
            Console.WriteLine(Token);
        }

        public void AddCodeGeneratingNodeItem(INode node, string codeToGenerate)
        {
            Console.WriteLine(codeToGenerate);
        }

        public void AddIconItem(string iconName, string text, string description)
        {
            Console.WriteLine($"{iconName}, {text}");
        }

        public void AddModule(DModule module, string nameOverride = null)
        {
            Console.WriteLine(module);
        }

        public void AddPackage(string packageName)
        {
            Console.WriteLine(packageName);
        }

        public void AddPropertyAttribute(string AttributeText)
        {
            Console.WriteLine(AttributeText);
        }

        public void AddTextItem(string Text, string Description)
        {
            Console.WriteLine(Text);
        }

        public void NotifyTimeout()
        {
            //throw new NotImplementedException();
        }

        public void SetSuggestedItem(string item)
        {
            Console.WriteLine(item);
        }
    }

    public class DLanguageService : ILanguageService
    {
        private readonly JobRunner intellisenseJobRunner;

        private static readonly ConditionalWeakTable<ISourceFile, DDataAssociation> dataAssociations =
            new ConditionalWeakTable<ISourceFile, DDataAssociation>();

        public DLanguageService()
        {
            IndentationStrategy = new DefaultIndentationStrategy();
            intellisenseJobRunner = new JobRunner();

            Task.Factory.StartNew(() => { intellisenseJobRunner.RunLoop(new CancellationToken()); });
        }

        public IIndentationStrategy IndentationStrategy
        {
            get;
        }

        public IEnumerable<char> IntellisenseTriggerCharacters { get { return new[] { '.', '>', ':' }; } }

        public IEnumerable<char> IntellisenseSearchCharacters { get { return new[] { '(', ')', '.', ':', '-', '>', ';' }; } }

        public IEnumerable<char> IntellisenseCompleteCharacters { get { return new[] { '.', ':', ';', '-', ' ', '(', '=', '+', '*', '/', '%', '|', '&', '!', '^' }; } }

        public string Title
        {
            get
            {
                return "D";
            }
        }

        public Type BaseTemplateType
        {
            get;
        }

        public async Task<List<CodeCompletionData>> CodeCompleteAtAsync(ISourceFile sourceFile, int line, int column, List<UnsavedFile> unsavedFiles, string filter = "")
        {
            var codeCompletionGenerator = new DCompletionDataGenerator();

            var associatedData = GetAssociatedData(sourceFile);
            associatedData.EditorContext.CaretLocation = new CodeLocation(column, line);
            associatedData.EditorContext.SyntaxTree = GetAndParseModule(sourceFile, unsavedFiles);

            await Task.Factory.StartNew(() =>
            {
                CodeCompletion.GenerateCompletionData(associatedData.EditorContext, codeCompletionGenerator, 'p');
            });

            return new List<CodeCompletionData>();
        }

        private DModule GetAndParseModule(ISourceFile file, List<UnsavedFile> unsavedFiles)
        {
            DModule ast = null;

            if (unsavedFiles.Count > 0 && unsavedFiles.First().FileName.CompareFilePath(file.FilePath) == 0)
            {
                ast = DParser.ParseString(unsavedFiles.First().Contents);
            }
            else
            {
                ast = DParser.ParseFile(file.FilePath);
            }

            return ast;
        }

        public async Task<CodeAnalysisResults> RunCodeAnalysisAsync(ISourceFile file, List<UnsavedFile> unsavedFiles, Func<bool> interruptRequested)
        {
            var result = new CodeAnalysisResults();

            var associatedData = GetAssociatedData(file);

            await Task.Factory.StartNew(() =>
            {
                var ast = GetAndParseModule(file, unsavedFiles);

                var highlightingVisitor = new HighlightVisitor();

                ast.Accept(highlightingVisitor);

                result.SyntaxHighlightingData = highlightingVisitor.Highlights;
            });

            associatedData.TextColorizer.SetTransformations(result.SyntaxHighlightingData);

            return result;
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

        public void RegisterSourceFile(IIntellisenseControl intellisenseControl, ICompletionAssistant completionAssistant,
            TextEditor.TextEditor editor, ISourceFile file, TextDocument doc)
        {
            DDataAssociation association = null;

            if (dataAssociations.TryGetValue(file, out association))
            {
                throw new Exception("Source file already registered with language service.");
            }

            association = new DDataAssociation(doc);

            association.EditorContext.ParseCache = (file.Project as DUBProject).ParseCache;

            dataAssociations.Add(file, association);

            association.IntellisenseManager = new DIntellisenseManager(this, intellisenseControl, completionAssistant, file, editor);

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
            };

            editor.AddHandler(InputElement.KeyDownEvent, association.TunneledKeyDownHandler, RoutingStrategies.Tunnel);
            editor.AddHandler(InputElement.KeyUpEvent, association.TunneledKeyUpHandler, RoutingStrategies.Tunnel);
            editor.AddHandler(InputElement.KeyUpEvent, association.KeyUpHandler, RoutingStrategies.Tunnel);

            editor.TextInput += association.TextInputHandler;
        }

        public void UnregisterSourceFile(TextEditor.TextEditor editor, ISourceFile file)
        {
            throw new NotImplementedException();
        }

        public bool CanHandle(ISourceFile file)
        {
            bool result = false;

            switch (Path.GetExtension(file.Location))
            {
                case ".d":
                    result = true;
                    break;
            }

            if (!(file.Project is DUBProject))
            {
                result = false;
            }

            return result;
        }

        public int Format(TextDocument textDocument, uint offset, uint length, int cursor)
        {
            throw new NotImplementedException();
        }

        public int Comment(TextDocument textDocument, ISegment segment, int caret = -1, bool format = true)
        {
            throw new NotImplementedException();
        }

        public int UnComment(TextDocument textDocument, ISegment segment, int caret = -1, bool format = true)
        {
            throw new NotImplementedException();
        }

        public async Task<SignatureHelp> SignatureHelp(ISourceFile file, UnsavedFile buffer, List<UnsavedFile> unsavedFiles, int line, int column, int offset,
            string methodName)
        {
            return null;
        }

        public async Task<Symbol> GetSymbolAsync(ISourceFile file, List<UnsavedFile> unsavedFiles, int offset)
        {
            return null;
        }

        public Task<List<Symbol>> GetSymbolsAsync(ISourceFile file, List<UnsavedFile> unsavedFiles, string name)
        {
            throw new NotImplementedException();
        }

        private DDataAssociation GetAssociatedData(ISourceFile sourceFile)
        {
            DDataAssociation result = null;

            if (!dataAssociations.TryGetValue(sourceFile, out result))
            {
                throw new Exception("Tried to parse file that has not been registered with the language service.");
            }

            return result;
        }
    }

    internal class DDataAssociation
    {
        public DDataAssociation(TextDocument textDocument)
        {
            BackgroundRenderers = new List<IBackgroundRenderer>();
            DocumentLineTransformers = new List<IDocumentLineTransformer>();

            TextColorizer = new TextColoringTransformer(textDocument);
            TextMarkerService = new TextMarkerService(textDocument);


            BackgroundRenderers.Add(TextMarkerService);

            DocumentLineTransformers.Add(TextColorizer);

            EditorContext = new EditorData();

        }

        public DIntellisenseManager IntellisenseManager { get; set; }
        public EditorData EditorContext { get; }
        public TextColoringTransformer TextColorizer { get; }
        public TextMarkerService TextMarkerService { get; }
        public List<IBackgroundRenderer> BackgroundRenderers { get; }
        public List<IDocumentLineTransformer> DocumentLineTransformers { get; }
        public EventHandler<KeyEventArgs> TunneledKeyUpHandler { get; set; }
        public EventHandler<KeyEventArgs> TunneledKeyDownHandler { get; set; }
        public EventHandler<KeyEventArgs> KeyUpHandler { get; set; }
        public EventHandler<KeyEventArgs> KeyDownHandler { get; set; }
        public EventHandler<TextInputEventArgs> TextInputHandler { get; set; }
    }
}
