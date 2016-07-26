using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using ReactiveUI;

namespace AvalonStudio.MVVM.DataVirtualization
{
	/// <summary>
	///     Specialized list implementation that provides data virtualization. The collection is divided up into pages,
	///     and pages are dynamically fetched from the IItemsProvider when required. Stale pages are removed after a
	///     configurable period of time.
	///     Intended for use with large collections on a network or disk resource that cannot be instantiated locally
	///     due to memory consumption or fetch latency.
	/// </summary>
	/// <remarks>
	///     The IList implmentation is not fully complete, but should be sufficient for use as read only collection
	///     data bound to a suitable ItemsControl.
	/// </remarks>
	/// <typeparam name="T"></typeparam>
	public class VirtualizingCollection<T> : ViewModel, IList<DataWrapper<T>>, IList where T : class
	{
		private void _itemsProvider_PropertyChanged(object sender, PropertyChangedEventArgs e)
		{
			switch (e.PropertyName)
			{
				case "Count":
					LoadCount();
					break;
			}
		}

		#region Constructors

		/// <summary>
		///     Initializes a new instance of the <see cref="VirtualizingCollection&lt;T&gt;" /> class.
		/// </summary>
		/// <param name="itemsProvider">The items provider.</param>
		/// <param name="pageSize">Size of the page.</param>
		/// <param name="pageTimeout">The page timeout.</param>
		public VirtualizingCollection(IItemsProvider<T> itemsProvider, int pageSize, int pageTimeout)
		{
			ItemsProvider = itemsProvider;
			ItemsProvider.PropertyChanged += _itemsProvider_PropertyChanged;
			PageSize = pageSize;
			PageTimeout = pageTimeout;
		}

		/// <summary>
		///     Initializes a new instance of the <see cref="VirtualizingCollection&lt;T&gt;" /> class.
		/// </summary>
		/// <param name="itemsProvider">The items provider.</param>
		/// <param name="pageSize">Size of the page.</param>
		public VirtualizingCollection(IItemsProvider<T> itemsProvider, int pageSize)
		{
			ItemsProvider = itemsProvider;
			ItemsProvider.PropertyChanged += _itemsProvider_PropertyChanged;
			PageSize = pageSize;
		}

		/// <summary>
		///     Initializes a new instance of the <see cref="VirtualizingCollection&lt;T&gt;" /> class.
		/// </summary>
		/// <param name="itemsProvider">The items provider.</param>
		public VirtualizingCollection(IItemsProvider<T> itemsProvider)
		{
			ItemsProvider = itemsProvider;
			ItemsProvider.PropertyChanged += _itemsProvider_PropertyChanged;
		}

		#endregion

		#region ItemsProvider

		/// <summary>
		///     Gets the items provider.
		/// </summary>
		/// <value>The items provider.</value>
		public IItemsProvider<T> ItemsProvider { get; }

		#endregion

		#region PageSize

		/// <summary>
		///     Gets the size of the page.
		/// </summary>
		/// <value>The size of the page.</value>
		public int PageSize { get; } = 100;

		#endregion

		#region PageTimeout

		/// <summary>
		///     Gets the page timeout.
		/// </summary>
		/// <value>The page timeout.</value>
		public long PageTimeout { get; } = 10000;

		#endregion

		#region IList<DataWrapper<T>>, IList

		#region Count

		private int _count = -1;

		/// <summary>
		///     Gets the number of elements contained in the <see cref="T:System.Collections.Generic.ICollection`1" />.
		///     The first time this property is accessed, it will fetch the count from the IItemsProvider.
		/// </summary>
		/// <value></value>
		/// <returns>
		///     The number of elements contained in the <see cref="T:System.Collections.Generic.ICollection`1" />.
		/// </returns>
		public int Count
		{
			get
			{
				if (_count == -1)
				{
					_count = 0;
					LoadCount();
				}
				return _count;
			}
			protected set
			{
				_count = value;
				this.RaisePropertyChanged(nameof(Count));
			}
		}

		#endregion

		private void RemovePage(int pageIndex)
		{
			if (pageIndex == 0)
			{
				return;
			}

			var removePage = true;
			DataPage<T> page;
			if (Pages.TryGetValue(pageIndex, out page))
			{
				removePage = !page.IsInUse;
			}

			if (removePage)
			{
				Pages.Remove(pageIndex);
			}
		}

		public void CleanPagesAround(ulong index)
		{
			var pageIndex = (int) index/PageSize;

			var keysToRemove = Pages.Keys.Where(p => p != pageIndex).ToList();

			foreach (var key in keysToRemove)
			{
				RemovePage(key);
			}
		}

		#region Indexer

		/// <summary>
		///     Gets the item at the specified index. This property will fetch
		///     the corresponding page from the IItemsProvider if required.
		/// </summary>
		/// <value></value>
		public virtual DataWrapper<T> this[int index]
		{
			get
			{
				// determine which page and offset within page
				var pageIndex = index/PageSize;
				var pageOffset = index%PageSize;

				// request primary page
				RequestPage(pageIndex);

                //if accessing upper 50 % then request next page

                if (pageOffset > PageSize / 2 && pageIndex < Count / PageSize)
                {
                    RequestPage(pageIndex + 1);
                }

                //if accessing lower 50 % then request prev page

                if (pageOffset < PageSize / 2 && pageIndex > 0)
                {
                    RequestPage(pageIndex - 1);
                }

                //remove stale pages

                CleanUpPages();

                // return requested item
                return Pages[pageIndex].Items[pageOffset];
			}
			set { throw new NotSupportedException(); }
		}

		object IList.this[int index]
		{
			get { return this[index]; }
			set { throw new NotSupportedException(); }
		}

		#endregion

		#region IEnumerator<DataWrapper<T>>, IEnumerator

		/// <summary>
		///     Returns an enumerator that iterates through the collection.
		/// </summary>
		/// <remarks>
		///     This method should be avoided on large collections due to poor performance.
		/// </remarks>
		/// <returns>
		///     A <see cref="T:System.Collections.Generic.IEnumerator`1" /> that can be used to iterate through the collection.
		/// </returns>
		public IEnumerator<DataWrapper<T>> GetEnumerator()
		{
			for (var i = 0; i < Count; i++)
			{
				yield return this[i];
			}
		}

		/// <summary>
		///     Returns an enumerator that iterates through a collection.
		/// </summary>
		/// <returns>
		///     An <see cref="T:System.Collections.IEnumerator" /> object that can be used to iterate through the collection.
		/// </returns>
		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}

		#endregion

		#region Add

		/// <summary>
		///     Not supported.
		/// </summary>
		/// <param name="item">The object to add to the <see cref="T:System.Collections.Generic.ICollection`1" />.</param>
		/// <exception cref="T:System.NotSupportedException">
		///     The <see cref="T:System.Collections.Generic.ICollection`1" /> is read-only.
		/// </exception>
		public void Add(DataWrapper<T> item)
		{
			throw new NotSupportedException();
		}

		int IList.Add(object value)
		{
			throw new NotSupportedException();
		}

		#endregion

		#region Contains

		bool IList.Contains(object value)
		{
			return Contains((DataWrapper<T>) value);
		}

		/// <summary>
		///     Not supported.
		/// </summary>
		/// <param name="item">The object to locate in the <see cref="T:System.Collections.Generic.ICollection`1" />.</param>
		/// <returns>
		///     Always false.
		/// </returns>
		public bool Contains(DataWrapper<T> item)
		{
			foreach (var page in Pages.Values)
			{
				if (page.Items.Contains(item))
				{
					return true;
				}
			}
			return false;
		}

		#endregion

		#region Clear

		/// <summary>
		///     TODO
		/// </summary>
		public void Clear()
		{
			throw new NotSupportedException();
		}

		#endregion

		#region IndexOf

		int IList.IndexOf(object value)
		{
			return IndexOf((DataWrapper<T>) value);
		}

		/// <summary>
		///     TODO
		/// </summary>
		/// <param name="item">The object to locate in the <see cref="T:System.Collections.Generic.IList`1" />.</param>
		/// <returns>
		///     TODO
		/// </returns>
		public int IndexOf(DataWrapper<T> item)
		{
			foreach (var keyValuePair in Pages)
			{
				var indexWithinPage = keyValuePair.Value.Items.IndexOf(item);
				if (indexWithinPage != -1)
				{
					return PageSize*keyValuePair.Key + indexWithinPage;
				}
			}
			return -1;
		}

		#endregion

		#region Insert

		/// <summary>
		///     Not supported.
		/// </summary>
		/// <param name="index">The zero-based index at which <paramref name="item" /> should be inserted.</param>
		/// <param name="item">The object to insert into the <see cref="T:System.Collections.Generic.IList`1" />.</param>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///     <paramref name="index" /> is not a valid index in the <see cref="T:System.Collections.Generic.IList`1" />.
		/// </exception>
		/// <exception cref="T:System.NotSupportedException">
		///     The <see cref="T:System.Collections.Generic.IList`1" /> is read-only.
		/// </exception>
		public void Insert(int index, DataWrapper<T> item)
		{
			throw new NotSupportedException();
		}

		void IList.Insert(int index, object value)
		{
			Insert(index, (DataWrapper<T>) value);
		}

		#endregion

		#region Remove

		/// <summary>
		///     Not supported.
		/// </summary>
		/// <param name="index">The zero-based index of the item to remove.</param>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///     <paramref name="index" /> is not a valid index in the <see cref="T:System.Collections.Generic.IList`1" />.
		/// </exception>
		/// <exception cref="T:System.NotSupportedException">
		///     The <see cref="T:System.Collections.Generic.IList`1" /> is read-only.
		/// </exception>
		public void RemoveAt(int index)
		{
			throw new NotSupportedException();
		}

		void IList.Remove(object value)
		{
			throw new NotSupportedException();
		}

		/// <summary>
		///     Not supported.
		/// </summary>
		/// <param name="item">The object to remove from the <see cref="T:System.Collections.Generic.ICollection`1" />.</param>
		/// <returns>
		///     true if <paramref name="item" /> was successfully removed from the
		///     <see cref="T:System.Collections.Generic.ICollection`1" />; otherwise, false. This method also returns false if
		///     <paramref name="item" /> is not found in the original <see cref="T:System.Collections.Generic.ICollection`1" />.
		/// </returns>
		/// <exception cref="T:System.NotSupportedException">
		///     The <see cref="T:System.Collections.Generic.ICollection`1" /> is read-only.
		/// </exception>
		public bool Remove(DataWrapper<T> item)
		{
			throw new NotSupportedException();
		}

		#endregion

		#region CopyTo

		/// <summary>
		///     Not supported.
		/// </summary>
		/// <param name="array">
		///     The one-dimensional <see cref="T:System.Array" /> that is the destination of the elements copied
		///     from <see cref="T:System.Collections.Generic.ICollection`1" />. The <see cref="T:System.Array" /> must have
		///     zero-based indexing.
		/// </param>
		/// <param name="arrayIndex">The zero-based index in <paramref name="array" /> at which copying begins.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///     <paramref name="array" /> is null.
		/// </exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///     <paramref name="arrayIndex" /> is less than 0.
		/// </exception>
		/// <exception cref="T:System.ArgumentException">
		///     <paramref name="array" /> is multidimensional.
		///     -or-
		///     <paramref name="arrayIndex" /> is equal to or greater than the length of <paramref name="array" />.
		///     -or-
		///     The number of elements in the source <see cref="T:System.Collections.Generic.ICollection`1" /> is greater than the
		///     available space from <paramref name="arrayIndex" /> to the end of the destination <paramref name="array" />.
		///     -or-
		///     Type <paramref name="T" /> cannot be cast automatically to the type of the destination <paramref name="array" />.
		/// </exception>
		public void CopyTo(DataWrapper<T>[] array, int arrayIndex)
		{
			throw new NotSupportedException();
		}

		void ICollection.CopyTo(Array array, int index)
		{
			throw new NotSupportedException();
		}

		#endregion

		#region Misc

		/// <summary>
		///     Gets an object that can be used to synchronize access to the <see cref="T:System.Collections.ICollection" />.
		/// </summary>
		/// <value></value>
		/// <returns>
		///     An object that can be used to synchronize access to the <see cref="T:System.Collections.ICollection" />.
		/// </returns>
		public object SyncRoot
		{
			get { return this; }
		}

		/// <summary>
		///     Gets a value indicating whether access to the <see cref="T:System.Collections.ICollection" /> is synchronized
		///     (thread safe).
		/// </summary>
		/// <value></value>
		/// <returns>
		///     Always false.
		/// </returns>
		public bool IsSynchronized
		{
			get { return false; }
		}

		/// <summary>
		///     Gets a value indicating whether the <see cref="T:System.Collections.Generic.ICollection`1" /> is read-only.
		/// </summary>
		/// <value></value>
		/// <returns>
		///     Always true.
		/// </returns>
		public bool IsReadOnly
		{
			get { return true; }
		}

		/// <summary>
		///     Gets a value indicating whether the <see cref="T:System.Collections.IList" /> has a fixed size.
		/// </summary>
		/// <value></value>
		/// <returns>
		///     Always false.
		/// </returns>
		public bool IsFixedSize
		{
			get { return false; }
		}

		#endregion

		#endregion

		#region Paging

		public Dictionary<int, DataPage<T>> Pages { get; private set; } = new Dictionary<int, DataPage<T>>();

		/// <summary>
		///     Cleans up any stale pages that have not been accessed in the period dictated by PageTimeout.
		/// </summary>
		public void CleanUpPages()
		{
			var keys = Pages.Keys.ToArray();
			foreach (var key in keys)
			{
				// page 0 is a special case, since WPF ItemsControl access the first item frequently
				if (key != 0 && (DateTime.Now - Pages[key].TouchTime).TotalMilliseconds > PageTimeout)
				{
					var removePage = true;
					DataPage<T> page;
					if (Pages.TryGetValue(key, out page))
					{
						removePage = !page.IsInUse;
					}

					if (removePage)
					{
						Pages.Remove(key);
						//Trace.WriteLine("Removed Page: " + key);
					}
				}
			}
		}

		/// <summary>
		///     Makes a request for the specified page, creating the necessary slots in the dictionary,
		///     and updating the page touch time.
		/// </summary>
		/// <param name="pageIndex">Index of the page.</param>
		protected virtual void RequestPage(int pageIndex)
		{
			if (!Pages.ContainsKey(pageIndex))
			{
				// Create a page of empty data wrappers.
				var pageLength = Math.Min(PageSize, Count - pageIndex*PageSize);
				var page = new DataPage<T>(pageIndex*PageSize, pageLength);
				Pages.Add(pageIndex, page);
				//Trace.WriteLine("Added page: " + pageIndex);
				LoadPage(pageIndex, pageLength);
			}
			else
			{
				Pages[pageIndex].TouchTime = DateTime.Now;
			}
		}

		/// <summary>
		///     Populates the page within the dictionary.
		/// </summary>
		/// <param name="pageIndex">Index of the page.</param>
		/// <param name="page">The page.</param>
		protected virtual void PopulatePage(int pageIndex, IList<T> dataItems)
		{
			//Trace.WriteLine("Page populated: " + pageIndex);
			DataPage<T> page;
			if (Pages.TryGetValue(pageIndex, out page))
			{
				page.Populate(dataItems);
			}
		}

		/// <summary>
		///     Removes all cached pages. This is useful when the count of the
		///     underlying collection changes.
		/// </summary>
		protected void EmptyCache()
		{
			Pages = new Dictionary<int, DataPage<T>>();
		}

		#endregion

		#region Load methods

		/// <summary>
		///     Loads the count of items.
		/// </summary>
		protected virtual void LoadCount()
		{
			Count = FetchCount();
		}

		/// <summary>
		///     Loads the page of items.
		/// </summary>
		/// <param name="pageIndex">Index of the page.</param>
		/// <param name="pageLength">Number of items in the page.</param>
		protected virtual void LoadPage(int pageIndex, int pageLength)
		{
			var count = 0;
			PopulatePage(pageIndex, FetchPage(pageIndex, pageLength, out count));
			Count = count;
		}

		#endregion

		#region Fetch methods

		/// <summary>
		///     Fetches the requested page from the IItemsProvider.
		/// </summary>
		/// <param name="pageIndex">Index of the page.</param>
		/// <returns></returns>
		protected IList<T> FetchPage(int pageIndex, int pageLength, out int count)
		{
			return ItemsProvider.FetchRange(pageIndex*PageSize, pageLength, out count);
		}

		/// <summary>
		///     Fetches the count of items from the IItemsProvider.
		/// </summary>
		/// <returns></returns>
		protected int FetchCount()
		{
			return ItemsProvider.Count;
		}

		#endregion
	}
}