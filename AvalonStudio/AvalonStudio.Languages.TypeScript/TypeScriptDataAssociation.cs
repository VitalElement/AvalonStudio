using AvalonStudio.TextEditor.Document;

namespace AvalonStudio.Languages.TypeScript
{
    internal class TypeScriptDataAssociation
    {
        private TextDocument textDocument;

        public TypeScriptDataAssociation(TextDocument textDocument)
        {
            this.textDocument = textDocument;
        }
    }
}