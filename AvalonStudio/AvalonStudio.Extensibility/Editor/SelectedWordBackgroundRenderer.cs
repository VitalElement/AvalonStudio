using Avalonia.Media;
using AvaloniaEdit.Document;
using AvaloniaEdit.Rendering;
using System;
using System.Linq;

namespace AvalonStudio.TextEditor.Rendering
{
    public class SelectedWordBackgroundRenderer : IBackgroundRenderer
    {
        private readonly IBrush highlightBrush;

        public SelectedWordBackgroundRenderer()
        {
            highlightBrush = Brush.Parse("#113D6F");
        }

        public string SelectedWord { get; set; }

        public KnownLayer Layer => KnownLayer.Background;

        public void Draw(TextView textView, DrawingContext drawingContext)
        {
            foreach(var line in textView.VisualLines)
            {
                var text = line.Document.GetText(line.StartOffset, line.LastDocumentLine.EndOffset - line.StartOffset);


                if (!string.IsNullOrEmpty(SelectedWord) && text.Contains(SelectedWord))
                {
                    var startIndex = 0;

                    while (startIndex != -1)
                    {
                        startIndex = text.IndexOf(SelectedWord, startIndex);

                        if (startIndex != -1)
                        {
                            var rect =
                                BackgroundGeometryBuilder.GetRectsForSegment(textView,
                                    new TextSegment
                                    {
                                        StartOffset = startIndex + line.StartOffset,
                                        EndOffset = startIndex + line.StartOffset + SelectedWord.Length
                                    }).FirstOrDefault();

                            if (rect != null)
                            {
                                drawingContext.FillRectangle(highlightBrush, rect);

                                startIndex += SelectedWord.Length;
                            }
                            else
                            {
                                break;
                            }
                        }
                    }
                }
            }
           
        }
    }
}