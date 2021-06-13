using AvalonStudio.Projects;
using System.IO;
using System.Threading.Tasks;

namespace AvalonStudio.Extensibility.Projects.OpenableItems
{
    [ExportSolutionType("csproj")]
    class SingleProjectSolution : IOpenableItem
    {
        public string Description => "CSharp project.";

        public async Task<ISolution> LoadAsync(string path)
        {
            var solution = VisualStudioSolution.Create(string.Empty, Path.GetFileNameWithoutExtension(path), save: false);
            var projIdentifier = ProjectUtils.GetProjectTypeGuidForProject(path);
            
            if (!projIdentifier.HasValue)
                return null;

            var project = await ProjectUtils.LoadProjectFileAsync(solution, projIdentifier.Value, path);
            solution.AddItem(project, projIdentifier);

            return solution;
        }
    }
}
