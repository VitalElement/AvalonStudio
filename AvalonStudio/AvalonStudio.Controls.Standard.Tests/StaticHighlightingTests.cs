using AvaloniaEdit.Document;
using AvalonStudio.Controls.Standard.CodeEditor.Highlighting;
using AvalonStudio.Controls.Standard.CodeEditor.Highlighting.Resources;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;
using Xunit;

namespace AvalonStudio.Controls.Standard.Tests
{
    public class StaticHighlightingTests
    {
        [Fact]
        public void CSharp_Highlighting_Can_Detect_BuiltInTypes()
        {
            using (var s = (Resources.OpenStream("C#.sublime-syntax")))
            {
                using (var sr = new StreamReader(s))
                {
                    var definition = CodeEditor.Highlighting.Sublime3.Sublime3Format.ReadHighlighting(sr);

                    string testCode = @"namespace Test
{
    public class TestClass
    {
        public void Main (int x, int y)
        {
        }
    }
}";

                    RunHighlightingTest(definition, testCode);
                }
            }
        }

        internal static void RunHighlightingTest(SyntaxHighlightingDefinition highlighting, string inputText)
        {
            var document = new TextDocument(inputText);
            var sb = new StringBuilder();
            int lineNumber = 0;

            //var expectedSegments = new List<Tuple<DocumentLocation, string>>();
           /* using (var sr = new StringReader(inputText))
            {
                while (true)
                {
                    var lineText = sr.ReadLine();
                    if (lineText == null)
                        break;
                    var idx = lineText.IndexOf('^');
                    if (idx >= 0)
                    {
                        expectedSegments.Add(Tuple.Create(new DocumentLocation(lineNumber, idx + 1), lineText.Substring(idx + 1).Trim()));
                    }
                    else
                    {
                        lineNumber++;
                        sb.AppendLine(lineText);
                    }
                }
            }
            editor.Text = sb.ToString();*/

            highlighting.PrepareMatches();
            var syntaxHighlighting = new SyntaxHighlighting(highlighting, document);

            //var line = editor.GetLine (6); {
            foreach (var line in document.Lines)
            {
                if (document.GetText(line).Contains("void"))
                {
                    var coloredSegments = syntaxHighlighting.GetHighlightedLineAsync(line, CancellationToken.None).Result.Segments;

                    foreach (var segment in coloredSegments)
                    {
                        var text = document.GetText(line.Offset + segment.Offset, segment.Length);
                    }
                }



                //for (int i = 0; i < expectedSegments.Count; i++)
                //{
                //    var seg = expectedSegments[i];
                //    if (seg.Item1.Line == line.LineNumber)
                //    {
                //        var matchedSegment = coloredSegments.FirstOrDefault(s => s.Contains(seg.Item1.Column - 1));
                //        Assert.NotNull(matchedSegment, "No segment found at : " + seg.Item1);
                //        foreach (var segi in seg.Item2.Split(new[] { " " }, StringSplitOptions.RemoveEmptyEntries))
                //        {
                //            Console.WriteLine("line " + line.LineNumber + " : " + editor.GetTextAt(line));
                //            Console.WriteLine(segi);
                //            Console.WriteLine(string.Join(", ", matchedSegment.ScopeStack.ToArray()));
                //            string mk = null;
                //            int d = 0;
                //            var expr = StackMatchExpression.Parse(segi);
                //            var matchResult = expr.MatchesStack(matchedSegment.ScopeStack, ref mk);
                //            Assert.IsTrue(matchResult.Item1, "Wrong color at " + seg.Item1 + " expected " + segi + " was " + string.Join(", ", matchedSegment.ScopeStack.ToArray()));
                //        }
                //        expectedSegments.RemoveAt(i);
                //        i--;
                //    }
                //}
            }
            //Assert.AreEqual(0, expectedSegments.Count, "Not all segments matched.");
        }

    }
}
