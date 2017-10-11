namespace AvalonStudio.Projects
{
    public interface ISolutionItem : IItem
    {
        ISolution Solution { get; set; }

        ISolutionFolder Parent { get; set; }
    }
}
