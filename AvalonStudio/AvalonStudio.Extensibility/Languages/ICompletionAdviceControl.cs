namespace AvalonStudio.Languages
{
	public interface ICompletionAdviceControl
	{
		bool IsVisible { get; set; }

		Symbol Symbol { get; set; }

		int SelectedIndex { get; set; }

		int Count { get; set; }
	}
}