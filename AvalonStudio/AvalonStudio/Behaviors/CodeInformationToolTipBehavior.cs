namespace AvalonStudio.Behaviors
{
    using AvalonStudio.Controls;
    using AvalonStudio.TextEditor;

    public class CodeInformationToolTipBehavior : PopupBehavior
    {
        private EditorViewModel editorVm;
        private TextEditor editor;

        protected override void OnAttached()
        {
            base.OnAttached();

            editor = AssociatedObject as TextEditor;

            if (editor != null)
            {
                editor.DataContextChanged += Editor_DataContextChanged;                
            }
        }

        private void Editor_DataContextChanged(object sender, System.EventArgs e)
        {
            editorVm = editor.DataContext as EditorViewModel;
        }

        protected override void OnDetaching()
        {
            if(editor != null)
            {
                editor.DataContextChanged -= Editor_DataContextChanged;
            }
        }

        public override bool OnBeforePopupOpen()
        {
            bool result = false;

            if (editorVm != null)
            {
                result = editorVm.UpdateHoverProbe(editor.TextView.GetOffsetFromPoint(lastPoint));
            }

            return result;
        }
    }
}
