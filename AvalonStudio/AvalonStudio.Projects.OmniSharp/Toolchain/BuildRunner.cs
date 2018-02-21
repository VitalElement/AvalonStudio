using AvalonStudio.Projects.OmniSharp.DotnetCli;
using AvalonStudio.Projects.OmniSharp.MSBuild;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace AvalonStudio.Projects.OmniSharp.Toolchain
{
    class JobRunner<T, TRes>
    {
        AutoResetEvent _ev = new AutoResetEvent(false);
        Queue<Job> _items = new Queue<Job>();
        private bool _finished;

        struct Job
        {
            public T Data { get; set; }
            public TaskCompletionSource<TRes> Tcs { get; set; }
        }

        public Task<TRes> Add(T job)
        {
            var tcs = new TaskCompletionSource<TRes>();
            lock (_items)
            {
                _items.Enqueue(new Job { Data = job, Tcs = tcs });
                _ev.Set();
            }

            return tcs.Task;
        }


        public void Stop()
        {
            lock (_items)
            {
                _finished = true;
                _ev.Set();
            }
        }

        public JobRunner(int concurrency, Func<T, TRes> cb)
        {
            for (var c = 0; c < concurrency; c++)
            {
                new Thread(() =>
                {
                    while (true)
                    {
                        var job = default(Job);
                        lock (_items)
                        {
                            if (_items.Count > 0)
                            {
                                job = _items.Dequeue();
                                if (_items.Count > 0)
                                    _ev.Set();
                            }
                            else if (_finished)
                            {
                                //Unblock other threads
                                _ev.Set();
                                return;
                            }
                        }
                        if (job.Tcs == null)
                        {
                            _ev.WaitOne();
                            continue;
                        }

                        try
                        {
                            job.Tcs.SetResult(cb(job.Data));
                        }
                        catch (Exception e)
                        {
                            job.Tcs.SetException(e);
                        }

                    }


                }).Start();
            }
        }
    }


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
