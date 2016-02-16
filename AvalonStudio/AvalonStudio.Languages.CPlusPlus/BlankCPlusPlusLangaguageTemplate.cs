namespace AvalonStudio.Languages.CPlusPlus
{    
    using AvalonStudio.Projects;
    using System;

    public class BlankCPlusPlusLangaguageTemplate : IProjectTemplate
    {
        public virtual string DefaultProjectName
        {
            get
            {
                return "EmptyProject";
            }
        }

        public virtual string Description
        {
            get
            {
                return "Creates an empty C/C++ project.";
            }
        }

        public virtual string Title
        {
            get
            {
                return "Empty C/C++ Project";
            }
        }

        public void Generate(ISolution solution, string name)
        {
            
        }
    }
}
