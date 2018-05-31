using System.Collections.Generic;

namespace AvalonStudio.Menus.Settings
{
    public class MenuSettings
    {
        public Dictionary<string, MenuItem> Menus { get; set; }

        public MenuSettings()
        {
            Menus = new Dictionary<string, MenuItem>();
        }
    }
}
