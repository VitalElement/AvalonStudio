using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using AvalonStudio.Debugging;
using AvalonStudio.TestFrameworks;
using AvalonStudio.Toolchains;
using System.Composition;

namespace AvalonStudio.Projects
{
	//[InheritedExport(typeof (IProject))]
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

		IList<object> ConfigurationPages { get; }
		string Extension { get; }

		//IDictionary<string, string> Settings { get; }        

		// TODO perhaps this shouldnt be tied to IProject?
		string Executable { get; set; }

		dynamic ToolchainSettings { get; }

		dynamic DebugSettings { get; }

		void AddReference(IProject project);

		void RemoveReference(IProject project);

        ISourceFile FindFile(string path);

        event EventHandler FileAdded;


		/// <summary>
		///     Resolves all references in the project.
		/// </summary>
		void ResolveReferences();

		IProject Load(ISolution solution, string filePath);

		void Save();
	}
}