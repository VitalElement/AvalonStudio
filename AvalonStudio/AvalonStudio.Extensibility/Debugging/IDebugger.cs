using AvalonStudio.Extensibility;
using AvalonStudio.Projects;

namespace AvalonStudio.Debugging
{
    /// <summary>
    /// Provides method for dealing with managing debuggers.
    /// </summary>
    public interface IDebugger : IInstallable, IExtension
    {
        object GetSettingsControl(IProject project);

        string BinDirectory { get; }
    }
}
