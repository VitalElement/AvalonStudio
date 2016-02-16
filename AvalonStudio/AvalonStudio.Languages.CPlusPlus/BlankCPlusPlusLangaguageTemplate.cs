namespace AvalonStudio.Languages.CPlusPlus
{
    using AvalonStudio.Extensibility.Projects;
    using AvalonStudio.Projects;
    using System;

    public class BlankCPlusPlusLangaguageTemplate : IProjectTemplate
    {
        public string Description
        {
            get
            {
                return "Creates an empty C/C++ project.";
            }
        }

        public string Title
        {
            get
            {
                return "Empty C/C++ Project";
            }
        }

        public void Generate(ISolution solution)
        {
            throw new NotImplementedException();
        }
    }
}
