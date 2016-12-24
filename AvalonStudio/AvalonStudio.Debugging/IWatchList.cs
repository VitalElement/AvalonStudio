namespace AvalonStudio.Debugging
{
	public interface IWatchList
	{
		void AddWatch(string expression);
		void RemoveWatch(WatchViewModel watch);
	}
}