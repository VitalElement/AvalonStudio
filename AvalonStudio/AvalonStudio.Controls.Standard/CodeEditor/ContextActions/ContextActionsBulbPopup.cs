// Copyright (c) 2014 AlphaSierraPapa for the SharpDevelop Team
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy of this
// software and associated documentation files (the "Software"), to deal in the Software
// without restriction, including without limitation the rights to use, copy, modify, merge,
// publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons
// to whom the Software is furnished to do so, subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in all copies or
// substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED,
// INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR
// PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE
// FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR
// OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER
// DEALINGS IN THE SOFTWARE.

using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Markup;
using Avalonia.Markup.Xaml.Data;
using Avalonia.Media;
using Avalonia.Media.Imaging;
using Avalonia.Styling;
using System;
using System.Globalization;
using System.Linq;
using System.Windows.Input;

namespace AvalonStudio.Controls.Standard.CodeEditor.ContextActions
{
    internal sealed class ContextActionsBulbPopup : ExtendedPopup
    {
        private readonly MenuItem _mainItem;
        private readonly Image _headerImage;
        private bool _isOpen;

        public ContextActionsBulbPopup(Control parent) : base(parent)
        {
            UseLayoutRounding = true;

            StaysOpen = true;

            _headerImage = new Image();

            _mainItem = new MenuItem
            {
                Styles = { CreateItemContainerStyle() },
                Header = new Grid { Height=20, Width=20, Background=Brushes.Gold }
                //_headerImage
            };

            _mainItem.SubmenuOpened += (sender, args) =>
            {
                if (ReferenceEquals(sender, _mainItem))
                {
                    _isOpen = true;
                    MenuOpened?.Invoke(this, EventArgs.Empty);
                }
            };

            Closed += (sender, args) =>
            {
                if (_isOpen)
                {
                    _isOpen = false;
                    MenuClosed?.Invoke(this, EventArgs.Empty);
                }
            };

            var menu = new Menu
            {
                Background = Brushes.Transparent,
                BorderBrush = _mainItem.BorderBrush,
                BorderThickness = _mainItem.BorderThickness,
                Items = new[] { _mainItem }
            };            
            
            Child = new Grid { Height = 20, Width = 20, Background = Brushes.Red };
        }

        public IBitmap Icon
        {
            get => _headerImage.Source;
            set => _headerImage.Source = value;
        }

        public event EventHandler MenuOpened;
        public event EventHandler MenuClosed;

        private Style CreateItemContainerStyle()
        {
            var style = new Style(c => c.OfType<MenuItem>())
            {
                Setters = new[]
                {
                    new Setter(MenuItem.CommandProperty,
                        new Binding { Converter = new ActionCommandConverter(this) })
                }
            };
            ;
            return style;
        }

        public System.Collections.IEnumerable ItemsSource
        {
            get => _mainItem.Items;
            set => _mainItem.Items = value;
        }

        public bool HasItems => _mainItem.Items.Cast<object>().Any();

        protected override void OnKeyDown(KeyEventArgs e)
        {
            base.OnKeyDown(e);
            if (e.Key == Key.Escape)
                IsOpenIfFocused = false;
        }

        public new void Close()
        {
            IsOpenIfFocused = false;
        }

        public bool IsMenuOpen
        {
            get => _mainItem.IsSubMenuOpen;
            set => _mainItem.IsSubMenuOpen = value;
        }

        public Func<object, ICommand> CommandProvider { get; set; }

        public new void Focus()
        {
            _mainItem.Focus();
        }

        public void OpenAtLineStart(AvaloniaEdit.TextEditor editor)
        {
            SetPosition(editor, editor.TextArea.Caret.Line, 1);
            IsOpenIfFocused = true;
        }

        private void SetPosition(AvaloniaEdit.TextEditor editor, int line, int column, bool openAtWordStart = false)
        {
            var document = editor.Document;

            if (openAtWordStart)
            {
                var offset = document.GetOffset(line, column);
                var wordStart = document.FindPreviousWordStart(offset);
                if (wordStart != -1)
                {
                    var wordStartLocation = document.GetLocation(wordStart);
                    line = wordStartLocation.Line;
                    column = wordStartLocation.Column;
                }
            }

            var caretScreenPos = editor.TextArea.TextView.GetPosition(line, column);
            var visualLine = editor.TextArea.TextView.GetVisualLine(line);
            var height = visualLine.Height - 1;
            _headerImage.Width = _headerImage.Height = height;
            HorizontalOffset = 0;            
            PlacementTarget = editor.TextArea.TextView;
            VerticalOffset = caretScreenPos.Y - height - 1 - PlacementTarget.Bounds.Height;
            // TODO:
            //Placement = PlacementMode.Relative;
        }

        private class ActionCommandConverter : IValueConverter
        {
            private readonly ContextActionsBulbPopup _owner;

            public ActionCommandConverter(ContextActionsBulbPopup owner)
            {
                _owner = owner;
            }

            public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
            {
                return _owner.CommandProvider?.Invoke(value);
            }

            public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
            {
                throw new NotSupportedException();
            }
        }

    }
}