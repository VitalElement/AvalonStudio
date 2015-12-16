namespace AvalonStudio.Toolchains
{
    using AvalonStudio.Utils;
    using Projects;
    using System.Threading.Tasks;

    public interface IToolChain
    {
        Task<bool> Build(IConsole console, Project project);

        Task Clean(IConsole console, Project project);        
    }
}

    
