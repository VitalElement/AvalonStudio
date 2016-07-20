namespace AvalonStudio.Extensibility.ToolBars
{
	using MVVM;

	public interface IToolBars
	{
		IObservableCollection<IToolBar> Items { get; }
		bool Visible { get; set; }
	}
}