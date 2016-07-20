using Avalonia;
using AvalonStudio.Languages;
using AvalonStudio.MVVM;
using ReactiveUI;

namespace AvalonStudio.Controls
{
	public class CompletionHintViewModel : ViewModel, ICompletionAdviceControl
	{
		private int count;

		private bool isVisible;

		private Thickness position;

		private int selectedIndex;

		private Symbol symbol;

		public Thickness Position
		{
			get { return position; }
			set { this.RaiseAndSetIfChanged(ref position, value); }
		}

		public string SelectedAdvice
		{
			get { return symbol?.Name; }
		}

		public string ListPositionText
		{
			get { return string.Format("{0} of {1}", SelectedIndex + 1, Count); }
		}

		public bool ListPositionVisibility
		{
			get
			{
				if (Count > 1)
				{
					return true;
				}
				return false;
			}
		}

		public bool IsVisible
		{
			get { return isVisible; }
			set { this.RaiseAndSetIfChanged(ref isVisible, value); }
		}

		public Symbol Symbol
		{
			get { return symbol; }
			set
			{
				this.RaiseAndSetIfChanged(ref symbol, value);
				this.RaisePropertyChanged(nameof(ListPositionText));
				this.RaisePropertyChanged(nameof(ListPositionVisibility));
				this.RaisePropertyChanged(nameof(SelectedAdvice));
			}
		}

		public int SelectedIndex
		{
			get { return selectedIndex; }
			set { this.RaiseAndSetIfChanged(ref selectedIndex, value); }
		}

		public int Count
		{
			get { return count; }
			set { this.RaiseAndSetIfChanged(ref count, value); }
		}
	}
}