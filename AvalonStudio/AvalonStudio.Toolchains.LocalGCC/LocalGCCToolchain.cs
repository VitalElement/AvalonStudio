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

namespace AvalonStudio.Toolchains.LocalGCC
{
    [ExportToolchain]
    [Shared]
    public class LocalGCCToolchain : GCCToolchain
    {
        private static string _contentDirectory;

        public static string ContentDirectory
        {
            get
            {
                if (Platform.PlatformIdentifier != Platforms.PlatformID.Win32NT)
                {
                    return string.Empty;
                }
                else
                {
                    if (_contentDirectory == null)
                    {
                        _contentDirectory = PackageManager.GetPackageDirectory("Gcc").ToPlatformPath();
                    }

                    return _contentDirectory;
                }
            }
        }

        protected override bool RunWithSystemPaths => true;

        public override string BinDirectory
        {
            get
            {
                if (Platform.PlatformIdentifier != Platforms.PlatformID.Win32NT)
                {
                    return string.Empty;
                }
                else
                {
                    return Path.Combine(ContentDirectory, "bin");
                }
            }
        }

        public override string Prefix => string.Empty;

        public override string GetBaseLibraryArguments(IStandardProject superProject)
        {
            return string.Empty;
        }

        public override string LDName => "g++";

        public string LinkerScript { get; set; }

        public override string GDBExecutable => Path.Combine(BinDirectory, "gdb" + Platform.ExecutableExtension);

        public override Version Version
        {
            get { return new Version(1, 0, 0); }
        }

        public override string Description
        {
            get { return "GCC based toolchain PC."; }
        }

        public override string ExecutableExtension => Platform.ExecutableExtension;

        public override string StaticLibraryExtension => ".a";

        [ImportingConstructor]
        public LocalGCCToolchain(IStatusBar statusBar)
            : base(statusBar)
        {
        }

        private string GetLinkerScriptLocation(IStandardProject project)
        {
            return Path.Combine(project.CurrentDirectory, "link.ld");
        }

        public override string GetLinkerArguments(IStandardProject superProject, IStandardProject project)
        {
            var settings = project.GetToolchainSettings<GccToolchainSettings>();

            var result = string.Empty;

            result += string.Format("-static-libgcc -static-libstdc++ -Wl,-Map={0}.map ",
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

            var settings = superProject.GetToolchainSettings<GccToolchainSettings>();

            result += "-Wall -c ";

            if (settings.CompileSettings.DebugInformation)
            {
                result += "-g ";
            }

            // TODO make this an option.
            result += "-ffunction-sections -fdata-sections ";

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

                if (!settings.CompileSettings.Rtti)
                {
                    result += "-fno-rtti ";
                }

                if (!settings.CompileSettings.Exceptions)
                {
                    result += "-fno-exceptions ";
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

            if (Platform.OSDescription == "Windows")
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

        public override bool SupportsFile(ISourceFile file)
        {
            var result = false;

            if (Path.GetExtension(file.Location) == ".cpp" || Path.GetExtension(file.Location) == ".c")
            {
                result = true;
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

        public async override Task<bool> InstallAsync(IConsole console, IProject project)
        {
            if (Platform.PlatformIdentifier == Platforms.PlatformID.Win32NT)
            {
                var status = await PackageManager.EnsurePackage("AvalonStudio.Toolchains.GCC", (project as CPlusPlusProject).ToolchainVersion, console);
                
                switch(status)
                {
                    case PackageEnsureStatus.Installed:
                        // this ensures content directory is re-evaluated if we just installed the toolchain.
                        _contentDirectory = null;
                        break;

                    case PackageEnsureStatus.NotFound:
                    case PackageEnsureStatus.Unknown:
                        return false;
                }
            }

            return await base.InstallAsync(console, project);
        }
    }
}