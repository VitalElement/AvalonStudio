using System;

namespace AvalonStudio.Projects
{
    public interface IProjectItem : IComparable<IProjectItem>
    {
        string Name { get; }

        IProject Project { get; set; }
        IProjectFolder Parent { get; set; }
    }

    public static class IProjectItemExtensions
    {
        public static int CompareProjectItems(this IProjectItem item, IProjectItem other)
        {
            if (item is IReferenceFolder && !(other is IReferenceFolder))
            {
                return -1;
            }
            else if (item is IProjectFolder && !(other is IProjectFolder))
            {
                if (other is IReferenceFolder)
                {
                    return 1;
                }
                else
                {
                    return -1;
                }
            }
            else if (item is ISourceFile && !(other is ISourceFile))
            {
                return 1;
            }
            else
            {
                return item.Name.CompareTo(other.Name);
            }
        }
    }
}