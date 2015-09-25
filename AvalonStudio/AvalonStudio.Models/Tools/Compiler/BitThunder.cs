using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AvalonStudio.Models.Tools.Compiler
{
    public class BitThunderToolChain : ToolChain
    {
        public override async Task<bool> Build(IConsole console, Solutions.Project project, System.Threading.CancellationTokenSource cancellationSource)
        {
            console.Clear();

            await Task.Factory.StartNew(() =>
           {
               string objectsFile = Path.Combine(project.CurrentDirectory, "objects.mk");

               if (File.Exists(objectsFile))
               {
                   File.Delete(objectsFile);
               }

               using (var writer = File.CreateText(objectsFile))
               {
                   project.VisitAllFiles((f) =>
                   {
                       if (f.IsCodeFile && !f.IsHeaderFile)
                       {
                           var objectFile = Path.GetFileNameWithoutExtension(f.LocationRelativeToParent) +  ".o";
                           writer.WriteLine(string.Format("objs += $(APP)/{0}", objectFile));
                       }

                       return false;
                   });

                   writer.Close();
               }

               var startInfo = new ProcessStartInfo();

               startInfo.FileName = Path.Combine(project.SelectedConfiguration.ToolChain.Settings.ToolChainLocation, "bin\\make.exe");
               startInfo.WorkingDirectory = project.CurrentDirectory;
               startInfo.EnvironmentVariables["PATH"] = startInfo.EnvironmentVariables["PATH"] = AppendPath(startInfo.EnvironmentVariables["PATH"], Path.Combine(project.SelectedConfiguration.ToolChain.Settings.ToolChainLocation, "bin;"));
               startInfo.EnvironmentVariables["PATH"] = startInfo.EnvironmentVariables["PATH"] = AppendPath(startInfo.EnvironmentVariables["PATH"], Path.Combine(project.SelectedConfiguration.ToolChain.Settings.ToolChainLocation, "0.6.2\\bin;"));
               startInfo.EnvironmentVariables["PATH"] = startInfo.EnvironmentVariables["PATH"] = AppendPath(startInfo.EnvironmentVariables["PATH"], Path.Combine(project.SelectedConfiguration.ToolChain.Settings.ToolChainLocation, "Python34;"));
               startInfo.Arguments = "-j16";
               console.WriteLine("Executing: " + this.Name);

                // Hide console window
                startInfo.UseShellExecute = false;
               startInfo.RedirectStandardOutput = true;
               startInfo.RedirectStandardError = true;
               startInfo.CreateNoWindow = true;

               using (var process = Process.Start(startInfo))
               {
                   process.OutputDataReceived += (sender, e) =>
                   {
                       console.WriteLine(e.Data);
                   };

                   process.ErrorDataReceived += (sender, e) =>
                   {
                       console.WriteLine(e.Data);
                   };

                   process.BeginOutputReadLine();

                   process.BeginErrorReadLine();

                   process.WaitForExit();
               }
           });

            project.Executable = Path.Combine(project.CurrentDirectory, "vmthunder.elf");

            return true;
        }

        public async Task<bool> MenuConfig(IConsole console, Solutions.Project project, System.Threading.CancellationTokenSource cancellationSource)
        {
            console.Clear();

            await Task.Factory.StartNew(() =>
           {
               var startInfo = new ProcessStartInfo();

               startInfo.FileName = Path.Combine(project.SelectedConfiguration.ToolChain.Settings.ToolChainLocation, "bin\\make.exe");
               startInfo.WorkingDirectory = project.CurrentDirectory;
               startInfo.EnvironmentVariables["Path"] = startInfo.EnvironmentVariables["Path"] = AppendPath(startInfo.EnvironmentVariables["Path"], Path.Combine(project.SelectedConfiguration.ToolChain.Settings.ToolChainLocation, "bin;"));
               startInfo.EnvironmentVariables["Path"] = startInfo.EnvironmentVariables["Path"] = AppendPath(startInfo.EnvironmentVariables["Path"], Path.Combine(project.SelectedConfiguration.ToolChain.Settings.ToolChainLocation, "Python34;"));
               startInfo.Arguments = "menuconfig";

                // Hide console window
                startInfo.UseShellExecute = false;

               using (var process = Process.Start(startInfo))
               {
                   process.WaitForExit();
               }
           });

            return true;
        }

        public bool InitProject(IConsole console, string bitThunderPath, string projectPath)
        {
            var startInfo = new ProcessStartInfo();

            startInfo.FileName = Path.Combine(Settings.ToolChainLocation, "bin\\make.exe");
            startInfo.WorkingDirectory = projectPath;
            startInfo.EnvironmentVariables["Path"] = startInfo.EnvironmentVariables["Path"] = AppendPath(startInfo.EnvironmentVariables["Path"], Path.Combine(Settings.ToolChainLocation, "bin;"));
            startInfo.EnvironmentVariables["Path"] = startInfo.EnvironmentVariables["Path"] = AppendPath(startInfo.EnvironmentVariables["Path"], Path.Combine(Settings.ToolChainLocation, "Python34;"));
            startInfo.Arguments = string.Format("-C {0} PROJECT_DIR={1} project.init", bitThunderPath.Replace("\\", "/"), projectPath.Replace("\\", "/"));

            // Hide console window
            startInfo.UseShellExecute = false;
            startInfo.CreateNoWindow = true;

            using (var process = Process.Start(startInfo))
            {
                process.WaitForExit();
            }

            return true;
        }

        public override async Task Clean(IConsole console, Solutions.Project project, System.Threading.CancellationTokenSource cancellationSource)
        {
            console.Clear();

            await Task.Factory.StartNew(() =>
           {
               var startInfo = new ProcessStartInfo();

               startInfo.FileName = Path.Combine(project.SelectedConfiguration.ToolChain.Settings.ToolChainLocation, "bin\\make.exe");
               startInfo.WorkingDirectory = project.CurrentDirectory;
               startInfo.EnvironmentVariables["Path"] = startInfo.EnvironmentVariables["Path"] = AppendPath(startInfo.EnvironmentVariables["Path"], Path.Combine(project.SelectedConfiguration.ToolChain.Settings.ToolChainLocation, "bin;"));
               startInfo.EnvironmentVariables["Path"] = startInfo.EnvironmentVariables["Path"] = AppendPath(startInfo.EnvironmentVariables["Path"], Path.Combine(project.SelectedConfiguration.ToolChain.Settings.ToolChainLocation, "Python34;"));
               startInfo.Arguments = "clean";
               console.WriteLine("Executing: " + this.Name);

                // Hide console window
                startInfo.UseShellExecute = false;
               startInfo.RedirectStandardOutput = true;
               startInfo.RedirectStandardError = true;
               startInfo.CreateNoWindow = true;

               using (var process = Process.Start(startInfo))
               {
                    //Task.Factory.StartNew (() =>
                    //{
                    while (!process.StandardOutput.EndOfStream)
                   {
                       console.WriteLine(process.StandardOutput.ReadLine());
                   }
                    //});

                    //Task.Factory.StartNew (() =>
                    //{
                    while (!process.StandardError.EndOfStream)
                   {
                       console.WriteLine(process.StandardError.ReadLine());
                   }
                    //});

                    process.WaitForExit();
               }
           });
        }

        public override string GDBExecutable
        {
            get
            {
                string binDirectory = Path.Combine(Settings.ToolChainLocation, "0.6.2\\bin");
                return Path.Combine(binDirectory, "arm-eabi-bt-gdb.exe");
            }
        }
    }
}
