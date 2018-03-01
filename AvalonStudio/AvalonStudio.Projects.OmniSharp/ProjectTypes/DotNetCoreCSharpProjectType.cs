using RoslynPad.Roslyn;
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
            await RoslynWorkspace.CreateWorkspaceAsync(solution);

            return await OmniSharpProject.Create(solution, filePath);
        }
    }
}
