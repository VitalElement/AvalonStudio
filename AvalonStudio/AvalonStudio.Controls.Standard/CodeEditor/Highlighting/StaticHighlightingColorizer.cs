﻿using Avalonia.Media;
using Avalonia.Threading;
using AvaloniaEdit.Document;
using AvaloniaEdit.Highlighting;
using AvaloniaEdit.Rendering;
using AvalonStudio.CodeEditor;
using AvalonStudio.Extensibility.Editor;
using AvalonStudio.Languages;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;

namespace AvalonStudio.Controls.Standard.CodeEditor.Highlighting
{
    public class StaticHighlightingColorizer : GenericLineTransformer
    {
        private readonly SyntaxHighlightingDefinition _definition;
        private CodeEditor _editor;
        private SyntaxHighlighting _highlighter;
        private readonly bool _isFixedHighlighter;

        /// <summary>
        /// Creates a new HighlightingColorizer instance.
        /// </summary>
        /// <param name="definition">The highlighting definition.</param>
        public StaticHighlightingColorizer(SyntaxHighlightingDefinition definition)
        {
            _definition = definition ?? throw new ArgumentNullException(nameof(definition));
        }

        /// <summary>
        /// Creates a new HighlightingColorizer instance that uses a fixed highlighter instance.
        /// The colorizer can only be used with text views that show the document for which
        /// the highlighter was created.
        /// </summary>
        /// <param name="highlighter">The highlighter to be used.</param>
        public StaticHighlightingColorizer(SyntaxHighlighting highlighter)
        {
            _highlighter = highlighter ?? throw new ArgumentNullException(nameof(highlighter));
            _isFixedHighlighter = true;
        }

        /// <summary>
        /// Creates a new HighlightingColorizer instance.
        /// Derived classes using this constructor must override the <see cref="CreateHighlighter"/> method.
        /// </summary>
        protected StaticHighlightingColorizer()
        {
        }

        private void TextView_DocumentChanged(object sender, EventArgs e)
        {
            var textView = (CodeEditor)sender;
            DeregisterServices(textView);
            RegisterServices(textView);
        }

        /// <summary>
        /// This method is called when a text view is removed from this HighlightingColorizer,
        /// and also when the TextDocument on any associated text view changes.
        /// </summary>
        protected virtual void DeregisterServices(CodeEditor editor)
        {
            if (_highlighter != null)
            {
                if (_isInHighlightingGroup)
                {
                    //_highlighter.EndHighlighting();
                    _isInHighlightingGroup = false;
                }
                //_highlighter.HighlightingStateChanged -= OnHighlightStateChanged;
                // remove highlighter if it is registered
                if (editor.TextArea.TextView.Services.GetService(typeof(IHighlighter)) == _highlighter)
                {
                    editor.TextArea.TextView.Services.RemoveService(typeof(IHighlighter));
                }
                if (!_isFixedHighlighter)
                {
                    _highlighter?.Dispose();
                    _highlighter = null;
                }
            }
        }

        /// <summary>
        /// This method is called when a new text view is added to this HighlightingColorizer,
        /// and also when the TextDocument on any associated text view changes.
        /// </summary>
        protected virtual void RegisterServices(CodeEditor editor)
        {
            if (editor.Document != null)
            {
                if (!_isFixedHighlighter)
                    _highlighter = editor.DocumentAccessor.Document != null ? CreateHighlighter(editor.TextArea.TextView, editor.Document) : null;
                if (_highlighter != null && _highlighter.Document == editor.DocumentAccessor.Document)
                {
                    //add service only if it doesn't already exist
                    if (editor.TextArea.TextView.Services.GetService(typeof(IHighlighter)) == null)
                    {
                        editor.TextArea.TextView.Services.AddService(typeof(IHighlighter), _highlighter);
                    }

                    //_highlighter.HighlightingStateChanged += OnHighlightStateChanged;
                }
            }
        }

        /// <summary>
        /// Creates the IHighlighter instance for the specified text document.
        /// </summary>
        protected virtual SyntaxHighlighting CreateHighlighter(TextView textView, IDocument document)
        {
            if (_definition != null)
                return new SyntaxHighlighting(_definition, document);
            throw new NotSupportedException("Cannot create a highlighter because no IHighlightingDefinition was specified, and the CreateHighlighter() method was not overridden.");
        }

        /// <inheritdoc/>
        public void OnAddToEditor(CodeEditor editor)
        {
            if (_editor != null)
            {
                throw new InvalidOperationException("Cannot use a HighlightingColorizer instance in multiple text views. Please create a separate instance for each text view.");
            }

            _editor = editor;
            editor.DocumentChanged += TextView_DocumentChanged;
            editor.TextArea.TextView.VisualLineConstructionStarting += TextView_VisualLineConstructionStarting;
            editor.TextArea.TextView.VisualLinesChanged += TextView_VisualLinesChanged;
            RegisterServices(editor);
        }

        /// <inheritdoc/>
        public void OnRemoveFromEditor(CodeEditor editor)
        {
            DeregisterServices(editor);
            editor.DocumentChanged -= TextView_DocumentChanged;
            editor.TextArea.TextView.VisualLineConstructionStarting -= TextView_VisualLineConstructionStarting;
            editor.TextArea.TextView.VisualLinesChanged -= TextView_VisualLinesChanged;
            _editor = null;
        }

        private bool _isInHighlightingGroup;

        private void TextView_VisualLineConstructionStarting(object sender, VisualLineConstructionStartEventArgs e)
        {
            if (_highlighter != null)
            {
                // Force update of highlighting state up to the position where we start generating visual lines.
                // This is necessary in case the document gets modified above the FirstLineInView so that the highlighting state changes.
                // We need to detect this case and issue a redraw (through OnHighlightStateChanged)
                // before the visual line construction reuses existing lines that were built using the invalid highlighting state.
                _lineNumberBeingColorized = e.FirstLineInView.LineNumber - 1;
                if (!_isInHighlightingGroup)
                {
                    // avoid opening group twice if there was an exception during the previous visual line construction
                    // (not ideal, but better than throwing InvalidOperationException "group already open"
                    // without any way of recovering)
                    //_highlighter.BeginHighlighting();
                    _isInHighlightingGroup = true;
                }
                //_highlighter.UpdateHighlightingState(_lineNumberBeingColorized);
                _lineNumberBeingColorized = 0;
            }
        }

        private void TextView_VisualLinesChanged(object sender, EventArgs e)
        {
            if (_highlighter != null && _isInHighlightingGroup)
            {
                //_highlighter.EndHighlighting();
                _isInHighlightingGroup = false;
            }
        }

        private AvaloniaEdit.Document.DocumentLine _lastColorizedLine;

        /// <inheritdoc/>
        protected override void Colorize(ITextRunConstructionContext context)
        {
            _lastColorizedLine = null;
            base.Colorize(context);
            if (_lastColorizedLine != context.VisualLine.LastDocumentLine)
            {
                if (_highlighter != null)
                {
                    // In some cases, it is possible that we didn't highlight the last document line within the visual line
                    // (e.g. when the line ends with a fold marker).
                    // But even if we didn't highlight it, we'll have to update the highlighting state for it so that the
                    // proof inside TextViewDocumentHighlighter.OnHighlightStateChanged holds.
                    _lineNumberBeingColorized = context.VisualLine.LastDocumentLine.LineNumber;

                    var highlightedLine = _highlighter.GetHighlightedLineAsync(context.Document.GetLineByNumber(_lineNumberBeingColorized), CancellationToken.None).Result;

                    _lineNumberBeingColorized = 0;
                }
            }
            _lastColorizedLine = null;
        }

        private int _lineNumberBeingColorized;

        public IBrush GetBrush(ColorScheme ColorScheme, HighlightType type)
        {
            IBrush result;

            switch (type)
            {
                case HighlightType.DelegateName:
                    result = ColorScheme.DelegateName;
                    break;

                case HighlightType.Comment:
                    result = ColorScheme.Comment;
                    break;

                case HighlightType.Identifier:
                    result = ColorScheme.Identifier;
                    break;

                case HighlightType.Keyword:
                    result = ColorScheme.Keyword;
                    break;

                case HighlightType.Literal:
                    result = ColorScheme.Literal;
                    break;

                case HighlightType.NumericLiteral:
                    result = ColorScheme.NumericLiteral;
                    break;

                case HighlightType.Punctuation:
                    result = ColorScheme.Punctuation;
                    break;

                case HighlightType.InterfaceName:
                    result = ColorScheme.InterfaceType;
                    break;

                case HighlightType.ClassName:
                    result = ColorScheme.Type;
                    break;

                case HighlightType.CallExpression:
                    result = ColorScheme.CallExpression;
                    break;

                case HighlightType.EnumTypeName:
                    result = ColorScheme.EnumType;
                    break;

                case HighlightType.Operator:
                    result = ColorScheme.Operator;
                    break;

                case HighlightType.StructName:
                    result = ColorScheme.StructName;
                    break;

                default:
                    result = Brushes.Red;
                    break;
            }

            return result;
        }

        /// <summary>
        /// Applies a highlighting color to a visual line element.
        /// </summary>
        protected virtual void ApplyColorToElement(VisualLineElement element, HighlightingColor color)
        {
            ApplyColorToElement(element, color, CurrentContext);
        }

        internal static void ApplyColorToElement(VisualLineElement element, HighlightingColor color, ITextRunConstructionContext context)
        {
            if (color.Foreground != null)
            {
                var b = color.Foreground.GetBrush(context);
                if (b != null)
                    element.TextRunProperties.ForegroundBrush = b;
            }
            if (color.Background != null)
            {
                var b = color.Background.GetBrush(context);
                if (b != null)
                    element.BackgroundBrush = b;
            }
            if (color.FontStyle != null || color.FontWeight != null)
            {
                var tf = element.TextRunProperties.Typeface;
                element.TextRunProperties.Typeface = tf;
            }
            //if(color.Underline ?? false)
            //	element.TextRunProperties.SetTextDecorations(TextDecorations.Underline);
        }

        /// <summary>
        /// This method is responsible for telling the TextView to redraw lines when the highlighting state has changed.
        /// </summary>
        /// <remarks>
        /// Creation of a VisualLine triggers the syntax highlighter (which works on-demand), so it says:
        /// Hey, the user typed "/*". Don't just recreate that line, but also the next one
        /// because my highlighting state (at end of line) changed!
        /// </remarks>
        private void OnHighlightStateChanged(int fromLineNumber, int toLineNumber)
        {
            if (_lineNumberBeingColorized != 0)
            {
                // Ignore notifications for any line except the one we're interested in.
                // This improves the performance as Redraw() can take quite some time when called repeatedly
                // while scanning the document (above the visible area) for highlighting changes.
                if (toLineNumber <= _lineNumberBeingColorized)
                {
                    return;
                }
            }

            // The user may have inserted "/*" into the current line, and so far only that line got redrawn.
            // So when the highlighting state is changed, we issue a redraw for the line immediately below.
            // If the highlighting state change applies to the lines below, too, the construction of each line
            // will invalidate the next line, and the construction pass will regenerate all lines.

            Debug.WriteLine("OnHighlightStateChanged forces redraw of lines {0} to {1}", fromLineNumber, toLineNumber);

            // If the VisualLine construction is in progress, we have to avoid sending redraw commands for
            // anything above the line currently being constructed.
            // It takes some explanation to see why this cannot happen.
            // VisualLines always get constructed from top to bottom.
            // Each VisualLine construction calls into the highlighter and thus forces an update of the
            // highlighting state for all lines up to the one being constructed.

            // To guarantee that we don't redraw lines we just constructed, we need to show that when
            // a VisualLine is being reused, the highlighting state at that location is still up-to-date.

            // This isn't exactly trivial and the initial implementation was incorrect in the presence of external document changes
            // (e.g. split view).

            // For the first line in the view, the TextView.VisualLineConstructionStarting event is used to check that the
            // highlighting state is up-to-date. If it isn't, this method will be executed, and it'll mark the first line
            // in the view as requiring a redraw. This is safely possible because that event occurs before any lines are reused.

            // Once we take care of the first visual line, we won't get in trouble with other lines due to the top-to-bottom
            // construction process.

            // We'll prove that: if line N is being reused, then the highlighting state is up-to-date until (end of) line N-1.

            // Start of induction: the first line in view is reused only if the highlighting state was up-to-date
            // until line N-1 (no change detected in VisualLineConstructionStarting event).

            // Induction step:
            // If another line N+1 is being reused, then either
            //     a) the previous line (the visual line containing document line N) was newly constructed
            // or  b) the previous line was reused
            // In case a, the construction updated the highlighting state. This means the stack at end of line N is up-to-date.
            // In case b, the highlighting state at N-1 was up-to-date, and the text of line N was not changed.
            //   (if the text was changed, the line could not have been reused).
            // From this follows that the highlighting state at N is still up-to-date.

            // The above proof holds even in the presence of folding: folding only ever hides text in the middle of a visual line.
            // Our Colorize-override ensures that the highlighting state is always updated for the LastDocumentLine,
            // so it will always invalidate the next visual line when a folded line is constructed
            // and the highlighting stack has changed.

            if (fromLineNumber == toLineNumber)
            {
                _editor.TextArea.TextView.Redraw(_editor.Document.GetLineByNumber(fromLineNumber));
            }
            else
            {
                // If there are multiple lines marked as changed; only the first one really matters
                // for the highlighting during rendering.
                // However this callback is also called outside of the rendering process, e.g. when a highlighter
                // decides to re-highlight some section based on external feedback (e.g. semantic highlighting).
                var fromLine = _editor.Document.GetLineByNumber(fromLineNumber);
                var toLine = _editor.Document.GetLineByNumber(toLineNumber);
                var startOffset = fromLine.Offset;
                _editor.TextArea.TextView.Redraw(startOffset, toLine.EndOffset - startOffset);
            }

            /*
			 * Meta-comment: "why does this have to be so complicated?"
			 * 
			 * The problem is that I want to re-highlight only on-demand and incrementally;
			 * and at the same time only repaint changed lines.
			 * So the highlighter and the VisualLine construction both have to run in a single pass.
			 * The highlighter must take care that it never touches already constructed visual lines;
			 * if it detects that something must be redrawn because the highlighting state changed,
			 * it must do so early enough in the construction process.
			 * But doing it too early means it doesn't have the information necessary to re-highlight and redraw only the desired parts.
			 */
        }

        protected override void TransformLine(AvaloniaEdit.Document.DocumentLine line, ITextRunConstructionContext context)
        {
            if (_highlighter != null)
            {
                var lineText = context.Document.GetText(line);

                _lineNumberBeingColorized = line.LineNumber;

                var hl = _highlighter.GetHighlightedLineAsync(line, CancellationToken.None).Result;
                _lineNumberBeingColorized = 0;

                foreach (var section in hl.Segments)
                {
                    var colorKey = section.ColorStyleKey;

                    var levels = colorKey.Split('.');

                    var brush = ColorScheme.CurrentColorScheme[colorKey];

                    if (brush != null)
                    {
                        SetTextStyle(line, section.Offset, section.Length, brush);
                    }
                    else
                    {
                        System.Console.WriteLine($"{colorKey}:{lineText}");
                    }
                }
            }
            _lastColorizedLine = line;
        }
    }
}
