using System.IO;
using System.Threading.Tasks;
using AvalonStudio.Projects;

namespace AvalonStudio.LanguageSupport.TypeScript.Projects
{
    public class BlankTypeScriptProjectTemplate : IProjectTemplate
    {
        public string DefaultProjectName => "TypeScriptProject";

        public string Description => "Creates a blank TypeScript project";

        public string Title => "Empty TypeScript Project";

        public virtual async Task<IProject> Generate(ISolution solution, string name)
        {
            var location = Path.Combine(solution.CurrentDirectory, name);

            Directory.CreateDirectory(location);

            IProject project = await TypeScriptProject.Create(solution, location);

            project = solution.AddProject(project);

            if (solution.StartupProject == null)
            {
                solution.StartupProject = project;
            }

            return project;
        }

        public virtual void BeforeActivation()
        {
        }

        public virtual void Activation()
        {
        }
    }
}