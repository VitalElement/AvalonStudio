namespace AvalonStudio.Projects.Raw
{
    using System;
    using System.IO;
    using AvalonStudio.Platforms;

    public class RawFile : ISourceFile
    {
        public static RawFile FromPath(IProject project, IProjectFolder parent, string filePath)
        {
            return new RawFile { Project = project, Parent = parent, File = filePath.ToPlatformPath() };
        }

        public string CurrentDirectory
        {
            get { return Path.GetDirectoryName(Location); }
        }

        public string File { get; set; }

        public string Extension
        {
            get { return Path.GetExtension(File); }
        }

        public string Location
        {
            get { return Path.Combine(Project.CurrentDirectory,  File); }
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
            return File.CompareFilePath(other.File);
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
