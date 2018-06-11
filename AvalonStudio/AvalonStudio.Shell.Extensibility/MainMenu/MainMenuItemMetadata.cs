using AvalonStudio.Menus;
using System.ComponentModel;

namespace AvalonStudio.MainMenu
{
    public class MainMenuItemMetadata
    {
        public MenuPath Path { get; set; }

        [DefaultValue(null)]
        public string DefaultGroup { get; set; }

        [DefaultValue(50000)]
        public int DefaultOrder { get; set; }
    }
}
