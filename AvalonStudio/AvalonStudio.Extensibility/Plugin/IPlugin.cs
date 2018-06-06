using AvalonStudio.Projects;
using AvalonStudio.Utils;
using System;
using System.Threading.Tasks;

namespace AvalonStudio.Extensibility.Plugin
{
    public interface IInstallable
    {
        Task<bool> InstallAsync(IConsole console, IProject project);
    }
}