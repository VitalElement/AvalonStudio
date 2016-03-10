namespace AvalonStudio.Projects
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.ComponentModel.Composition;
    using Perspex.Controls;
    using Debugging;
    using Toolchains;
    using TestFrameworks;

    [InheritedExport(typeof(IProject))]
    public interface IProject : IProjectFolder, IComparable<IProject>
    {
        ISolution Solution { get; }

        /// <summary>
        /// List of references with the project.
        /// </summary>
        ObservableCollection<IProject> References { get; }

        IToolChain ToolChain { get; set; }
        IDebugger Debugger { get; set; }
        ITestFramework TestFramework { get; set; }

        ISourceFile FindFile(string path);

        bool Hidden { get; set; }

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

        // TODO perhaps this shouldnt be tied to IProject?
        string Executable { get; set; }

        dynamic ToolchainSettings { get; }

        dynamic DebugSettings { get; }

        void Save();
    }
}
