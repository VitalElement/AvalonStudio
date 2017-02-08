using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using AvalonStudio.Extensibility;
using AvalonStudio.Extensibility.Git;
using AvalonStudio.Models.Tools.Debuggers.Local;
using AvalonStudio.Projects;
using AvalonStudio.Projects.CPlusPlus;
using AvalonStudio.Shell;
using AvalonStudio.TestFrameworks.Catch;
using AvalonStudio.Toolchains.LocalGCC;
using AvalonStudio.Utils;
using AvalonStudio.Toolchains.GCC;

namespace AvalonStudio.Languages.CPlusPlus
{
	public class CPlusPlusUnitTestTemplate : BlankCPlusPlusLanguageTemplate, IConsole
	{
		public override string DefaultProjectName
		{
			get { return "UnitTestProject"; }
		}

		public override string Description
		{
			get { return "Creates a Unit Test Project using Catch Test Framework."; }
		}

		public override string Title
		{
			get { return "C/C++ Unit Test Project"; }
		}

		public void Clear()
		{
			throw new NotImplementedException();
		}

		public void OverWrite(string data)
		{
			Console.WriteLine(data);
		}

		public void Write(char data)
		{
			throw new NotImplementedException();
		}

		public void Write(string data)
		{
			throw new NotImplementedException();
		}

		public void WriteLine()
		{
			throw new NotImplementedException();
		}

		public void WriteLine(string data)
		{
			throw new NotImplementedException();
		}

		public override async Task<IProject> Generate(ISolution solution, string name)
		{
			var shell = IoC.Get<IShell>();
			var project = await base.Generate(solution, name);
			var catchProjectDir = Path.Combine(solution.CurrentDirectory, "AvalonStudio.Testing.Catch");
			var catchProjectFile = Path.Combine(catchProjectDir, "CatchTestFramework.acproj");
			if (!Directory.Exists(catchProjectDir))
			{
				await
					Git.ClonePublicHttpSubmodule(this, "https://github.com/VitalElement/AvalonStudio.Testing.Catch.git",
						catchProjectDir);
			}

			// Add project
			var catchProject = Solution.LoadProjectFile(solution, catchProjectFile);
			catchProject.Hidden = true;

			solution.AddProject(catchProject);

			// Reference catch.
			project.AddReference(catchProject);

			await SourceFile.Create(project, "UnitTest1.cpp", Platform);

			project.ToolChain = shell.ToolChains.FirstOrDefault(tc => tc is LocalGCCToolchain);
			project.Debugger = shell.Debuggers.FirstOrDefault(d => d is LocalDebugAdaptor);
			project.TestFramework = shell.TestFrameworks.FirstOrDefault(d => d is CatchTestFramework);
            project.ToolChain.ProvisionSettings(project);

            var settings = LocalGCCToolchain.GetSettings(project);
			settings.CompileSettings.Exceptions = true;
			settings.CompileSettings.Rtti = true;
			settings.CompileSettings.Optimization = OptimizationLevel.Debug;
			settings.CompileSettings.CustomFlags += " -Wno-unknown-pragmas ";

			project.Save();
			solution.Save();

			return project;
		}
	}
}