using System.ComponentModel;
using AvalonStudio.Menus;

namespace AvalonStudio.Toolbars
{
    public class ToolbarDefaultGroupMetadata
    {
        public string ToolbarName { get; set; }
        public MenuPath Path { get; set; }

        [DefaultValue(50000)]
        public int DefaultOrder { get; set; }
    }
}