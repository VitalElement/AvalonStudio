namespace AvalonStudio.Projects
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public class AvalonStudioSolutionType : ISolutionType
    {
        public string Description
        {
            get
            {
                return "AvalonStudio Solution";
            }
        }

        public List<string> Extensions
        {
            get
            {
                return new List<string> { "asln" };
            }
        }

        public async Task<ISolution> LoadAsync(string path)
        {
            return await Task.Factory.StartNew(() => { return Solution.Load(path); });
        }
    }
}
