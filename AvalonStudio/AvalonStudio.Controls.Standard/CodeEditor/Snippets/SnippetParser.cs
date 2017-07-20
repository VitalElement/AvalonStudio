using AvaloniaEdit.Snippets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace AvalonStudio.Controls.Standard.CodeEditor.Snippets
{
    public class SnippetParser
    {
        static readonly Regex pattern = new Regex(@"\$\{([^\}]*)\}", RegexOptions.CultureInvariant);

        public static List<ISnippetElementProvider> SnippetElementProviders { get; } = new List<ISnippetElementProvider> { new DefaultSnippetElementProvider() };        

        public static Snippet Parse(string snippetText)
        {
            snippetText = "for (${type=int} ${counter=i} = ${initial=0}; ${counter} ${Caret}< ${end=end}; ${counter}++) {\n\t${Selection}\n}";

            snippetText = "private ${type=int} ${field=myVar};\n\npublic ${type} ${property=MyProperty}\n{\n\tget { return ${field}; }\n\tset { ${field} = value; }${Caret}\n}";

            if (snippetText == null)
                throw new ArgumentNullException("text");
            var replaceableElements = new Dictionary<string, SnippetReplaceableTextElement>(StringComparer.OrdinalIgnoreCase);
            foreach (Match m in pattern.Matches(snippetText))
            {
                string val = m.Groups[1].Value;
                int equalsSign = val.IndexOf('=');
                if (equalsSign > 0)
                {
                    string name = val.Substring(0, equalsSign);
                    replaceableElements[name] = new SnippetReplaceableTextElement();
                }
            }

            var snippet = new Snippet();
            int pos = 0;
            foreach (Match m in pattern.Matches(snippetText))
            {
                if (pos < m.Index)
                {
                    snippet.Elements.Add(new SnippetTextElement { Text = snippetText.Substring(pos, m.Index - pos) });
                    pos = m.Index;
                }

                snippet.Elements.Add(CreateElementForValue(replaceableElements, m.Groups[1].Value, m.Index, snippetText));

                pos = m.Index + m.Length;
            }
            if (pos < snippetText.Length)
            {
                snippet.Elements.Add(new SnippetTextElement { Text = snippetText.Substring(pos) });
            }
            if (!snippet.Elements.Any(e => e is SnippetCaretElement))
            {
                int index = snippet.Elements.FindIndex(e2 => e2 is SnippetSelectionElement);
                if (index > -1)
                    snippet.Elements.Insert(index + 1, new SnippetCaretElement());
            }
            return snippet;
        }

        static SnippetElement CreateElementForValue(Dictionary<string, SnippetReplaceableTextElement> replaceableElements, string val, int offset, string snippetText)
        {
            SnippetReplaceableTextElement srte;
            int equalsSign = val.IndexOf('=');
            if (equalsSign > 0)
            {
                string name = val.Substring(0, equalsSign);
                if (replaceableElements.TryGetValue(name, out srte))
                {
                    if (srte.Text == null)
                        srte.Text = val.Substring(equalsSign + 1);
                    return srte;
                }
            }

            foreach (ISnippetElementProvider provider in SnippetElementProviders)
            {
                SnippetElement element = provider.GetElement(new SnippetInfo(val, snippetText, offset));
                if (element != null)
                    return element;
            }

            if (replaceableElements.TryGetValue(val, out srte))
                return new SnippetBoundElement { TargetElement = srte };

            string result = GetValue(val);
            if (result != null)
                return new SnippetTextElement { Text = result };
            else
                return new SnippetReplaceableTextElement { Text = val }; // ${unknown} -> replaceable element
        }

        static string GetValue(string propertyName)
        {
            if ("ClassName".Equals(propertyName, StringComparison.OrdinalIgnoreCase))
            {
                // TODO implement a way to get the current class name!
            }

            // Todo implement a way to expand environment variables here...

            return propertyName;
        }
    }
}
