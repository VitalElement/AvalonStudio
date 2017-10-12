namespace AvalonStudio.Projects.CPlusPlus
{
    using System;
    using System.Collections.Generic;

    internal class CPlusPlusProjectType : IProjectType
    {
        public static readonly Guid TypeId = Guid.Parse("{DA891B1A-E1A3-4A1A-83CD-252F07B636ED}");
        public List<string> Extensions => new List<string>
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

        public IProject Load(ISolution solution, string filePath)
        {
            return CPlusPlusProject.Load(filePath, solution);
        }
    }
}