namespace AvalonStudio.Extensibility.Projects
{
    using AvalonStudio.Projects;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.Composition;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    [InheritedExport(typeof(IProjectTemplate))]
    public interface IProjectTemplate
    {
        string Title { get; }

        string Description { get; }

        IProject Generate();        
    }
}
