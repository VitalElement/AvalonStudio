using System;
using System.Collections.Generic;
using System.Text;

namespace AvalonStudio.Projects.CPlusPlus
{
    class CPlusPlusProjectType : IProjectType
    {
        public List<string> Extensions => new List<string>{ "acproj"} ;

        public string Description => "Avalon Studio C/C++ Projects";

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
