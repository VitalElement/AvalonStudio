using AvalonStudio.Platforms;
using AvalonStudio.Utils;
using System;
using System.Collections.ObjectModel;
using System.IO;
using AvalonStudio.Projects;

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
        public string LocationDirectory => Location;

		public IProject Project { get; set; }

		public void AddFile(ISourceFile file)
		{
            this.Items.InsertSorted(file);
		}

		public void AddFolder(IProjectFolder folder)
		{
            folder.Parent = this;
            Items.InsertSorted(folder);
        }

		public void RemoveFile(ISourceFile file)
		{
			throw new NotImplementedException();
		}

		public void RemoveFolder(IProjectFolder folder)
		{
			throw new NotImplementedException();
		}

        public void ExcludeFile(ISourceFile file)
        {
            throw new NotImplementedException();
        }

        public void ExcludeFolder(IProjectFolder folder)
        {
            throw new NotImplementedException();
        }

        public int CompareTo(IProjectFolder other)
        {
            return Location.CompareFilePath(other.Location);
        }

        public int CompareTo(string other)
        {
            return Location.CompareFilePath(other);
        }

        public int CompareTo(IProjectItem other)
        {
            return this.CompareProjectItems(other);
        }
    }
}