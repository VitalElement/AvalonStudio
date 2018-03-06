using System.Threading.Tasks;

namespace AvalonStudio.Projects
{
    [ExportSolutionType("sln")]
    public class VisualStudioSolutionType : ISolutionType
    {
        public string Description => "Solution File";

        public async Task<ISolution> LoadAsync(string path)
        {
            return await Task.Run(() => VisualStudioSolution.Load(path));
        }
    }
}
