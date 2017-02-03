using System;
using System.Threading;

namespace AvalonStudio.MVVM.DataVirtualization
{
	/// <summary>
	///     This collection should be used when items can be added or removed from the underlying data source.
	///     (Since it derives from AsyncVirtualizingCollection<T>, it also doesn't block the UI on long database queries.)
	/// </summary>
	public class DynamicAsyncVirtualizingCollection<T> : AsyncVirtualizingCollection<T> where T : class
	{
		private readonly int loadCountInterval;
		private readonly Timer timer;

		/// <summary>
		///     Constructor. Hooks up the event handler triggered when the timer ticks.
		/// </summary>
		/// <param name="itemsProvider">The items provider.</param>
		/// <param name="pageSize">Size of the page (number of items).</param>
		/// <param name="pageTimeout">The page timeout, in milliseconds.</param>
		/// <param name="loadCountInterval">The interval between count loads from the database, in milliseconds.</param>
		public DynamicAsyncVirtualizingCollection(IItemsProvider<T> itemsProvider, int pageSize, int pageTimeout,
			int loadCountInterval)
			: base(itemsProvider, pageSize, pageTimeout)
		{
			this.loadCountInterval = loadCountInterval;
            Console.WriteLine("Port to .NET Core required.");
			//timer = new Timer(TimerCallback);
		}

		/// <summary>
		///     As soon as we receive the count we start the timer to retrieve it again.
		/// </summary>
		protected override void LoadCountCompleted(object args)
		{
			base.LoadCountCompleted(args);
			timer.Change(TimeSpan.FromMilliseconds(loadCountInterval), TimeSpan.FromMilliseconds(loadCountInterval));
		}

		/// <summary>
		///     We retrieve the count of the underlying collection on a timer.
		/// </summary>
		private void TimerCallback(object state)
		{
			LoadCount();
		}
	}
}