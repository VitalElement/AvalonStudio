using ReactiveUI;
using System;
using System.Collections.ObjectModel;
using System.Linq;

namespace AvalonStudio.MVVM
{
    public class MutuallyExclusiveEnumerationCollection<T> : ObservableCollection<MutuallyExclusiveEnumeration<T>> where T : struct, IComparable
    {
        public MutuallyExclusiveEnumerationCollection(T defaultValue, Action<T> setter)
        {
            foreach (T value in Enum.GetValues(typeof(T)))
            {
                var enumClass = new MutuallyExclusiveEnumeration<T>();
                enumClass.Enumeration = value;
                enumClass.IsChecked = value.Equals(defaultValue) ? true : false;
                
                Add(enumClass);
            }

            Command = ReactiveCommand.Create<object>((o) =>
            {
                T myEnum = (T)o;

                var collection = this;

                var theClass = collection.First(t => t.Enumeration.Equals(myEnum));

                // ok, they want to check this one, let them and uncheck all else
                foreach (var iter in collection)
                {
                    iter.IsChecked = false;
                }

                theClass.IsChecked = true;

                setter(myEnum);
            });
        }

        public ReactiveCommand Command { get; private set; }
    }
}