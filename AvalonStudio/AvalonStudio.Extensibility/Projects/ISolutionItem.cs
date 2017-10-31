using System;

namespace AvalonStudio.Projects
{
    public interface ISolutionItem : IItem, IComparable<ISolutionItem>
    {
        Guid Id { get; set; }

        ISolution Solution { get; set; }

        ISolutionFolder Parent { get; set; }
    }
}
