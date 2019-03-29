using AvalonStudio.Projects;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace AvalonStudio.Platforms
{
    public static class InterfaceExtensions
    {
        /// <summary>
        /// Waits asynchronously for the process to exit.
        /// </summary>
        /// <param name="process">The process to wait for cancellation.</param>
        /// <param name="cancellationToken">A cancellation token. If invoked, the task will return 
        /// immediately as canceled.</param>
        /// <returns>A Task representing waiting for the process to end.</returns>
        public static Task WaitForExitAsync(this Process process,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            var tcs = new TaskCompletionSource<object>();
            process.EnableRaisingEvents = true;
            process.Exited += (sender, args) => tcs.TrySetResult(null);
            if (cancellationToken != default(CancellationToken))
                cancellationToken.Register(tcs.SetCanceled);

            return tcs.Task;
        }

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
                { "SolutionName", project.Solution.Name },
                { "CopyCommand", Platform.PlatformIdentifier == PlatformID.Win32NT ? "copy" : "cp" }
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
                    { "TargetDir", Path.Combine(project.CurrentDirectory, Path.GetDirectoryName(project.Executable) + "/").ToPlatformPath() }
                });
            }

            return environment;
        }
    }
}
