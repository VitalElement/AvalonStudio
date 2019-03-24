using Avalonia;
using Avalonia.Input;
using Avalonia.Media;
using AvaloniaEdit.Editing;
using AvalonStudio.Extensibility.Editor;
using AvalonStudio.Extensibility.Theme;
using System;
using System.Collections.Generic;
using System.Text;

namespace AvalonStudio.Controls.Editor.ContextActions
{
    public class ContextActionsMargin : AbstractMargin
    {
        private DrawingGroup _bulbIcon;
        private int _line;
        private const int margin = 10;
        private CodeEditor _editor;

        protected int Line => _line;

        public void SetBulb(int line)
        {
            _line = line;
            InvalidateVisual();
        }

        public void ClearBulb()
        {
            _line = 0;
            InvalidateVisual();
        }

        protected virtual void OnOpenPopup()
        {

        }

        protected virtual void OnClosePopup()
        {

        }

        public ContextActionsMargin(CodeEditor editor)
        {
            if (Application.Current.Styles.TryGetResource("Bulb", out object bulbIcon))
            {
                _bulbIcon = bulbIcon as DrawingGroup;
            }

            _editor = editor;
        }

        public override void Render(DrawingContext context)
        {
            context.FillRectangle(ColorScheme.CurrentColorScheme.Background, new Rect(Bounds.Size));

            if (_line > 0)
            {
                using (context.PushPreTransform(Matrix.CreateTranslation(margin, ((_line - 1) * TextView.DefaultLineHeight) - TextView.VerticalOffset)))
                {
                    _bulbIcon.Draw(context);
                }
            }            
        }
        
        protected override void OnPointerMoved(PointerEventArgs e)
        {   
            if (_line > 0)
            {
                var textView = TextView;

                var position = e.GetPosition(this);

                if (position.X > margin && position.X < (Bounds.Width - margin))
                {
                    var offset = _editor.GetOffsetFromPoint(position);

                    if (offset != -1)
                    {
                        var currentLine = textView.Document.GetLineByOffset(offset).LineNumber; // convert from text line to visual line.

                        if (currentLine == _line)
                        {
                            OnOpenPopup();
                            return;
                        }
                    }
                }

                OnClosePopup();
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
