using AvalonStudio.Menus;
using System.ComponentModel;

namespace AvalonStudio.MainMenu
{
    public class MainMenuDefaultGroupMetadata
    {
        public MenuPath Path { get; set; }

        [DefaultValue(50000)]
        public int DefaultOrder { get; set; }
    }
}