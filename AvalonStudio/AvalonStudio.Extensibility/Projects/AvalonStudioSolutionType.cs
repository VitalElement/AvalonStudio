using System.Threading.Tasks;

namespace AvalonStudio.Projects
{
    [ExportSolutionType("asln")]
    public class AvalonStudioSolutionType : ISolutionType
    {
        public string Description => "AvalonStudio Solution";

        public async Task<ISolution> LoadAsync(string path)
        {
            return await AvalonStudioSolution.LoadAsync(path);
        }
    }
}