using System;
using Avalonia.Media;
using AvalonStudio.TextEditor.Rendering;

namespace AvalonStudio.Languages.CPlusPlus.Rendering
{
	internal class DefineTextLineTransformer : IDocumentLineTransformer
	{
		private readonly SolidColorBrush brush = new SolidColorBrush(Color.FromArgb(0xD0, 0xB8, 0x48, 0xFF));
		private readonly IBrush pragmaBrush = Brush.Parse("#68217A");

        public event EventHandler<EventArgs> DataChanged;

        public void TransformLine(TextView textView, VisualLine line)
		{
			if (line.RenderedText.Text.Contains("#define") && !line.RenderedText.Text.Trim().StartsWith("//"))
			{
				var startIndex = line.RenderedText.Text.IndexOf("#define");

				line.RenderedText.SetForegroundBrush(pragmaBrush, startIndex, 7);
				line.RenderedText.SetForegroundBrush(brush, startIndex + 7, line.RenderedText.Text.Length - 7);
			}
		}
	}
}