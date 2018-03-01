using AvalonStudio.Extensibility.Projects;
using AvalonStudio.Extensibility.Threading;
using AvalonStudio.Projects.OmniSharp.MSBuild;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace AvalonStudio.Projects.OmniSharp.Toolchain
{
    class BuildRunner : IDisposable
    {
        private JobRunner<(IProject project, TaskCompletionSource<(bool result, IProject project)>), bool> _runner;

        private BlockingCollection<MSBuildHost> _nodes;

        public BuildRunner()
        {
            _nodes = new BlockingCollection<MSBuildHost>(Environment.ProcessorCount);
        }

        public async Task InitialiseAsync()
        {
            var tasks = new List<Task>();

            for (int i = 0; i < Environment.ProcessorCount; i++)
            {
                var newNode = new MSBuildHost(DotNetCliService.Instance.Info.BasePath, i + 1);

                _nodes.Add(newNode);

                tasks.Add(newNode.EnsureConnectionAsync());
            }

            await Task.WhenAll(tasks);
        }

        public void Dispose()
        {
            foreach (var node in _nodes)
            {
                node.Dispose();
            }

            _runner.Stop();

            _nodes.Dispose();
        }

        private MSBuildHost GetNode()
        {
            MSBuildHost result = null;

            result = _nodes.Take();

            return result;
        }

        private void FreeNode(MSBuildHost node)
        {
            _nodes.Add(node);
        }

        public void Start(CancellationToken cancellationToken, Func<MSBuildHost, IProject, bool> buildTask)
        {
            if (_runner == null)
            {
                _runner = new JobRunner<(IProject project, TaskCompletionSource<(bool result, IProject project)>), bool>(Environment.ProcessorCount, context =>
                {
                    var node = GetNode();

                    var result = buildTask(node, context.project);

                    FreeNode(node);

                    context.Item2.SetResult((result, context.project));

                    return result;
                });
            }
        }

        public Task<(bool result, IProject project)> Queue(IProject project)
        {
            var tcs = new TaskCompletionSource<(bool result, IProject project)>();

            _runner.Add((project, tcs));

            return tcs.Task;
        }
    }
}
