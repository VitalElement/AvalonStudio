using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Dynamic;
using System.IO;
using System.Threading.Tasks;
using Avalonia.Controls;
using AvalonStudio.Platforms;
using AvalonStudio.Projects;
using AvalonStudio.Projects.DUB;
using AvalonStudio.Projects.Standard;
using AvalonStudio.Toolchains.Standard;
using AvalonStudio.Utils;

namespace AvalonStudio.Toolchains.LDC
{
	public class LDCToolchain : IToolChain
	{
		private string BaseDirectory
		{
			get { return Path.Combine(Platform.ReposDirectory, "AvalonStudio.Toolchains.LDC", "bin"); }
		}

	    private string DubDirectory
	    {
	        get { return Path.Combine(Platform.ReposDirectory, "AvalonStudio.Toolchains.D", "Dub"); }
	    }

	    public void BeforeActivation()
	    {
	        
	    }

	    public void Activation()
	    {
	        
	    }

	    public string Name { get; }
	    public Version Version { get; }
	    public string Description { get; }

	    public IList<string> Includes { get; }

	    public Task<bool> Build(IConsole console, IProject project, string label = "", IEnumerable<string> defines = null)
	    {
            var startInfo = new ProcessStartInfo();

	        startInfo.WorkingDirectory = project.CurrentDirectory;
	        startInfo.FileName = startInfo.FileName = Path.Combine(BaseDirectory, "dub" + Platform.ExecutableExtension);
            startInfo.Arguments = string.Format("--compiler=ldc2");
            startInfo.EnvironmentVariables["PATH"] = startInfo.EnvironmentVariables["PATH"] = AppendPath(startInfo.EnvironmentVariables["PATH"], DubDirectory + ";");
            startInfo.EnvironmentVariables["PATH"] = startInfo.EnvironmentVariables["PATH"] = AppendPath(startInfo.EnvironmentVariables["PATH"], BaseDirectory + ";");

            // Hide console window
            startInfo.UseShellExecute = false;
                startInfo.RedirectStandardOutput = true;
                startInfo.RedirectStandardError = true;
                startInfo.CreateNoWindow = true;

            TaskCompletionSource<bool> buildCompleted = new TaskCompletionSource<bool>();

            Task.Factory.StartNew(() =>
	        {
                using (var process = Process.Start(startInfo))
                {
                    process.OutputDataReceived += (sender, e) => { console.WriteLine(e.Data); };

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

                    buildCompleted.SetResult(process.ExitCode == 0);
                }
            });

	        return buildCompleted.Task;
	    }

        public static string AppendPath(string path, string addition)
        {
            if (path[path.Length - 1] != ';')
            {
                path += ";";
            }

            return path + addition;
        }


        public Task Clean(IConsole console, IProject project)
	    {
            var startInfo = new ProcessStartInfo();

            startInfo.WorkingDirectory = project.CurrentDirectory;
            startInfo.FileName = startInfo.FileName = Path.Combine(BaseDirectory, "dub" + Platform.ExecutableExtension);
            startInfo.Arguments = string.Format("clean --all-packages");
            startInfo.EnvironmentVariables["PATH"] = startInfo.EnvironmentVariables["PATH"] = AppendPath(startInfo.EnvironmentVariables["PATH"], DubDirectory + ";");

            // Hide console window
            startInfo.UseShellExecute = false;
            startInfo.RedirectStandardOutput = true;
            startInfo.RedirectStandardError = true;
            startInfo.CreateNoWindow = true;

            TaskCompletionSource<bool> buildCompleted = new TaskCompletionSource<bool>();

            Task.Factory.StartNew(() =>
            {
                using (var process = Process.Start(startInfo))
                {
                    process.OutputDataReceived += (sender, e) => { console.WriteLine(e.Data); };

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

                    buildCompleted.SetResult(process.ExitCode == 0);
                }
            });

            return buildCompleted.Task;
        }

	    public UserControl GetSettingsControl(IProject project)
	    {
	        throw new NotImplementedException();
	    }

	    public IList<object> GetConfigurationPages(IProject project)
	    {
	        throw new NotImplementedException();
	    }

	    public void ProvisionSettings(IProject project)
	    {
	        
	    }

	    public bool CanHandle(IProject project)
	    {
	        return project is DUBProject;
	    }
	}
}