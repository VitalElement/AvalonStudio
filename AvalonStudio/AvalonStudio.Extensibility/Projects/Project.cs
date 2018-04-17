using AvalonStudio.Extensibility;
using AvalonStudio.Shell;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace AvalonStudio.Projects
{
    public class ProjectUtils
    {
        public static Guid? GetProjectTypeGuidForProject(string fileName)
        {
            var shell = IoC.Get<IShell>();

            var extension = Path.GetExtension(fileName);

            var projectType = shell.ProjectTypes.FirstOrDefault(
                p => extension.EndsWith(p.Metadata.DefaultExtension));

            if (projectType == null)
            {
                projectType = shell.ProjectTypes.FirstOrDefault(
                    p => p.Metadata.PossibleExtensions.Any(e => extension.EndsWith(e)));
            }

            return projectType?.Metadata.ProjectTypeGuid;
        }

        public static async Task<IProject> LoadProjectFileAsync(ISolution solution, Guid projectTypeId, string fileName)
        {   
            var projectType = projectTypeId.GetProjectType();

            if (projectType != null)
            {
                return await projectType.LoadAsync(solution, fileName);
            }

            return new UnsupportedProjectType(solution, fileName);
        }
    }
}
