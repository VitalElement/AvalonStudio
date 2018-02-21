using AvalonStudio.Extensibility;
using AvalonStudio.Extensibility.Projects;
using AvalonStudio.Extensibility.Shell;
using AvalonStudio.Projects.OmniSharp.DotnetCli;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AvalonStudio.Projects.OmniSharp.ProjectTypes
{
    class DotNetCoreCSharpProjectType : IProjectType
    {
        public static Guid DotNetCoreCSharpTypeId = Guid.Parse("{9A19103F-16F7-4668-BE54-9A1E7A4F7556}");

        public virtual Guid ProjectTypeId { get; } = DotNetCoreCSharpTypeId;

        public List<string> Extensions { get; } = new List<string>
        {
            "csproj"
        };

        public string Description => "Dotnet Core C# Projects";

        public void Activation()
        {
        }

        public void BeforeActivation()
        {
        }

        public async Task<IProject> LoadAsync(ISolution solution, string filePath)
        {
            if(solution is VisualStudioSolution vsSolution)
            {
                if(!vsSolution.IsRestored)
                {
                    var statusBar = IoC.Get<IStatusBar>();

                    statusBar.SetText($"Restoring Packages for solution: {solution.Name}");

                    await vsSolution.Restore(DotNetCliService.Instance.DotNetPath, null, statusBar);

                    statusBar.ClearText();
                }
            }

            return await OmniSharpProject.Create(solution, filePath);
        }
    }
}
