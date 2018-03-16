namespace AvalonStudio.Projects
{
    using AvalonStudio.Platforms;
    using System;
    using System.Collections.ObjectModel;
    using System.IO;

    public class FileSystemFolder : IProjectFolder, IDeleteable
    {
        private string _name;

        public FileSystemFolder(string path)
        {
            _name = Path.GetFileName(path);
            Location = path;

            Items = new ObservableCollection<IProjectItem>();
        }

        public ObservableCollection<IProjectItem> Items { get; }

        public bool CanRename => true;

        public string Name
        {
            get { return _name; }
            set
            {
                if (value != Name)
                {
                    var newLocation = Path.Combine(Path.GetDirectoryName(Location), value);

                    Directory.Move(Location, newLocation);
                }
            }
        }

        public IProjectFolder Parent { get; set; }

        public string Location { get; private set; }

        public string LocationDirectory => Location;

        public IProject Project { get; set; }

        public void ExcludeFile(ISourceFile file)
        {
            throw new NotImplementedException();
        }

        public void ExcludeFolder(IProjectFolder folder)
        {
            Project.ExcludeFolder(folder);
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

        public void Delete()
        {
            Directory.Delete(Location, true);
        }
    }
}