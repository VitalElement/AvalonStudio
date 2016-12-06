using System.Reactive.Disposables;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace AvalonStudio.Controls
{
	public class FindInFile : UserControl
	{
		private readonly CompositeDisposable disposables;

		public FindInFile()
		{
			disposables = new CompositeDisposable();

			InitializeComponent();
		}

		protected override void OnAttachedToVisualTree(VisualTreeAttachmentEventArgs e)
		{
			disposables.Add(RequestBringIntoViewEvent.AddClassHandler<FindInFile>(i => OnRequesteBringIntoView));
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