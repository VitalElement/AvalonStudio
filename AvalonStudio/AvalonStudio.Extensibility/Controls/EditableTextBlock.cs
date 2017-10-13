using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using System;
using System.Collections.Generic;
using System.Text;

namespace AvalonStudio.Extensibility.Controls
{
    public class EditableTextBlock : TextBlock
    {
        public EditableTextBlock()
        {
            this.GetObservable(InEditModeProperty).Subscribe(editMode =>
            {

            });
        }

        public static readonly StyledProperty<bool> InEditModeProperty =
            AvaloniaProperty.Register<EditableTextBlock, bool>(nameof(InEditMode));

        public bool InEditMode
        {
            get { return GetValue(InEditModeProperty); }
            set { SetValue(InEditModeProperty, value); }
        }

        protected override void OnPointerPressed(PointerPressedEventArgs e)
        {
            base.OnPointerPressed(e);

            if(e.MouseButton == MouseButton.Middle)
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
