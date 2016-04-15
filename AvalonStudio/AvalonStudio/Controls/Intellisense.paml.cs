using Perspex.Controls;
using Perspex;

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
            this.LoadFromXaml();
        }

        
    }
}
