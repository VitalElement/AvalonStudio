using Mono.Debugging.Client;

namespace AvalonStudio.Debugging
{
    public interface IWatchList
    {
        bool AddWatch(string expression);

        void Add(ObjectValue value);

        void Remove(ObjectValue value);
    }
}