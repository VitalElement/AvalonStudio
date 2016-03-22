namespace AvalonStudio.Projects
{
    using Extensibility;
    using Platform;
    using MVVM;
    using Newtonsoft.Json;
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.IO;
    using System.Linq;
    using Utils;

    public class Solution : SerializedObject<Solution>, ISolution
    {
        public const string Extension = "asln";

        public static IProject LoadProjectFile (ISolution solution, string fileName)
        {
            IProject result = null;

            var extension = Path.GetExtension(fileName).Remove(0, 1);

            var projectType = Workspace.Instance.ProjectTypes.FirstOrDefault((p) => p.Extension == extension);

            if (projectType != null)
            {
                result = projectType.Load(solution, fileName);
            }
            else
            {
                // create an unloaded project type.
            }

            if(result.ToolChain != null)
            {
                result.ToolChain.ProvisionSettings(result);
            }

            return result;
        }

        private static IProject LoadProject (ISolution solution, string reference)
        {
            IProject result = null;

            var extension = Path.GetExtension(reference).Remove(0,1);

            var projectType = Workspace.Instance.ProjectTypes.FirstOrDefault((p) => p.Extension == extension);
            var projectFilePath = Path.Combine(solution.CurrentDirectory, reference).ToPlatformPath();

            if (projectType != null && File.Exists(projectFilePath))
            {
                result = projectType.Load(solution, projectFilePath);
            }
            else
            {
                Console.WriteLine("Failed to load " + projectFilePath);
                // create an unloaded project type.
            }

            if (result?.ToolChain != null)
            {
                result.ToolChain.ProvisionSettings(result);
            }

            return result;
        }
        

        public static Solution Load(string fileName)
        {
            Solution solution = null;
            
            solution = Deserialize(fileName);            

            solution.CurrentDirectory = (Path.GetDirectoryName(fileName) + Platform.DirectorySeperator).ToPlatformPath();

            Console.WriteLine("Solution directory is " + solution.CurrentDirectory);

            foreach (var projectReference in solution.ProjectReferences)
            {
                var proj = LoadProject(solution, projectReference);

                // todo null returned here we need a placeholder.
                if (proj != null)
                {
                    solution.Projects.InsertSorted(proj);
                }
            }

            foreach (var project in solution.Projects)
            {
                project.ResolveReferences();
            }

            solution.Name = Path.GetFileNameWithoutExtension(fileName);

            solution.StartupProject = solution.Projects.SingleOrDefault((p) => p.Name == solution.StartupItem);

            return solution;
        }

        public IProject FindProject(string name)
        {
            IProject result = null;

            foreach (var project in Projects)
            {
                if (project.Name == name)
                {
                    result = project;
                    break;
                }
            }

            if (result == null)
            {
                throw new Exception(string.Format("Unable to find project with name {0}", name));
            }

            return result;
        }

        public IProject AddProject(IProject project)
        {
            var currentProject = Projects.Where((p) => p.Name == project.Name).FirstOrDefault();

            if (currentProject == null)
            {
                ProjectReferences.Add(CurrentDirectory.MakeRelativePath(project.Location));
                Projects.InsertSorted(project);
                currentProject = project;
            }

            return currentProject;
        }

        public void RemoveProject(IProject project)
        {
            Projects.Remove(project);
            ProjectReferences.Remove(CurrentDirectory.MakeRelativePath(project.Location));
        }

        public void Save()
        {
            StartupItem = StartupProject?.Name;
            
            for(int i = 0; i < ProjectReferences.Count; i++)
            {
                ProjectReferences[i] = ProjectReferences[i].ToAvalonPath();
            }

            Serialize(Path.Combine(CurrentDirectory, Name + "." + Extension));
        }

        public static Solution Create(string location, string name)
        {
            var result = new Solution();

            result.Name = name;
            result.CurrentDirectory = location + Platform.DirectorySeperator ;
            result.Save();

            return result;
        }

        public ISourceFile FindFile(ISourceFile file)
        {
            ISourceFile result = null;                

            foreach (var project in Projects)
            {
                result = project.FindFile(file);

                if (result != null)
                {
                    break;
                }
            }

            return result;
        }

        public Solution()
        {
            ProjectReferences = new List<string>();
            Projects = new ObservableCollection<IProject>();
        }

        [JsonIgnore]
        public string CurrentDirectory { get; private set; }

        [JsonIgnore]
        public ObservableCollection<IProject> Projects { get; set; }        

        [JsonIgnore]
        public IProject StartupProject { get; set; }        

        public string Name { get; set; }

        public string StartupItem { get; set; }

        [JsonProperty("Projects")]
        public IList<string> ProjectReferences { get; set; }
    }
}
