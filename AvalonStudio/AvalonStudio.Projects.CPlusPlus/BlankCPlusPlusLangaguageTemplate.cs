using System.IO;
using System.Threading.Tasks;
using AvalonStudio.Projects;
using AvalonStudio.Projects.CPlusPlus;

namespace AvalonStudio.Languages.CPlusPlus
{
	public class BlankCPlusPlusLangaguageTemplate : IProjectTemplate
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

			solution.Save();

			return project;
		}
	}
}