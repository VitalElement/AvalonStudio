namespace AvalonStudio.Projects
{
    using AvalonStudio.Platforms;
    using System;
    using System.IO;

    public class FileSystemFile : ISourceFile
    {
        public static FileSystemFile FromPath(IProject project, IProjectFolder parent, string filePath)
        {
            return new FileSystemFile { Project = project, Parent = parent, FilePath = filePath.ToPlatformPath() };
        }

        public string CurrentDirectory
        {
            get { return Path.GetDirectoryName(Location); }
        }

        public string FilePath { get; set; }

        public string Extension
        {
            get { return Path.GetExtension(FilePath); }
        }

        public string Location
        {
            get { return Path.Combine(Project.CurrentDirectory, FilePath); }
        }

        public bool CanRename => true;

        public string Name
        {
            get { return Path.GetFileName(Location); }
            set
            {
                if (value != Name)
                {
                    var newLocation = Path.Combine(CurrentDirectory, value);

                    System.IO.File.Move(Location, newLocation);
                }
            }
        }

        public IProjectFolder Parent { get; set; }

        public IProject Project { get; set; }

        public Language Language
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public event EventHandler FileModifiedExternally;

        public int CompareTo(string other)
        {
            return Location.CompareFilePath(other);
        }

        public int CompareTo(ISourceFile other)
        {
            return FilePath.CompareFilePath(other.FilePath);
        }

        public int CompareTo(IProjectItem other)
        {
            return this.CompareProjectItems(other);
            throw new NotImplementedException();
        }

        public void RaiseFileModifiedEvent()
        {
            FileModifiedExternally?.Invoke(this, new EventArgs());
        }
    }
}