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

        public IEnumerable<char> IntellisenseSearchCharacters { get; } = new[]
        {
            '(', ')', '.', ':', '-', '<', '>', '[', ']', ';', '"', '#', ',',' ','/'
        };

        public IEnumerable<char> IntellisenseCompleteCharacters { get; } = new[]
        {
            ',', '.', ';', '-', ' ', '(', ')', '[', ']', '<', '>', '=', '+', '*', '%', '|', '&', '!', '^', '/'
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
            var start = _editor.Document.GetLocation((int)offset);
            var end = _editor.Document.GetLocation((int)(offset + length));

            using (_editor.Document.RunUpdate())
            {
                for (int i = start.Line; i < end.Line; i++)
                {
                    XmlIndentationTextInputHelper.Indent(_editor, _editor.Document.Lines[i].Offset);
                }
            }
            
            return _editor.Offset;
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
