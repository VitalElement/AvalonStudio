namespace AvalonStudio.Toolchains
{
    using AvalonStudio.Utils;
    using Projects;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IToolChain
    {
        Task<bool> Build(IConsole console, IProject project);

        Task Clean(IConsole console, IProject project);        

        List<string> Includes { get; }
    }
}

    
