using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Threading.Tasks;
using Avalonia.Media;
using Avalonia.Threading;
using AvalonStudio.MVVM;
using ReactiveUI;

namespace AvalonStudio.Controls
{
	public class DocumentTabsViewModel : ViewModel
	{
		private IBrush backgroundBrush;

		private ObservableCollection<EditorViewModel> documents;

		private IBrush hoverTabBackgroundBrush;

		private EditorViewModel selectedDocument;

		private IBrush tabBackgroundBrush;
		private IBrush tabBrush;
		private readonly IBrush tabHighlightBrush;

		private readonly IBrush temporaryTabBrush;

		private readonly IBrush temporaryTabHighlighBrush;

		public DocumentTabsViewModel()
		{
			Documents = new ObservableCollection<EditorViewModel>();
			Documents.CollectionChanged += (sender, e) =>
			{
				if (e.Action == NotifyCollectionChangedAction.Remove)
				{
					Dispatcher.UIThread.InvokeAsync(async () =>
					{
						await Task.Delay(25);
						GC.Collect();
					});
				}
			};

			tabBrush = Brush.Parse("#007ACC");
			tabHighlightBrush = Brush.Parse("#1c97ea");

			temporaryTabBrush = Brush.Parse("#68217A");
			temporaryTabHighlighBrush = Brush.Parse("#B064AB");
		}

		public ObservableCollection<EditorViewModel> Documents
		{
			get { return documents; }
			set { this.RaiseAndSetIfChanged(ref documents, value); }
		}

		public EditorViewModel SelectedDocument
		{
			get { return selectedDocument; }
			set
			{
				this.RaiseAndSetIfChanged(ref selectedDocument, value);

				// Dispatcher invoke is hack to make sure the Editor propery has been generated.
				Dispatcher.UIThread.InvokeAsync(() => { value?.Model.Editor?.Focus(); });

				if (value == TemporaryDocument)
				{
					TabBackgroundBrush = temporaryTabBrush;
					HoverTabBackgroundBrush = temporaryTabHighlighBrush;
				}
				else
				{
					TabBackgroundBrush = tabBackgroundBrush;
					HoverTabBackgroundBrush = tabHighlightBrush;
				}
			}
		}

		public EditorViewModel TemporaryDocument { get; set; }

		public IBrush TabBackgroundBrush
		{
			get { return tabBackgroundBrush; }
			set { this.RaiseAndSetIfChanged(ref tabBackgroundBrush, value); }
		}

		public IBrush HoverTabBackgroundBrush
		{
			get { return hoverTabBackgroundBrush; }
			set { this.RaiseAndSetIfChanged(ref hoverTabBackgroundBrush, value); }
		}
	}
}