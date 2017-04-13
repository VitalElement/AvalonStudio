using Mono.Debugging.Client;

namespace AvalonStudio.Debugging
{
	public interface IWatchList
	{
        void Add(ObjectValue value);
        void Remove(ObjectValue value);
	}
}