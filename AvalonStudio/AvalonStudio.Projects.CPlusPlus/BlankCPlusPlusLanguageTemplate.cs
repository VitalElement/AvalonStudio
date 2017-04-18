using System.IO;
using System.Threading.Tasks;

namespace AvalonStudio.Projects.CPlusPlus
{
    public class BlankCPlusPlusLanguageTemplate : IProjectTemplate
    {
        public virtual string DefaultProjectName
        {
            get { return "EmptyProject"; }
        }

        public virtual string Description
        {
            get { return "Creates an empty C/C++ project."; }
        }

        public virtual string Title
        {
            get { return "Empty C/C++ Project"; }
        }

        public void Activation()
        {
        }

        public void BeforeActivation()
        {
        }

        public virtual async Task<IProject> Generate(ISolution solution, string name)
        {
            var location = Path.Combine(solution.CurrentDirectory, name);

            if (!Directory.Exists(location))
            {
                Directory.CreateDirectory(location);
            }

            IProject project = CPlusPlusProject.Create(solution, location, name);

            project = solution.AddProject(project);

            if (solution.StartupProject == null)
            {
                solution.StartupProject = project;
            }

            return project;
        }
    }
}