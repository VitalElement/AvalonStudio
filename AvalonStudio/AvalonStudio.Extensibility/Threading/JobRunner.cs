namespace AvalonStudio.Extensibility.Threading
{
    using Avalonia.Platform;
    using Avalonia.Threading;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading;
    using System.Threading.Tasks;
    

    public class JobRunner
    {
        private readonly Queue<Job> _queue = new Queue<Job>();
        private readonly AutoResetEvent _event = new AutoResetEvent(false);

        public JobRunner()
        {
        }

        public void RunLoop(CancellationToken cancellationToken)
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                _event.WaitOne();

                if (cancellationToken.IsCancellationRequested)
                    return;

                RunJobs();


            }
        }

        /// <summary>
        /// Runs continuations pushed on the loop.
        /// </summary>
        public void RunJobs()
        {
            while (true)
            {
                Job job;

                lock (_queue)
                {
                    if (_queue.Count == 0)
                        return;
                    job = _queue.Dequeue();
                }

                if (job.TaskCompletionSource == null)
                {
                    job.Action();
                }
                else
                {
                    try
                    {
                        job.Action();
                        job.TaskCompletionSource.SetResult(null);
                    }
                    catch (Exception e)
                    {
                        job.TaskCompletionSource.SetException(e);
                    }
                }
            }
        }

        /// <summary>
        /// Invokes a method on the main loop.
        /// </summary>
        /// <param name="action">The method.</param>
        /// <param name="priority">The priority with which to invoke the method.</param>
        /// <returns>A task that can be used to track the method's execution.</returns>
        public Task InvokeAsync(Action action, DispatcherPriority priority = DispatcherPriority.Normal)
        {
            var job = new Job(action, priority, false);
            AddJob(job);
            return job.TaskCompletionSource.Task;
        }

        /// <summary>
        /// Post action that will be invoked on main thread
        /// </summary>
        /// <param name="action">The method.</param>
        /// 
        /// <param name="priority">The priority with which to invoke the method.</param>
        internal void Post(Action action, DispatcherPriority priority)
        {
            // TODO: Respect priority.
            AddJob(new Job(action, priority, true));
        }

        private void AddJob(Job job)
        {
            var needWake = false;
            lock (_queue)
            {
                needWake = _queue.Count == 0;
                _queue.Enqueue(job);
            }
            if (needWake)
                _event.Set();
        }

        /// <summary>
        /// A job to run.
        /// </summary>
        private class Job
        {
            /// <summary>
            /// Initializes a new instance of the <see cref="Job"/> class.
            /// </summary>
            /// <param name="action">The method to call.</param>
            /// <param name="priority">The job priority.</param>
            /// <param name="throwOnUiThread">Do not wrap excepption in TaskCompletionSource</param>
            public Job(Action action, DispatcherPriority priority, bool throwOnUiThread)
            {
                Action = action;
                Priority = priority;
                TaskCompletionSource = throwOnUiThread ? null : new TaskCompletionSource<object>();
            }

            /// <summary>
            /// Gets the method to call.
            /// </summary>
            public Action Action { get; }

            /// <summary>
            /// Gets the job priority.
            /// </summary>
            public DispatcherPriority Priority { get; }

            /// <summary>
            /// Gets the task completion source.
            /// </summary>
            public TaskCompletionSource<object> TaskCompletionSource { get; }
        }

    }
}
