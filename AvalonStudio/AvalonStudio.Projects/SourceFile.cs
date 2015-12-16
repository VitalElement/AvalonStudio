using Newtonsoft.Json;
using System.IO;

namespace AvalonStudio.Projects
{
    public class SourceFile
    {
        public string File { get; set; }
        public string Flags { get; set; }

        public void SetProject(Project project)
        {
            this.Project = project;
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
        public Project Project { get; private set; }

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
    }
}
