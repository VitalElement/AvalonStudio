using Avalonia.Media;
using AvalonStudio.TextEditor.Document;
using System;
using System.Linq;

namespace AvalonStudio.TextEditor.Rendering
{
    public class SelectedWordBackgroundRenderer : IBackgroundRenderer
    {
        private readonly IBrush highlightBrush;

        private string selectedWord;

        public SelectedWordBackgroundRenderer()
        {
            highlightBrush = Brush.Parse("#113D6F");
        }

        public string SelectedWord
        {
            get
            {
                return selectedWord;
            }
            set
            {
                selectedWord = value;

                if (DataChanged != null)
                {
                    DataChanged(this, new EventArgs());
                }
            }
        }

        public event EventHandler<EventArgs> DataChanged;

        public void TransformLine(TextView textView, DrawingContext drawingContext, VisualLine line)
        {
            if (!string.IsNullOrEmpty(SelectedWord) && line.RenderedText.Text.Contains(SelectedWord))
            {
                var startIndex = 0;

                while (startIndex != -1)
                {
                    startIndex = line.RenderedText.Text.IndexOf(SelectedWord, startIndex);

                    if (startIndex != -1)
                    {
                        var rect =
                            VisualLineGeometryBuilder.GetRectsForSegment(textView,
                                new TextSegment
                                {
                                    StartOffset = startIndex + line.Offset,
                                    EndOffset = startIndex + line.Offset + SelectedWord.Length
                                }).First();

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