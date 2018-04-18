using AvalonStudio.Controls.Standard.CodeEditor.Highlighting.TextMate;
using System;
using System.Collections.Generic;
using System.IO;

namespace AvalonStudio.Controls.Standard.CodeEditor.Highlighting
{
    public static class SyntaxHighlightingService
    {
        static LanguageBundle builtInBundle = new LanguageBundle("default", null);
        static List<LanguageBundle> languageBundles = new List<LanguageBundle>();

        internal static IEnumerable<LanguageBundle> AllBundles
        {
            get
            {
                return languageBundles;
            }
        }

        static object LoadFile(LanguageBundle bundle, string file, Func<Stream> openStream)
        {
            if (file.EndsWith(".tmLanguage", StringComparison.OrdinalIgnoreCase))
            {

                using (var stream = openStream())
                {
                    var highlighting = TextMateFormat.ReadHighlighting(stream);

                    if (highlighting != null)
                        bundle.Add(highlighting);

                    return highlighting;

                }
            }

            return null;
        }

        static void PrepareMatches()
        {
            foreach (var bundle in languageBundles)
            {
                foreach (var h in bundle.Highlightings)
                    h.PrepareMatches();
            }
        }


    }
}
