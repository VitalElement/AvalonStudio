using AvaloniaEdit.Document;
using AvalonStudio.Controls.Standard.CodeEditor.Highlighting;
using AvalonStudio.Controls.Standard.CodeEditor.Highlighting.Resources;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using Xunit;

namespace AvalonStudio.Controls.Standard.Tests
{
    public class StaticHighlightingTests
    {
        private static SyntaxHighlightingDefinition LoadDefinition(string resourceName)
        {
            using (var s = (Resources.OpenStream(resourceName)))
            {
                if (resourceName.EndsWith(".sublime-syntax"))
                {
                    using (var sr = new StreamReader(s))
                    {
                        return CodeEditor.Highlighting.Sublime3.Sublime3Format.ReadHighlighting(sr);
                    }
                }
                else if (resourceName.EndsWith(".tmLanguage"))
                {
                    return CodeEditor.Highlighting.TextMate.TextMateFormat.ReadHighlighting(s);
                }
                else if (resourceName.EndsWith(".tmLanguage.json"))
                {
                    return CodeEditor.Highlighting.TextMate.TextMateFormat.ReadHighlightingFromJson(s);
                }
            }

            return null;
        }



        [Fact]
        public void CSharp_Highlighting_Can_Detect_BuiltInTypes()
        {
            var definition = LoadDefinition("C#.sublime-syntax");

            string testCode = @"namespace Test
{
    public class TestClass
    {
        public void Main (int x, int y)
        {
        }
    }
}";

            var highlightedLines = RunHighlightingTest(definition, testCode);

            Assert.Equal(highlightedLines[4].Segments[3].ColorStyleKey, "keyword.other.void.source.cs");
        }

        internal static List<HighlightedLine> RunHighlightingTest(SyntaxHighlightingDefinition highlighting, string inputText)
        {
            var result = new List<HighlightedLine>();

            var document = new TextDocument(inputText);

            highlighting.PrepareMatches();

            var syntaxHighlighting = new SyntaxHighlighting(highlighting, document);

            foreach (var line in document.Lines)
            {
                var coloredSegments = syntaxHighlighting.GetHighlightedLineAsync(line, CancellationToken.None).Result;

                result.Add(coloredSegments);
            }

            return result;
        }

    }
}
