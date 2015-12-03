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
                        
            TextTransformations = new List<TextTransformation>();
        }

        private TextEditor editor;

        public void SetTransformations ()
        {
            var transformations = new List<TextTransformation>();

            foreach (var transform in editor.SyntaxHighlightingData)
            {
                transformations.Add(new TextTransformation { Foreground = GetBrush(transform.Type), StartAnchor = editor.TextDocument.CreateAnchor(transform.Start), EndAnchor = editor.TextDocument.CreateAnchor(transform.Start + transform.Length) });
            }

            TextTransformations = transformations;
        }

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
            foreach (var transform in TextTransformations)
            {               
                if (transform.EndAnchor.Offset >= line.Offset)
                {
                    var formattedOffset = 0;

                    if (transform.StartAnchor.Offset > line.Offset)
                    {
                        formattedOffset = transform.StartAnchor.Offset - line.Offset;
                    }                    

                    formattedText.SetForegroundBrush(transform.Foreground, formattedOffset, transform.EndAnchor.Offset);
                }

                if(transform.StartAnchor.Offset > line.EndOffset)
                {
                    break;
                }
            }
        }
    }
}
