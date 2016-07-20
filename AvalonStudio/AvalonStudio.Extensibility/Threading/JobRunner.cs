using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace AvalonStudio.Extensibility.Threading
{
	public class JobRunner
	{
		private readonly AutoResetEvent _event = new AutoResetEvent(false);
		private readonly Queue<Job> _queue = new Queue<Job>();

		public void RunLoop(CancellationToken cancellationToken)
		{
			cancellationToken.Register(() => { _event.Set(); });

			while (!cancellationToken.IsCancellationRequested)
			{
				_event.WaitOne();

				if (cancellationToken.IsCancellationRequested)
				{
					return;
				}

				RunJobs();
			}
		}

		/// <summary>
		///     Runs continuations pushed on the loop.
		/// </summary>
		public void RunJobs()
		{
			while (true)
			{
				Job job;

				lock (_queue)
				{
					if (_queue.Count == 0)
					{
						return;
					}

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
		///     Invokes a method on the main loop.
		/// </summary>
		/// <param name="action">The method.</param>
		/// <returns>A task that can be used to track the method's execution.</returns>
		public Task InvokeAsync(Action action)
		{
			var job = new Job(action, false);

			AddJob(job);

			return job.TaskCompletionSource.Task;
		}

		/// <summary>
		///     Post action that will be invoked on main thread
		/// </summary>
		/// <param name="action">The method.</param>
		internal void Post(Action action)
		{
			AddJob(new Job(action, true));
		}

		private void AddJob(Job job)
		{
			bool needWake;

			lock (_queue)
			{
				needWake = _queue.Count == 0;
				_queue.Enqueue(job);
			}

			if (needWake)
			{
				_event.Set();
			}
		}

		/// <summary>
		///     A job to run.
		/// </summary>
		private class Job
		{
			/// <summary>
			///     Initializes a new instance of the <see cref="Job" /> class.
			/// </summary>
			/// <param name="action">The method to call.</param>
			/// <param name="throwOnUiThread">Do not wrap excepption in TaskCompletionSource</param>
			public Job(Action action, bool throwOnUiThread)
			{
				Action = action;
				TaskCompletionSource = throwOnUiThread ? null : new TaskCompletionSource<object>();
			}

			/// <summary>
			///     Gets the method to call.
			/// </summary>
			public Action Action { get; }


			/// <summary>
			///     Gets the task completion source.
			/// </summary>
			public TaskCompletionSource<object> TaskCompletionSource { get; }
		}
	}
}