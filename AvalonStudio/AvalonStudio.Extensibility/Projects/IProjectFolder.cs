using System.Collections.ObjectModel;

namespace AvalonStudio.Projects
{
	public interface IProjectFolder : IProjectItem
	{
		/// <summary>
		///     List of items within the project folder
		/// </summary>
		ObservableCollection<IProjectItem> Items { get; }

		string Location { get; }

		string LocationDirectory { get; }

		void AddFile(ISourceFile file);
		void AddFolder(IProjectFolder folder);

		void RemoveFile(ISourceFile file);
		void RemoveFolder(IProjectFolder folder);
	}
}