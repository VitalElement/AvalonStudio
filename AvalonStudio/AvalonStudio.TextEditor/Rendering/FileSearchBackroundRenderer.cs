using System;
using System.Linq;
using Avalonia.Media;
using AvalonStudio.TextEditor.Document;

namespace AvalonStudio.TextEditor.Rendering
{
	public class FileSearchBackroundRenderer : IBackgroundRenderer
	{
		private readonly IBrush _highlightBrush;
	    private bool _caseSensitive;

		private string _selectedWord;

		public FileSearchBackroundRenderer()
		{
			_highlightBrush = Brush.Parse("#ff8400");
		}

		public string SelectedWord
		{
			get { return _selectedWord; }
			set
			{
				_selectedWord = value;

                DataChanged?.Invoke(this, new EventArgs());
            }
		}

        public bool CaseSensitive {
            get { return _caseSensitive; }
            set {
                _caseSensitive = value;

                DataChanged?.Invoke(this, new EventArgs());
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
					startIndex = line.RenderedText.Text.IndexOf(SelectedWord, startIndex, CaseSensitive ? StringComparison.Ordinal : StringComparison.CurrentCultureIgnoreCase);

					if (startIndex != -1)
					{
						var rect =
							VisualLineGeometryBuilder.GetRectsForSegment(textView,
								new TextSegment
								{
									StartOffset = startIndex + line.Offset,
									EndOffset = startIndex + line.Offset + SelectedWord.Length
								}).First();

						drawingContext.FillRectangle(_highlightBrush, rect);

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