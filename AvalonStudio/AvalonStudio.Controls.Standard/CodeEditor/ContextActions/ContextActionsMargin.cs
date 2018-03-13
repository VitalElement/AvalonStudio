using Avalonia;
using Avalonia.Media;
using AvaloniaEdit.Editing;
using System;
using System.Collections.Generic;
using System.Text;

namespace AvalonStudio.Controls.Standard.CodeEditor.ContextActions
{
    public class ContextActionsMargin : AbstractMargin
    {
        private DrawingGroup _bulbIcon;
        private int _line;
        private const int margin = 2;

        public void SetBulb (int line)
        {
            _line = line;
            InvalidateVisual();
        }

        public void ClearBulb()
        {
            _line = 0;
        }

        public ContextActionsMargin()
        {
            if (Application.Current.Styles.TryGetResource("Bulb", out object bulbIcon))
            {
                _bulbIcon = bulbIcon as DrawingGroup;
            }
        }

        public override void Render(DrawingContext context)
        {
            if (_line > 0)
            {
                using (context.PushPreTransform(Matrix.CreateTranslation(margin, ((_line - 1) * TextView.DefaultLineHeight) - TextView.VerticalOffset)))
                {
                    _bulbIcon.Draw(context);
                }
            }
        }

        protected override Size MeasureOverride(Size availableSize)
        {
            if (TextView != null)
            {
                return new Size(TextView.DefaultLineHeight + margin + margin, 0);
            }

            return new Size(0, 0);
        }
    }
}
