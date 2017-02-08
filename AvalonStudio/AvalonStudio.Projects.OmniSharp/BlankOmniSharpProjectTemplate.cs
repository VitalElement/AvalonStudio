using System;
using System.Threading.Tasks;

namespace AvalonStudio.Projects.OmniSharp
{
    public class BlankOmniSharpProjectTemplate : IProjectTemplate
    {
        public string DefaultProjectName => "OmniSharpProject";

        public string Description => "Creates an empty C# project that uses OmniSharp";

        public string Title => "Empty C# Project (OmniSharp)";

        public void Activation()
        {
            
        }

        public void BeforeActivation()
        {
            
        }

        public Task<IProject> Generate(ISolution solution, string name)
        {
            throw new NotImplementedException();
        }
    }
}