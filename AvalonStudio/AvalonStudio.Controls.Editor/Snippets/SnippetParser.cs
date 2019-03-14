using AvaloniaEdit.Snippets;
using AvalonStudio.Languages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace AvalonStudio.Controls.Editor.Snippets
{
    public class SnippetParser
    {
        static readonly Regex pattern = new Regex(@"\$\{([^\}]*)\}", RegexOptions.CultureInvariant);
        static readonly Regex functionPattern = new Regex(@"^([a-zA-Z]+)\(([^\)]*)\)$", RegexOptions.CultureInvariant);

        public static List<ISnippetElementProvider> SnippetElementProviders { get; } = new List<ISnippetElementProvider> { new DefaultSnippetElementProvider() };

        public static Snippet Parse(ILanguageService languageService, int caret, int line, int column, string snippetText)
        {
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

                snippet.Elements.Add(CreateElementForValue(languageService, caret, line, column, replaceableElements, m.Groups[1].Value, m.Index, snippetText));

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

        static SnippetElement CreateElementForValue(ILanguageService languageService, int caret, int line, int column, Dictionary<string, SnippetReplaceableTextElement> replaceableElements, string val, int offset, string snippetText)
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

            Match m = functionPattern.Match(val);
            if (m.Success)
            {
                Func<string, string> f = GetFunction(languageService, m.Groups[1].Value);
                
                if (f != null)
                {
                    string innerVal = m.Groups[2].Value;

                    if (replaceableElements.TryGetValue(innerVal, out srte))
                        return new FunctionBoundElement { TargetElement = srte, Function = f };
                    string result2 = GetValue(languageService, caret, line, column, innerVal);
                    if (result2 != null)
                        return new SnippetTextElement { Text = f(result2) };
                    else
                        return new SnippetTextElement { Text = f(innerVal) };
                }
                else if (replaceableElements.TryGetValue("_" + m.Groups[1].Value, out srte))
                {
                    return new SnippetBoundElement { TargetElement = srte };
                }
                else
                {
                    return replaceableElements["_" + m.Groups[1].Value] = new SnippetReplaceableTextElement { Text = "_" + m.Groups[1].Value };
                }
            }

            string result = GetValue(languageService, caret, line, column, val);
            if (result != null)
                return new SnippetTextElement { Text = result };
            else if (replaceableElements.TryGetValue(val, out srte))
            {
                return new SnippetBoundElement { TargetElement = srte };
            }
            else
            {
                return replaceableElements[val] = new SnippetReplaceableTextElement { Text = val }; // ${unknown} -> replaceable element
            }
        }

        static string GetValue(ILanguageService languageService, int offset, int line, int column, string propertyName)
        {
            // evaluate things like class name, function name, etc.
            if (languageService != null && languageService.SnippetDynamicVariables != null && languageService.SnippetDynamicVariables.ContainsKey(propertyName))
            {
                return languageService.SnippetDynamicVariables[propertyName](offset, line, column); //todo pass in line / column here..
            }

            // Todo implement a way to expand environment variables here...

            return null;
        }

        static Func<string, string> GetFunction(ILanguageService languageService, string name)
        {
            if ("toLower".Equals(name, StringComparison.OrdinalIgnoreCase))
                return s => s.ToLower();
            if ("toUpper".Equals(name, StringComparison.OrdinalIgnoreCase))
                return s => s.ToUpper();

            if (languageService != null && languageService.SnippetCodeGenerators != null && languageService.SnippetCodeGenerators.ContainsKey(name))
            {
                return languageService.SnippetCodeGenerators[name];
            }

            return null;
        }

        public sealed class FunctionBoundElement : SnippetBoundElement
        {
            internal Func<string, string> Function
            {
                set { function = value; }
            }

            private Func<string, string> function;

            public override string ConvertText(string input)
            {
                return function(input);
            }
        }
    }
}
