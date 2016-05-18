using Avalonia.Controls;
using Avalonia;
using System.Reactive.Disposables;

namespace AvalonStudio.Controls
{
    public class Intellisense : UserControl
    {
        private CompositeDisposable disposables;

        public Intellisense()
        {
            disposables = new CompositeDisposable();

            this.InitializeComponent();
        }

        protected override void OnAttachedToVisualTree(VisualTreeAttachmentEventArgs e)
        {
            disposables.Add(RequestBringIntoViewEvent.AddClassHandler<Intellisense>(i => OnRequesteBringIntoView));
        }

        protected override void OnDetachedFromVisualTree(VisualTreeAttachmentEventArgs e)
        {
            disposables.Dispose();
        }

        private void OnRequesteBringIntoView(RequestBringIntoViewEventArgs e)
        {
            e.Handled = true;
        }

        private void InitializeComponent()
        {
            Avalonia.Markup.Xaml.AvaloniaXamlLoader.Load(this);
        }

        
    }
}
