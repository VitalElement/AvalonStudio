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

namespace AvalonStudio.Toolchains.Clang
{
	public class ClangGCCToolchain : GCCToolchain
	{
		private string BaseDirectory
		{
			get { return Path.Combine(Platform.ReposDirectory, "AvalonStudio.Toolchains.Clang", "bin"); }
		}

		#region Settings

		public string LinkerScript { get; set; }

		#endregion

		//public void GenerateLinkerScript(Project project)
		//{
		//    var template = new ArmGCCLinkTemplate(project.SelectedConfiguration);

		//    string linkerScript = GetLinkerScriptLocation(project);

		//    if (File.Exists(linkerScript))
		//    {
		//        File.Delete(linkerScript);
		//    }

		//    var sw = File.CreateText(linkerScript);

		//    sw.Write(template.TransformText());

		//    sw.Close();
		//}

		//private string GetLinkerScriptLocation(Project project)
		//{
		//    return Path.Combine(project.CurrentDirectory, "link.ld");
		//}


		public override Version Version
		{
			get { return new Version(1, 0, 0); }
		}

		public override string Description
		{
			get { return "GCC based toolchain for Clang."; }
		}

		public override string GDBExecutable
		{
			get { return Path.Combine(BaseDirectory, "arm-none-eabi-gdb" + Platform.ExecutableExtension); }
		}

		public override string ExecutableExtension
		{
			get { return ".elf"; }
		}

		public override string StaticLibraryExtension
		{
			get { return ".a"; }
		}

		public override void ProvisionSettings(IProject project)
		{
			ProvisionClangSettings(project);
		}

		public static ClangToolchainSettings ProvisionClangSettings(IProject project)
		{
			var result = GetSettings(project);

			if (result == null)
			{
				project.ToolchainSettings.ClangToolchainSettings = new ClangToolchainSettings();
				result = project.ToolchainSettings.ClangToolchainSettings;
				project.Save();
			}

			return result;
		}

		public static ClangToolchainSettings GetSettings(IProject project)
		{
			ClangToolchainSettings result = null;

			try
			{
				if (project.ToolchainSettings.ClangToolchainSettings is ExpandoObject)
				{
					result =
						(project.ToolchainSettings.ClangToolchainSettings as ExpandoObject).GetConcreteType<ClangToolchainSettings>();
				}
				else
				{
					result = project.ToolchainSettings.ClangToolchainSettings;
				}
			}
			catch (Exception e)
			{
			}

			return result;
		}

		public override CompileResult Compile(IConsole console, IStandardProject superProject, IStandardProject project,
			ISourceFile file, string outputFile)
		{
			var result = new CompileResult();

			var startInfo = new ProcessStartInfo();

			if (file.Language == Language.Cpp)
			{
				startInfo.FileName = Path.Combine(BaseDirectory, "arm-none-eabi-g++" + Platform.ExecutableExtension);
			}
			else
			{
				startInfo.FileName = Path.Combine(BaseDirectory, "arm-none-eabi-gcc" + Platform.ExecutableExtension);
			}


			startInfo.WorkingDirectory = file.CurrentDirectory;

			if (!File.Exists(startInfo.FileName))
			{
				result.ExitCode = -1;
				console.WriteLine("Unable to find compiler (" + startInfo.FileName + ") Please check project compiler settings.");
			}
			else
			{
				var fileArguments = string.Empty;

				if (file.Language == Language.Cpp)
				{
					fileArguments = "-x c++ -fno-use-cxa-atexit";
				}

				startInfo.Arguments = string.Format("{0} {1} {2} -o{3} -MMD -MP", fileArguments,
					GetCompilerArguments(superProject, project, file), file.Location, outputFile);

				// Hide console window
				startInfo.UseShellExecute = false;
				startInfo.RedirectStandardOutput = true;
				startInfo.RedirectStandardError = true;
				startInfo.CreateNoWindow = true;

				//console.WriteLine (Path.GetFileNameWithoutExtension(startInfo.FileName) + " " + startInfo.Arguments);

				using (var process = Process.Start(startInfo))
				{
					process.OutputDataReceived += (sender, e) => { console.WriteLine(e.Data); };

					process.ErrorDataReceived += (sender, e) =>
					{
						if (e.Data != null)
						{
							console.WriteLine();
							console.WriteLine(e.Data);
						}
					};

					process.BeginOutputReadLine();

					process.BeginErrorReadLine();

					process.WaitForExit();

					result.ExitCode = process.ExitCode;
				}
			}

			return result;
		}

		private string GetLinkerScriptLocation(IStandardProject project)
		{
			return Path.Combine(project.CurrentDirectory, "link.ld");
		}

		private void GenerateLinkerScript(IStandardProject project)
		{
			var settings = GetSettings(project).LinkSettings;
			var template = new ArmGCCLinkTemplate(settings);

			var linkerScript = GetLinkerScriptLocation(project);

			if (File.Exists(linkerScript))
			{
				File.Delete(linkerScript);
			}

			var sw = File.CreateText(linkerScript);

			sw.Write(template.TransformText());

			sw.Close();
		}

		public override LinkResult Link(IConsole console, IStandardProject superProject, IStandardProject project,
			CompileResult assemblies, string outputPath)
		{
			var settings = GetSettings(superProject);
			var result = new LinkResult();

			var startInfo = new ProcessStartInfo();

			startInfo.FileName = Path.Combine(BaseDirectory, "arm-none-eabi-gcc" + Platform.ExecutableExtension);

			if (project.Type == ProjectType.StaticLibrary)
			{
				startInfo.FileName = Path.Combine(BaseDirectory, "arm-none-eabi-ar" + Platform.ExecutableExtension);
			}

			startInfo.WorkingDirectory = project.Solution.CurrentDirectory;

			if (!File.Exists(startInfo.FileName))
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
			else
			{
				GenerateLinkerScript(superProject);
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


			// TODO linked libraries won't make it in on nano... Please fix -L directory placement in compile string.
			switch (settings.LinkSettings.Library)
			{
				case LibraryType.NanoCLib:
					linkedLibraries += "-lm -lc_nano -lsupc++_nano -lstdc++_nano ";
					break;

				case LibraryType.BaseCLib:
					linkedLibraries += "-lm -lc -lstdc++ -lsupc++ ";
					break;

				case LibraryType.SemiHosting:
					linkedLibraries += "-lm -lgcc -lc -lrdimon ";
					break;

				case LibraryType.Retarget:
					linkedLibraries += "-lm -lc -lnosys -lstdc++ -lsupc++ ";
					break;
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
					GetLinkerArguments(project), executable, objectArguments, linkedLibraries, libs);
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
		}

		public override ProcessResult Size(IConsole console, IStandardProject project, LinkResult linkResult)
		{
			var result = new ProcessResult();

			var startInfo = new ProcessStartInfo();
			startInfo.FileName = Path.Combine(BaseDirectory, "arm-none-eabi-size" + Platform.ExecutableExtension);

			if (!File.Exists(startInfo.FileName))
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

		public override string GetLinkerArguments(IStandardProject project)
		{
			var settings = GetSettings(project);

			var result = string.Empty;

			result += string.Format("{0} ", settings.LinkSettings.MiscLinkerArguments);

			switch (settings.CompileSettings.Fpu)
			{
				case FPUSupport.Soft:
					result += " -mfpu=fpv4-sp-d16 -mfloat-abi=softfp ";
					break;

				case FPUSupport.Hard:
					result += " -mfpu=fpv4-sp-d16 -mfloat-abi=hard ";
					break;
			}

			if (settings.LinkSettings.NotUseStandardStartupFiles)
			{
				result += "-nostartfiles ";
			}

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

			result += string.Format(" -L{0} -Wl,-T\"{1}\"", project.CurrentDirectory, GetLinkerScriptLocation(project));

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

			if (file == null || file.Language == Language.Cpp)
			{
				switch (settings.CompileSettings.CppLanguageStandard)
				{
					case CppLanguageStandard.Cpp98:
						result += "-std=c++98 ";
						break;

					case CppLanguageStandard.Cpp03:
						result += "-std=c++03 ";
						break;

					case CppLanguageStandard.Cpp11:
						result += "-std=c++11 ";
						break;

					case CppLanguageStandard.Cpp14:
						result += "-std=c++14 ";
						break;

					case CppLanguageStandard.Cpp17:
						result += "-std=c++17 ";
						break;

					default:
						break;
				}
			}

			if (file == null || file.Language == Language.C)
			{
				switch (settings.CompileSettings.CLanguageStandard)
				{
					case CLanguageStandard.C89:
						result += "-std=c89 ";
						break;

					case CLanguageStandard.C99:
						result += "-std=c99 ";
						break;

					case CLanguageStandard.C11:
						result += "-std=c11 ";
						break;
				}
			}


			switch (settings.CompileSettings.Fpu)
			{
				case FPUSupport.Soft:
					result += "-mfpu=fpv4-sp-d16 -mfloat-abi=soft ";
					break;

				case FPUSupport.Hard:
					result += "-mfpu=fpv4-sp-d16 -mfloat-abi=hard ";
					break;
			}


			// TODO remove dependency on file?
			if (file != null)
			{
				if (file.Language == Language.Cpp)
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

			switch (settings.CompileSettings.Fpu)
			{
				case FPUSupport.Soft:
				{
					result += "-mfpu=fpv4-sp-d16 -mfloat-abi=soft ";
				}
					break;

				case FPUSupport.Hard:
				{
					result += "-mfpu=fpv4-sp-d16 -mfloat-abi=hard ";
				}
					break;
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
				switch (file.Language)
				{
					case Language.C:
					{
						foreach (var arg in superProject.CCompilerArguments)
						{
							result += string.Format(" {0}", arg);
						}
					}
						break;

					case Language.Cpp:
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
				Path.Combine(Platform.ReposDirectory, "AvalonStudio.Toolchains.Clang", "arm-none-eabi", "include"),
				Path.Combine(Platform.ReposDirectory, "AvalonStudio.Toolchains.Clang", "arm-none-eabi", "include", "c++", "5.4.1"),
				Path.Combine(Platform.ReposDirectory, "AvalonStudio.Toolchains.Clang", "arm-none-eabi", "include", "c++", "5.4.1",
					"arm-none-eabi", "thumb"),
				Path.Combine(Platform.ReposDirectory, "AvalonStudio.Toolchains.Clang", "lib", "gcc", "arm-none-eabi", "5.4.1",
					"include")
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

		public override UserControl GetSettingsControl(IProject project)
		{
			throw new Exception();
		}
	}
}