namespace AvalonStudio.Projects
{    
    public enum Language
    {
        C,
        Cpp
    }

    public interface ISourceFile : IProjectItem
    {
        string File { get; }
        string Location { get; }
        Language Language { get; }
    }
}
