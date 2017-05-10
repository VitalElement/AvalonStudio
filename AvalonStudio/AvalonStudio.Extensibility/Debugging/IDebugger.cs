namespace AvalonStudio.Debugging
{
    using AvalonStudio.Extensibility.Plugin;
    using AvalonStudio.Projects;

    /// <summary>
    /// Provides method for dealing with managing debuggers.
    /// </summary>
    public interface IDebugger : IExtension, IInstallable
    {
        object GetSettingsControl(IProject project);
    }
}
