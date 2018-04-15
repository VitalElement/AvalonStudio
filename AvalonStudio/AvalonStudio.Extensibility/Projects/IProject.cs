using AvalonStudio.Debugging;
using AvalonStudio.TestFrameworks;
using AvalonStudio.Toolchains;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace AvalonStudio.Projects
{
    public interface IProject : IProjectFolder, ISolutionItem, IComparable<IProject>, IDisposable
    {
        /// <summary>
        ///     List of references with the project.
        /// </summary>
        ObservableCollection<IProject> References { get; }

        IToolchain ToolChain { get; set; }
        IDebugger Debugger2 { get; set; }

        ITestFramework TestFramework { get; set; }

        bool Hidden { get; set; }

        /// <summary>
        ///     The directory the project file resides in.
        /// </summary>
        string CurrentDirectory { get; }

        IList<object> ConfigurationPages { get; }

        //IDictionary<string, string> Settings { get; }

        // TODO perhaps this shouldnt be tied to IProject?
        string Executable { get; set; }

        dynamic ToolchainSettings { get; }

        dynamic Settings { get; }

        dynamic DebugSettings { get; }

        void AddReference(IProject project);

        /// <summary>
        /// Removes a reference from the project.
        /// </summary>
        /// <param name="project">The project to remove.</param>
        /// <returns>true if a reference was removed, false if no reference was removed.</returns>
        bool RemoveReference(IProject project);

        ISourceFile FindFile(string path);

        IReadOnlyList<ISourceFile> SourceFiles { get; }

        event EventHandler<ISourceFile> FileAdded;

        /// <summary>
        ///     Resolves all references in the project.
        /// </summary>
        Task ResolveReferencesAsync();

        /// <summary>
        /// This is called only once when a project is loaded and is used to populate the files.
        /// </summary>
        Task LoadFilesAsync();

        Task UnloadAsync();

        void Save();

        bool IsItemSupported(string languageName);
    }
}