using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using AvalonStudio.Extensibility.MainMenu.Models;
using AvalonStudio.MVVM;
using ReactiveUI;

namespace AvalonStudio.Extensibility.MainMenu.ViewModels
{
    public class MainMenuViewModel : ViewModel<MenuModel>, IMenu
    {
        private readonly IMenuBuilder _menuBuilder;

        private bool _autoHide;

        public MainMenuViewModel(IMenuBuilder menuBuilder) : base(new MenuModel())
        {
            _menuBuilder = menuBuilder;
        }

        public bool AutoHide
        {
            get { return _autoHide; }
            private set
            {
                if (_autoHide == value)
                    return;

                _autoHide = value;

                this.RaiseAndSetIfChanged(ref _autoHide, value);
            }
        }

        public event NotifyCollectionChangedEventHandler CollectionChanged;

        public int Count
        {
            get { return Model.Count; }
        }

        public bool IsReadOnly
        {
            get { return false; }
        }

        public MenuItemBase this[int index]
        {
            get { return Model[index]; }

            set { Model[index] = value; }
        }

        public int IndexOf(MenuItemBase item)
        {
            return Model.IndexOf(item);
        }

        public void Insert(int index, MenuItemBase item)
        {
            Model.Insert(index, item);
        }

        public void RemoveAt(int index)
        {
            Model.RemoveAt(index);
        }

        public void Add(MenuItemBase item)
        {
            Model.Add(item);
        }

        public void Clear()
        {
            Model.Clear();
        }

        public bool Contains(MenuItemBase item)
        {
            return Model.Contains(item);
        }

        public void CopyTo(MenuItemBase[] array, int arrayIndex)
        {
            Model.CopyTo(array, arrayIndex);
        }

        public bool Remove(MenuItemBase item)
        {
            return Model.Remove(item);
        }

        public IEnumerator<MenuItemBase> GetEnumerator()
        {
            return Model.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return Model.GetEnumerator();
        }
    }
}