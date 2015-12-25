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

        public void TransformLine(TextView textView, DrawingContext context, Rect lineBounds, VisualLine line)
        {
            if(!string.IsNullOrEmpty(editor.SelectedWord) && line.RenderedText.Text.Contains (editor.SelectedWord))
            {
                int startIndex = 0;

                while (startIndex != -1)
                {
                    startIndex = line.RenderedText.Text.IndexOf(editor.SelectedWord, startIndex);

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
