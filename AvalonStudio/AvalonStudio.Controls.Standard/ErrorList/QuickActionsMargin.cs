using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Media;
using Avalonia.Media.Imaging;
using AvaloniaEdit.Editing;
using AvalonStudio.Controls.Standard.ErrorList;
using AvalonStudio.Documents;
using AvalonStudio.Platforms;
using AvalonStudio.Projects;
using Mono.Debugging.Client;
using System.Linq;
using System.Reflection;

namespace AvalonStudio.Controls.Standard.ErrorList
{
    public class QuickActionsMargin : AbstractMargin
    {
        private readonly IEditor _editor;
        private readonly IErrorList _errorList;
        private readonly ISourceFile _file;
        private static readonly Bitmap _bulbIcon;

        private int previewLine;
        private bool previewPointVisible;

        static QuickActionsMargin()
        {
            FocusableProperty.OverrideDefaultValue(typeof(QuickActionsMargin), true);

            var assembly = Assembly.GetExecutingAssembly();
            var resourceName = "AvalonStudio.Controls.Standard.ErrorList.Assets.light-bulb.png";

            using (var stream = assembly.GetManifestResourceStream(resourceName))
            {
                _bulbIcon = new Bitmap(stream);
            }
        }

        public QuickActionsMargin(IEditor editor, IErrorList errorList, ISourceFile file)
        {
            _errorList = errorList;
            _file = file;
            _editor = editor;
        }

        public override void Render(DrawingContext context)
        {
            context.FillRectangle(Brush.Parse("#333333"), Bounds);

            if (TextView.VisualLinesValid)
            {
                if (TextView.VisualLines.Count > 0)
                {
                    var firstLine = TextView.VisualLines.FirstOrDefault();
                    var height = firstLine.Height;
                    Width = height;
                    var textView = TextView;

                    foreach (var visualLine in TextView.VisualLines)
                    {
                        var fixits = _errorList.GetFixits(_file);

                        var match = fixits.FindOverlappingSegments(visualLine.FirstDocumentLine).FirstOrDefault();

                        if (match != null)
                        {
                            context.DrawImage(_bulbIcon, 1, new Rect(0, 0, _bulbIcon.PixelWidth, _bulbIcon.PixelHeight), new Rect(-5,
                                    visualLine.GetTextLineVisualYPosition(visualLine.TextLines[0], AvaloniaEdit.Rendering.VisualYPosition.LineTop) - TextView.VerticalOffset,
                                   Bounds.Width, height));
                        }
                    }
                }
            }
        }

        protected override Size MeasureOverride(Size availableSize)
        {
            if (TextView != null)
            {
                return new Size(TextView.DefaultLineHeight * 1.5, 0);
            }

            return new Size(0, 0);
        }

        protected override async void OnPointerReleased(PointerReleasedEventArgs e)
        {
            previewPointVisible = true;

            var textView = TextView;

            var offset = _editor.GetOffsetFromPoint(e.GetPosition(this));

            if (offset != -1)
            {
                var fixits = _errorList.GetFixits(_file);

                var document = _editor.GetDocument();
                var line = document.GetLineByOffset(offset);

                var match = fixits.FindOverlappingSegments(line).FirstOrDefault();

                if(match != null)
                {
                    await match.ApplyReplacementsAsync();
                }
            }

            InvalidateVisual();
        }

        protected override void OnPointerPressed(PointerPressedEventArgs e)
        {
            e.Handled = true;
        }
    }
}
