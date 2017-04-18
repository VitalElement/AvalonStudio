using AvalonStudio.Projects;
using System;

namespace AvalonStudio.Shell
{
    public class SolutionChangedEventArgs : EventArgs
    {
        public ISolution OldValue { get; set; }
        public ISolution NewValue { get; set; }
    }
}