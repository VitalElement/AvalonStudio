using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using System.Reactive.Disposables;

namespace AvalonStudio.Controls.Standard.CodeEditor
{
    public class Intellisense : UserControl
    {
        private readonly CompositeDisposable disposables;

        public Intellisense()
        {
            disposables = new CompositeDisposable();

            InitializeComponent();
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
            AvaloniaXamlLoader.Load(this);
        }
    }
}