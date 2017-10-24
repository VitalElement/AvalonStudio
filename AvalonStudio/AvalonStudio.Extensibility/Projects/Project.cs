using AvalonStudio.Extensibility;
using AvalonStudio.Shell;
using System;
using System.IO;
using System.Linq;

namespace AvalonStudio.Projects
{
    public class Project
    {
        public static IProject LoadProjectFile (string fileName)
        {
            var shell = IoC.Get<IShell>();

            var extension = Path.GetExtension(fileName).Remove(0, 1);

            var projectType = shell.ProjectTypes.FirstOrDefault(p => p.Extensions.Contains(extension));

            if(projectType != null)
            {
                return LoadProjectFile(projectType.ProjectTypeId, fileName);
            }

            return new UnsupportedProjectType(fileName);
        }

        public static IProject LoadProjectFile(Guid projectTypeId, string fileName)
        {   
            var projectType = projectTypeId.GetProjectType();

            if (projectType != null)
            {
                return projectType.Load(fileName);
            }

            return new UnsupportedProjectType(fileName);
        }
    }
}
