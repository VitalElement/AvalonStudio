using System.Collections.ObjectModel;

namespace AvalonStudio.Projects
{
	public interface ISolution
	{
		string Name { get; }

		IProject StartupProject { get; set; }

		ObservableCollection<IProject> Projects { get; }

		string CurrentDirectory { get; }

		IProject AddProject(IProject project);

		ISourceFile FindFile(ISourceFile path);

		void RemoveProject(IProject project);

		void Save();
	}
}