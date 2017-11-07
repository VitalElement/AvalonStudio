using AvalonStudio.Projects;
using System;

namespace AvalonStudio.Shell
{
    public enum BuildType
    {
        Clean,
        Build
    }

    public class BuildEventArgs : EventArgs
    {
        public BuildEventArgs(BuildType type, IProject project)
        {
            Type = type;
            Project = project;
        }

        public BuildType Type { get; }

        public IProject Project { get; }
    }
}
