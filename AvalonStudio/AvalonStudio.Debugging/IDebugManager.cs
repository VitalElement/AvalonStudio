namespace AvalonStudio.Debugging
{
    using Projects;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public interface IDebugManager
    {
        /// <summary>
        /// The project currently being debugged.
        /// </summary>
        IProject Project { get; }
    }
}
