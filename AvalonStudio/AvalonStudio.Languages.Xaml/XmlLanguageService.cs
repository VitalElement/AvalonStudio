using AvalonStudio.Documents;
using AvalonStudio.Editor;
using AvalonStudio.Extensibility.Languages.CompletionAssistance;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;

namespace AvalonStudio.Languages.Xaml
{
    internal class XmlLanguageService : ILanguageService
    {
        private static readonly List<ITextEditorInputHelper> s_InputHelpers = new List<ITextEditorInputHelper>
        {
            new XmlIndentationTextInputHelper(),
            new CompleteCloseTagCodeEditorHelper(),
            new TerminateElementCodeEditorHelper(),
            /*new InsertQuotesForPropertyValueCodeEditorHelper(),
            new InsertExtraNewLineBetweenAttributesOnEnterCodeInputHelper()*/
        };

        protected ITextEditor _editor;

        public virtual string LanguageId => "xml";

        public IEnumerable<ITextEditorInputHelper> InputHelpers => s_InputHelpers;

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

        public virtual bool CanTriggerIntellisense(char currentChar, char previousChar)
        {
            bool result = false;

            if (currentChar == '<' || currentChar == ' ' || currentChar == '.')
            {
                return true;
            }

            return result;
        }

        public virtual Task<CodeCompletionResults> CodeCompleteAtAsync(int index, int line, int column, IEnumerable<UnsavedFile> unsavedFiles, char lastChar, string filter = "")
        {
            return Task.FromResult<CodeCompletionResults>(null);
        }

        public int Comment(int firstLine, int endLine, int caret = -1, bool format = true)
        {
            return caret;
        }

        public int Format(uint offset, uint length, int cursor)
        {
            var text = _editor.Document.GetText((int)offset, (int)length);

            XmlDocument doc = null;
            try
            {
                doc = new XmlDocument
                {
                    XmlResolver = null // Prevent DTDs from being downloaded.
                };

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
            catch (Exception)
            {
                return cursor;
            }

            var stringBuilder = new StringBuilder();

            var element = XElement.Parse(text);

            var settings = new XmlWriterSettings
            {
                OmitXmlDeclaration = true,
                Indent = true,
                NamespaceHandling = NamespaceHandling.OmitDuplicates,
                IndentChars = "  "
            };

            using (var xmlWriter = XmlWriter.Create(stringBuilder, settings))
            {
                element.Save(xmlWriter);
            }

            _editor.Document.Replace(0, _editor.Document.TextLength, stringBuilder.ToString());

            return cursor;
        }

        public Task<QuickInfoResult> QuickInfo(IEnumerable<UnsavedFile> unsavedFiles, int offset)
        {
            return Task.FromResult<QuickInfoResult>(null);
        }

        public Task<List<Symbol>> GetSymbolsAsync(IEnumerable<UnsavedFile> unsavedFiles, string name)
        {
            return Task.FromResult<List<Symbol>>(null);
        }

        public bool IsValidIdentifierCharacter(char data)
        {
            return char.IsLetterOrDigit(data);
        }

        public virtual void RegisterEditor(ITextEditor editor)
        {
            _editor = editor;
        }

        public virtual void UnregisterEditor()
        {

        }

        public Task<CodeAnalysisResults> RunCodeAnalysisAsync(IEnumerable<UnsavedFile> unsavedFiles, Func<bool> interruptRequested)
        {
            return Task.FromResult(new CodeAnalysisResults());
        }

        public Task<SignatureHelp> SignatureHelp(IEnumerable<UnsavedFile> unsavedFiles, int offset, string methodName)
        {
            return Task.FromResult<SignatureHelp>(null);
        }

        public int UnComment(int firstLine, int endLine, int caret = -1, bool format = true)
        {
            return caret;
        }

        public Task<GotoDefinitionInfo> GotoDefinition(int offset)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<SymbolRenameInfo>> RenameSymbol(string renameTo)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<IContextActionProvider> GetContextActionProviders()
        {
            return Enumerable.Empty<IContextActionProvider>();
        }
    }
}
