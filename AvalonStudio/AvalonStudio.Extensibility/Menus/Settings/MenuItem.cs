using System.Collections.Generic;

namespace AvalonStudio.Menus.Settings
{
    public class MenuItem
    {
        public bool Enabled { get;set; }

        public int Order { get; set; }
        public string Group { get; set; }

        public Dictionary<string, MenuGroup> Groups { get; set; }
        public Dictionary<string, MenuItem> Items { get; set; }

        public MenuItem()
        {
            Enabled = true;

            Groups = new Dictionary<string, MenuGroup>();
            Items = new Dictionary<string, MenuItem>();
        }
    }
}
