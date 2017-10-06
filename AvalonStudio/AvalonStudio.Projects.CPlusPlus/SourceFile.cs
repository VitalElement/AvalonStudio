using AvalonStudio.Platforms;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Threading.Tasks;

namespace AvalonStudio.Projects.CPlusPlus
{
    public class SourceFile : ISourceFile
    {
        private SourceFile()
        {
        }

        public string Flags { get; set; }

        public string FilePath { get; set; }

        public event EventHandler FileModifiedExternally;

        public int CompareTo(ISourceFile other)
        {
            return FilePath.CompareFilePath(other.FilePath);
        }

        [JsonIgnore]
        public string Location
        {
            get { return Path.Combine(Project.CurrentDirectory, FilePath); }
        }

        [JsonIgnore]
        public IProject Project { get; set; }

        [JsonIgnore]
        public Language Language
        {
            get
            {
                var result = Language.C;

                switch (Path.GetExtension(FilePath))
                {
                    case ".c":
                        result = Language.C;
                        break;

                    case ".cpp":
                        result = Language.Cpp;
                        break;
                }

                return result;
            }
        }

        public string Extension
        {
            get { return Path.GetExtension(FilePath); }
        }

        public string Name
        {
            get { return Path.GetFileName(Location); }
        }

        public IProjectFolder Parent { get; set; }

        public string CurrentDirectory
        {
            get { return Path.GetDirectoryName(Location); }
        }

        public void SetProject(IProject project)
        {
            Project = project;
        }

        public static SourceFile FromPath(IProject project, IProjectFolder parent, string filePath)
        {
            return new SourceFile { Project = project, Parent = parent, FilePath = filePath.ToPlatformPath() };
        }

        public static Task<ISourceFile> Create(IProjectFolder parent, string name, string text = "")
        {
            if (parent.Project == null)
            {
                throw new ArgumentNullException("parent.Project");
            }

            var filePath = Path.Combine(parent.LocationDirectory, name);

            TaskCompletionSource<ISourceFile> fileAddedCompletionSource = new TaskCompletionSource<ISourceFile>();

            EventHandler<ISourceFile> fileAddedHandler = (sender, e) =>
            {
                var newFile = parent.Project.FindFile(filePath);

                if (newFile != null)
                {
                    fileAddedCompletionSource.SetResult(newFile);
                }
            };

            parent.Project.FileAdded += fileAddedHandler;

            using (var file = System.IO.File.CreateText(filePath))
            {
                file.Write(text);
            }

            fileAddedCompletionSource.Task.ContinueWith((f) =>
            {
                parent.Project.FileAdded -= fileAddedHandler;
            });

            return fileAddedCompletionSource.Task;
        }

        public int CompareTo(IProjectItem other)
        {
            return this.CompareProjectItems(other);
        }

        public int CompareTo(string other)
        {
            return Location.CompareFilePath(other);
        }

        public void RaiseFileModifiedEvent()
        {
            FileModifiedExternally?.Invoke(this, new EventArgs());
        }
    }
}