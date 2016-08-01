using System;
using Avalonia.Media;
using AvalonStudio.TextEditor.Rendering;

namespace AvalonStudio.Languages.CPlusPlus.Rendering
{
	internal class PragmaMarkTextLineTransformer : IDocumentLineTransformer
	{
		private readonly SolidColorBrush brush = new SolidColorBrush(Color.FromArgb(0xD0, 0xB8, 0x48, 0xFF));
		private readonly SolidColorBrush pragmaBrush = new SolidColorBrush(Color.FromArgb(0x88, 0xB8, 0x48, 0xFF));

		public event EventHandler<EventArgs> DataChanged;

		public void TransformLine(TextView textView, VisualLine line)
		{
            if (!line.RenderedText.Text.Trim().StartsWith("//"))
            {
                if (line.RenderedText.Text.Contains("#pragma mark"))
                {
                    var startIndex = line.RenderedText.Text.IndexOf("#pragma mark");

                    line.RenderedText.SetForegroundBrush(pragmaBrush, startIndex, 12);
                    line.RenderedText.SetForegroundBrush(brush, startIndex + 12, line.RenderedText.Text.Length - 12);
                }
                else if (line.RenderedText.Text.Contains("#pragma"))
                {
                    var startIndex = line.RenderedText.Text.IndexOf("#pragma");

                    line.RenderedText.SetForegroundBrush(pragmaBrush, startIndex, 7);
                }
            }
		}
	}
}