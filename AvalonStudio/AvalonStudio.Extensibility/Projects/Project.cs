using AvalonStudio.Extensibility;
using AvalonStudio.Extensibility.Studio;
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
            var extension = Path.GetExtension(fileName);

            var projectType = IoC.Get<IStudio>().ProjectTypes.FirstOrDefault(
                p => extension.EndsWith(p.Metadata.DefaultExtension));

            if (projectType == null)
            {
                projectType = IoC.Get<IStudio>().ProjectTypes.FirstOrDefault(
                    p => p.Metadata.PossibleExtensions.Any(e => extension.EndsWith(e)));
            }

            return projectType?.Metadata.ProjectTypeGuid;
        }

        public static async Task<IProject> LoadProjectFileAsync(ISolution solution, Guid projectTypeId, string fileName)
        {   
            var projectType = projectTypeId.GetProjectType();

            if (projectType != null)
            {
                try
                {
                    return await projectType.LoadAsync(solution, fileName);
                }
                catch (Exception)
                {

                }
            }

            return new UnsupportedProjectType(solution, fileName);
        }
    }
}
