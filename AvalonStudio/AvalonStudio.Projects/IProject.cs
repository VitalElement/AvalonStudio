namespace AvalonStudio.Projects
{
    using System.Collections.Generic;

    public interface IProject
    {
        string Name { get; }

        ISolution Solution { get; }

        /// <summary>
        /// List of references with the project.
        /// </summary>
        IList<IProject> References { get; }

        /// <summary>
        /// List of files within the project.
        /// </summary>
        IList<ISourceFile> SourceFiles { get; }

        /// <summary>
        /// The directory the project file resides in.
        /// </summary>
        string CurrentDirectory { get; }

        /// <summary>
        /// The location of the project file
        /// </summary>
        string Location { get; }
    }
}
