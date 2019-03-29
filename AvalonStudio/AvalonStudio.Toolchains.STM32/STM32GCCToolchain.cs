using AvalonStudio.Extensibility.Shell;
using AvalonStudio.Packages;
using AvalonStudio.Packaging;
using AvalonStudio.Platforms;
using AvalonStudio.Projects;
using AvalonStudio.Projects.CPlusPlus;
using AvalonStudio.Projects.Standard;
using AvalonStudio.Toolchains.GCC;
using AvalonStudio.Utils;
using System;
using System.Collections.Generic;
using System.Composition;
using System.IO;
using System.Threading.Tasks;

namespace AvalonStudio.Toolchains.STM32
{
    [ExportToolchain]
    [Shared]
    public class STM32GCCToolchain : GCCToolchain
    {
        private static string _contentDirectory;

        public static string ContentDirectory
        {
            get
            {
                if (_contentDirectory == null)
                {
                    _contentDirectory = PackageManager.GetPackageDirectory("Clang").ToPlatformPath();
                }

                return _contentDirectory;
            }
        }
        public override string BinDirectory => Path.Combine(ContentDirectory, "bin");

        public string LinkerScript { get; set; }

        public override Version Version => new Version(1, 0, 0);

        public override string Description => "GCC based toolchain for STM32.";

        public override string GDBExecutable => Path.Combine(BinDirectory, "arm-none-eabi-gdb" + Platform.ExecutableExtension);

        public override string LibraryQueryCommand => "arm-none-eabi-gcc";

        public override string ExecutableExtension => ".elf";

        public override string StaticLibraryExtension => ".a";

        public override string Prefix => "arm-none-eabi-";

        [ImportingConstructor]
        public STM32GCCToolchain(IStatusBar statusBar)
            : base (statusBar)
        {
        }

        private string GetLinkerScriptLocation(IStandardProject project)
        {
            return Path.Combine(project.CurrentDirectory, "link.ld");
        }

        private void GenerateLinkerScript(IStandardProject project)
        {
            var settings = project.GetToolchainSettings<GccToolchainSettings>().LinkSettings;

            throw new NotImplementedException();
            //var template = new ArmGCCLinkTemplate(settings);

            var linkerScript = GetLinkerScriptLocation(project);

            if (System.IO.File.Exists(linkerScript))
            {
                System.IO.File.Delete(linkerScript);
            }

            using (var sw = System.IO.File.CreateText(linkerScript))
            {
                // sw.Write(template.TransformText());
            }
        }

        public override string GetBaseLibraryArguments(IStandardProject superProject)
        {
            var settings = superProject.GetToolchainSettings<GccToolchainSettings>();
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
                    result += "-lm -lc -lnosys -lgcc -lstdc++ -lsupc++ ";
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

            var settings = project.GetToolchainSettings<GccToolchainSettings>();

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

            if (settings.LinkSettings.UseMemoryLayout)
            {
                result += string.Format(" -L{0} -Wl,-T\"{1}\"", project.CurrentDirectory, GetLinkerScriptLocation(project));
            }

            return result;
        }

        public override string GetCompilerArguments(IStandardProject superProject, IStandardProject project, ISourceFile file)
        {
            var result = string.Empty;

            var settings = superProject.GetToolchainSettings<GccToolchainSettings>();

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

                    case CppLanguageStandard.Gnu11:
                        result += "-std=gnu++11 ";
                        break;

                    case CppLanguageStandard.Gnu14:
                        result += "-std=gnu++14 ";
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

                case OptimizationLevel.Size:
                    {
                        result += "-Os ";
                    }
                    break;

                case OptimizationLevel.Speed:
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

        public override bool CanHandle(IProject project)
        {
            var result = false;

            if (project is CPlusPlusProject)
            {
                result = true;
            }

            return result;
        }

        public enum AssemblyFormat
        {
            Binary,
            IntelHex,
            Elf32
        }

        public async override Task<bool> InstallAsync(IConsole console, IProject project)
        {
            bool result = true;

            if (await PackageManager.EnsurePackage("AvalonStudio.Toolchains.Clang", (project as CPlusPlusProject).ToolchainVersion, console) == PackageEnsureStatus.Installed)
            {
                // this ensures content directory is re-evaluated if we just installed the toolchain.
                _contentDirectory = null;
            }

            if (result)
            {
                result = await base.InstallAsync(console, project);
            }

            return result;
        }
    }
}