namespace AvalonStudio.Projects
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public class AvalonStudioSolutionType : ISolutionType
    {
        public string Description => "AvalonStudio Solution";

        public List<string> Extensions { get; } = new List<string> { "asln" };

        public void Activation()
        {
        }

        public void BeforeActivation()
        {
        }

        public async Task<ISolution> LoadAsync(string path)
        {
            return await Task.Factory.StartNew(() => { return AvalonStudioSolution.Load(path); });
        }
    }
}