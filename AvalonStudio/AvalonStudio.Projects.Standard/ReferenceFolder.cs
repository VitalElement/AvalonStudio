using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AvalonStudio.Projects.Standard
{
    public class ReferenceFolder : IReferenceFolder
    {
        public ReferenceFolder(IProject project)
        {
            references = project.References;        

            Parent = project;
            Project = project;         
        }

        public string Name
        {
            get { return "References"; }
        }


        private ObservableCollection<IProject> references;
        public ObservableCollection<IProject> References
        {
            get { return references; }
            set { this.references = value; }
        }


        public IProject Project { get; set; }

        public IProjectFolder Parent { get; set; }        
    }
}

