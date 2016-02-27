using Perspex.Controls;
using Perspex.Markup.Xaml;

namespace AvalonStudio.Controls
{
    public class Intellisense : UserControl
    {
        public Intellisense()
        {
            this.InitializeComponent();

            RequestBringIntoViewEvent.AddClassHandler<Intellisense>(i => OnRequesteBringIntoView);
        }                   

        private void OnRequesteBringIntoView(RequestBringIntoViewEventArgs e)
        {
            e.Handled = true;
        }

        private void InitializeComponent()
        {
            PerspexXamlLoader.Load(this);
        }

        
    }
}
