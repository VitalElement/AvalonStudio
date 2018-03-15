using System.Threading.Tasks;

namespace AvalonStudio.Projects.CPlusPlus
{
    [ExportProjectType("acproj", Description, ProjectTypeGuid)]
    internal class CPlusPlusProjectType : IProjectType
    {
        private const string ProjectTypeGuid = "da891b1a-e1a3-4a1a-83cd-252f07b636ed";
        private const string Description = "Avalon Studio C/C++ Project";

        public Task<IProject> LoadAsync(ISolution solution, string filePath)
        {
            return Task.Run(()=> CPlusPlusProject.LoadFromFile(filePath) as IProject);
        }
    }
}