using AvalonStudio.Extensibility;
using AvalonStudio.Extensibility.Projects;
using AvalonStudio.Extensibility.Studio;
using AvalonStudio.Platforms;
using AvalonStudio.Shell;
using AvalonStudio.Utils;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace AvalonStudio.Projects
{
    [Obsolete("No longer used or supported. Remains here only to allow migration of projects still on the old format.")]
    public class AvalonStudioSolution : ISolution
    {
        public const string Extension = "asln";

        public ObservableCollection<ISolutionItem> Items { get; }

        public AvalonStudioSolution()
        {
            ProjectReferences = new List<string>();
            Items = new ObservableCollection<ISolutionItem>();
            Parent = Solution = this;
        }

        public string StartupItem { get; set; }

        [JsonProperty("Projects")]
        public IList<string> ProjectReferences { get; set; }

        public T AddItem<T>(T item, Guid? itemGuid = null, ISolutionFolder parent = null) where T : ISolutionItem
        {
            if (item is IProject project)
            {
                var currentProject = Projects.FirstOrDefault(p => p.Name == project.Name);

                if (currentProject != null) return (T)currentProject;
                ProjectReferences.Add(CurrentDirectory.MakeRelativePath(project.Location));
                Items.InsertSorted(project);
                currentProject = project;

                return (T)currentProject;
            }

            return item;
        }

        public void RemoveItem(ISolutionItem item)
        {
            if (item is IProject project)
            {
                Items.Remove(project);
                ProjectReferences.Remove(CurrentDirectory.MakeRelativePath(project.Location).ToAvalonPath());
            }

        }

        public void VisitChildren(Action<ISolutionItem> visitor)
        {
            foreach (var child in Items)
            {
                if (child is ISolutionFolder folder)
                {
                    folder.VisitChildren(visitor);
                }

                visitor(child);
            }
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
        public IEnumerable<IProject> Projects => Items.OfType<IProject>();

        [JsonIgnore]
        public IProject StartupProject { get; set; }

        [JsonIgnore]
        public string Name
        {
            get => Path.GetFileNameWithoutExtension(Location);
            set { }
        }

        [JsonIgnore]
        public bool CanRename => false;

        [JsonIgnore]
        public string Location { get; private set; }
        public ISolution Solution { get; set; }
        public ISolutionFolder Parent { get; set; }

        public Guid Id { get; set; }

        private static async Task<IProject> LoadProjectAsync(ISolution solution, string reference)
        {
            IProject result = null;

            var extension = Path.GetExtension(reference);

            var projectFilePath = Path.Combine(solution.CurrentDirectory, reference).ToPlatformPath();

            var projectType = IoC.Get<IStudio>().ProjectTypes.FirstOrDefault(
                p => extension.EndsWith(p.Metadata.DefaultExtension));
            
            if (projectType == null)
            {
                projectType = IoC.Get<IStudio>().ProjectTypes.FirstOrDefault(
                    p => p.Metadata.PossibleExtensions.Any(e => extension.EndsWith(e)));
            }

            if (projectType != null && File.Exists(projectFilePath))
            {
                result = await projectType.Value.LoadAsync(solution, projectFilePath);
            }
            else
            {
                Console.WriteLine("Failed to load " + projectFilePath);
            }

            return result;
        }

        public static VisualStudioSolution ConvertToSln(ISolution solution)
        {
            var result = VisualStudioSolution.Create(solution.CurrentDirectory, solution.Name, true, AvalonStudioSolution.Extension);

            foreach (var item in solution.Items)
            {
                if (item is IProject project)
                {
                    result.AddItem(project);
                }
            }

            result.StartupProject = solution.StartupProject;

            result.Save();

            return result;
        }

        public static async Task<ISolution> LoadAsync(string fileName)
        {
            try
            {
                return VisualStudioSolution.Load(fileName);
            }
            catch (Exception)
            {
                var solution = SerializedObject.Deserialize<AvalonStudioSolution>(fileName);

                solution.Location = fileName.NormalizePath().ToPlatformPath();
                solution.CurrentDirectory = (Path.GetDirectoryName(fileName) + Platform.DirectorySeperator).ToPlatformPath();

                foreach (var projectReference in solution.ProjectReferences)
                {
                    var proj = await LoadProjectAsync(solution, projectReference);

                    // todo null returned here we need a placeholder.
                    if (proj != null)
                    {
                        proj.Solution = solution;
                        solution.Items.InsertSorted(proj);
                    }
                }

                foreach (var project in solution.Projects)
                {
                    await project.ResolveReferencesAsync();
                }

                solution.StartupProject = solution.Projects.SingleOrDefault(p => p.Name == solution.StartupItem);

                var console = IoC.Get<IConsole>();

                console.WriteLine("Migrating ASLN to SLN format. Opening this file again will overwrite the newly created SLN file.");
                console.WriteLine("Please delte ASLN file when you are happy with the migration.");

                return ConvertToSln(solution);
            }
        }

        public IProject FindProject(string name)
        {
            throw new NotImplementedException();
        }

        public int CompareTo(ISolutionItem other)
        {
            return this.DefaultCompareTo(other);
        }

        public void UpdateItem(ISolutionItem item)
        {
            throw new NotImplementedException();
        }

        public Task LoadProjectsAsync()
        {
            throw new NotImplementedException();
        }

        public Task LoadSolutionAsync()
        {
            throw new NotImplementedException();
        }

        public IProject FindProjectByPath(string path)
        {
            throw new NotImplementedException();
        }

        public Task UnloadSolutionAsync()
        {
            return Task.CompletedTask;
        }

        public Task UnloadProjectsAsync()
        {
            return Task.CompletedTask;
        }

        public Task RestoreSolutionAsync()
        {
            return Task.CompletedTask;
        }
    }
}