namespace AvalonStudio.TextEditor.Rendering
{
    using AvalonStudio.TextEditor.Document;
    using Perspex.Media;
    using System.Collections.Generic;

    public class TextColoringTransformer : IDocumentLineTransformer
    {
        public TextColoringTransformer (TextEditor editor)
        {
            this.editor = editor;
        }

        private TextEditor editor;

        public List<TextTransformation> TextTransformations;

        public Brush GetBrush(HighlightType type)
        {
            Brush result;

            switch(type)
            {
                case HighlightType.Comment:
                    result = editor.CommentBrush;
                    break;

                case HighlightType.Identifier:
                    result= editor.IdentifierBrush;
                    break;

                case HighlightType.Keyword:
                    result = editor.KeywordBrush;
                    break;

                case HighlightType.Literal:
                    result = editor.LiteralBrush;
                    break;

                case HighlightType.Punctuation:
                    result = editor.PunctuationBrush;
                    break;

                case HighlightType.UserType:
                    result = editor.UserTypeBrush;
                    break;

                default:
                    result = Brushes.Red;
                    break;
            }

            return result;
        }

        public void ColorizeLine(DocumentLine line, FormattedText formattedText)
        {
            foreach(var transform in editor.SyntaxHighlightingData)
            {               
                if (transform.Start >= line.Offset)
                {
                    var formattedOffset = transform.Start - line.Offset;

                    var brush = GetBrush(transform.Type);

                    formattedText.SetForegroundBrush(brush, formattedOffset, transform.Length);
                }
            }
        }
    }
}
