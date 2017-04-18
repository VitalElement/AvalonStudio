namespace AvalonStudio.Projects
{
    using AvalonStudio.Platforms;
    using System;
    using System.IO;

    public class File : ISourceFile
    {
        public static File FromPath(IProject project, IProjectFolder parent, string filePath)
        {
            return new File { Project = project, Parent = parent, FilePath = filePath.ToPlatformPath() };
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

        public string Name
        {
            get { return Path.GetFileName(Location); }
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
            return this.CompareProjectItems(other); throw new NotImplementedException();
        }

        public void RaiseFileModifiedEvent()
        {
            FileModifiedExternally?.Invoke(this, new EventArgs());
        }
    }
}