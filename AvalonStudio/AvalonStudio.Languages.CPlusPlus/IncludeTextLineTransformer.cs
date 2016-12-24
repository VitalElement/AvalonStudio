using System;
using Avalonia.Media;
using AvalonStudio.TextEditor.Rendering;

namespace AvalonStudio.Languages.CPlusPlus.Rendering
{
	internal class IncludeTextLineTransformer : IDocumentLineTransformer
	{
		private readonly IBrush brush = Brush.Parse("#D69D85");

        public event EventHandler<EventArgs> DataChanged;

        public void TransformLine(TextView textView, VisualLine line)
		{
            if (line.RenderedText.Text.Contains("#include") && !line.RenderedText.Text.Trim().StartsWith("//"))
            {
                var startIndex = line.RenderedText.Text.IndexOf("#include");

                line.RenderedText.SetForegroundBrush(brush, startIndex, line.RenderedText.Text.Length - startIndex);
            }
        }
	}
}