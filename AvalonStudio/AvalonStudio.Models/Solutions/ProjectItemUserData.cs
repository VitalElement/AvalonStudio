using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AvalonStudio.Models.Solutions
{
    public class ProjectItemUserData
    {
        private ProjectItemUserData()
        {
        }

        public ProjectItemUserData (Guid guid) : this()
        {
            Id = guid;
        }

        public Guid Id { get; set; }

        public bool IsExpanded { get; set; }        

        public int SelectedDebugAdaptorIndex { get; set; }

        public int SelectedConfigurationIndex { get; set; }    
        
        public bool BreakOnMain { get; set; }
        public bool RunImmediately { get; set; }    
    }
}
