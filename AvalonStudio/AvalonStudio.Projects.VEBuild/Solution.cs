namespace AvalonStudio.Projects.VEBuild
{
    using Newtonsoft.Json;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using Utils;

    public class Solution : SerializedObject<Solution>, ISolution
    {
        public const string solutionExtension = "vsln";
        public const string projectExtension = "vproj";

        public static Solution Load(string fileName)
        {            
            var solution = Deserialize(fileName);
            
            solution.CurrentDirectory = Path.GetDirectoryName(fileName);

            if (!Directory.Exists(solution.CurrentDirectory))
            {
                throw new Exception(string.Format("Directory does not exist {0}", fileName));
            }

            var subfolders = Directory.GetDirectories(solution.CurrentDirectory);

            foreach (var subfolder in subfolders)
            {
                var projectFile = string.Format("{0}.{1}", Path.GetFileName(subfolder), projectExtension);
                var projectLocation = Path.Combine(subfolder, projectFile);

                if (File.Exists(projectLocation))
                {
                    solution.Projects.Add(VEBuildProject.Load(projectLocation, solution));
                }
            }

            foreach(var project in solution.Projects)
            {
                project.ResolveReferences();
            }

            solution.Name = Path.GetFileNameWithoutExtension(fileName);
            
            solution.StartupProject = solution.Projects.SingleOrDefault((p) => p.Name == solution.StartupItem);            

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

        public string Name { get; set; }

        public string StartupItem { get; set; }
    }
}
