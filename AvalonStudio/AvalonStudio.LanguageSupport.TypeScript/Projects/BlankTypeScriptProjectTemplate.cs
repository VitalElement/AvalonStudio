using AvalonStudio.Projects;
using System.IO;
using System.Threading.Tasks;

namespace AvalonStudio.LanguageSupport.TypeScript.Projects
{
    public class BlankTypeScriptProjectTemplate : IProjectTemplate
    {
        public string DefaultProjectName => "TypeScriptProject";

        public string Description => "Creates a blank TypeScript project";

        public string Title => "Empty TypeScript Project";

        public virtual async Task<IProject> Generate(ISolutionFolder solutionFolder, string name)
        {
            var location = Path.Combine(solutionFolder.Solution.CurrentDirectory, name);

            Directory.CreateDirectory(location);

            IProject project = await TypeScriptProject.Create(location);

            project = solutionFolder.Solution.AddItem(project, solutionFolder);

            if (solutionFolder.Solution.StartupProject == null)
            {
                solutionFolder.Solution.StartupProject = project;
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