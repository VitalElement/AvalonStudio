using AvalonStudio.Projects;
using AvalonStudio.Shell;
using AvalonStudio.Utils;
using Microsoft.DotNet.Cli.Sln.Internal;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;

namespace AvalonStudio.Extensibility.Projects
{
    public class VisualStudioSolution : ISolution
    {
        private SlnFile _solutionModel;
        private Dictionary<Guid, ISolutionItem> _solutionItems;

        public static VisualStudioSolution Load(string fileName)
        {
            return new VisualStudioSolution(SlnFile.Read(fileName));
        }

        private VisualStudioSolution(SlnFile solutionModel)
        {
            var shell = IoC.Get<IShell>();
            _solutionModel = solutionModel;
            _solutionItems = new Dictionary<Guid, ISolutionItem>();

            Parent = Solution = this;

            Id = Guid.NewGuid();

            Items = new ObservableCollection<ISolutionItem>();

            LoadFolders();

            LoadProjects();

            ResolveReferences();

            BuildTree();

            this.PrintTree();
        }

        private void BuildTree()
        {
            var nestedProjects = _solutionModel.Sections.FirstOrDefault(section => section.Id == "NestedProjects");

            if (nestedProjects != null)
            {
                foreach (var nestedProject in nestedProjects.Properties)
                {
                    _solutionItems[Guid.Parse(nestedProject.Key)].SetParentInternal(_solutionItems[Guid.Parse(nestedProject.Value)] as ISolutionFolder);
                }
            }
        }

        private void LoadFolders()
        {
            var solutionFolders = _solutionModel.Projects.Where(p => p.TypeGuid == ProjectTypeGuids.SolutionFolderGuid);

            foreach (var solutionFolder in solutionFolders)
            {
                var newItem = new SolutionFolder(solutionFolder.Id, solutionFolder.Name, this);
                _solutionItems.Add(newItem.Id, newItem);
                Items.InsertSorted(newItem);
            }
        }

        private void ResolveReferences()
        {
            foreach (var project in Projects)
            {
                project.ResolveReferences();
            }
        }

        private void LoadProjects()
        {
            var solutionProjects = _solutionModel.Projects.Where(p => p.TypeGuid != ProjectTypeGuids.SolutionFolderGuid);

            foreach (var project in solutionProjects)
            {
                var projectType = project.GetProjectType();

                IProject newProject = null;

                var projectLocation = Path.Combine(this.CurrentDirectory, project.FilePath);

                if (projectType != null)
                {
                    newProject = projectType.Load(this, projectLocation);
                }
                else
                {
                    newProject = new UnsupportedProjectType(this, projectLocation);
                }

                newProject.Id = Guid.Parse(project.Id);
                newProject.Solution = this;
                (newProject as ISolutionItem).Parent = this;

                _solutionItems.Add(newProject.Id, newProject);
                Items.InsertSorted(newProject);
            }
        }

        public string Name => Path.GetFileNameWithoutExtension(_solutionModel.FullPath);

        public string Location => _solutionModel.FullPath;

        public IProject StartupProject { get; set; }

        public IEnumerable<IProject> Projects => _solutionItems.Select(kv => kv.Value).OfType<IProject>();

        public string CurrentDirectory => Path.GetDirectoryName(_solutionModel.FullPath) + "\\";

        public ObservableCollection<ISolutionItem> Items { get; private set; }

        public ISolution Solution { get; set; }

        public ISolutionFolder Parent { get; set; }

        public Guid Id { get; set; }

        public IProject AddProject(IProject project)
        {
            var currentProject = Projects.FirstOrDefault(p => p.Location == project.Location);
            
            if (currentProject == null)
            {
                project.Id = Guid.NewGuid();
                project.Solution = this;
                (project as ISolutionItem).Parent = this;

                _solutionItems.Add(project.Id, project);
                Items.InsertSorted(project);

                _solutionModel.Projects.Add(new SlnProject
                {
                    Id = project.Id.GetGuidString(),
                    TypeGuid = project.ProjectTypeId.GetGuidString(),
                    Name = project.Name,
                    FilePath = CurrentDirectory.MakeRelativePath(project.Location)
                });
            }
            else
            {
                return currentProject;
            }

            return project;
        }

        public void RemoveProject(IProject project)
        {
            Items.Remove(project);
            _solutionItems.Remove(project.Id);

            var currentSlnProject = _solutionModel.Projects.FirstOrDefault(slnProj => Guid.Parse(slnProj.Id) == project.Id);

            if(currentSlnProject != null)
            {
                _solutionModel.Projects.Remove(currentSlnProject);
            }
        }

        public ISourceFile FindFile(string path)
        {
            throw new NotImplementedException();
        }

        public void Save()
        {
            _solutionModel.Write();
        }

        public void SetItemParent(ISolutionItem item, ISolutionFolder parent)
        {
            var nestedProjects = _solutionModel.Sections.FirstOrDefault(section => section.Id == "NestedProjects");

            if (nestedProjects != null)
            {
                nestedProjects.Properties.Remove(item.Id.GetGuidString());
            }

            item.SetParentInternal(parent);

            if (parent != this)
            {
                if(nestedProjects == null)
                {
                    _solutionModel.Sections.Add(new SlnSection() { Id = "NestedProjects", SectionType = SlnSectionType.PreProcess });
                }

                nestedProjects.Properties[item.Id.GetGuidString()] = parent.Id.GetGuidString();
            }

            _solutionModel.Write();
        }

        public int CompareTo(ISolutionItem other)
        {
            return this.DefaultCompareTo(other);
        }
    }
}
