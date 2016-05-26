namespace AvalonStudio.Behaviors
{
    using AvalonStudio.Controls;
    using AvalonStudio.TextEditor;
    using System.Threading.Tasks;
    public class CodeInformationToolTipBehavior : PopupBehavior
    {
        private EditorViewModel editorVm;
        private TextEditor editor;

        protected override void OnAttached()
        {
            editor = AssociatedObject as TextEditor;

            if (editor != null)
            {
                editor.DataContextChanged += Editor_DataContextChanged;                
            }

            base.OnAttached();
        }

        private void Editor_DataContextChanged(object sender, System.EventArgs e)
        {
            editorVm = editor.DataContext as EditorViewModel;
        }

        protected override void OnDetaching()
        {
            editorVm = null;

            if(editor != null)
            {
                editor.DataContextChanged -= Editor_DataContextChanged;

                editor = null;
            }

            base.OnDetaching();
        }

        public override async Task<bool> OnBeforePopupOpen()
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
