using System;
using System.Collections.Generic;
using System.Text;

namespace AvalonStudio.Projects.OmniSharp.ProjectTypes
{
    class CSharpProjectType : IProjectType
    {
        public static Guid TypeId = Guid.Parse("{9A19103F-16F7-4668-BE54-9A1E7A4F7556}");

        public Guid ProjectTypeId { get; } = TypeId;

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

        public IProject Load(ISolution solution, string filePath)
        {
            return OmniSharpProject.Create(solution, filePath).GetAwaiter().GetResult();
        }
    }
}
