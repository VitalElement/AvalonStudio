using Avalonia.Ide.CompletionEngine;
using AvaloniaEdit.Document;
using AvaloniaEdit.Indentation;
using System;
using System.Collections.Generic;
using System.Text;

namespace AvalonStudio.Languages.Xaml
{
    class XamlIndentationStrategy : DefaultIndentationStrategy
    {
        public override void IndentLine(TextDocument document, DocumentLine line)
        {
            //Check if we are not inside a tag
            var textBefore = document.Text.Substring(0, Math.Max(0, line.EndOffset));
            var state = XmlParser.Parse(textBefore);
            if (state.State == XmlParser.ParserState.None)
            {
                //Find latest tag end
                var idx = textBefore.LastIndexOf('>');
                if (idx != -1)
                {
                    state = XmlParser.Parse(textBefore.Substring(0, Math.Max(0, idx)));
                    if (state.TagName.StartsWith('/'))
                    {
                        //TODO: find matching starting tag. XmlParser can't do that right now.
                        base.IndentLine(document, line);
                        return;
                    }
                    //Find starting '<'
                    bool insideAttribute = false;
                    for (; idx >= 0; idx--)
                    {
                        var ch = textBefore[idx];
                        if (ch == '"')
                            insideAttribute = !insideAttribute;
                        if (ch == '<' && !insideAttribute)
                        {
                            var textBeforeTag = textBefore.Substring(0, idx);
                            var lineStartIdx = textBeforeTag.LastIndexOf('\n');
                            if (lineStartIdx != -1)
                            {
                                //TODO: Do something about '\t' characters
                                var prefixLength = (idx - lineStartIdx) + 1;
                                document.Replace(line.EndOffset, 0,
                                    new string(' ', prefixLength));
                            }
                            return;
                        }
                    }
                }
            }
            else
            {
                base.IndentLine(document, line);
            }
        }
    }
}
