using System;
using Avalonia.Media;
using AvalonStudio.TextEditor.Rendering;

namespace AvalonStudio.Languages.CPlusPlus.Rendering
{
	internal class IncludeTextLineTransformer : IDocumentLineTransformer
	{
		private readonly IBrush brush = Brush.Parse("#D69D85");
        private readonly IBrush pragmaBrush = Brush.Parse("#9B9B9B");

        public event EventHandler<EventArgs> DataChanged;

        public void TransformLine(TextView textView, VisualLine line)
		{
            if (line.RenderedText.Text.Contains("#include") && !line.RenderedText.Text.Trim().StartsWith("//"))
            {
                var startIndex = line.RenderedText.Text.IndexOf("#include");

                line.RenderedText.SetForegroundBrush(pragmaBrush, startIndex, 8);
                line.RenderedText.SetForegroundBrush(brush, startIndex + 9, line.RenderedText.Text.Length - startIndex);
            }
        }
	}
}