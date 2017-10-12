using AvalonStudio.Projects;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AvalonStudio.Extensibility.Projects
{
    public class VisualStudioSolutionType : ISolutionType
    {
        public List<string> Extensions { get; } = new List<string> { "sln" };

        public string Description => "Solution File";

        public void Activation()
        {
        }

        public void BeforeActivation()
        {
        }

        public Task<ISolution> LoadAsync(string path)
        {
            throw new NotImplementedException();
        }
    }
}
