namespace AvalonStudio.Projects
{
    public interface IProjectItem
    {
        string Name { get; }

        IProject Project { get; set; }
        IProjectFolder Parent { get; set; }
    }
}
