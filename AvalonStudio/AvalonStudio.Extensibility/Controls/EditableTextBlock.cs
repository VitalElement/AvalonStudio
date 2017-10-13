using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Input;
using Avalonia.LogicalTree;
using System;

namespace AvalonStudio.Controls
{
    public class EditableTextBlock : TemplatedControl
    {
        private TextBox _textBox;

        public EditableTextBlock()
        {
            this.GetObservable(InEditModeProperty).Subscribe(editMode =>
            {

            });
        }

        public static readonly StyledProperty<string> TextProperty = AvaloniaProperty.Register<EditableTextBlock, string>(nameof(Text));

        public string Text
        {
            get { return GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }

        public static readonly StyledProperty<bool> InEditModeProperty =
            AvaloniaProperty.Register<EditableTextBlock, bool>(nameof(InEditMode));

        public bool InEditMode
        {
            get { return GetValue(InEditModeProperty); }
            set { SetValue(InEditModeProperty, value); }
        }

        protected override void OnTemplateApplied(TemplateAppliedEventArgs e)
        {
            base.OnTemplateApplied(e);

            _textBox = e.NameScope.Find<TextBox>("PART_TextBox");

            _textBox.KeyUp += _textBox_KeyUp;
        }

        protected override void OnAttachedToLogicalTree(LogicalTreeAttachmentEventArgs e)
        {
            base.OnAttachedToLogicalTree(e);
        }

        protected override void OnDetachedFromLogicalTree(LogicalTreeAttachmentEventArgs e)
        {
            base.OnDetachedFromLogicalTree(e);

            _textBox.KeyUp -= _textBox_KeyUp;
        }

        private void _textBox_KeyUp(object sender, KeyEventArgs e)
        {
            if(e.Key == Key.Enter)
            {
                InEditMode = false;
            }
        }

        protected override void OnPointerPressed(PointerPressedEventArgs e)
        {
            base.OnPointerPressed(e);

            if (e.MouseButton == MouseButton.Middle)
            {
                InEditMode = true;
            }
            else if (e.ClickCount == 2)
            {
                InEditMode = true;
            }
        }
    }
}
