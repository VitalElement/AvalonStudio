using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AvalonStudio.Models.Solutions;

namespace AvalonStudio.Models.Tools
{
    public class ClangRename
    {
        public static void Rename(Project project, string mainFile, List<string> filesToSearch, UInt32 offset, string newName)
        {
            ClangFormat result = new ClangFormat();
            ProcessStartInfo startInfo = new ProcessStartInfo();

            startInfo.FileName = Path.Combine(Directory.GetCurrentDirectory(), "clang-rename.exe");

            string files = string.Empty;

            foreach(var file in filesToSearch)
            {
                files += file + " ";
            }

            string includes = string.Empty;

            foreach (var include in project.IncludeArguments)
            {
                includes += string.Format("-extra-arg={0} ", include);
            }

            string defines = string.Empty;

            foreach (var define in project.SelectedConfiguration.Defines)
            {
                if (define != string.Empty)
                {
                    defines += (string.Format("-extra-arg=-D{0} ", define));
                }
            }

            startInfo.WorkingDirectory = Path.GetDirectoryName(mainFile);


            startInfo.Arguments = string.Format("-offset={2} -new-name={3} {4} {5} -extra-arg=-D__GNUC__ -pn -pl {0} {1} --", mainFile, files, offset, newName, includes, defines);

            // Hide console window
            startInfo.UseShellExecute = false;
            startInfo.RedirectStandardOutput = true;
            startInfo.RedirectStandardError = true; //we can get the erros text now.
            startInfo.RedirectStandardInput = true;
            startInfo.CreateNoWindow = true;

            using (var process = Process.Start(startInfo))
            {
                process.OutputDataReceived += (sender, e) =>
                {
                    Console.Write(e.Data);
                };

                process.ErrorDataReceived += (sender, e) =>
                {
                    Console.Write(e.Data);
                };


                process.BeginOutputReadLine();
                process.BeginErrorReadLine();

                process.WaitForExit();       
            }
        }
    }
}
