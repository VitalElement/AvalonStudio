﻿namespace AvalonStudio.Projects
{
    using Perspex.Controls;
    using System.Collections.Generic;
    using System.ComponentModel.Composition;
    using Toolchains;

    [InheritedExport(typeof(IProject))]
    public interface IProject : IProjectFolder
    {
        ISolution Solution { get; }

        /// <summary>
        /// List of references with the project.
        /// </summary>
        IList<IProject> References { get; }

        IToolChain ToolChain { get; set; }

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

        IList<TabItem> ConfigurationPages { get; }

        // TODO should these 2 methods be in seperate class?
        IProject Load(ISolution solution, string filePath);
        string Extension { get; }

        //IDictionary<string, string> Settings { get; }

        dynamic ToolchainSettings { get; }

        dynamic DebugSettings { get; }

        void Save();
    }
}
