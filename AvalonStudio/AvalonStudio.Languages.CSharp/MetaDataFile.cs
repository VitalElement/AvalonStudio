using AvalonStudio.Platforms;
using AvalonStudio.Projects;
using Microsoft.CodeAnalysis;
using System;
using System.IO;
using System.Threading.Tasks;

namespace AvalonStudio.Languages.CSharp
{
    public class MetaDataFile : ISourceFile
    {
        private string _filePath;
        private string _extension;
        private string _currentDirectory;
        private string _location;
        private Document _document;

        public MetaDataFile(IProject parentProject, Document document, string metaDataPath, string name)
        {
            _filePath = metaDataPath;
            _extension = Path.GetExtension(metaDataPath);
            _currentDirectory = Path.GetDirectoryName(metaDataPath);
            _location = _filePath;
            Project = parentProject;
            Name = $"{name} [metadata]";
            _document = document;
        }

        public string ContentType => "C#";

        public Document Document => _document;

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
        }

        public void Delete()
        {
        }

        public async Task<string> GetTextAsync ()
        {
            return (await _document.GetTextAsync()).ToString();
        }

        public Stream OpenText()
        {
            var source = _document.GetTextAsync().GetAwaiter().GetResult().ToString();

            MemoryStream stream = new MemoryStream();
            StreamWriter writer = new StreamWriter(stream);
            writer.Write(source);
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
