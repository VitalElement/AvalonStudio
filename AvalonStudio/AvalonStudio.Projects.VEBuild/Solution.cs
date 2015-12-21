namespace AvalonStudio.Projects.VEBuild
{
    using Newtonsoft.Json;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;

    public class Solution : SerializedObject<Solution>, ISolution
    {
        public const string solutionExtension = "vsln";
        public const string projectExtension = "vproj";

        public static Solution Load(string directory)
        {            
            var solution = new Solution();
            solution.CurrentDirectory = directory;

            if (!Directory.Exists(directory))
            {
                throw new Exception(string.Format("Directory does not exist {0}", directory));
            }

            var subfolders = Directory.GetDirectories(directory);

            foreach (var subfolder in subfolders)
            {
                var projectFile = string.Format("{0}.{1}", Path.GetFileName(subfolder), projectExtension);
                var projectLocation = Path.Combine(subfolder, projectFile);

                if (File.Exists(projectLocation))
                {
                    solution.Projects.Add(VEBuildProject.Load(projectLocation, solution));

                }
            }

            solution.Name = Path.GetFileNameWithoutExtension(directory);

            return solution;
        }

        public IProject FindProject (string name)
        {
            IProject result = null;

            foreach(var project in Projects)
            {
                if(project.Name == name)
                {
                    result = project;
                    break;
                }
            }

            if(result == null)
            {
                throw new Exception(string.Format("Unable to find project with name {0}", name));
            }

            return result;
        }

        public IProject AddProject (IProject project)
        {
            var currentProject = Projects.Where((p) => p.Name == project.Name).FirstOrDefault();

            if(currentProject == null)
            {
                Projects.Add(project);
                currentProject = project;
            }

            return currentProject;
        }

        public void Save()
        {
            Serialize(Path.Combine(CurrentDirectory, Name + "." + solutionExtension));
        }

        public Solution()
        {
            Projects = new List<IProject>();
        }

        [JsonIgnore]
        public string CurrentDirectory { get; private set; }
        
        [JsonIgnore]
        public IList<IProject> Projects { get; set; }

        [JsonIgnore]
        public IProject StartupProject { get; private set; }

        public string Name { get; private set; }
    }
}
