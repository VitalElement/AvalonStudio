namespace AvalonStudio.TestFrameworks.Catch
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Projects;
    using System.Diagnostics;
    using Utils;
    using System.IO;
    using Platform;

    public class CatchTestFramework : ITestFramework
    {
        public async Task<IEnumerable<Test>> EnumerateTestsAsync(IConsole console, IProject project)
        {
            List<Test> result = new List<Test>();

            if(project.TestFramework != null && project.TestFramework is CatchTestFramework)
            {
                if (await project.ToolChain?.Build(console, project))
                {
                    var startInfo = new ProcessStartInfo();
                    startInfo.FileName = Path.Combine(project.CurrentDirectory, project.Executable).ToPlatformPath();
                    startInfo.Arguments = "--list-test-names-only";
                    
                    // Hide console window
                    startInfo.UseShellExecute = false;
                    startInfo.RedirectStandardOutput = true;
                    startInfo.RedirectStandardError = true;
                    startInfo.CreateNoWindow = true;

                    console.Write(string.Format("Enumerating {0} for tests...", project.Executable));

                    using (var process = Process.Start(startInfo))
                    {
                        process.OutputDataReceived += (sender, e) =>
                        {
                            console.WriteLine(e.Data);
                        };

                        process.ErrorDataReceived += (sender, e) =>
                        {
                            if (e.Data != null)
                            {
                                console.WriteLine();
                                console.WriteLine(e.Data);
                            }
                        };

                        process.BeginOutputReadLine();

                        process.BeginErrorReadLine();

                        process.WaitForExit();

                        console.WriteLine("Done");
                    }
                }
                else
                {
                    console.WriteLine("Unable to run tests, build failed.");
                }
            }

            return result;
        }
    }
}
