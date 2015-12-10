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
                editorVm.OnKeyDowm(e);
            }
        }
    }
}
