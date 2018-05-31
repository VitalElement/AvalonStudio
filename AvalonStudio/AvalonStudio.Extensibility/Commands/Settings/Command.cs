using System.Collections.Generic;

namespace AvalonStudio.Commands.Settings
{
    public class Command
    {
        public List<string> KeyGestures { get; set; }

        public Command()
        {
            KeyGestures = new List<string>();
        }
    }
}
