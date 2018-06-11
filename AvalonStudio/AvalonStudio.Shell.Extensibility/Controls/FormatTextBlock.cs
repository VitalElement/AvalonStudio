using Avalonia;
using Avalonia.Controls;
using Avalonia.Media;
using System.Collections.Generic;
using System;

namespace AvalonStudio.Controls
{
    public class FormattedTextBlock : TextBlock
    {
        public FormattedTextBlock()
        {
            this.GetObservable(SpansProperty).Subscribe(spans =>
            {
                if(spans == null)
                {
                    FormattedText.Spans = null;
                }
                else
                {
                    FormattedText.Spans = spans;
                }
            });

            this.GetObservable(StyledTextProperty).Subscribe(styledText =>
            {
                if(styledText == null)
                {
                    Spans = null;
                    Text = null;
                }
                else
                {
                    Text = styledText.Text;
                    Spans = styledText.Spans;                    
                }
            });
        }

        private static readonly AvaloniaProperty<StyledText> StyledTextProperty =
            AvaloniaProperty.Register<FormattedTextBlock, StyledText>(nameof(StyledText));

        public StyledText StyledText
        {
            get => GetValue(StyledTextProperty);
            set => SetValue(StyledTextProperty, value);
        }

        private static readonly AvaloniaProperty<IReadOnlyList<FormattedTextStyleSpan>> SpansProperty =
            AvaloniaProperty.Register<FormattedTextBlock, IReadOnlyList<FormattedTextStyleSpan>>(nameof(Spans));

        public IReadOnlyList<FormattedTextStyleSpan> Spans
        {
            get { return GetValue(SpansProperty); }
            set { SetValue(SpansProperty, value); }
        }
    }
}
