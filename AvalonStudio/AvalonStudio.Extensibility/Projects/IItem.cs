namespace AvalonStudio.Projects
{
    public interface IItem
    {
        string Name { get; set; }

        bool CanRename { get; }
    }
}
