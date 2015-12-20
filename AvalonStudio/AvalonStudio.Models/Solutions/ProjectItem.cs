using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using AvalonStudio.Utils;
using AvalonStudio.Projects.Standard;

namespace AvalonStudio.Models.Solutions
{
    [Serializable]
    [XmlInclude (typeof (ProjectFolder))]
    [XmlInclude (typeof (ProjectFile))]
    public abstract class ProjectItem : Item, IComparable<ProjectItem>
    {
        public ProjectItem (Solution solution, Project project, Item container) : base (container)
        {
            this.Solution = solution;
            this.Project = project;

            if (container is ProjectFolder)
            {
                this.Container = container as ProjectFolder;
            }

            this.SaveChangesEnabled = true;
        }

        internal virtual void SetSolution (Solution solution, Project project, ProjectFolder container)
        {
            this.Solution = solution;
            this.Project = project;
            this.Container = container;
            this.SaveChangesEnabled = true;
        }
        
        public void SaveChanges ()
        {
            if (this.SaveChangesEnabled)
            {
                this.Solution.SaveChanges ();
            }
        }

        public int CompareTo(ProjectItem other)
        {
            return this.Title.CompareTo(other.Title);
        }        

        [XmlIgnore]
        public virtual string Title
        {
            get
            {
                return Path.GetFileNameWithoutExtension(FileName);
            }
        }        

        public string LocationRelativeToParent { get; set; }

        public virtual string CurrentDirectory
        {
            get
            {
                return Path.Combine (Container.CurrentDirectory, LocationRelativeToParent);
            }
        }

        protected bool SaveChangesEnabled { get; set; }

        [XmlIgnore]
        public virtual string Location
        {
            get { return Path.Combine (Container.CurrentDirectory, LocationRelativeToParent); }
        }

        [XmlIgnore]
        public virtual string RelativeLocation
        {
            get { return Project.CurrentDirectory.MakeRelativePath(this.Location); }
        }

        [XmlIgnore]
        public virtual string RelativeLocationWithProject
        {
            get
            {
                return Path.GetFileNameWithoutExtension(Project.FileName) + "->" + RelativeLocation;
            }
        }

        [XmlIgnore]
        public Project Project { get; protected set; }

        [XmlIgnore]
        public ProjectFolder Container { get; protected set; }
        
        [XmlIgnore]
        public Solution Solution;

        public SolutionFolder Parent
        {
            get
            {
                return Solution.GetParent(this);
            }
        }
    }
}
