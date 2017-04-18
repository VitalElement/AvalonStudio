namespace AvalonStudio.Projects.OmniSharp
{
    using System.Collections.Generic;
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

        public void Activation()
        {
        }

        public void BeforeActivation()
        {
        }

        public async Task<ISolution> LoadAsync(string path)
        {
            return await OmniSharpSolution.Create(path);
        }
    }
}