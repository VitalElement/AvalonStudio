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
                { "TargetPath", Path.Combine(project.CurrentDirectory, project.Executable).ToPlatformPath() },
                { "OutDir", Path.Combine(project.CurrentDirectory,Path.GetDirectoryName(project.Executable) + "/").ToPlatformPath() },
                { "ProjectName", project.Name },
                { "ProjectPath", Path.Combine(project.CurrentDirectory, project.Location).ToPlatformPath() },
                { "ProjectFileName", Path.GetFileName(project.Location) },
                { "TargetExt", Path.GetExtension(project.Executable) },
                { "TargetFileName", Path.GetFileName(project.Executable) },
                { "DevEnvDir", project.ToolChain?.BinDirectory + "/".ToPlatformPath()  },
                { "TargetDir", Path.Combine(project.CurrentDirectory,Path.GetDirectoryName(project.Executable) + "/").ToPlatformPath() },
                { "ProjectDir", Path.Combine(project.CurrentDirectory,Path.GetDirectoryName(project.Location) + "/").ToPlatformPath() },
                { "SolutionFileName", Path.GetFileName(project.Solution.Location) },
                { "SolutionPath", project.Solution.Location.ToPlatformPath() },
                { "SolutionDir", Path.GetDirectoryName(project.Solution.Location) + "/".ToPlatformPath() },
                { "SolutionName", project.Solution.Name }
            };

            return environment;
        }
    }
}
