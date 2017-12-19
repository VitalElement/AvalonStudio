using AvaloniaEdit.Indentation;
using AvalonStudio.Documents;
using AvalonStudio.Editor;
using AvalonStudio.Extensibility.Languages.CompletionAssistance;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace AvalonStudio.Languages.Xaml
{
    class XmlLanguageService : ILanguageService
    {
        private static List<ICodeEditorInputHelper> s_InputHelpers = new List<ICodeEditorInputHelper>
        {
            new CompleteCloseTagCodeEditorHelper(),
            new TerminateElementCodeEditorHelper(),
            new InsertQuotesForPropertyValueCodeEditorHelper(),
            new InsertExtraNewLineBetweenAttributesOnEnterCodeInputHelper()
        };

        public IIndentationStrategy IndentationStrategy { get; } = new XamlIndentationStrategy();

        public virtual string Title => "XML";

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

        public virtual string Identifier => "XML";

        public void Activation()
        {
        }

        public void BeforeActivation()
        {
        }

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
            return cursor;
        }

        public Task<Symbol> GetSymbolAsync(IEditor editor, List<UnsavedFile> unsavedFiles, int offset)
        {
            return Task.FromResult<Symbol>(null);
        }

        public Task<List<Symbol>> GetSymbolsAsync(IEditor editor, List<UnsavedFile> unsavedFiles, string name)
        {
            return Task.FromResult<List<Symbol>>(null);
        }

        public bool IsValidIdentifierCharacter(char data)
        {
            return char.IsLetterOrDigit(data);
        }

        public virtual void RegisterSourceFile(IEditor editornew)
        {
        }

        public virtual void UnregisterSourceFile(IEditor editor)
        {

        }

        public Task<CodeAnalysisResults> RunCodeAnalysisAsync(IEditor editor, List<UnsavedFile> unsavedFiles, Func<bool> interruptRequested)
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
    }
}
