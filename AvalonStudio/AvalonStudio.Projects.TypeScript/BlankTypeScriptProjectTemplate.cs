using System;
using System.Threading.Tasks;

namespace AvalonStudio.Projects.TypeScript
{
    public class BlankTypeScriptProjectTemplate : IProjectTemplate
    {
        public string DefaultProjectName => "TypeScriptProject";

        public string Description => "Creates a blank TypeScript project";

        public string Title => "Empty TypeScript Project";

        public Task<IProject> Generate(ISolution solution, string name)
        {
            throw new NotImplementedException();
        }
    }
}