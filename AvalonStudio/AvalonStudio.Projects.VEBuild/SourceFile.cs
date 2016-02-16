namespace AvalonStudio.Projects.VEBuild
{
    using Newtonsoft.Json;
    using System.IO;

    public class SourceFile : ISourceFile
    {
        public string File { get; set; }
        public string Flags { get; set; }

        public void SetProject(IProject project)
        {
            this.Project = project;
        }

        public static SourceFile Create(IProject project, string location, string name, string text = "")
        {
            var filePath = Path.Combine(location, name);
            var file = System.IO.File.CreateText(filePath);
            file.Write(text);
            file.Close();

            return new SourceFile() { File = filePath, Project = project };
        }

        [JsonIgnore]
        public string Location
        {
            get
            {
                return Path.Combine(Project.CurrentDirectory, File);
            }
        }

        [JsonIgnore]
        public IProject Project { get; set; }

        [JsonIgnore]
        public Language Language
        {
            get
            {
                var result = Language.C;

                switch (Path.GetExtension(File))
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

        public string Name
        {
            get
            {
                return Path.GetFileName(Location);
            }
        }
    }
}
