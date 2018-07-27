using AvaloniaEdit.Indentation;
using AvalonStudio.Controls;
using AvalonStudio.Documents;
using AvalonStudio.Editor;
using AvalonStudio.Extensibility.Languages;
using AvalonStudio.Extensibility.Languages.CompletionAssistance;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;

namespace AvalonStudio.Languages.Xaml
{
    internal class XmlLanguageService : ILanguageService
    {
        private static List<ICodeEditorInputHelper> s_InputHelpers = new List<ICodeEditorInputHelper>
        {
            new CompleteCloseTagCodeEditorHelper(),
            new TerminateElementCodeEditorHelper(),
            new InsertQuotesForPropertyValueCodeEditorHelper(),
            new InsertExtraNewLineBetweenAttributesOnEnterCodeInputHelper()
        };

        public event EventHandler<DiagnosticsUpdatedEventArgs> DiagnosticsUpdated;

        public IIndentationStrategy IndentationStrategy { get; } = new XamlIndentationStrategy();

        public virtual string LanguageId => "xml";

        public IEnumerable<ICodeEditorInputHelper> InputHelpers => s_InputHelpers;

        public IDictionary<string, Func<string, string>> SnippetCodeGenerators => new Dictionary<string, Func<string, string>>();

        public IDictionary<string, Func<int, int, int, string>> SnippetDynamicVariables => new Dictionary<string, Func<int, int, int, string>>();

        public IEnumerable<char> IntellisenseSearchCharacters => new[]
        {
            '(', ')', '.', ':', '-', '<', '>', '[', ']', ';', '"', '#', ','
        };

        public IEnumerable<char> IntellisenseCompleteCharacters => new[]
        {
            ',', '.', ':', ';', '-', ' ', '(', ')', '[', ']', '<', '>', '=', '+', '*', '/', '%', '|', '&', '!', '^'
        };

        public IObservable<SyntaxHighlightDataList> AdditionalHighlightingData => throw new NotImplementedException();

        public ISyntaxHighlightingProvider SyntaxHighlighter => throw new NotImplementedException();

        public virtual bool CanHandle(IEditor editor)
        {
            var result = false;

            switch (Path.GetExtension(editor.SourceFile.Location))
            {
                case ".xml":
                case ".csproj":
                    result = true;
                    break;
            }

            return result;
        }

        public virtual bool CanTriggerIntellisense(char currentChar, char previousChar)
        {
            bool result = false;

            if (currentChar == '<' || currentChar == ' ' || currentChar == '.')
            {
                return true;
            }

            return result;
        }

        public virtual Task<CodeCompletionResults> CodeCompleteAtAsync(IEditor editor, int index, int line, int column, List<UnsavedFile> unsavedFiles, char lastChar, string filter = "")
        {
            return Task.FromResult<CodeCompletionResults>(null);
        }

        public int Comment(IEditor editor, int firstLine, int endLine, int caret = -1, bool format = true)
        {
            return caret;
        }

        public int Format(IEditor editor, uint offset, uint length, int cursor)
        {
            var text = editor.Document.GetText((int)offset, (int)length);

            XmlDocument doc = null;
            try
            {
                doc = new XmlDocument();
                doc.XmlResolver = null; // Prevent DTDs from being downloaded.
                doc.LoadXml(text);
            }
            catch (XmlException ex)
            {
                // handle xml files without root element (https://bugzilla.xamarin.com/show_bug.cgi?id=4748)
                if (ex.Message == "Root element is missing.")
                {

                }

                return cursor;
            }
            catch (Exception ex)
            {
                return cursor;
            }

            var stringBuilder = new StringBuilder();

            var element = XElement.Parse(text);

            var settings = new XmlWriterSettings();
            settings.OmitXmlDeclaration = true;
            settings.Indent = true;
            settings.NamespaceHandling = NamespaceHandling.OmitDuplicates;
            settings.Indent = true;
            settings.IndentChars = "  ";

            using (var xmlWriter = XmlWriter.Create(stringBuilder, settings))
            {
                element.Save(xmlWriter);
            }

            editor.Document.Replace(0, editor.Document.TextLength, stringBuilder.ToString());

            return cursor;
        }

        public Task<QuickInfoResult> QuickInfo(IEditor editor, List<UnsavedFile> unsavedFiles, int offset)
        {
            return Task.FromResult<QuickInfoResult>(null);
        }

        public Task<List<Symbol>> GetSymbolsAsync(IEditor editor, List<UnsavedFile> unsavedFiles, string name)
        {
            return Task.FromResult<List<Symbol>>(null);
        }

        public bool IsValidIdentifierCharacter(char data)
        {
            return char.IsLetterOrDigit(data);
        }

        public virtual void RegisterSourceFile(ITextEditor editornew)
        {
        }

        public virtual void UnregisterSourceFile(IEditor editor)
        {

        }

        public Task<CodeAnalysisResults> RunCodeAnalysisAsync(ITextEditor editor, List<UnsavedFile> unsavedFiles, Func<bool> interruptRequested)
        {
            return Task.FromResult(new CodeAnalysisResults());
        }

        public Task<SignatureHelp> SignatureHelp(IEditor editor, List<UnsavedFile> unsavedFiles, int offset, string methodName)
        {
            return Task.FromResult<SignatureHelp>(null);
        }

        public int UnComment(IEditor editor, int firstLine, int endLine, int caret = -1, bool format = true)
        {
            return caret;
        }

        public Task<GotoDefinitionInfo> GotoDefinition(IEditor editor, int offset)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<SymbolRenameInfo>> RenameSymbol(IEditor editor, string renameTo)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<IContextActionProvider> GetContextActionProviders(IEditor editor)
        {
            return Enumerable.Empty<IContextActionProvider>();
        }
    }
}
