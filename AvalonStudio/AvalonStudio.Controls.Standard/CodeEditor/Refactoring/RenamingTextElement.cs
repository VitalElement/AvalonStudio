using Avalonia.Media;
using AvaloniaEdit.Document;
using AvaloniaEdit.Editing;
using AvaloniaEdit.Rendering;
using System;
using System.Collections.Generic;
using System.Text;

namespace AvalonStudio.Controls.Standard.CodeEditor.Refactoring
{
    public sealed class RenameSegment : ISegment
    {
        private readonly TextAnchor _start;
        private readonly TextAnchor _end;

        /// <inheritdoc/>
        public int Offset => _start.Offset;

        /// <inheritdoc/>
        public int Length => Math.Max(0, _end.Offset - _start.Offset);

        /// <inheritdoc/>
        public int EndOffset => Math.Max(_start.Offset, _end.Offset);

        /// <summary>
        /// Creates a new AnchorSegment using the specified anchors.
        /// The anchors must have <see cref="TextAnchor.SurviveDeletion"/> set to true.
        /// </summary>
        public RenameSegment(TextAnchor start, TextAnchor end)
        {
            if (start == null)
                throw new ArgumentNullException(nameof(start));
            if (end == null)
                throw new ArgumentNullException(nameof(end));
            if (!start.SurviveDeletion)
                throw new ArgumentException("Anchors for AnchorSegment must use SurviveDeletion", nameof(start));
            if (!end.SurviveDeletion)
                throw new ArgumentException("Anchors for AnchorSegment must use SurviveDeletion", nameof(end));
            _start = start;
            _end = end;
        }

        /// <summary>
        /// Creates a new AnchorSegment that creates new anchors.
        /// </summary>
        public RenameSegment(TextDocument document, int offset, int length)
        {
            _start = document?.CreateAnchor(offset) ?? throw new ArgumentNullException(nameof(document));
            _start.SurviveDeletion = true;
            _start.MovementType = AnchorMovementType.BeforeInsertion;
            _end = document.CreateAnchor(offset + length);
            _end.SurviveDeletion = true;
            _end.MovementType = AnchorMovementType.AfterInsertion;
        }
    }

    internal class RenamingTextElement
    {
        private RenameSegment _segment;
        private TextArea _textArea;
        private Renderer _background;
        private RenamingTextElement _target;

        public RenameSegment Segment => _segment;

        public string Text
        {
            get => _textArea.Document.GetText(_segment);
            set
            {
                var offset = _segment.Offset;
                var length = _segment.Length;
                _textArea.Document.Replace(offset, length, value);
                if (length == 0)
                {
                    // replacing an empty anchor segment with text won't enlarge it, so we have to recreate it
                    _segment = new RenameSegment(_textArea.Document, offset, value.Length);
                }
            }
        }

        private string GetText()
        {
            return _textArea.Document.GetText(_segment.Offset, _segment.Length);
        }

        public event EventHandler<DocumentChangeEventArgs> TextChanging;
        public event EventHandler TextChanged;
        private string lastText;

        void OnDocumentTextChanged(object sender, EventArgs e)
        {
            if (Text != lastText)
            {
                TextChanged?.Invoke(this, e);
            }
        }

        void OnDocumentTextChanging(object sender, DocumentChangeEventArgs e)
        {
            lastText = Text;
            
            TextChanging?.Invoke(this, e);
        }

        public RenamingTextElement(TextArea textArea, int start, int length)
        {
            _textArea = textArea;
            _segment = new RenameSegment(textArea.Document, start, length);
        }

        public void Activate(RenamingTextElement target)
        {
            if (_background != null)
            {
                throw new Exception("Unexpected operation.");
            }

            _background = new Renderer { Element = this };
            _textArea.TextView.BackgroundRenderers.Add(_background);

            if (target != null && target != this)
            {
                _target = target;
            }
            else
            {
                // Be careful with references from the document to the editing/snippet layer - use weak events
                // to prevent memory leaks when the text area control gets dropped from the UI while the snippet is active.
                // The InsertionContext will keep us alive as long as the snippet is in interactive mode.
                TextDocumentWeakEventManager.TextChanged.AddHandler(_textArea.Document, OnDocumentTextChanged);
                TextDocumentWeakEventManager.Changing.AddHandler(_textArea.Document, OnDocumentTextChanging);
            }
        }

        public void UpdateTextToTarget()
        {
            // Don't copy text if the segments overlap (we would get an endless loop).
            // This can happen if the user deletes the text between the replaceable element and the bound element.
            if (SimpleSegment.GetOverlap(_segment, _target.Segment) == SimpleSegment.Invalid)
            {
                var offset = _segment.Offset;
                var length = _segment.Length;
                var text = _target.Text;
                if (length != text.Length || text != _textArea.Document.GetText(offset, length))
                {
                    // Call replace only if we're actually changing something.
                    // Without this check, we would generate an empty undo group when the user pressed undo.
                    _textArea.Document.Replace(offset, length, text);
                    if (length == 0)
                    {
                        // replacing an empty anchor segment with text won't enlarge it, so we have to recreate it
                        _segment = new RenameSegment(_textArea.Document, offset, text.Length);
                    }
                }
            }
        }

        public void Deactivate()
        {
            _textArea.TextView.BackgroundRenderers.Remove(_background);

            if (_target != null)
            {
                _target = null;
            }

            TextDocumentWeakEventManager.TextChanged.RemoveHandler(_textArea.Document, OnDocumentTextChanged);
        }
    }

    internal class Renderer : IBackgroundRenderer
    {
        private static readonly IBrush BackgroundBrush = CreateBackgroundBrush();
        private static readonly Pen ActiveBorderPen = CreateBorderPen();

        private static IBrush CreateBackgroundBrush()
        {
            var b = new SolidColorBrush(Colors.LimeGreen) { Opacity = 0.4 };
            return b;
        }

        private static Pen CreateBorderPen()
        {
            var p = new Pen(Brushes.WhiteSmoke, dashStyle: DashStyle.Dot);
            return p;
        }

        internal RenamingTextElement Element;

        public KnownLayer Layer => KnownLayer.Background;

        public void Draw(TextView textView, DrawingContext drawingContext)
        {
            var s = Element.Segment;

            if (s != null)
            {
                var geoBuilder = new BackgroundGeometryBuilder
                {
                    AlignToWholePixels = true,
                    BorderThickness = ActiveBorderPen?.Thickness ?? 0
                };

                geoBuilder.AddSegment(textView, s);

                var geometry = geoBuilder.CreateGeometry();

                if (geometry != null)
                {
                    drawingContext.DrawGeometry(BackgroundBrush, null, geometry);
                }
            }
        }
    }

}
