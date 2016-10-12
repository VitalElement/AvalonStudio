using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Dynamic;
using System.IO;
using Avalonia.Controls;
using AvalonStudio.Platforms;
using AvalonStudio.Projects;
using AvalonStudio.Projects.Standard;
using AvalonStudio.Toolchains.GCC;
using AvalonStudio.Toolchains.Standard;
using AvalonStudio.Utils;

namespace AvalonStudio.Toolchains.LocalGCC
{
	public class LocalGCCToolchain : GCCToolchain
	{
		private string BaseDirectory
		{
			get { return Path.Combine(Platform.ReposDirectory, "AvalonStudio.Toolchains.LocalGCC"); }
		}

        public override string Prefix => string.Empty;

        public override string BinDirectory => Path.Combine(BaseDirectory, "bin");

        public override string GetBaseLibraryArguments(IStandardProject superProject)
        {
            return string.Empty;
        }

        public string LinkerScript { get; set; }

		public override string GDBExecutable
		{
			get { return Path.Combine(BaseDirectory, "bin", "gdb" + Platform.ExecutableExtension); }
		}

		public override Version Version
		{
			get { return new Version(1, 0, 0); }
		}

		public override string Description
		{
			get { return "GCC based toolchain PC."; }
		}

		public override string ExecutableExtension
		{
			get { return ".exe"; }
		}

		public override string StaticLibraryExtension
		{
			get { return ".a"; }
		}

		public override void ProvisionSettings(IProject project)
		{
			ProvisionLocalGccSettings(project);
		}

		public static LocalGccToolchainSettings ProvisionLocalGccSettings(IProject project)
		{
			var result = GetSettings(project);

			if (result == null)
			{
				project.ToolchainSettings.LocalGCC = new LocalGccToolchainSettings();
				result = project.ToolchainSettings.LocalGCC;
				project.Save();
			}

			return result;
		}

		public static LocalGccToolchainSettings GetSettings(IProject project)
		{
			LocalGccToolchainSettings result = null;

			try
			{
				if (project.ToolchainSettings.LocalGCC is ExpandoObject)
				{
					result = (project.ToolchainSettings.LocalGCC as ExpandoObject).GetConcreteType<LocalGccToolchainSettings>();
				}
				else
				{
					result = project.ToolchainSettings.LocalGCC;
				}
			}
			catch (Exception)
			{
			}

			return result;
		}

		private string GetLinkerScriptLocation(IStandardProject project)
		{
			return Path.Combine(project.CurrentDirectory, "link.ld");
		}

		/*public override LinkResult Link(IConsole console, IStandardProject superProject, IStandardProject project,
			CompileResult assemblies, string outputPath)
		{
			var settings = GetSettings(superProject);
			var result = new LinkResult();

			var startInfo = new ProcessStartInfo();

			if (Platform.PlatformIdentifier == PlatformID.Unix)
			{
				startInfo.FileName = "g++";
			}
			else
			{
				startInfo.FileName = Path.Combine(BaseDirectory, "bin", "g++" + Platform.ExecutableExtension);
			}

			if (project.Type == ProjectType.StaticLibrary)
			{
				if (Platform.PlatformIdentifier == PlatformID.Unix)
				{
					startInfo.FileName = "ar";
				}
				else
				{
					startInfo.FileName = Path.Combine(BaseDirectory, "bin", "ar" + Platform.ExecutableExtension);
				}
			}

			startInfo.WorkingDirectory = project.Solution.CurrentDirectory;

			if (!System.IO.File.Exists(startInfo.FileName) && Platform.PlatformIdentifier != PlatformID.Unix)
			{
				result.ExitCode = -1;
				console.WriteLine("Unable to find linker executable (" + startInfo.FileName + ") Check project compiler settings.");
				return result;
			}

			var objectArguments = string.Empty;
			foreach (var obj in assemblies.ObjectLocations)
			{
				objectArguments += obj + " ";
			}

			var libs = string.Empty;
			foreach (var lib in assemblies.LibraryLocations)
			{
				libs += lib + " ";
			}

			var outputDir = Path.GetDirectoryName(outputPath);

			if (!Directory.Exists(outputDir))
			{
				Directory.CreateDirectory(outputDir);
			}

			var outputName = Path.GetFileNameWithoutExtension(outputPath) + ExecutableExtension;

			if (project.Type == ProjectType.StaticLibrary)
			{
				outputName = Path.GetFileNameWithoutExtension(outputPath) + StaticLibraryExtension;
			}

			var executable = Path.Combine(outputDir, outputName);

			var linkedLibraries = string.Empty;

			foreach (var libraryPath in project.StaticLibraries)
			{
				var relativePath = Path.GetDirectoryName(libraryPath);

				var libName = Path.GetFileNameWithoutExtension(libraryPath).Substring(3);

				linkedLibraries += string.Format("-L\"{0}\" -l{1} ", relativePath, libName);
			}

			foreach (var lib in project.BuiltinLibraries)
			{
				linkedLibraries += string.Format("-l{0} ", lib);
			}

			// Hide console window
			startInfo.UseShellExecute = false;
			startInfo.RedirectStandardOutput = true;
			startInfo.RedirectStandardError = true;
			startInfo.RedirectStandardInput = true;
			startInfo.CreateNoWindow = true;

			if (project.Type == ProjectType.StaticLibrary)
			{
				startInfo.Arguments = string.Format("rvs {0} {1}", executable, objectArguments);
			}
			else
			{
				startInfo.Arguments = string.Format("{0} -o{1} {2} -Wl,--start-group {3} {4} -Wl,--end-group",
					GetLinkerArguments(superProject, project), executable, objectArguments, linkedLibraries, libs);
			}

			//console.WriteLine(Path.GetFileNameWithoutExtension(startInfo.FileName) + " " + startInfo.Arguments);
			//console.WriteLine ("[LL] - " + startInfo.Arguments);

			using (var process = Process.Start(startInfo))
			{
				process.OutputDataReceived += (sender, e) =>
				{
					//console.WriteLine(e.Data);
				};

				process.ErrorDataReceived += (sender, e) =>
				{
					if (e.Data != null && !e.Data.Contains("creating"))
					{
						console.WriteLine(e.Data);
					}
				};

				process.BeginOutputReadLine();

				process.BeginErrorReadLine();

				process.WaitForExit();

				result.ExitCode = process.ExitCode;

				if (result.ExitCode == 0)
				{
					result.Executable = executable;
				}
			}

			return result;
		}*/

		public override ProcessResult Size(IConsole console, IStandardProject project, LinkResult linkResult)
		{
			var result = new ProcessResult();

			var startInfo = new ProcessStartInfo();

			if (Platform.PlatformIdentifier == PlatformID.Unix)
			{
				startInfo.FileName = "size";
			}
			else
			{
				startInfo.FileName = Path.Combine(BaseDirectory, "bin", "size" + Platform.ExecutableExtension);
			}

			if (!System.IO.File.Exists(startInfo.FileName) && Platform.PlatformIdentifier != PlatformID.Unix)
			{
				console.WriteLine("Unable to find tool (" + startInfo.FileName + ") check project compiler settings.");
				result.ExitCode = -1;
				return result;
			}

			startInfo.Arguments = string.Format("{0}", linkResult.Executable);

			// Hide console window
			startInfo.UseShellExecute = false;
			startInfo.RedirectStandardOutput = true;
			startInfo.RedirectStandardError = true;
			startInfo.RedirectStandardInput = true;
			startInfo.CreateNoWindow = true;


			using (var process = Process.Start(startInfo))
			{
				process.OutputDataReceived += (sender, e) => { console.WriteLine(e.Data); };

				process.ErrorDataReceived += (sender, e) => { console.WriteLine(e.Data); };

				process.BeginOutputReadLine();

				process.BeginErrorReadLine();

				process.WaitForExit();

				result.ExitCode = process.ExitCode;
			}

			return result;
		}

        public override string GetLinkerArguments(IStandardProject superProject, IStandardProject project)
        {
			var settings = GetSettings(project);

			var result = string.Empty;

			result += string.Format("-flto -static-libgcc -static-libstdc++ -Wl,-Map={0}.map ",
				Path.GetFileNameWithoutExtension(project.Name));

			result += string.Format("{0} ", settings.LinkSettings.MiscLinkerArguments);

			if (settings.LinkSettings.DiscardUnusedSections)
			{
				result += "-Wl,--gc-sections ";
			}

			switch (settings.CompileSettings.Optimization)
			{
				case OptimizationLevel.None:
					result += " -O0";
					break;

				case OptimizationLevel.Level1:
					result += " -O1";
					break;

				case OptimizationLevel.Level2:
					result += " -O2";
					break;

				case OptimizationLevel.Level3:
					result += " -O3";
					break;
			}

			return result;
		}

		public override string GetCompilerArguments(IStandardProject superProject, IStandardProject project, ISourceFile file)
		{
			var result = string.Empty;

			//var settings = GetSettings(project).CompileSettings;
			var settings = GetSettings(superProject);

			result += "-Wall -c ";

			if (settings.CompileSettings.DebugInformation)
			{
				result += "-g ";
			}

			// TODO remove dependency on file?
			if (file != null)
			{
				if (file.Extension == ".cpp")
				{
					if (!settings.CompileSettings.Rtti)
					{
						result += "-fno-rtti ";
					}

					if (!settings.CompileSettings.Exceptions)
					{
						result += "-fno-exceptions ";
					}
				}
			}

			// TODO make this an option.
			result += "-ffunction-sections -fdata-sections ";

			switch (settings.CompileSettings.Optimization)
			{
				case OptimizationLevel.None:
				{
					result += "-O0 ";
				}
					break;

				case OptimizationLevel.Debug:
				{
					result += "-Og ";
				}
					break;

				case OptimizationLevel.Level1:
				{
					result += "-O1 ";
				}
					break;

				case OptimizationLevel.Level2:
				{
					result += "-O2 ";
				}
					break;

				case OptimizationLevel.Level3:
				{
					result += "-O3 ";
				}
					break;
			}

			switch (settings.CompileSettings.OptimizationPreference)
			{
				case OptimizationPreference.Size:
				{
					result += "-Os ";
				}
					break;

				case OptimizationPreference.Speed:
				{
					result += "-Ofast ";
				}
					break;
			}

			result += settings.CompileSettings.CustomFlags + " ";

			// toolchain includes

			// Referenced includes
			var referencedIncludes = project.GetReferencedIncludes();

			foreach (var include in referencedIncludes)
			{
				result += string.Format("-I\"{0}\" ", Path.Combine(project.CurrentDirectory, include));
			}

			// global includes
			var globalIncludes = superProject.GetGlobalIncludes();

			foreach (var include in globalIncludes)
			{
				result += string.Format("-I\"{0}\" ", include);
			}

			// public includes
			foreach (var include in project.PublicIncludes)
			{
				result += string.Format("-I\"{0}\" ", Path.Combine(project.CurrentDirectory, include));
			}

			// includes
			foreach (var include in project.Includes)
			{
				result += string.Format("-I\"{0}\" ", Path.Combine(project.CurrentDirectory, include.Value));
			}

			var referencedDefines = project.GetReferencedDefines();
			foreach (var define in referencedDefines)
			{
				result += string.Format("-D{0} ", define);
			}

			// global includes
			var globalDefines = superProject.GetGlobalDefines();

			foreach (var define in globalDefines)
			{
				result += string.Format("-D{0} ", define);
			}

			foreach (var define in project.Defines)
			{
				result += string.Format("-D{0} ", define.Value);
			}

			if (Platform.PlatformIdentifier == PlatformID.Win32NT)
			{
				result += string.Format("-D{0} ", "WIN32NT");
			}

			foreach (var arg in superProject.ToolChainArguments)
			{
				result += string.Format(" {0}", arg);
			}

			foreach (var arg in superProject.CompilerArguments)
			{
				result += string.Format(" {0}", arg);
			}

			// TODO factor out this code from here!
			if (file != null)
			{
				switch (file.Extension)
				{
					case ".c":
					{
						foreach (var arg in superProject.CCompilerArguments)
						{
							result += string.Format(" {0}", arg);
						}
					}
						break;

					case ".cpp":
					{
						foreach (var arg in superProject.CppCompilerArguments)
						{
							result += string.Format(" {0}", arg);
						}
					}
						break;
				}
			}

			return result;
		}

		public override List<string> GetToolchainIncludes()
		{
			return new List<string>
			{
                Path.Combine(BaseDirectory, "lib", "gcc", "x86_64-w64-mingw32", "5.2.0", "include"),
                Path.Combine(BaseDirectory, "lib", "gcc", "x86_64-w64-mingw32", "5.2.0", "include-fixed"),
                Path.Combine(BaseDirectory, "x86_64-w64-mingw32", "include"),
                Path.Combine(BaseDirectory, "x86_64-w64-mingw32", "include", "c++"),
                Path.Combine(BaseDirectory, "x86_64-w64-mingw32", "include", "c++", "x86_64-w64-mingw32"),
                Path.Combine(BaseDirectory, "x86_64-w64-mingw32", "include", "c++", "x86_64-w64-mingw32", "backward")
            };
		}

		public override bool SupportsFile(ISourceFile file)
		{
			var result = false;

			if (Path.GetExtension(file.Location) == ".cpp" || Path.GetExtension(file.Location) == ".c")
			{
				result = true;
			}

			return result;
		}

		public override IList<object> GetConfigurationPages(IProject project)
		{
			var result = new List<object>();

			result.Add(new CompileSettingsFormViewModel(project));
			result.Add(new LinkerSettingsFormViewModel(project));

			return result;
		}

		public override bool CanHandle(IProject project)
		{
			var result = false;

			if (project is IStandardProject)
			{
				result = true;
			}

			return result;
		}
	}
}