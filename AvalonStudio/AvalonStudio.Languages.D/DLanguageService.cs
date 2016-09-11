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

namespace AvalonStudio.Languages.D
{
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
            return new List<CodeCompletionData>();
        }

        public async Task<CodeAnalysisResults> RunCodeAnalysisAsync(ISourceFile file, List<UnsavedFile> unsavedFiles, Func<bool> interruptRequested)
        {
            var result = new CodeAnalysisResults();

            var associatedData = GetAssociatedData(file);

            await Task.Factory.StartNew(() =>
            {
                var ast = DParser.ParseFile(file.FilePath);

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
            dataAssociations.Add(file, association);
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

            // ???
            /*if (!(file.Project.Solution is DUBProject))
            {
                result = false;
            }*/

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

        public Task<SignatureHelp> SignatureHelp(ISourceFile file, UnsavedFile buffer, List<UnsavedFile> unsavedFiles, int line, int column, int offset,
            string methodName)
        {
            throw new NotImplementedException();
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
            
        }

        
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
