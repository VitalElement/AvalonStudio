namespace AvalonStudio.Projects
{
    using System.Collections.Generic;
    using Toolchains;
    public interface IProject : IProjectFolder
    {
        ISolution Solution { get; }

        /// <summary>
        /// List of references with the project.
        /// </summary>
        IList<IProject> References { get; }        

        IToolChain ToolChain { get; }

        /// <summary>
        /// The directory the project file resides in.
        /// </summary>
        string CurrentDirectory { get; }

        /// <summary>
        /// The location of the project file
        /// </summary>
        string Location { get; }

        /// <summary>
        /// Resolves all references in the project.
        /// </summary>
        void ResolveReferences();

        void Save();
    }
}
