using AvalonStudio.Projects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AvalonStudio.Shell
{
    public class SolutionChangedEventArgs : EventArgs
    {
        public ISolution OldValue { get; set; }
        public ISolution NewValue { get; set; }
    }
}
