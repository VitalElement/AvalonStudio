using System;
using System.Collections.ObjectModel;
using System.IO;

namespace AvalonStudio.Projects.Standard
{
	public class StandardProjectFolder : IProjectFolder
	{
		public StandardProjectFolder(string path)
		{
			Name = Path.GetFileName(path);
			Location = path;

			Items = new ObservableCollection<IProjectItem>();
		}

		public ObservableCollection<IProjectItem> Items { get; }

		public string Name { get; }

		public IProjectFolder Parent { get; set; }

		public string Location { get; }
		public string LocationDirectory { get; private set; }

		public IProject Project { get; set; }

		public void AddFile(ISourceFile file)
		{
			throw new NotImplementedException();
		}

		public void AddFolder(IProjectFolder folder)
		{
			throw new NotImplementedException();
		}

		public void RemoveFile(ISourceFile file)
		{
			throw new NotImplementedException();
		}

		public void RemoveFolder(IProjectFolder folder)
		{
			throw new NotImplementedException();
		}
	}
}