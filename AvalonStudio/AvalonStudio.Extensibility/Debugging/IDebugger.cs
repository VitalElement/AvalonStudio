namespace AvalonStudio.Debugging
{
    using AvalonStudio.Projects;

    /// <summary>
    /// Provides method for dealing with managing debuggers.
    /// </summary>
    public interface IDebugger
    {
        object GetSettingsControl(IProject project);
    }
}
