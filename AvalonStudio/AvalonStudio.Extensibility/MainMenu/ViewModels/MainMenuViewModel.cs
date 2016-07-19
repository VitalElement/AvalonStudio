namespace AvalonStudio.Extensibility.MainMenu.ViewModels
{
	using Models;
	using MVVM;
	using ReactiveUI;
	using System.ComponentModel.Composition;
	using System;
	using System.Collections;
	using System.Collections.Generic;
	using AvalonStudio.MVVM;
	using System.Collections.Specialized;

	[Export(typeof(IMenu))]
	public class MainMenuViewModel : ViewModel<MenuModel>, IPartImportsSatisfiedNotification, IMenu
	{
		private readonly IMenuBuilder _menuBuilder;

		private bool _autoHide;

		public event NotifyCollectionChangedEventHandler CollectionChanged;

		//private readonly SettingsPropertyChangedEventManager<Properties.Settings> _settingsEventManager =
		//    new SettingsPropertyChangedEventManager<Properties.Settings>(Properties.Settings.Default);

		[ImportingConstructor]
		public MainMenuViewModel(IMenuBuilder menuBuilder) : base(new MenuModel())
		{            
			_menuBuilder = menuBuilder;
			//_autoHide = Properties.Settings.Default.AutoHideMainMenu;
			//_settingsEventManager.AddListener(s => s.AutoHideMainMenu, value => { AutoHide = value; });
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

		public int Count
		{
			get
			{
				return Model.Count;
			}
		}

		public bool IsReadOnly
		{
			get
			{
				return false;
			}
		}

		public MenuItemBase this[int index]
		{
			get
			{
				return Model[index];
			}

			set
			{
				Model[index] = value;
			}
		}

		void IPartImportsSatisfiedNotification.OnImportsSatisfied()
		{
			_menuBuilder.BuildMenuBar(MenuDefinitions.MainMenuBar, Model);
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