using Perspex.Controls;
using Perspex.Input;
using Perspex.Markup.Xaml;

namespace AvalonStudio.Controls
{
    public class Editor : UserControl
    {
        public Editor()
        {
            this.InitializeComponent();

            AddHandler(InputElement.KeyDownEvent, OnKeyDown, Perspex.Interactivity.RoutingStrategies.Tunnel);
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
    }
}
