using AvalonStudio.Extensibility;
using AvalonStudio.Shell;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace AvalonStudio.Projects
{
    public class Project
    {
        public static async Task<IProject> LoadProjectFileAsync (ISolution solution, string fileName)
        {
            var shell = IoC.Get<IShell>();

            var extension = Path.GetExtension(fileName).Remove(0, 1);

            var projectType = shell.ProjectTypes.FirstOrDefault(p => p.Extensions.Contains(extension));

            if(projectType != null)
            {
                return await LoadProjectFileAsync(solution, projectType.ProjectTypeId, fileName);
            }

            return new UnsupportedProjectType(fileName);
        }

        public static async Task<IProject> LoadProjectFileAsync(ISolution solution, Guid projectTypeId, string fileName)
        {   
            var projectType = projectTypeId.GetProjectType();

            if (projectType != null)
            {
                return await projectType.LoadAsync(solution, fileName);
            }

            return new UnsupportedProjectType(fileName);
        }
    }
}
