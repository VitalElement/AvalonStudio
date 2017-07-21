using AvaloniaEdit.Snippets;
using AvalonStudio.Extensibility;
using AvalonStudio.Extensibility.Plugin;
using AvalonStudio.Languages;
using System;
using System.Collections.Generic;
using System.Text;

namespace AvalonStudio.Controls.Standard.CodeEditor.Snippets
{
    public class SnippetManager : IExtension
    {
        public void Activation()
        {
        }

        public void BeforeActivation()
        {
            IoC.RegisterConstant(this);

            // Todo parse snippets directory.

            AddSnippet("cpp", new CodeSnippet { Name = "propfull", Description = "cpp property implementation", Snippet = "${type=int} ${ClassName}::get_${property=Property}()\n{\n\treturn ${ToFieldName(property)};\n}\n\nvoid ${ClassName}::set_${property}(${type} value)\n{\t${ToFieldName(property)} = value;${Caret}\n}" });
        }

        public void AddSnippet (string languageId, CodeSnippet snippet)
        {
            if(!_snippets.ContainsKey(languageId))
            {
                _snippets.Add(languageId, new Dictionary<string, CodeSnippet>());
            }

            if (!_snippets[languageId].ContainsKey(snippet.Name))
            {
                _snippets[languageId][snippet.Name] = snippet;
            }
        }

        private Dictionary<string, IDictionary<string, CodeSnippet>> _snippets = new Dictionary<string, IDictionary<string, CodeSnippet>>();

        public IDictionary<string, CodeSnippet> GetSnippets(ILanguageService languageService)
        {
            return _snippets[languageService.LanguageId];
        }
    }
}
