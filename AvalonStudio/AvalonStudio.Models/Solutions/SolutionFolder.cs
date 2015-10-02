namespace AvalonStudio.Models.Solutions
{
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Xml.Serialization;

    public class SolutionFolder : Item
    {
        private SolutionFolder() : base (null)
        {
            Children = new ObservableCollection<Item>();            
        }

        public SolutionFolder (Solution solution, Item parent, string name) : base (parent)
        {
            Solution = solution;
            Children = new ObservableCollection<Item>();
            Name = name;
        }

        public SolutionFolder AddFolder(string name)
        {
            var folder = new SolutionFolder(this.Solution, this, name);

            AttachItem(folder);

            return folder;
        }

        public void AddProject (Project project, UnloadedProject unloadedProject)
        {
            project.ParentId = this.Id;
            Children.Add(project);
            Solution.UnloadedChildren.Add(unloadedProject);
            Solution.LoadedProjects.Add(project);
            Solution.SaveChanges();
        }

        public void AttachItem (Item item)
        {
            item.ParentId = this.Id;

            if(item is Project)
            {                
                Solution.UnloadedChildren.Add((item as Project).GetUnloadedProject());
            }
            else
            {
                Solution.UnloadedChildren.Add(item);
            }

            Children.Add(item);
            
            Solution.SaveChanges();
        }        

        public void DetachItem (Item item)
        {            
            Children.Remove(item);

            if (item is Project)
            {
                var unloadedProject = Solution.UnloadedChildren.OfType<UnloadedProject>().First((up) => up.Id == item.Id);

                Solution.UnloadedChildren.Remove(unloadedProject);                
            }
            else
            {
                Solution.UnloadedChildren.Remove(item);
            }

            Solution.SaveChanges();
        }

        public void RemoveItem (Item item)
        {
            if(item is SolutionFolder)
            {
                var folder = item as SolutionFolder;

                var children = folder.Children;

                foreach (var child in children)
                {
                    RemoveItem(child);
                }
            }

            if(item is Project)
            {
                Solution.LoadedProjects.Remove(item as Project);
            }

            DetachItem(item);
        }

        public SolutionFolder GetParent (Item item)
        {
            SolutionFolder result = null;

            if (Id == item.ParentId)
            {
                result = this;
            }
            else
            {
                foreach (var child in Children)
                {
                    if (child is SolutionFolder)
                    {
                        var subFolder = child as SolutionFolder;

                        if (child.Id == item.ParentId)
                        {
                            result = subFolder;
                            break;
                        }
                        else
                        {
                            var childResult = subFolder.GetParent(item);

                            if (childResult != null)
                            {
                                result = childResult;
                                break;
                            }
                        }
                    }
                }
            }

            return result;
        }

       

        [XmlIgnore]
        public Solution Solution { get; set; }

        [XmlIgnore]
        public ObservableCollection<Item> Children { get; set; }        

        public string Name { get; set; }

        public override string FileName
        {
            get
            {
                return Name;
            }
            set
            {
                Name = value;
            }
        }
    }
}
