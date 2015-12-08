namespace AvalonStudio.TextEditor.Rendering
{
    using AvalonStudio.TextEditor.Document;
    using Perspex;
    using Perspex.Media;
    using System.Linq;

    public class SelectedWordTextLineTransformer : IDocumentLineTransformer
    {
        public SelectedWordTextLineTransformer(TextEditor editor)
        {
            this.editor = editor;
            this.highlightBrush = Brush.Parse("#113D6F");
        }

        private TextEditor editor;
        private Brush highlightBrush;

        public void TransformLine(TextView textView, DrawingContext context, Rect lineBounds, DocumentLine line, FormattedText formattedText)
        {
            if(!string.IsNullOrEmpty(editor.SelectedWord) && formattedText.Text.Contains (editor.SelectedWord))
            {
                int startIndex = 0;

                while (startIndex != -1)
                {
                    startIndex = formattedText.Text.IndexOf(editor.SelectedWord, startIndex);

                    if (startIndex != -1)
                    {
                        var rect = VisualLineGeometryBuilder.GetRectsForSegment(textView, new TextSegment() { StartOffset = startIndex + line.Offset, EndOffset = startIndex + line.Offset + editor.SelectedWord.Length }).First();

                        context.FillRectangle(highlightBrush, rect);

                        startIndex += editor.SelectedWord.Length;
                    }                    
                }
            }
        }
    }
}
