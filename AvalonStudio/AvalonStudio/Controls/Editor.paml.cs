namespace AvalonStudio.Controls
{
    using Perspex.Controls;
    using Perspex.Controls.Primitives;
    using Perspex.Input;
    using Perspex;
    using AvalonStudio.TextEditor;

    public class Editor : UserControl
    {
        private TextEditor editor;
        private EditorViewModel editorViewModel;

        public Editor()
        {
            this.InitializeComponent();

            AddHandler(InputElement.KeyDownEvent, OnKeyDown, Perspex.Interactivity.RoutingStrategies.Tunnel);
            AddHandler(InputElement.KeyUpEvent, OnKeyUp, Perspex.Interactivity.RoutingStrategies.Tunnel);
            editor = this.Find<TextEditor>("editor");         
            
            DataContextChanged += (sender, e) =>
            {
                if(editorViewModel != DataContext)
                {
                    editorViewModel = DataContext as EditorViewModel;

                    if (editorViewModel != null && editor != null)
                    {
                        editorViewModel.Model.Editor = editor;
                        editor.Focus();                  
                    }
                }                
            };            
        }

        private void InitializeComponent()
        {
            this.LoadFromXaml();
            
        }
        

        protected void OnKeyUp (object sender, KeyEventArgs e)
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
    }
}
