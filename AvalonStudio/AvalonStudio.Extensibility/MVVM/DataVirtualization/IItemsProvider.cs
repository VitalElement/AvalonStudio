using System.Collections.Generic;
using System.ComponentModel;

namespace AvalonStudio.MVVM.DataVirtualization
{
    /// <summary>
    ///     Represents a provider of collection details.
    /// </summary>
    /// <typeparam name="T">The type of items in the collection.</typeparam>
    public interface IItemsProvider<T> : INotifyPropertyChanged
    {
        /// <summary>
        ///     Fetches the total number of items available.
        /// </summary>
        /// <returns></returns>
        int Count { get; }

        /// <summary>
        ///     Fetches a range of items.
        /// </summary>
        /// <param name="startIndex">The start index.</param>
        /// <param name="count">The number of items to fetch.</param>
        /// <returns></returns>
        IList<T> FetchRange(int startIndex, int pageCount, out int overallCount);
    }
}