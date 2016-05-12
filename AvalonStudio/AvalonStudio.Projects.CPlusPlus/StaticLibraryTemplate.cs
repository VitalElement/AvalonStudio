using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AvalonStudio.Projects;
using AvalonStudio.Projects.CPlusPlus;

namespace AvalonStudio.Languages.CPlusPlus
{
    public class StaticLibraryTemplate : BlankCPlusPlusLangaguageTemplate
    {
        public override string Title
        {
            get
            {
                return "Static Library C/C++ Project";
            }
        }

        public override string DefaultProjectName
        {
            get
            {
                return "StaticLibrary";
            }
        }

        public override string Description
        {
            get
            {
                return "Creates a Static Library project for C/C++";
            }
        }

        public override async Task<IProject> Generate(ISolution solution, string name)
        {
            var project = await base.Generate(solution, name) as CPlusPlusProject;

            project.Type = Projects.Standard.ProjectType.StaticLibrary;

            project.Save();

            return project;
        }
    }
}
