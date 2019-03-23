using AvalonStudio.Controls;
using AvalonStudio.Extensibility.Editor;
using System;
using System.Threading.Tasks;

namespace AvalonStudio.Shell.Behaviors
{
    public class EditorToolTipBehavior : TooltipBehavior
    {
        private AvaloniaEdit.TextEditor editor;
        private EditorViewModel editorVm;

        protected override void OnAttached()
        {
            editor = AssociatedObject as AvaloniaEdit.TextEditor;

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

        public override Task<bool> OnBeforePopupOpen()
        {
            var result = false;

            if (editorVm != null)
            {
               /* result = await editorVm.UpdateToolTipAsync(editor.TextArea.TextView.GetOffsetFromPoint(MouseDevice.Instance.GetPosition(editor.TextView.TextSurface)));*/
            }

            return Task.FromResult(result);
        }
    }
}