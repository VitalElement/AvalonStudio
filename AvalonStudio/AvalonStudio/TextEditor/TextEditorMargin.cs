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
    using Perspex;
    using Rendering;
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

        protected override void OnAttachedToVisualTree(VisualTreeAttachmentEventArgs e)
        {            
            base.OnAttachedToVisualTree(e);

            textEditor = Parent.Parent.Parent.Parent.Parent.Parent as TextEditor;

            if(textEditor == null)
            {
                throw new Exception("Margin must be contained inside a TextEditor control.");
            }
        }

        public override void Render(DrawingContext context)
        {
            if (textEditor.TextDocument != null)
            {
                TextInfo info = new TextInfo();

                var charRect = VisualLineGeometryBuilder.GetTextPosition(textEditor.TextView, 0);

                info.LineHeight = charRect.Height;
                info.CharWidth = charRect.Width;

                info.NumLines = (int)(Bounds.Height / info.LineHeight);

                Render(context, info);
            }
        }

        public abstract void Render(DrawingContext context, TextInfo textInfo);

        protected TextEditor textEditor;
    }
}
