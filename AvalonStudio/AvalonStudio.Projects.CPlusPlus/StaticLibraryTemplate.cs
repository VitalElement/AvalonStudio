using AvalonStudio.Projects;
using AvalonStudio.Projects.CPlusPlus;
using AvalonStudio.Projects.Standard;
using System.Threading.Tasks;

namespace AvalonStudio.Languages.CPlusPlus
{
    public class StaticLibraryTemplate : BlankCPlusPlusLanguageTemplate
    {
        public override string Title
        {
            get { return "Static Library C/C++ Project"; }
        }

        public override string DefaultProjectName
        {
            get { return "StaticLibrary"; }
        }

        public override string Description
        {
            get { return "Creates a Static Library project for C/C++"; }
        }

        public override async Task<IProject> Generate(ISolution solution, string name)
        {
            var project = await base.Generate(solution, name) as CPlusPlusProject;

            project.Type = ProjectType.StaticLibrary;
            project.Includes.Add(new Include() { Exported = true, Value = "./" });

            project.Save();

            return project;
        }
    }
}