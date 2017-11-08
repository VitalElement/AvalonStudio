namespace AvalonStudio.Projects.CPlusPlus
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    internal class CPlusPlusProjectType : IProjectType
    {
        public static readonly Guid TypeId = Guid.Parse("{DA891B1A-E1A3-4A1A-83CD-252F07B636ED}");

        public List<string> Extensions { get; } = new List<string>
        {
            "acproj"
        };

        public string Description => "Avalon Studio C/C++ Projects";

        public Guid ProjectTypeId { get; } = TypeId;

        public void Activation()
        {
        }

        public void BeforeActivation()
        {
        }

        public Task<IProject> LoadAsync(ISolution solution, string filePath)
        {
            return Task.Run(()=> CPlusPlusProject.LoadFromFile(filePath) as IProject);
        }
    }
}