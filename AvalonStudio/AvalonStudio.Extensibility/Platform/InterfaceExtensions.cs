using AvalonStudio.Projects;
using System.Collections;
using System.Collections.Generic;
using System.IO;

namespace AvalonStudio.Platforms
{
    public static class InterfaceExtensions
    {
        public static IDictionary AppendRange(this IDictionary dictionary, IDictionary appendData)
        {
            foreach (var key in appendData.Keys)
            {
                if (!dictionary.Contains(key))
                {
                    dictionary.Add(key, appendData[key]);
                }
            }

            return dictionary;
        }

        public static IDictionary GetEnvironmentVariables(this IProject project)
        {
            var environment = new Dictionary<string, string>
            {
                { "TargetPath", project.Executable },
                { "OutDir", Path.GetDirectoryName(project.Executable) + "/" },
                { "ProjectName", project.Name },
                { "ProjectPath", project.Location },
                { "ProjectFilName", Path.GetFileName(project.Location) },
                { "TargetExt", Path.GetExtension(project.Executable) },
                { "TargetFileName", Path.GetFileName(project.Executable) },
                { "DevEnvDir", project.ToolChain?.BinDirectory + "/"  },
                { "TargetDir", Path.GetDirectoryName(project.Executable) + "/" },
                { "ProjectDir", Path.GetDirectoryName(project.Location) + "/" },
                { "SolutionFileName", Path.GetFileName(project.Solution.Location) },
                { "SolutionPath", project.Solution.Location },
                { "SolutionDir", Path.GetDirectoryName(project.Solution.Location) + "/" },
                { "SolutionName", project.Solution.Name }
            };

            return environment;
        }
    }
}
