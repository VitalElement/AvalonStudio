using AvalonStudio.Menus;
using AvalonStudio.Menus.Models;
using AvalonStudio.Menus.Settings;
using AvalonStudio.Menus.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Composition;
using System.Linq;

namespace AvalonStudio.MainMenu
{
    [Export]
    [Shared]
    public class MainMenuService
    {
        private readonly MenuSettingsService _menuSettingsService;

        private readonly IEnumerable<Lazy<IMenuItem, MainMenuItemMetadata>> _menuItems;
        private readonly IEnumerable<Lazy<object, MainMenuDefaultGroupMetadata>> _defaultGroups;

        private MenuViewModel _viewModel;

        [ImportingConstructor]
        public MainMenuService(
            MenuSettingsService menuSettingsService,
            [ImportMany(ExportContractNames.MainMenu)] IEnumerable<Lazy<IMenuItem, MainMenuItemMetadata>> menuItems,
            [ImportMany(ExportContractNames.MainMenu)] IEnumerable<Lazy<object, MainMenuDefaultGroupMetadata>> defaultGroups)
        {
            _menuSettingsService = menuSettingsService;

            _menuItems = menuItems;
            _defaultGroups = defaultGroups;
        }

        public MenuViewModel GetMainMenu()
        {
            if (_viewModel != null)
            {
                return _viewModel;
            }

            var itemsByDepth = _menuItems.GroupBy(i => i.Metadata.Path.Count).OrderBy(i => i.Key);
            var rootSettings = _menuSettingsService.GetRootSettings(ExportContractNames.MainMenu);

            var builder = ImmutableList.CreateBuilder<MenuItemModel>();

            Dictionary<MenuPath, MenuItem> parentDepthMenuItems = new Dictionary<MenuPath, MenuItem>()
            {
                [new MenuPath(Array.Empty<string>())] = rootSettings
            };

            foreach (var depth in itemsByDepth)
            {
                if (depth.Key == 0)
                {
                    continue;
                }

                var currentDepthMenuItems = new Dictionary<MenuPath, MenuItem>();

                foreach (var item in depth)
                {
                    var path = item.Metadata.Path;

                    // if the parent item is not registered, it won't be included,
                    // even if it's referenced by other items
                    if (parentDepthMenuItems.TryGetValue(path.Parent, out var parentItem))
                    {
                        var itemName = path[path.Count - 1];

                        if (!parentItem.Items.TryGetValue(itemName, out var itemSettings))
                        {
                            itemSettings = new MenuItem()
                            {
                                Group = item.Metadata.DefaultGroup ?? "(default)",
                                Order = item.Metadata.DefaultOrder
                            };

                            parentItem.Items.Add(itemName, itemSettings);

                            if (!parentItem.Groups.ContainsKey(itemSettings.Group))
                            {
                                var group = _defaultGroups.SingleOrDefault(
                                    g => g.Metadata.Path.Parent == path.Parent
                                    && g.Metadata.Path[g.Metadata.Path.Count - 1] == itemSettings.Group);

                                if (group == null)
                                {
                                    parentItem.Groups.Add(itemSettings.Group, new MenuGroup());
                                }
                                else
                                {
                                    parentItem.Groups.Add(itemSettings.Group, new MenuGroup() { Order = group.Metadata.DefaultOrder });
                                }
                            }
                        }

                        currentDepthMenuItems.Add(path, itemSettings);
                    }
                }

                parentDepthMenuItems = currentDepthMenuItems;
            }

            _menuSettingsService.SaveMenuSettings();

            foreach (var itemModel in GetChildren(new MenuPath(Array.Empty<string>()), rootSettings, 1))
            {
                builder.Add(itemModel);
            }

            return _viewModel = new MenuViewModel(builder.ToImmutable());

            IEnumerable<MenuItemModel> GetChildren(MenuPath parent, MenuItem itemSettings, int depth)
            {
                var depthItems = itemsByDepth.SingleOrDefault(d => d.Key == depth);

                var skipSeparator = true;

                foreach (var group in itemSettings.Items
                    .GroupBy(i => i.Value.Group)
                    .OrderBy(g => itemSettings.Groups[g.Key].Order))
                {
                    if (!skipSeparator)
                    {
                        yield return new MenuItemSeparatorModel();
                    }

                    skipSeparator = true;

                    foreach (var item in group.OrderBy(i => i.Value.Order))
                    {
                        if (!item.Value.Enabled)
                        {
                            continue;
                        }

                        var menuItem = _menuItems.SingleOrDefault(
                            i => i.Metadata.Path.Parent == parent && i.Metadata.Path[i.Metadata.Path.Count - 1] == item.Key);

                        if (menuItem != null)
                        {
                            skipSeparator = false;

                            yield return new MenuItemModel(
                                menuItem, GetChildren(menuItem.Metadata.Path, item.Value, depth + 1));
                        }
                    }
                }
            }
        }
    }
}
