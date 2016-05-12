namespace AvalonStudio.TextEditor.Rendering
{
    using AvalonStudio.TextEditor.Document;
    using Avalonia;
    using Avalonia.Media;
    using System.Linq;
    using System;

    public class SelectedWordBackgroundRenderer : IBackgroundRenderer
    {
        public SelectedWordBackgroundRenderer()
        {
            this.highlightBrush = Brush.Parse("#113D6F");
        }
                
        private IBrush highlightBrush;

        public event EventHandler<EventArgs> DataChanged;

        private string selectedWord;
        public string SelectedWord
        {
            get { return selectedWord; }
            set
            {
                selectedWord = value;

                if(DataChanged != null)
                {
                    DataChanged(this, new EventArgs());
                }
            }
        }   


        public void TransformLine(TextView textView, DrawingContext drawingContext, VisualLine line)
        {
            if (!string.IsNullOrEmpty(SelectedWord) && line.RenderedText.Text.Contains(SelectedWord))
            {
                int startIndex = 0;

                while (startIndex != -1)
                {
                    startIndex = line.RenderedText.Text.IndexOf(SelectedWord, startIndex);

                    if (startIndex != -1)
                    {
                        var rect = VisualLineGeometryBuilder.GetRectsForSegment(textView, new TextSegment() { StartOffset = startIndex + line.Offset, EndOffset = startIndex + line.Offset + SelectedWord.Length }).First();

                        drawingContext.FillRectangle(highlightBrush, rect);

                        startIndex += SelectedWord.Length;
                    }
                }
            }
        }

        public void Draw(TextView textView, DrawingContext drawingContext)
        {
            
        }
    }
}
