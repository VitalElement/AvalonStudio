using AvalonStudio.Languages;
using AvalonStudio.Platforms;
using System;
using System.IO;

namespace AvalonStudio.Projects
{
    public class FileSystemFile : ISourceFile
    {
        public static FileSystemFile FromPath(IProject project, IProjectFolder parent, string filePath)
        {
            return new FileSystemFile(project, parent, filePath.ToPlatformPath());
        }

        private FileSystemFile(IProject project, IProjectFolder parent, string filePath)
        {
            Project = project;
            Parent = parent;
            FilePath = filePath;
        }

        public string ContentType => ContentTypeServiceInstance.Instance.GetContentTypeForExtension(Extension);

        public string CurrentDirectory
        {
            get { return Path.GetDirectoryName(Location); }
        }

        public string FilePath { get; }

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

                    File.Move(Location, newLocation);
                }
            }
        }

        public IProjectFolder Parent { get; set; }

        public IProject Project { get; set; }

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
        }

        public void RaiseFileModifiedEvent()
        {
            FileModifiedExternally?.Invoke(this, EventArgs.Empty);
        }

        public Stream OpenText()
        {
            return new FileStream(Location, FileMode.Open, FileAccess.Read, FileShare.ReadWrite | FileShare.Delete, bufferSize: 4096, useAsync: false);
        }

        public void Delete()
        {
            File.Delete(Location);
        }
    }
}