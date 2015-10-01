using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AvalonStudio.Models.Tools.Debuggers
{
    public class ObjDump
    {
        public ObjDump()
        {

        }

        public void Start(string objDumpExecutable, string inputFile)
        {
            var startInfo = new ProcessStartInfo();



            startInfo.FileName = objDumpExecutable;
            startInfo.Arguments = string.Format("\"{0}\" -d -C", inputFile);

            // Hide console window
            startInfo.UseShellExecute = false;
            startInfo.RedirectStandardOutput = true;
            startInfo.CreateNoWindow = true;

            var process = Process.Start(startInfo);

            process.OutputDataReceived += (sender, e) =>
            {
                if (e.Data != null&& e.Data != string.Empty)
                {
                    var data = e.Data.Split('\t');

                    Console.Write(data.Count());
                }
            };

            process.BeginOutputReadLine();

            process.WaitForExit();
        }
    }
}
