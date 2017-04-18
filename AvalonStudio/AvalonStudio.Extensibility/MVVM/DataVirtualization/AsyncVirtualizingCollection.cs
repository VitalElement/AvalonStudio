using ReactiveUI;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Threading;

namespace AvalonStudio.MVVM.DataVirtualization
{
    /// <summary>
    ///     Derived VirtualizatingCollection, performing loading asychronously.
    /// </summary>
    /// <typeparam name="T">The type of items in the collection</typeparam>
    public class AsyncVirtualizingCollection<T> : VirtualizingCollection<T>, INotifyCollectionChanged where T : class
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="AsyncVirtualizingCollection&lt;T&gt;" /> class.
        /// </summary>
        /// <param name="itemsProvider">The items provider.</param>
        /// <param name="pageSize">Size of the page.</param>
        /// <param name="pageTimeout">The page timeout.</param>
        public AsyncVirtualizingCollection(IItemsProvider<T> itemsProvider, int pageSize, int pageTimeout)
            : base(itemsProvider, pageSize, pageTimeout)
        {
            SynchronizationContext = SynchronizationContext.Current;
        }

        #region SynchronizationContext

        /// <summary>
        ///     Gets the synchronization context used for UI-related operations. This is obtained as
        ///     the current SynchronizationContext when the AsyncVirtualizingCollection is created.
        /// </summary>
        /// <value>The synchronization context.</value>
        protected SynchronizationContext SynchronizationContext { get; }

        #endregion SynchronizationContext

        #region INotifyCollectionChanged

        /// <summary>
        ///     Occurs when the collection changes.
        /// </summary>
        public event NotifyCollectionChangedEventHandler CollectionChanged;

        /// <summary>
        ///     Raises the <see cref="E:CollectionChanged" /> event.
        /// </summary>
        /// <param name="e">
        ///     The <see cref="System.Collections.Specialized.NotifyCollectionChangedEventArgs" /> instance containing
        ///     the event data.
        /// </param>
        protected virtual void OnCollectionChanged(NotifyCollectionChangedEventArgs e)
        {
            var h = CollectionChanged;
            if (h != null)
                h(this, e);
        }

        /// <summary>
        ///     Fires the collection reset event.
        /// </summary>
        private void FireCollectionReset()
        {
            var e = new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset);
            OnCollectionChanged(e);
        }

        #endregion INotifyCollectionChanged

        #region IsLoading

        private bool _isLoading;

        /// <summary>
        ///     Gets or sets a value indicating whether the collection is loading.
        /// </summary>
        /// <value>
        ///     <c>true</c> if this collection is loading; otherwise, <c>false</c>.
        /// </value>
        public bool IsLoading
        {
            get
            {
                return _isLoading;
            }
            set
            {
                if (value != _isLoading)
                {
                    _isLoading = value;
                    this.RaisePropertyChanged(nameof(IsLoading));
                }
            }
        }

        public int RequestsWaiting { get; set; }

        private bool _isInitializing;

        public bool IsInitializing
        {
            get
            {
                return _isInitializing;
            }
            set
            {
                if (value != _isInitializing)
                {
                    _isInitializing = value;
                    this.RaisePropertyChanged(nameof(IsInitializing));
                }
            }
        }

        #endregion IsLoading

        #region Load overrides

        /// <summary>
        ///     Asynchronously loads the count of items.
        /// </summary>
        protected override void LoadCount()
        {
            if (Count == 0)
            {
                IsInitializing = true;
            }
            ThreadPool.QueueUserWorkItem(LoadCountWork);
        }

        /// <summary>
        ///     Performed on background thread.
        /// </summary>
        /// <param name="args">None required.</param>
        private void LoadCountWork(object args)
        {
            var count = FetchCount();
            SynchronizationContext.Send(LoadCountCompleted, count);
        }

        /// <summary>
        ///     Performed on UI-thread after LoadCountWork.
        /// </summary>
        /// <param name="args">Number of items returned.</param>
        protected virtual void LoadCountCompleted(object args)
        {
            var newCount = (int)args;
            TakeNewCount(newCount);
            IsInitializing = false;
        }

        private void TakeNewCount(int newCount)
        {
            if (newCount != Count)
            {
                Count = newCount;
                EmptyCache();
                FireCollectionReset();
            }
        }

        /// <summary>
        ///     Asynchronously loads the page.
        /// </summary>
        /// <param name="index">The index.</param>
        protected override void LoadPage(int pageIndex, int pageLength)
        {
            IsLoading = true;
            RequestsWaiting++;

            ThreadPool.QueueUserWorkItem(LoadPageWork, new[] { pageIndex, pageLength });
        }

        /// <summary>
        ///     Performed on background thread.
        /// </summary>
        /// <param name="args">Index of the page to load.</param>
        private void LoadPageWork(object state)
        {
            var args = (int[])state;
            var pageIndex = args[0];
            var pageLength = args[1];
            var overallCount = 0;
            var dataItems = FetchPage(pageIndex, pageLength, out overallCount);
            SynchronizationContext.Send(LoadPageCompleted, new object[] { pageIndex, dataItems, overallCount });
        }

        /// <summary>
        ///     Performed on UI-thread after LoadPageWork.
        /// </summary>
        /// <param name="args">object[] { int pageIndex, IList(T) page }</param>
        private void LoadPageCompleted(object state)
        {
            var args = (object[])state;
            var pageIndex = (int)args[0];
            var dataItems = (IList<T>)args[1];
            var newCount = (int)args[2];
            TakeNewCount(newCount);

            PopulatePage(pageIndex, dataItems);

            IsLoading = false;
            RequestsWaiting--;
        }

        #endregion Load overrides
    }
}