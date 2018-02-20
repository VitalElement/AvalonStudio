using System.Collections.Concurrent;
using System.Linq;
using System.Threading.Tasks;

namespace AvalonStudio.Projects.OmniSharp.Toolchain
{
    class BuildQueue
    {
        BlockingCollection<(IProject project, TaskCompletionSource<(bool result, IProject project)> completionSource)> _tasks;

        public BuildQueue()
        {
            _tasks = new BlockingCollection<(IProject project, TaskCompletionSource<(bool result, IProject project)> completionSource)>();   
        }

        public bool Contains(IProject project)
        {
            return _tasks.Select(t => t.project).Contains(project);
        }

        public Task<(bool result, IProject project)> BuildAsync(IProject project)
        {
            var tcs = new TaskCompletionSource<(bool result, IProject project)>();

            _tasks.Add((project, tcs));

            return tcs.Task;
        }

        public int Count => _tasks.Count;

        internal (IProject project, TaskCompletionSource<(bool result, IProject project)> completionSource) GetNextItem() => _tasks.Take();
    }
}
