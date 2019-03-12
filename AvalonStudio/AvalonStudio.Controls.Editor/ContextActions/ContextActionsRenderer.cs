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

using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Media.Imaging;
using Avalonia.Threading;
using AvalonStudio.Extensibility.Editor;
using AvalonStudio.Languages;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;

namespace AvalonStudio.Controls.Editor.ContextActions
{
    public sealed class ContextActionsRenderer : ContextActionsMargin, IDisposable
    {
        private const int DelayMoveMilliseconds = 500;

        private readonly ObservableCollection<IContextActionProvider> _providers;
        private readonly CodeEditor _editor;
        private readonly TextMarkerService _textMarkerService;

        private bool _popupOpened;
        private ContextActionsBulbPopup _popup;
        private CancellationTokenSource _cancellationTokenSource;
        private IEnumerable<object> _actions;
        private Subject<Unit> _diagnosticsChanged;

        public ContextActionsRenderer(CodeEditor editor, TextMarkerService textMarkerService) : base(editor)
        {
            _editor = editor ?? throw new ArgumentNullException(nameof(editor));
            _textMarkerService = textMarkerService;

            editor.KeyDown += ContextActionsRenderer_KeyDown;
            _providers = new ObservableCollection<IContextActionProvider>();
            _providers.CollectionChanged += Providers_CollectionChanged;

            editor.TextArea.TextView.ScrollOffsetChanged += TextView_ScrollOffsetChanged; ;

            editor.HookupLoadedUnloadedAction(HookupWindowMove);

            var positionChanged = Observable.FromEventPattern(editor.TextArea.Caret, nameof(editor.TextArea.Caret.PositionChanged)).Select(o => Unit.Default);
            var textChanged = Observable.FromEventPattern(editor.Document, nameof(editor.Document.TextChanged)).Select(o => Unit.Default);


            _diagnosticsChanged = new Subject<Unit>();

            _diagnosticsChanged.Merge(positionChanged).Merge(textChanged).Throttle(TimeSpan.FromMilliseconds(100))
                .ObserveOn(AvaloniaScheduler.Instance).Subscribe(async e =>
                {
                    await QueryCodeActions();
                });
        }

        private void TextView_ScrollOffsetChanged(object sender, EventArgs e)
        {
            _diagnosticsChanged.OnNext(Unit.Default);
        }

        public void OnDiagnosticsUpdated()
        {
            _diagnosticsChanged.OnNext(Unit.Default);
        }

        private async Task QueryCodeActions()
        {
            await LoadActionsWithCancellationAsync();

            ClosePopup();

            // Don't show the context action popup when the caret is outside the editor boundaries
            var textView = _editor.TextArea.TextView;
            var editorRect = new Rect((Point)textView.ScrollOffset, textView.Bounds.Size);
            var caretRect = _editor.TextArea.Caret.CalculateCaretRectangle();
            if (!editorRect.Contains(caretRect))
                return;

            if (!await LoadActionsWithCancellationAsync().ConfigureAwait(true)) return;

            CreatePopup();
            _popup.ItemsSource = _actions;
            if (_popup.HasItems)
            {
                SetBulb(_editor.Line);
            }
            else
            {
                ClearBulb();
            }
        }

        protected override void OnOpenPopup()
        {
            _popup.ItemsSource = _actions;
            _popup.OpenAtLine(_editor, Line);

            _popupOpened = true;
        }

        protected override void OnClosePopup()
        {
            _popupOpened = false;
            ClosePopup();
        }

        public IBitmap IconImage { get; set; }

        private void HookupWindowMove(bool enable)
        {
            var window = _editor.FindAncestorByType<Window>();
            if (window != null)
            {
                window.DetachLocationChanged(WindowOnLocationChanged);
                if (enable)
                {
                    window.AttachLocationChanged(WindowOnLocationChanged);
                }
            }
        }

        private void WindowOnLocationChanged(object sender, EventArgs eventArgs)
        {
            if (_popup?.IsOpen == true)
            {
                _popup.HorizontalOffset += double.Epsilon;
                _popup.HorizontalOffset -= double.Epsilon;
            }
        }

        public void Dispose()
        {
            var window = _editor.FindAncestorByType<Window>();
            if (window != null)
            {
                window.DetachLocationChanged(WindowOnLocationChanged);
            }

            ClosePopup();
        }

        public IList<IContextActionProvider> Providers => _providers;

        private void Providers_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            _diagnosticsChanged.OnNext(Unit.Default);
        }

        private async void ContextActionsRenderer_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key != Key.OemPeriod ||
                e.Modifiers != InputModifiers.Control
                ) return;

            if (Line <= 0)
            {
                await QueryCodeActions();
            }

            if (Line <= 0)
            {
                return;
            }

            CreatePopup();
            if (_popup.IsOpen && _popup.ItemsSource != null)
            {
                _popup.IsMenuOpen = true;
            }
            else
            {
                _popup.ItemsSource = _actions;

                if (_popup.HasItems)
                {
                    _popup.OpenAtLine(_editor, Line);
                    _popup.IsMenuOpen = true;
                }
            }
        }

        private void CreatePopup()
        {
            if (_popup == null)
            {
                _popup = new ContextActionsBulbPopup(_editor.TextArea, this) { CommandProvider = GetActionCommand, Icon = IconImage, Height = 20, Width = 20 };

                _popup.MenuClosed += (sender, args) =>
                {
                    Dispatcher.UIThread.InvokeAsync(() =>
                    {
                        ClearBulb();
                        _editor.Focus();
                        _diagnosticsChanged.OnNext(Unit.Default);
                    }, DispatcherPriority.Background);
                };
            }
        }

        private async Task<bool> LoadActionsWithCancellationAsync()
        {
            _cancellationTokenSource = new CancellationTokenSource();
            try
            {
                _actions = await LoadActionsAsync(_cancellationTokenSource.Token).ConfigureAwait(false);
                return true;
            }
            catch (Exception)
            {
                // ignored
            }
            _cancellationTokenSource = null;
            return false;
        }

        private ICommand GetActionCommand(object action)
        {
            return _providers.Select(provider => provider.GetActionCommand(action))
                .FirstOrDefault(command => command != null);
        }

        private async Task<IEnumerable<object>> LoadActionsAsync(CancellationToken cancellationToken)
        {
            var allActions = new List<object>();
            foreach (var provider in _providers)
            {
                var offset = _editor.TextArea.Caret.Offset;

                var line = _editor.Document.GetLineByOffset(offset);

                offset = line.Offset;
                var length = line.Length;

                var actions = await provider.GetCodeFixes(_editor.Editor, offset, length, cancellationToken).ConfigureAwait(true);
                allActions.AddRange(actions);
            }
            return allActions;
        }

        private void ClosePopup()
        {
            if (_popupOpened && _cancellationTokenSource != null)
            {
                _cancellationTokenSource.Cancel();
                _cancellationTokenSource = null;
            }

            if (_popup != null)
            {
                _popup.Close();
                _popup.IsMenuOpen = false;
                _popup.ItemsSource = null;
            }
        }
    }
}
