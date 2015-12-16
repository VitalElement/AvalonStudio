using Newtonsoft.Json;
using System.IO;
using System;

namespace AvalonStudio.Projects.VEBuild
{
    public class SourceFile : ISourceFile
    {
        public string File { get; set; }
        public string Flags { get; set; }

        public void SetProject(VEBuildProject project)
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
        public VEBuildProject Project { get; private set; }

        public Language Language
        {
            get;
        }

        //[JsonIgnore]
        //public Language Language
        //{
        //    get
        //    {
        //        var result = Language.C;

        //        switch (Path.GetExtension(File))
        //        {
        //            case ".c":
        //                result = Language.C;
        //                break;

        //            case ".cpp":
        //                result = Language.Cpp;
        //                break;
        //        }

        //        return result;
        //    }
        //}
    }
}
