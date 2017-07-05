using AvalonStudio.Packages;
using AvalonStudio.Platforms;
using AvalonStudio.Projects;
using AvalonStudio.Projects.Standard;
using AvalonStudio.Toolchains.GCC;
using AvalonStudio.Toolchains.Standard;
using AvalonStudio.Utils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using AvalonStudio.Projects.CPlusPlus;

namespace AvalonStudio.Toolchains.LocalGCC
{
    public class LocalGCCToolchain : GCCToolchain
    {
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
                    return Path.Combine(PackageManager.GetPackageDirectory("AvalonStudio.Toolchains.GCC"), "content");
                }
            }
        }

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

        public override IEnumerable<string> GetToolchainIncludes(ISourceFile file)
        {
            return new List<string>
            {
                Path.Combine(ContentDirectory, "lib", "gcc", "x86_64-w64-mingw32", "5.2.0", "include"),
                Path.Combine(ContentDirectory, "lib", "gcc", "x86_64-w64-mingw32", "5.2.0", "include-fixed"),
                Path.Combine(ContentDirectory, "x86_64-w64-mingw32", "include"),
                Path.Combine(ContentDirectory, "x86_64-w64-mingw32", "include", "c++"),
                Path.Combine(ContentDirectory, "x86_64-w64-mingw32", "include", "c++", "x86_64-w64-mingw32"),
                Path.Combine(ContentDirectory, "x86_64-w64-mingw32", "include", "c++", "x86_64-w64-mingw32", "backward")
            };
        }

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

        private string GetLinkerScriptLocation(IStandardProject project)
        {
            return Path.Combine(project.CurrentDirectory, "link.ld");
        }

        public override string GetLinkerArguments(IStandardProject superProject, IStandardProject project)
        {
            var settings = project.GetToolchainSettings<GccToolchainSettings>();

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

            var settings = superProject.GetToolchainSettings<GccToolchainSettings>();

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

                    result += "-std=c++14 ";
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

            if (project is CPlusPlusProject)
            {
                result = true;
            }

            return result;
        }

        public async override Task InstallAsync(IConsole console, IProject project)
        {
            if(Platform.PlatformIdentifier == Platforms.PlatformID.Win32NT)
            {
                await PackageManager.EnsurePackage("AvalonStudio.Toolchains.GCC", (project as CPlusPlusProject).ToolchainVersion,  console);
            }
        }
    }
}