namespace AvalonStudio.TextEditor
{
    using Perspex.Controls;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Perspex.Media;
    using Perspex.Rendering;

    public class TextInfo
    {
        public double LineHeight { get; set; }
        public double CharWidth { get; set; }
        public int NumLines { get; set; }
    }

    public abstract class TextEditorMargin : Control
    {
        public TextEditorMargin ()
        {
            
        }

        protected override void OnAttachedToVisualTree(IRenderRoot root)
        {            
            base.OnAttachedToVisualTree(root);

            textEditor = Parent.Parent.Parent.Parent.Parent.Parent as TextEditor;

            if(textEditor == null)
            {
                throw new Exception("Margin must be contained inside a TextEditor control.");
            }
        }

        public override void Render(DrawingContext context)
        {
            TextInfo info = new TextInfo();

            var charPos = textEditor.TextView.FormattedText.HitTestTextPosition(0);

            info.LineHeight = charPos.Height;
            info.CharWidth = charPos.Width;
            info.NumLines = (int)(Bounds.Height / info.LineHeight);

            Render(context, info);
        }

        public abstract void Render(DrawingContext context, TextInfo textInfo);

        protected TextEditor textEditor;
    }
}
