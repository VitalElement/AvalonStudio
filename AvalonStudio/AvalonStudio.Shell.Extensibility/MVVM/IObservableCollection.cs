using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;

namespace AvalonStudio.Extensibility.MVVM
{
    public interface IObservableCollection<T> : IList<T>, INotifyPropertyChanged, INotifyCollectionChanged
    {
    }
}