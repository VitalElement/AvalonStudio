namespace AvalonStudio.Toolchains
{
    using Projects;
    using System.Collections.Generic;
    using System.ComponentModel.Composition;
    using System.Threading.Tasks;
    using Utils;

    [InheritedExport(typeof(IToolChain))]
    public interface IToolChain
    {
        Task<bool> Build(IConsole console, IProject project);

        Task Clean(IConsole console, IProject project);        

        List<string> Includes { get; }
    }
}

    
