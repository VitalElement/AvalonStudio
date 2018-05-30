using System.Collections.Generic;

namespace AvalonStudio.Commands.Settings
{
    public class CommandSettings
    {
        public Dictionary<string, Command> Commands { get; set; }

        public CommandSettings()
        {
            Commands = new Dictionary<string, Command>();
        }
    }
}
