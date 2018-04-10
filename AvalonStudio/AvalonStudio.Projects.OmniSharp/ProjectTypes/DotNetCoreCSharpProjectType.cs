using AvalonStudio.Projects.OmniSharp.Roslyn;
using System.Threading.Tasks;

namespace AvalonStudio.Projects.OmniSharp.ProjectTypes
{
    [ExportProjectType("csproj", Description, ProjectTypeGuid)]
    internal class DotNetCoreCSharpProjectType : IProjectType
    {
        private const string ProjectTypeGuid = "9a19103f-16f7-4668-be54-9a1e7a4f7556";
        private const string Description = ".NET Core C# Project";

        public async Task<IProject> LoadAsync(ISolution solution, string filePath)
        {
            await RoslynWorkspace.CreateWorkspaceAsync(solution);

            return await OmniSharpProject.Create(solution, filePath);
        }
    }
}
