using System;
using System.Collections.ObjectModel;

namespace AvalonStudio.Projects
{
	public interface IProjectFolder : IProjectItem, IComparable<IProjectFolder>, IComparable<string>
    {
		/// <summary>
		///     List of items within the project folder
		/// </summary>
		ObservableCollection<IProjectItem> Items { get; }

		string Location { get; }

		string LocationDirectory { get; }
        
        void ExcludeFile(ISourceFile file);
        void ExcludeFolder(IProjectFolder folder);
	}
}