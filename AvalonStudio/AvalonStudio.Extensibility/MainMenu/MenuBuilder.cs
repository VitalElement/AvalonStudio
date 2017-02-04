using System.Linq;
using AvalonStudio.Extensibility.Commands;
using AvalonStudio.Extensibility.MainMenu.Models;
using AvalonStudio.Extensibility.Menus;

namespace AvalonStudio.Extensibility.MainMenu
{
	public class MenuBuilder : IMenuBuilder
	{
		private readonly ICommandService _commandService;
		private readonly MenuItemGroupDefinition[] _excludeMenuItemGroups;
		private readonly MenuItemDefinition[] _excludeMenuItems;
		private readonly MenuDefinition[] _excludeMenus;
		private readonly MenuBarDefinition[] _menuBars;
		private readonly MenuItemGroupDefinition[] _menuItemGroups;
		private readonly MenuItemDefinition[] _menuItems;
		private readonly MenuDefinition[] _menus;
        
		public MenuBuilder(
			ICommandService commandService,
			MenuBarDefinition[] menuBars,
			MenuDefinition[] menus,
			MenuItemGroupDefinition[] menuItemGroups,
			MenuItemDefinition[] menuItems,
			ExcludeMenuDefinition[] excludeMenus,
			ExcludeMenuItemGroupDefinition[] excludeMenuItemGroups,
			ExcludeMenuItemDefinition[] excludeMenuItems)
		{
			_commandService = commandService;
			_menuBars = menuBars;
			_menus = menus;
			_menuItemGroups = menuItemGroups;
			_menuItems = menuItems;
			_excludeMenus = excludeMenus.Select(x => x.MenuDefinitionToExclude).ToArray();
			_excludeMenuItemGroups = excludeMenuItemGroups.Select(x => x.MenuItemGroupDefinitionToExclude).ToArray();
			_excludeMenuItems = excludeMenuItems.Select(x => x.MenuItemDefinitionToExclude).ToArray();
		}

		public void BuildMenuBar(MenuBarDefinition menuBarDefinition, MenuModel result)
		{
			var menus = _menus
				.Where(x => x.MenuBar == menuBarDefinition)
				.Where(x => !_excludeMenus.Contains(x))
				.OrderBy(x => x.SortOrder);

			foreach (var menu in menus)
			{
				var menuModel = new TextMenuItem(menu);
				AddGroupsRecursive(menu, menuModel);
				if (menuModel.Children.Any())
					result.Add(menuModel);
			}
		}

		private void AddGroupsRecursive(MenuDefinitionBase menu, StandardMenuItem menuModel)
		{
			var groups = _menuItemGroups
				.Where(x => x.Parent == menu)
				.Where(x => !_excludeMenuItemGroups.Contains(x))
				.OrderBy(x => x.SortOrder)
				.ToList();

			for (var i = 0; i < groups.Count; i++)
			{
				var group = groups[i];
				var menuItems = _menuItems
					.Where(x => x.Group == group)
					.Where(x => !_excludeMenuItems.Contains(x))
					.OrderBy(x => x.SortOrder);

				foreach (var menuItem in menuItems)
				{
					var menuItemModel = menuItem.CommandDefinition != null
						? new CommandMenuItem(_commandService.GetCommand(menuItem.CommandDefinition), menuItem.CommandDefinition.Command,
							menuModel)
						: (StandardMenuItem) new TextMenuItem(menuItem);
					AddGroupsRecursive(menuItem, menuItemModel);
					menuModel.Add(menuItemModel);
				}

				if (i < groups.Count - 1 && menuItems.Any())
					menuModel.Add(new MenuItemSeparator());
			}
		}
	}
}