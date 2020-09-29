using AvalonStudio.Projects;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AvalonStudio.Extensibility.Projects.OpenableItems
{
    [ExportSolutionType("csproj")]
    class SingleProjectSolution : IOpenableItem
    {
        public string Description => "CSharp project.";

        public async Task<ISolution> LoadAsync(string path)
        {
            var solution = VisualStudioSolution.Create();
            var projIdentifier = ProjectUtils.GetProjectTypeGuidForProject(path);
            
            if (!projIdentifier.HasValue)
                return null;

            var project = await ProjectUtils.LoadProjectFileAsync(solution, projIdentifier.Value, path);
            solution.AddItem(project, projIdentifier);

            return solution;
        }
    }
}
