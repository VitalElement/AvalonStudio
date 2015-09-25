namespace ASBuild
{
    using AvalonStudio.Models.Solutions;
    using System;
    using System.IO;

    class Program
    {
        static void Main(string[] args)
        {            
            var console = new ProgramConsole();

            console.WriteLine("Avalon Studio Builder 0.1");

            Solution solution = null;

            switch (args.Length)
            {
                case 0:
                    console.WriteLine("Syntax: asbuid [clean] [path to vesln]");
                    break;

                case 1:
                    string path = args[0];
                    if(Path.GetDirectoryName(args[0]) == string.Empty)
                    {
                        path = Path.Combine(Directory.GetCurrentDirectory(), args[0]);
                    }

                    solution = Solution.LoadSolution(path);

                    if (solution != null)
                    {
                        solution.DefaultProject.Build(console, new System.Threading.CancellationTokenSource()).Wait();
                    }
                    break;

                case 2:
                    path = args[1];
                    if (Path.GetDirectoryName(args[1]) == string.Empty)
                    {
                        path = Path.Combine(Directory.GetCurrentDirectory(), args[1]);
                    }

                    solution = Solution.LoadSolution(path);

                    if (solution != null && args[0].ToLower () == "clean")
                    {
                        solution.DefaultProject.Clean(console, new System.Threading.CancellationTokenSource()).Wait();
                    }                    
                    break;
            }
        }
    }
}
