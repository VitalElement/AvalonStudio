using AvalonStudio.Projects;
using System;
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
                { "ProjectName", project.Name },
                { "ProjectPath", Path.Combine(project.CurrentDirectory, project.Location).ToPlatformPath() },
                { "ProjectFileName", Path.GetFileName(project.Location) },
                { "DevEnvDir", project.ToolChain?.BinDirectory + "/".ToPlatformPath()  },
                { "ProjectDir", Path.Combine(project.CurrentDirectory, Path.GetDirectoryName(project.Location) + "/").ToPlatformPath() },
                { "SolutionFileName", Path.GetFileName(project.Solution.Location) },
                { "SolutionPath", project.Solution.Location.ToPlatformPath() },
                { "SolutionDir", Path.GetDirectoryName(project.Solution.Location) + "/".ToPlatformPath() },
                { "SolutionName", project.Solution.Name }
            };

            if (project.Executable != null)
            {
                environment.AppendRange(new Dictionary<string, string>
                {
                    { "TargetExt", Path.GetExtension(project.Executable) },
                    { "TargetFileName", Path.GetFileName(project.Executable) },
                    { "TargetName", Path.GetFileNameWithoutExtension(project.Executable) },
                    { "TargetPath", Path.Combine(project.CurrentDirectory, project.Executable).ToPlatformPath() },
                    { "OutDir", Path.Combine(project.CurrentDirectory, Path.GetDirectoryName(project.Executable) + "/").ToPlatformPath() },
                    { "TargetExt", Path.GetExtension(project.Executable) },
                    { "TargetFileName", Path.GetFileName(project.Executable) },
                    { "TargetDir", Path.Combine(project.CurrentDirectory, Path.GetDirectoryName(project.Executable) + "/").ToPlatformPath() }
                });
            }

            return environment;
        }
    }
}
