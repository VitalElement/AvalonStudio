using AvalonStudio.Extensibility;
using AvalonStudio.Platforms;
using AvalonStudio.Shell;
using AvalonStudio.Utils;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;

namespace AvalonStudio.Projects
{
    public class Solution : ISolution
    {
        public const string Extension = "asln";

        public Solution()
        {
            ProjectReferences = new List<string>();
            Projects = new ObservableCollection<IProject>();
        }

        public string StartupItem { get; set; }

        [JsonProperty("Projects")]
        public IList<string> ProjectReferences { get; set; }

        public IProject AddProject(IProject project)
        {
            var currentProject = Projects.FirstOrDefault(p => p.Name == project.Name);

            if (currentProject != null) return currentProject;
            ProjectReferences.Add(CurrentDirectory.MakeRelativePath(project.Location));
            Projects.InsertSorted(project);
            currentProject = project;

            return currentProject;
        }

        public void RemoveProject(IProject project)
        {
            Projects.Remove(project);
            ProjectReferences.Remove(CurrentDirectory.MakeRelativePath(project.Location).ToAvalonPath());
        }

        public void Save()
        {
            StartupItem = StartupProject?.Name;

            for (var i = 0; i < ProjectReferences.Count; i++)
            {
                ProjectReferences[i] = ProjectReferences[i].ToAvalonPath();
            }

            SerializedObject.Serialize(Path.Combine(CurrentDirectory, Name + "." + Extension), this);
        }

        public ISourceFile FindFile(string file)
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

        [JsonIgnore]
        public string CurrentDirectory { get; set; }

        [JsonIgnore]
        public ObservableCollection<IProject> Projects { get; set; }

        [JsonIgnore]
        public IProject StartupProject { get; set; }

        public string Name { get; set; }

        [JsonIgnore]
        public string Location { get; private set; }

        public static IProject LoadProjectFile(ISolution solution, string fileName)
        {
            var shell = IoC.Get<IShell>();
            IProject result = null;

            var extension = Path.GetExtension(fileName).Remove(0, 1);

            var projectType = shell.ProjectTypes.FirstOrDefault(p => p.Extensions.Contains(extension));

            if (projectType != null)
            {
                result = projectType.Load(solution, fileName);

                result.ResolveReferences();
            }

            return result;
        }

        private static IProject LoadProject(ISolution solution, string reference)
        {
            var shell = IoC.Get<IShell>();
            IProject result = null;

            var extension = Path.GetExtension(reference).Remove(0, 1);

            var projectType = shell.ProjectTypes.FirstOrDefault(p => p.Extensions.Contains(extension));
            var projectFilePath = Path.Combine(solution.CurrentDirectory, reference).ToPlatformPath();

            if (projectType != null && System.IO.File.Exists(projectFilePath))
            {
                result = projectType.Load(solution, projectFilePath);
            }
            else
            {
                Console.WriteLine("Failed to load " + projectFilePath);
                // create an unloaded project type.
            }

            return result;
        }

        public static Solution Load(string fileName)
        {
            var solution = SerializedObject.Deserialize<Solution>(fileName);

            solution.Location = fileName.NormalizePath().ToPlatformPath();
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

            solution.StartupProject = solution.Projects.SingleOrDefault(p => p.Name == solution.StartupItem);

            return solution;
        }

        public IProject FindProject(string name)
        {
            var result = Projects.FirstOrDefault(project => project.Name == name);

            if (result == null)
            {
                throw new Exception($"Unable to find project with name {name}");
            }

            return result;
        }

        public static Solution Create(string location, string name, bool save = true)
        {
            var result = new Solution();

            result.Name = name;
            result.CurrentDirectory = location + Platform.DirectorySeperator;

            if (save)
            {
                result.Save();
            }

            return result;
        }
    }
}