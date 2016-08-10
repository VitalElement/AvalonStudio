using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using ReactiveUI;

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
                enumClass.Title = value.GetDescription();

                Add(enumClass);
            }

            Command = ReactiveCommand.Create();

            Command.Subscribe((o) =>
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

        public ReactiveCommand<object> Command { get; private set; }
    }

    public class MutuallyExclusiveEnumeration<T> : ViewModel where T : struct, IComparable
    {
        public MutuallyExclusiveEnumeration()
        {
            isChecked = false;
        }

        public T Enumeration { get; set; }

        private bool isChecked;
        public bool IsChecked
        {
            get { return isChecked; }
            set { this.RaiseAndSetIfChanged(ref isChecked, value); }
        }

        public string Title { get; set; }

    }
}
