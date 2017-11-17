using Avalonia;
using Avalonia.Controls;
using Avalonia.Media;
using Avalonia.Xaml.Interactivity;
using System.Collections.Generic; 

namespace AvalonStudio.Behaviors
{
    class FormattedTextBlock : TextBlock
    {
        private static readonly AvaloniaProperty<List<FormattedTextStyleSpan>> SpansProperty =
            AvaloniaProperty.Register<FormattedTextBlock, List<FormattedTextStyleSpan>>(nameof(Spans));

        public List<FormattedTextStyleSpan> Spans
        {
            get { return GetValue(SpansProperty); }
            set { SetValue(SpansProperty, value); }
        }

        public override void Render(DrawingContext context)
        {
            FormattedText.Spans = Spans.AsReadOnly();

            base.Render(context);
        }
    }
}
