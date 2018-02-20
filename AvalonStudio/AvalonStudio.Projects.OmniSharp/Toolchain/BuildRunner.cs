using System;
using System.Threading;
using System.Threading.Tasks;

namespace AvalonStudio.Projects.OmniSharp.Toolchain
{
    class BuildRunner
    {
        private BuildQueue _queue;

        public BuildRunner()
        {
            _queue = new BuildQueue();
        }

        public BuildQueue Queue => _queue;

        public void Start (Func<IProject, Task<bool>> buildActionAsync, CancellationToken cancellationToken)
        {
            for(int i = 0; i < Environment.ProcessorCount; i++)
            {
                new Thread(() =>
                {
                    while (!cancellationToken.IsCancellationRequested)
                    {
                        var (project, completionSource) = _queue.GetNextItem();

                        completionSource.SetResult((buildActionAsync(project).GetAwaiter().GetResult(), project));
                    }
                }).Start();
            }
        }

    }
}
