namespace AvalonStudio.Extensibility.MVVM
{
    using System.Collections;
    using System.Collections.Generic;
    using System.Collections.Specialized;
    using System.ComponentModel;

    public interface IObservableCollection<T> : IList<T>, ICollection<T>, IEnumerable<T>, IEnumerable, INotifyPropertyChanged, INotifyCollectionChanged
    {

    }
}
