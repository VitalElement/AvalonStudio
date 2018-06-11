using AvalonStudio.Shell.Extensibility.Platforms;
using AvalonStudio.Utils;
using System;
using System.Composition;
using System.IO;

namespace AvalonStudio.Menus.Settings
{
    [Export]
    [Shared]
    public class MenuSettingsService
    {
        private const string MenuSettingsFileName = "MenuSettings.json";

        private static readonly string MenuSettingsFilePath = Path.Combine(Platform.SettingsDirectory, MenuSettingsFileName);

        private readonly Lazy<MenuSettings> _menuSettings;

        public MenuSettingsService()
        {
            _menuSettings = new Lazy<MenuSettings>(LoadMenuSettings);
        }

        public MenuItem GetRootSettings(string menuName)
        {
            var menuSettings = _menuSettings.Value;

            if (!menuSettings.Menus.TryGetValue(menuName, out var rootItem))
            {
                rootItem = new MenuItem();
                menuSettings.Menus.Add(menuName, rootItem);
            }

            return rootItem;
        }

        private MenuSettings LoadMenuSettings()
        {
            if (File.Exists(MenuSettingsFilePath))
            {
                return SerializedObject.Deserialize<MenuSettings>(MenuSettingsFilePath)
                    ?? new MenuSettings();
            }

            return new MenuSettings();
        }

        public void SaveMenuSettings() => SerializedObject.Serialize(MenuSettingsFilePath, _menuSettings.Value);
    }
}
