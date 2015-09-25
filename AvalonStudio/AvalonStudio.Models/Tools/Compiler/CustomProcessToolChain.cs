using System.Diagnostics;
using System.Threading.Tasks;

namespace AvalonStudio.Models.Tools.Compiler
{
    public class CustomProcessToolChain : ToolChain
    {
        internal CustomProcessToolChain () { }   //For xml

        //public CustomProcessToolChain (string name, string executableLocation, string arguments)
        //{
        //    this.ExecutableLocation = executableLocation;
        //    this.Arguments = arguments;
        //}

        public string ExecutableLocation { get; set; }
        public string Arguments { get; set; }

        public override async Task<bool> Build (IConsole console, Solutions.Project project, System.Threading.CancellationTokenSource cancellationSource)
        {
            console.Clear ();
            await Task.Factory.StartNew (() =>
            {
                var startInfo = new ProcessStartInfo ();

                startInfo.FileName = this.ExecutableLocation;//@"c:\VEStudio\Projects\bt\BitThunder\make.exe";
                startInfo.WorkingDirectory = project.CurrentDirectory;
                startInfo.Arguments = this.Arguments;
                console.WriteLine ("Executing: " + this.Name);

                // Hide console window
                startInfo.UseShellExecute = false;
                startInfo.RedirectStandardOutput = true;
                startInfo.RedirectStandardError = true;
                startInfo.CreateNoWindow = true;

                using (var process = Process.Start (startInfo))
                {
                    Task.Factory.StartNew (() =>
                    {
                        while (!process.StandardOutput.EndOfStream)
                        {
                            console.WriteLine (process.StandardOutput.ReadLine ());
                        }
                    });

                   Task.Factory.StartNew (() =>
                   {
                       while (!process.StandardError.EndOfStream)
                       {
                           console.WriteLine (process.StandardError.ReadLine ());
                       }
                   });

                    process.WaitForExit ();
                }
            });

            return true;
        }

        public override Task Clean (IConsole console, Solutions.Project project, System.Threading.CancellationTokenSource cancellationSource)
        {
            throw new System.NotImplementedException ();
        }

        public override string GDBExecutable
        {
            get { return string.Empty; }
        }
    }
}
