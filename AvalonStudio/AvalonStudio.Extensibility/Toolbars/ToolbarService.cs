using AvalonStudio.Menus;
using AvalonStudio.Menus.Models;
using AvalonStudio.Menus.Settings;
using AvalonStudio.Toolbars.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Composition;
using System.Linq;

namespace AvalonStudio.Toolbars
{
    [Export]
    [Shared]
    public class ToolbarService
    {
        private readonly MenuSettingsService _menuSettingsService;

        private readonly IEnumerable<Lazy<Toolbar, ToolbarMetadata>> _toolbars;
        private readonly IEnumerable<Lazy<IMenuItem, ToolbarItemMetadata>> _toolbarItems;
        private readonly IEnumerable<Lazy<object, ToolbarDefaultGroupMetadata>> _defaultGroups;

        private ImmutableDictionary<string, ToolbarViewModel> _models;

        [ImportingConstructor]
        public ToolbarService(
            MenuSettingsService menuSettingsService,
            [ImportMany] IEnumerable<Lazy<Toolbar, ToolbarMetadata>> toolbars,
            [ImportMany(ExportContractNames.Toolbars)] IEnumerable<Lazy<IMenuItem, ToolbarItemMetadata>> toolbarItems,
            [ImportMany(ExportContractNames.Toolbars)] IEnumerable<Lazy<object, ToolbarDefaultGroupMetadata>> defaultGroups)
        {
            _menuSettingsService = menuSettingsService;

            _toolbars = toolbars;
            _toolbarItems = toolbarItems;
            _defaultGroups = defaultGroups;
        }

        public IImmutableDictionary<string, ToolbarViewModel> GetToolbars()
        {
            if (_models != null)
            {
                return _models;
            }

            var rootItem = _menuSettingsService.GetRootSettings(ExportContractNames.Toolbars);
            var builder = ImmutableDictionary.CreateBuilder<string, ToolbarViewModel>();

            var groupedToolbarItems = _toolbarItems.GroupBy(i => i.Metadata.ToolbarName);

            foreach (var toolbar in _toolbars)
            {
                var items = rootItem.Items;
                var toolbarItems = groupedToolbarItems.SingleOrDefault(i => i.Key == toolbar.Metadata.Name);

                var toolbarItemsByName = new Dictionary<string, Lazy<IMenuItem>>();

                foreach (var item in toolbarItems)
                {
                    var itemName = item.Metadata.Path[0];

                    if (!items.TryGetValue(itemName, out var itemSettings))
                    {
                        itemSettings =
                            new MenuItem()
                            {
                                Group = item.Metadata.DefaultGroup ?? "(default)",
                                Order = item.Metadata.DefaultOrder
                            };

                        items.Add(itemName, itemSettings);
                    }

                    if (!rootItem.Groups.ContainsKey(itemSettings.Group))
                    {
                        var group = _defaultGroups.SingleOrDefault(
                            g => g.Metadata.ToolbarName == toolbar.Metadata.Name
                            && g.Metadata.Path[0] == itemSettings.Group);

                        if (group == null)
                        {
                            rootItem.Groups.Add(itemSettings.Group, new MenuGroup());
                        }
                        else
                        {
                            rootItem.Groups.Add(itemSettings.Group, new MenuGroup() { Order = group.Metadata.DefaultOrder });
                        }
                    }

                    toolbarItemsByName.Add(itemName, item);
                }

                _menuSettingsService.SaveMenuSettings();

                var modelsBuilder = ImmutableList.CreateBuilder<MenuItemModel>();

                var skipSeparator = true;

                foreach (var group in items
                    .GroupBy(i => i.Value.Group)
                    .OrderBy(i => rootItem.Groups[i.Key].Order))
                {
                    if (!skipSeparator)
                    {
                        modelsBuilder.Add(new MenuItemSeparatorModel());
                    }

                    skipSeparator = true;

                    foreach (var item in group.OrderBy(i => i.Value.Order))
                    {
                        if (item.Value.Enabled
                            && toolbarItemsByName.TryGetValue(item.Key, out var menuItem))
                        {
                            skipSeparator = false;
                            modelsBuilder.Add(new MenuItemModel(menuItem, null));
                        }
                    }
                }

                builder.Add(toolbar.Metadata.Name, new ToolbarViewModel(toolbar, modelsBuilder.ToImmutable()));
            }

            return _models = builder.ToImmutable();
        }
    }
}
