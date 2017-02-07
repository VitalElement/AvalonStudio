using System.Collections.ObjectModel;
using System.Composition;

namespace AvalonStudio.Projects
{    
    public interface ISolution
	{
		string Name { get; }        

        IProject StartupProject { get; set; }

		ObservableCollection<IProject> Projects { get; }

		string CurrentDirectory { get; }

		IProject AddProject(IProject project);

		ISourceFile FindFile(string path);

		void RemoveProject(IProject project);

		void Save();
	}
}