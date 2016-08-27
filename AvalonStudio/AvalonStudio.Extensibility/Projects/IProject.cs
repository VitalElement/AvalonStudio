using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.Composition;
using AvalonStudio.Debugging;
using AvalonStudio.TestFrameworks;
using AvalonStudio.Toolchains;

namespace AvalonStudio.Projects
{
	[InheritedExport(typeof (IProject))]
	public interface IProject : IProjectFolder, IComparable<IProject>
	{
		ISolution Solution { get; }

		/// <summary>
		///     List of references with the project.
		/// </summary>
		ObservableCollection<IProject> References { get; }

		IToolChain ToolChain { get; set; }
		IDebugger Debugger { get; set; }
		ITestFramework TestFramework { get; set; }

		bool Hidden { get; set; }

		/// <summary>
		///     The directory the project file resides in.
		/// </summary>
		string CurrentDirectory { get; }

		/// <summary>
		///     The location of the project file
		/// </summary>
		string Location { get; }

		IList<object> ConfigurationPages { get; }
		string Extension { get; }

		//IDictionary<string, string> Settings { get; }        

		// TODO perhaps this shouldnt be tied to IProject?
		string Executable { get; set; }

		dynamic ToolchainSettings { get; }

		dynamic DebugSettings { get; }

		void AddReference(IProject project);

		void RemoveReference(IProject project);

		ISourceFile FindFile(ISourceFile file);

        void RegisterFile(ISourceFile file);
        void RegisterFolder(IProjectFolder folder);
        void UnregisterFile(ISourceFile file);
        void UnregisterFolder(IProjectFolder folder);


		/// <summary>
		///     Resolves all references in the project.
		/// </summary>
		void ResolveReferences();

		// TODO should these 2 methods be in seperate class?
		IProject Load(ISolution solution, string filePath);

		void Save();
	}
}