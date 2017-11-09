using System;
using Avalonia.Controls;
using Avalonia.Ide.CompletionEngine;
using Avalonia.Input;
using Avalonia.Markup.Xaml;
using AvaloniaEdit.Highlighting;

namespace AvalonStudio.Languages.Xaml
{
    public class XamlEditorView : UserControl
    {
        private AvaloniaEdit.TextEditor _editor;
        public XamlEditorView()
        {
            InitializeComponent();
            _editor = this.FindControl<AvaloniaEdit.TextEditor>("editor");
            _editor.SyntaxHighlighting = HighlightingManager.Instance.GetDefinition("XML");
            _editor.TextArea.TextEntered += OnTextEntered;
        }

        void OnTextEntered(object sender, TextInputEventArgs args)
        {
            if (args.Text == ">")
            {
                var textBefore = _editor.Text.Substring(0, Math.Max(0, _editor.TextArea.Caret.Offset - 1));
                if (textBefore.Length>2 && textBefore[textBefore.Length - 1] != '/')
                {
                    var state = XmlParser.Parse(textBefore);
                    if (state.State == XmlParser.ParserState.InsideElement
                        || state.State == XmlParser.ParserState.StartElement 
                        || state.State == XmlParser.ParserState.AfterAttributeValue)
                    {
                        var caret = _editor.TextArea.Caret.Offset;
                        _editor.Document.Replace(_editor.TextArea.Caret.Offset, 0, $"</{state.TagName}>");
                        _editor.TextArea.Caret.Offset = caret;
                    }
                }
            }

            if (args.Text == "/")
            {
                var textBefore = _editor.Text.Substring(0, Math.Max(0, _editor.TextArea.Caret.Offset - 1));
                if (textBefore.Length > 2 && textBefore[textBefore.Length - 1] != '/')
                {
                    var state = XmlParser.Parse(textBefore);
                    if (state.State == XmlParser.ParserState.InsideElement
                        || state.State == XmlParser.ParserState.StartElement
                        || state.State == XmlParser.ParserState.AfterAttributeValue)
                    {
                        var caret = _editor.TextArea.Caret.Offset;
                        _editor.Document.Replace(_editor.TextArea.Caret.Offset, 0, $">");
                        _editor.TextArea.Caret.Offset = caret + 1;
                    }
                }
            }

            if (args.Text == "=")
            {
                var textBefore = _editor.Text.Substring(0, Math.Max(0, _editor.TextArea.Caret.Offset - 1));
                if (textBefore.Length > 2 && textBefore[textBefore.Length - 1] != '/')
                {
                    var state = XmlParser.Parse(textBefore);
                    if (state.State == XmlParser.ParserState.StartAttribute)
                    {
                        var caret = _editor.TextArea.Caret.Offset;
                        _editor.Document.Replace(_editor.TextArea.Caret.Offset, 0, "\"\" ");
                        _editor.TextArea.Caret.Offset = caret+1;
                    }
                }
            }

            if (args.Text == "\n")
            {
                //Check if we are not inside a tag
                var textBefore = _editor.Text.Substring(0, Math.Max(0, _editor.TextArea.Caret.Offset - 1));
                var state = XmlParser.Parse(textBefore);
                if (state.State == XmlParser.ParserState.None)
                {
                    //Find latest tag end
                    var idx = textBefore.LastIndexOf('>');
                    if (idx != -1)
                    {
                        state = XmlParser.Parse(textBefore.Substring(0, Math.Max(0, idx - 1)));
                        if (state.TagName.StartsWith('/'))
                        {
                            //TODO: find matching starting tag. XmlParser can't do that right now.
                            return;
                        }
                        //Find starting '<'
                        bool insideAttribute = false;
                        for(;idx>=0; idx--)
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
                                    var prefixLength = (idx - lineStartIdx) - 1;
                                    _editor.Document.Replace(_editor.TextArea.Caret.Offset, 0,
                                        new string(' ', prefixLength));
                                }
                                return;
                            }


                        }
                    }
                }
            }
        }
        
        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}