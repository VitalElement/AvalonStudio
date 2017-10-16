using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Data;
using Avalonia.Input;
using Avalonia.LogicalTree;
using Avalonia.Metadata;
using Avalonia.Threading;
using System;

namespace AvalonStudio.Controls
{
    public class EditableTextBlock : TemplatedControl
    {
        private string _text;
        private string _editText;
        private TextBox _textBox;

        public EditableTextBlock()
        {
            this.GetObservable(TextProperty).Subscribe(t =>
            {
                if (!InEditMode)
                {
                    EditText = t;
                }
            });
        }

        public static readonly DirectProperty<EditableTextBlock, string> TextProperty = TextBlock.TextProperty.AddOwner<EditableTextBlock>(
                o => o.Text,
                (o, v) => o.Text = v,
                defaultBindingMode: BindingMode.TwoWay,
                enableDataValidation: true);

        [Content]
        public string Text
        {
            get { return _text; }
            set
            {
                SetAndRaise(TextProperty, ref _text, value);
            }
        }

        public string EditText
        {
            get => _editText;
            set
            {
                SetAndRaise(EditTextProperty, ref _editText, value);
            }
        }

        public static readonly DirectProperty<EditableTextBlock, string> EditTextProperty =
                AvaloniaProperty.RegisterDirect<EditableTextBlock, string>(nameof(EditText), o => o.EditText, (o, v) => o.EditText = v);

        public static readonly StyledProperty<bool> InEditModeProperty =
            AvaloniaProperty.Register<EditableTextBlock, bool>(nameof(InEditMode), defaultBindingMode: BindingMode.TwoWay);

        public bool InEditMode
        {
            get { return GetValue(InEditModeProperty); }
            set { SetValue(InEditModeProperty, value); }
        }

        protected override void OnTemplateApplied(TemplateAppliedEventArgs e)
        {
            base.OnTemplateApplied(e);

            _textBox = e.NameScope.Find<TextBox>("PART_TextBox");
        }

        protected override void OnDetachedFromLogicalTree(LogicalTreeAttachmentEventArgs e)
        {
            base.OnDetachedFromLogicalTree(e);
        }

        protected override void OnKeyUp(KeyEventArgs e)
        {
            if (e.Key == Key.Enter || e.Key == Key.Escape)
            {
                ExitEditMode();
            }

            base.OnKeyUp(e);
        }

        private void EnterEditMode()
        {
            EditText = Text;
            InEditMode = true;
            (VisualRoot as IInputRoot).MouseDevice.Capture(_textBox);
            _textBox.CaretIndex = Text.Length;

            Dispatcher.UIThread.InvokeAsync(() =>
            {
                _textBox.Focus();
            });
        }

        private void ExitEditMode()
        {
            Text = EditText;

            InEditMode = false;
            (VisualRoot as IInputRoot).MouseDevice.Capture(null);
        }

        protected override void OnPointerPressed(PointerPressedEventArgs e)
        {
            base.OnPointerPressed(e);

            if (!InEditMode)
            {
                if (e.MouseButton == MouseButton.Middle)
                {
                    EnterEditMode();
                }
                else if (e.ClickCount == 2)
                {
                    EnterEditMode();
                }
            }
            else
            {
                var hit = this.InputHitTest(e.GetPosition(this));

                if (hit == null)
                {
                    ExitEditMode();
                }
            }
        }
    }
}
