using AvalonStudio.Projects;
using AvalonStudio.Utils;
using System.Threading.Tasks;

namespace AvalonStudio.Extensibility
{
    public interface IInstallable
    {
        Task<bool> InstallAsync(IConsole console, IProject project);
    }
}