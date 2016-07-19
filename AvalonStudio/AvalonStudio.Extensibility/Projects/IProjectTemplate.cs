namespace AvalonStudio.Projects
{
	using System.ComponentModel.Composition;
	using System.Threading.Tasks;

	[InheritedExport(typeof(IProjectTemplate))]
	public interface IProjectTemplate
	{
		string Title { get; }

		string DefaultProjectName { get; }

		string Description { get; }

		/// <summary>
		/// Generates a new project acording to the template and attaches it to the passed solution.
		/// Templates can add 1 or more projects to a solution.
		/// </summary>
		/// <param name="solution">Solution that the template will install to.</param>
		/// <param name="name">Name of the project.</param>        
		Task<IProject> Generate(ISolution solution, string name);
	}
}
