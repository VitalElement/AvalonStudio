using AvalonStudio.Toolchains;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AvalonStudio.Projects;
using AvalonStudio.Utils;
using AvalonStudio.Projects.CPlusPlus;
using Avalonia.Controls;
using System.IO;
using AvalonStudio.Platforms;
using System.Diagnostics;

namespace AvalonStudio.Toolchains.BitThunder
{
    public class BitThunderToolchain : IToolChain, IGDBToolchain
    {
        private string BaseDirectory
        {
            get { return Path.Combine(Platform.ReposDirectory, "AvalonStudio.Toolchains.BitThunder"); }
        }

        public string Description
        {
            get
            {
                return "BitThunder Toolchain.";
            }
        }

        public IList<string> Includes
        {
            get
            {
                return new List<string>();
            }
        }

        public string Name
        {
            get
            {
                return "BitThunder Toolchain";
            }
        }

        public Version Version
        {
            get
            {
                return new Version();
            }
        }

        public string GDBExecutable
        {
            get
            {
                return Path.Combine(BaseDirectory, "0.6.2", "bin", "arm-eabi-bitthunder-gdb" + Platform.ExecutableExtension);
            }
        }

        public void Activation()
        {

        }

        public void BeforeActivation()
        {

        }

        public static string AppendPath(string path, string addition)
        {
            if (path[path.Length - 1] != ';')
            {
                path += ";";
            }

            return path + addition;
        }

        public async Task<bool> Build(IConsole console, IProject project, string label = "")
        {
            bool result = false;

            var startInfo = new ProcessStartInfo();

            startInfo.FileName = Path.Combine(BaseDirectory, "bin", "make" + Platform.ExecutableExtension);
            startInfo.WorkingDirectory = project.CurrentDirectory;            
            startInfo.EnvironmentVariables["PATH"] = startInfo.EnvironmentVariables["PATH"] = AppendPath(startInfo.EnvironmentVariables["PATH"], Path.Combine(BaseDirectory, "bin;"));
            startInfo.EnvironmentVariables["PATH"] = startInfo.EnvironmentVariables["PATH"] = AppendPath(startInfo.EnvironmentVariables["PATH"], Path.Combine(BaseDirectory, "0.6.2\\bin;"));
            startInfo.EnvironmentVariables["PATH"] = startInfo.EnvironmentVariables["PATH"] = AppendPath(startInfo.EnvironmentVariables["PATH"], Path.Combine(BaseDirectory, "Python34;"));
            startInfo.Arguments = "-j4";
            if (!File.Exists(startInfo.FileName))
            {
                console.WriteLine("Unable to find compiler (" + startInfo.FileName + ") Please check project compiler settings.");
            }
            else
            {
                // Hide console window
                startInfo.UseShellExecute = false;
                startInfo.RedirectStandardOutput = true;
                startInfo.RedirectStandardError = true;
                startInfo.CreateNoWindow = true;

                //console.WriteLine (Path.GetFileNameWithoutExtension(startInfo.FileName) + " " + startInfo.Arguments);

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

                    await Task.Factory.StartNew(() =>
                    {
                        process.WaitForExit();
                    });

                    if(process.ExitCode == 0)
                    {
                        result = true;
                        project.Executable = project.Location.MakeRelativePath(Path.Combine(project.CurrentDirectory, "vmthunder.elf")).ToAvalonPath();
                    }

                }
            }

            return result;
        }

        public bool CanHandle(IProject project)
        {
            bool result = false;

            if (project is CPlusPlusProject)
            {
                result = true;
            }

            return result;
        }

        public async Task Clean(IConsole console, IProject project)
        {
            var startInfo = new ProcessStartInfo();

            startInfo.FileName = Path.Combine(BaseDirectory, "bin", "make" + Platform.ExecutableExtension);
            startInfo.WorkingDirectory = project.CurrentDirectory;
            startInfo.EnvironmentVariables["PATH"] = startInfo.EnvironmentVariables["PATH"] = AppendPath(startInfo.EnvironmentVariables["PATH"], Path.Combine(BaseDirectory, "bin;"));
            startInfo.EnvironmentVariables["PATH"] = startInfo.EnvironmentVariables["PATH"] = AppendPath(startInfo.EnvironmentVariables["PATH"], Path.Combine(BaseDirectory, "0.6.2\\bin;"));
            startInfo.EnvironmentVariables["PATH"] = startInfo.EnvironmentVariables["PATH"] = AppendPath(startInfo.EnvironmentVariables["PATH"], Path.Combine(BaseDirectory, "Python34;"));
            startInfo.Arguments = "clean";

            if (!File.Exists(startInfo.FileName))
            {
                console.WriteLine("Unable to find compiler (" + startInfo.FileName + ") Please check project compiler settings.");
            }
            else
            {
                // Hide console window
                startInfo.UseShellExecute = false;
                startInfo.RedirectStandardOutput = true;
                startInfo.RedirectStandardError = true;
                startInfo.CreateNoWindow = true;

                //console.WriteLine (Path.GetFileNameWithoutExtension(startInfo.FileName) + " " + startInfo.Arguments);

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

                    await Task.Factory.StartNew(() =>
                    {
                        process.WaitForExit();
                    });
                }
            }
        }

        public IList<object> GetConfigurationPages(IProject project)
        {
            return new List<object>();
        }


        public void ProvisionSettings(IProject project)
        {

        }

        UserControl IToolChain.GetSettingsControl(IProject project)
        {
            // TODO this api MUST be removed.
            throw new NotImplementedException();
        }
    }
}
