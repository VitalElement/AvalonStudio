namespace AvalonStudio.Projects.OmniSharp
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class VisualStudioSolutionType : ISolutionType
    {
        public string Description
        {
            get
            {
                return "Visual Studio Solution Files";
            }
        }

        public List<string> Extensions
        {
            get
            {
                return new List<string> { "sln" };
            }
        }

        public async Task<ISolution> LoadAsync(string path)
        {
            return await OmniSharpSolution.Create(path);
        }
    }
}
