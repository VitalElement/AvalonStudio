using AvaloniaEdit.Snippets;
using AvalonStudio.Extensibility;
using AvalonStudio.Extensibility.Plugin;
using AvalonStudio.Languages;
using AvalonStudio.Platforms;
using AvalonStudio.Utils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Linq;

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

            var snippetFolders = Directory.EnumerateDirectories(Platform.SnippetsFolder).Concat(Directory.EnumerateDirectories(Platform.InBuiltSnippetsFolder));

            foreach(var folder in snippetFolders)
            {
                foreach (var file in Directory.EnumerateFiles(folder))
                {
                    var snippet = SerializedObject.Deserialize<CodeSnippet>(file);
                    AddSnippet(Path.GetFileName(folder), snippet);
                }
            }
        }

        public void AddSnippet(string languageId, CodeSnippet snippet)
        {
            if (!_snippets.ContainsKey(languageId))
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
            if (_snippets.ContainsKey(languageService.LanguageId))
            {
                return _snippets[languageService.LanguageId];
            }
            else
            {
                return new Dictionary<string, CodeSnippet>();
            }
        }
    }
}
