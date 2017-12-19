using AvalonStudio.Platforms;
using AvalonStudio.Projects;
using System;
using System.IO;

namespace AvalonStudio.Languages.CSharp
{
    class MetaDataFile : ISourceFile
    {
        private string _filePath;
        private string _extension;
        private string _currentDirectory;
        private string _location;
        private string _source;        

        public MetaDataFile(IProject parentProject, string metaDataPath, string source)
        {
            _filePath = metaDataPath;
            _extension = Path.GetExtension(metaDataPath);
            _currentDirectory = Path.GetDirectoryName(metaDataPath);
            _location = _filePath;
            _source = source;
            Project = parentProject;
            Name = metaDataPath;
        }

        public string FilePath => _filePath;

        public string Extension => _extension;

        public string CurrentDirectory => _currentDirectory;

        public string Location => _location;

        public IProject Project { get; set; }
        public IProjectFolder Parent { get; set; }

        public string Name { get; set; }

        public bool CanRename => false;

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

        public Stream OpenText()
        {
            MemoryStream stream = new MemoryStream();
            StreamWriter writer = new StreamWriter(stream);
            writer.Write(_source);
            writer.Flush();
            stream.Position = 0;
            return stream;
        }

        public void RaiseFileModifiedEvent()
        {
            FileModifiedExternally?.Invoke(this, EventArgs.Empty);
        }
    }
}
