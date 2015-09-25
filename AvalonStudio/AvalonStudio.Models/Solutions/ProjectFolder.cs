using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Serialization;
using AvalonStudio.Utils;

namespace AvalonStudio.Models.Solutions
{
    public class ProjectFolder : ProjectItem
    {
        private ProjectFolder()
            : base(null, null, null)
        {

        }

        public ProjectFolder(Solution solution, Project project, Item container) : base(solution, project, container)
        {
            Children = new ObservableCollection<ProjectItem>();
        }

        public ProjectFolder(Solution solution, Project project, ProjectFolder container, string location)
            : this(solution, project, container)
        {
            this.LocationRelativeToParent = container.CurrentDirectory.MakeRelativePath(location) + "\\";
        }

        internal override void SetSolution(Solution solution, Project project, ProjectFolder folder)
        {
            base.SetSolution(solution, project, folder);

            foreach (ProjectItem item in this.Children)
            {
                item.SetSolution(solution, project, this);
            }
        }

        public void RemoveItem(ProjectItem item)
        {
            if (this.Children.Contains(item))
            {
                this.Children.Remove(item);
            }

            this.SaveChanges();
        }


        public ProjectFolder AddNewFolder(string name)
        {
            ProjectFolder result = null;

            string newFolder = Path.Combine(this.CurrentDirectory, name);

            result = new ProjectFolder(this.Solution, this.Project, this, newFolder);

            if (!Directory.Exists(newFolder))
            {
                Directory.CreateDirectory(newFolder);
            }

            AddChild(result);

            this.SaveChanges();

            return result;
        }

        public ProjectFile AddNewFile(string name, string content, FileType language = FileType.Inherit)
        {
            ProjectFile result = null;

            var location = Path.Combine(this.CurrentDirectory, name);

            if (!File.Exists(location))
            {
                using (var file = File.CreateText(location))
                {
                    file.Write(content);
                    file.Close();
                }

            }

            result = new ProjectFile(this.Solution, this.Project, this, location, language);

            AddChild(result);

            this.SaveChanges();

            return result;
        }

        public void AddChild(ProjectItem item)
        {
            int insertIndex = 0;
            int binaryIndex = 1;
            if (item is ProjectFolder)
            {
                binaryIndex = Children.OfType<ProjectFolder>().ToList().BinarySearch(item as ProjectFolder);
            }
            else
            {
                insertIndex = 0;

                var folders = Children.OfType<ProjectFolder>();

                insertIndex += folders.Count();

                binaryIndex = Children.OfType<ProjectFile>().ToList().BinarySearch(item as ProjectFile);
            }

            if (binaryIndex <= 0)
            {
                insertIndex += Math.Abs(binaryIndex) -1;
            }
            else
            {
                return;
            }

            if (insertIndex == -1)
            {
                throw new Exception("Unable to calculate index to insert item into collection.");
            }

            if (insertIndex > Children.Count())
            {
                insertIndex = Children.Count();
            }

            Children.Insert(insertIndex, item);
        }

        public void SortChildren()
        {
            var folders = Children.Where((c) => c is ProjectFolder).OrderBy((c) => c.Title);
            var files = Children.Where((c) => c is ProjectFile).OrderBy((c) => c.Title);
            Children = new ObservableCollection<ProjectItem>(folders.Concat(files));
        }

        private FileType GetLanguageFromExtension(string extension)
        {
            switch (extension.ToLower().Replace(".", ""))
            {
                case "c":
                    return FileType.C;

                case "cpp":
                case "hpp":
                    return FileType.CPlusPlus;

                case "h":
                    return FileType.C;

                case "s":
                    return FileType.Asm;

                default:
                    return FileType.File;
            }
        }

        public ProjectFile AddExistingFile(string filePath)
        {
            ProjectFile result = null;

            if (File.Exists(filePath))
            {
                if (!this.Children.Any((c) => c.Location == filePath))
                {
                    result = new ProjectFile(this.Solution, this.Project, this, filePath, GetLanguageFromExtension(Path.GetExtension(filePath)));

                    AddChild(result);

                    this.SaveChanges();
                }
            }

            return result;
        }

        private void RecurseImportFolder(string folder)
        {
            this.SaveChangesEnabled = false;

            if (Directory.Exists(folder))
            {
                foreach (string file in Directory.GetFiles(folder))
                {
                    var extension = Path.GetExtension(file);

                    switch (extension.ToLower())
                    {
                        case ".c":
                        case ".cpp":
                        case ".h":
                        case ".hpp":
                        case ".s":
                        case ".ld":
                        case ".mk":
                            AddExistingFile(file);
                            break;
                    }
                }

                foreach (string subFolder in Directory.GetDirectories(folder))
                {
                    if (!this.Children.Any((c) => c.Location == subFolder))
                    {
                        var newFolder = this.AddNewFolder(new DirectoryInfo(subFolder).Name);

                        newFolder.RecurseImportFolder(subFolder);

                        if (newFolder.Children.Count == 0)
                        {
                            this.Children.Remove(newFolder);
                        }
                    }
                }
            }

            this.SaveChangesEnabled = true;
        }

        public void ImportFolder(string folder)
        {
            RecurseImportFolder(folder);

            this.SaveChanges();
        }

        public bool VisitAllFiles(Func<ProjectFile, bool> func)
        {
            foreach (ProjectItem item in this.Children)
            {
                if (item is ProjectFolder)
                {
                    if ((item as ProjectFolder).VisitAllFiles(func))
                    {
                        return true;
                    }
                }
                else if (item is ProjectFile)
                {
                    if (func(item as ProjectFile))
                    {
                        return true;
                    }

                }
            }

            return false;
        }

        public bool VisitAllFolders(Func<ProjectFolder, bool> func)
        {
            foreach (ProjectItem item in this.Children)
            {
                if (item is ProjectFolder)
                {
                    if ((item as ProjectFolder).VisitAllFolders(func))
                    {
                        return true;
                    }

                    func(item as ProjectFolder);
                }
            }

            return false;
        }

        public List<string> GetFiles()
        {
            List<string> result = new List<string>();

            VisitAllFiles((i) =>
            {
                if (!i.IsHeaderFile)
                {
                    result.Add(i.Location);
                }

                return false;
            });

            return result;
        }

        public bool VisitAllChildren(Func<ProjectItem, bool> func)
        {
            if (func(this))
            {
                return true;
            }

            foreach (ProjectItem item in this.Children)
            {
                if (item is ProjectFolder)
                {
                    if ((item as ProjectFolder).VisitAllChildren(func))
                    {
                        return true;
                    }

                    if (func(item))
                    {
                        return true;
                    }
                }
                else if (item is ProjectFile)
                {
                    if (func(item))
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        public ObservableCollection<ProjectItem> Children { get; set; }

        [XmlIgnore]
        public override string FileName
        {
            get
            {
                var result = new DirectoryInfo(this.Location).Name;

                return result;
            }
            set
            {
                var newFolderName = Path.Combine(new DirectoryInfo(this.Location).Parent.FullName, value) + "\\";

                if (CurrentDirectory != newFolderName)
                {
                    Directory.Move(CurrentDirectory, newFolderName);
                    this.LocationRelativeToParent = Container.CurrentDirectory.MakeRelativePath(newFolderName) + "\\"; ;
                    this.SaveChanges();
                }

                Container.Children.Remove(this);
                Container.AddChild(this);
            }
        }
    }
}
