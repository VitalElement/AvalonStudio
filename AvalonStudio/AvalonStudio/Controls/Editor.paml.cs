namespace AvalonStudio.Controls
{
    using Perspex.Controls;
    using Perspex.Controls.Primitives;
    using Perspex.Input;
    using Perspex.Markup.Xaml;
    using AvalonStudio.TextEditor;

    public class Editor : UserControl
    {
        private TextEditor editor;

        public Editor()
        {
            this.InitializeComponent();

            AddHandler(InputElement.KeyDownEvent, OnKeyDown, Perspex.Interactivity.RoutingStrategies.Tunnel);
            editor = this.Find<TextEditor>("editor");

            DataContextChanged += (sender, e) =>
            {
                var editorVm = DataContext as WorkspaceViewModel;

                if (editorVm != null)
                {
                    editorVm.Editor.Model.Editor = editor;
                }
            };            
        }

        private void InitializeComponent()
        {
            PerspexXamlLoader.Load(this);
        }
        

        protected override void OnKeyUp (KeyEventArgs e)
        {
            var editorVm = DataContext as EditorViewModel;

            if(editorVm != null)
            {
                editorVm.OnKeyUp(e);
            }
        }

        protected void OnKeyDown(object sender, KeyEventArgs e)
        {
            var editorVm = DataContext as EditorViewModel;

            if (editorVm != null)
            {
                editorVm.OnKeyDown(e);
            }
        }        

        protected override void OnTextInput(TextInputEventArgs e)
        {
            var editorVm = DataContext as EditorViewModel;

            if (editorVm != null)
            {
                editorVm.OnTextInput(e);
            }
        }

        protected override void OnPointerMoved(PointerEventArgs e)
        {
            var editorVm = DataContext as EditorViewModel;

            if (editorVm != null)
            {
                editorVm.OnPointerMoved(e);
            }
        }
    }
}
