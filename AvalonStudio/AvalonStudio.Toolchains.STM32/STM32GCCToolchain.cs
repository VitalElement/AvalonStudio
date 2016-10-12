namespace AvalonStudio.Toolchains.STM32
{
    using AvalonStudio.Platforms;
    using AvalonStudio.Projects;
    using AvalonStudio.Projects.Standard;
    using AvalonStudio.Toolchains.GCC;
    using AvalonStudio.Utils;
    using System;
    using System.Collections.Generic;
    using System.Dynamic;
    using System.IO;

    public class STM32GCCToolchain : GCCToolchain
	{
        public override string BinDirectory => Path.Combine(Platform.ReposDirectory, "AvalonStudio.Toolchains.STM32", "bin");
        
        public string LinkerScript { get; set; }

        public override Version Version => new Version(1, 0, 0);

        public override string Description => "GCC based toolchain for STM32.";

		public override string GDBExecutable => Path.Combine(BinDirectory, "arm-none-eabi-gdb" + Platform.ExecutableExtension);

        public override string ExecutableExtension => ".elf";

        public override string StaticLibraryExtension => ".a";

        public override string Prefix => "arm-none-eabi-";

        public override void ProvisionSettings(IProject project)
		{
			ProvisionSTM32Settings(project);
		}

		public static STM32ToolchainSettings ProvisionSTM32Settings(IProject project)
		{
			var result = GetSettings(project);

			if (result == null)
			{
				project.ToolchainSettings.STM32ToolchainSettings = new STM32ToolchainSettings();
				result = project.ToolchainSettings.STM32ToolchainSettings;
				project.Save();
			}

			return result;
		}

		public static STM32ToolchainSettings GetSettings(IProject project)
		{
			STM32ToolchainSettings result = null;

			try
			{
				if (project.ToolchainSettings.STM32ToolchainSettings is ExpandoObject)
				{
					result =
						(project.ToolchainSettings.STM32ToolchainSettings as ExpandoObject).GetConcreteType<STM32ToolchainSettings>();
				}
				else
				{
					result = project.ToolchainSettings.STM32ToolchainSettings;
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

		private void GenerateLinkerScript(IStandardProject project)
		{
			var settings = GetSettings(project).LinkSettings;
			var template = new ArmGCCLinkTemplate(settings);

			var linkerScript = GetLinkerScriptLocation(project);

			if (System.IO.File.Exists(linkerScript))
			{
                System.IO.File.Delete(linkerScript);
			}

			var sw = System.IO.File.CreateText(linkerScript);

			sw.Write(template.TransformText());

			sw.Close();
		}

        public override string GetBaseLibraryArguments(IStandardProject superProject)
        {
            var settings = GetSettings(superProject);
            string result = string.Empty;

            // TODO linked libraries won't make it in on nano... Please fix -L directory placement in compile string.
            switch (settings.LinkSettings.Library)
            {
                case LibraryType.NanoCLib:
                    result += "-lm -lc_nano -lsupc++_nano -lstdc++_nano ";
                    break;

                case LibraryType.BaseCLib:
                    result += "-lm -lc -lstdc++ -lsupc++ ";
                    break;

                case LibraryType.SemiHosting:
                    result += "-lm -lgcc -lc -lrdimon ";
                    break;

                case LibraryType.Retarget:
                    result += "-lm -lc -lnosys -lstdc++ -lsupc++ ";
                    break;
            }

            return result;
        }

        public override string GetLinkerArguments(IStandardProject superProject, IStandardProject project)
        {
            if (superProject != null && project.Type != ProjectType.StaticLibrary)
            {
                GenerateLinkerScript(superProject);
            }

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

			result += "-Wall -c -fshort-enums ";

			if (settings.CompileSettings.DebugInformation)
			{
				result += "-g ";
			}

			if (file == null || file.Extension == ".cpp")
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

			if (file == null || file.Extension == ".c")
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
				Path.Combine(Platform.ReposDirectory, "AvalonStudio.Toolchains.STM32", "arm-none-eabi", "include"),
				Path.Combine(Platform.ReposDirectory, "AvalonStudio.Toolchains.STM32", "arm-none-eabi", "include", "c++", "5.4.1"),
				Path.Combine(Platform.ReposDirectory, "AvalonStudio.Toolchains.STM32", "arm-none-eabi", "include", "c++", "5.4.1",
					"arm-none-eabi", "thumb"),
				Path.Combine(Platform.ReposDirectory, "AvalonStudio.Toolchains.STM32", "lib", "gcc", "arm-none-eabi", "5.4.1",
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
	}
}