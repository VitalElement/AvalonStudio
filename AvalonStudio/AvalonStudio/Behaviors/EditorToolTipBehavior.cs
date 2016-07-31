using System;
using System.Threading.Tasks;
using Avalonia.Input;
using AvalonStudio.Controls;

namespace AvalonStudio.Behaviors
{
    public class EditorToolTipBehavior : TooltipBehavior
    {
        private TextEditor.TextEditor editor;
        private EditorViewModel editorVm;

        protected override void OnAttached()
        {
            editor = AssociatedObject as TextEditor.TextEditor;

            if (editor != null)
            {
                editor.DataContextChanged += Editor_DataContextChanged;
            }

            base.OnAttached();
        }

        private void Editor_DataContextChanged(object sender, EventArgs e)
        {
            editorVm = editor.DataContext as EditorViewModel;
        }

        protected override void OnDetaching()
        {
            editorVm = null;

            if (editor != null)
            {
                editor.DataContextChanged -= Editor_DataContextChanged;

                editor = null;
            }

            base.OnDetaching();
        }

        public override async Task<bool> OnBeforePopupOpen()
        {
            var result = false;

            if (editorVm != null)
            {
                result = await editorVm.UpdateToolTipAsync(editor.TextView.GetOffsetFromPoint(MouseDevice.Instance.GetPosition(editor.TextView.TextSurface)));
            }

            return result;
        }
    }
}