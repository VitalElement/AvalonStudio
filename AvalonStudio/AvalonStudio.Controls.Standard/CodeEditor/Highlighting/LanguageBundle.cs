using System;
using System.Collections.Generic;
using System.Text;

namespace AvalonStudio.Controls.Standard.CodeEditor.Highlighting
{
    class LanguageBundle
    {
        List<SyntaxHighlightingDefinition> highlightings = new List<SyntaxHighlightingDefinition>();
        /*List<TmSetting> settings = new List<TmSetting>();
        List<TmSnippet> snippets = new List<TmSnippet>();
        List<EditorTheme> editorThemes = new List<EditorTheme>();

        public IReadOnlyList<EditorTheme> EditorThemes
        {
            get
            {
                return editorThemes;
            }
        }*/

        public IReadOnlyList<SyntaxHighlightingDefinition> Highlightings
        {
            get
            {
                return highlightings;
            }
        }

        /*public IReadOnlyList<TmSetting> Settings
        {
            get
            {
                return settings;
            }
        }

        public IReadOnlyList<TmSnippet> Snippets
        {
            get
            {
                return snippets;
            }
        }*/

        public string Name { get; private set; }

        public string FileName { get; private set; }

        public LanguageBundle(string name, string fileName)
        {
            Name = name;
            FileName = fileName;
        }

        /*public void Add(EditorTheme theme)
        {
            editorThemes.Add(theme);
        }

        public void Remove(EditorTheme style)
        {
            editorThemes.Remove(style);
        }

        public void Add(TmSetting setting)
        {
            settings.Add(setting);
        }

        public void Add(TmSnippet snippet)
        {
            snippets.Add(snippet);
        }*/

        public void Add(SyntaxHighlightingDefinition highlighting)
        {
            highlightings.Add(highlighting);
        }
    }

}
