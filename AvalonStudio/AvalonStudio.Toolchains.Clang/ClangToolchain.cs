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
using System.Linq;
using System.Threading.Tasks;

namespace AvalonStudio.Toolchains.Clang
{
    public enum AssemblyFormat
    {
        Binary,
        IntelHex,
        Elf32
    }

    [ExportToolchain]
    [Shared]
    public class ClangToolchain : GCCToolchain
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
        public override string Prefix => string.Empty;
        public override string CCName => "clang";
        public override string CCPPName => "clang++";
        public override string LDName => "gcc";
        public override string ARName => "ar";
        public override string LDPrefix => "arm-none-eabi-";
        public override string SizePrefix => LDPrefix;
        public override string ARPrefix => LDPrefix;

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
            get { return Path.Combine(BinDirectory, "arm-none-eabi-gdb" + Platform.ExecutableExtension); }
        }

        public override string LibraryQueryCommand => "arm-none-eabi-gcc";

        public override string ExecutableExtension
        {
            get { return ".elf"; }
        }

        public override string StaticLibraryExtension
        {
            get { return ".a"; }
        }

        [ImportingConstructor]
        public ClangToolchain(IStatusBar statusBar)
            : base(statusBar)
        {
        }

        private string GetLinkerScriptLocation(IStandardProject project)
        {
            return Path.Combine(project.CurrentDirectory, "link.ld");
        }

        /*public override IEnumerable<string> GetToolchainIncludes(ISourceFile file)
        {
            return new List<string>
            {
                Path.Combine(ContentDirectory, "arm-none-eabi", "include", "c++", "6.3.1"),
                Path.Combine(ContentDirectory, "arm-none-eabi", "include", "c++", "6.3.1", "arm-none-eabi"),
                Path.Combine(ContentDirectory, "arm-none-eabi", "include", "c++", "6.3.1", "backward"),
                Path.Combine(ContentDirectory, "arm-none-eabi", "include"),
                Path.Combine(ContentDirectory, "lib", "gcc", "arm-none-eabi", "6.3.1", "include"),
                Path.Combine(ContentDirectory, "lib", "gcc", "arm-none-eabi", "6.3.1", "include-fixed"),
                Path.Combine(ContentDirectory, "arm-none-eabi", "include")
            };
        }*/

        private void GenerateLinkerScript(IStandardProject project)
        {
            var settings = project.GetToolchainSettings<GccToolchainSettings>().LinkSettings;

            var linkerScript = GetLinkerScriptLocation(project);

            if (System.IO.File.Exists(linkerScript))
            {
                System.IO.File.Delete(linkerScript);
            }

            throw new NotImplementedException();
            //var rendered = Template.Engine.CompileRenderAsync("ArmLinkerScriptTemplate.template", new { InRom1Start = settings.InRom1Start, InRom1Size = settings.InRom1Size, InRam1Start = settings.InRam1Start, InRam1Size = settings.InRam1Size }).GetAwaiter().GetResult();

            //using (var sw = System.IO.File.CreateText(linkerScript))
            //{
            //    sw.Write(rendered);
            //}
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
                    result += "-lm -lc -lg -lnosys -lstdc++ -lsupc++ ";
                    break;
            }

            return result;
        }

        public override string GetLinkerArguments(IStandardProject superProject, IStandardProject project)
        {
            var settings = project.GetToolchainSettings<GccToolchainSettings>();

            if (superProject != null && settings.LinkSettings.UseMemoryLayout && project.Type != ProjectType.StaticLibrary)
            {
                GenerateLinkerScript(superProject);
            }

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
                result += "-ggdb3 ";
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

            switch (settings.CompileSettings.Fpu)
            {
                case FPUSupport.Soft:
                    result += "-mfpu=fpv4-sp-d16 -mfloat-abi=softfp ";
                    break;

                case FPUSupport.Hard:
                    result += "-mfpu=fpv4-sp-d16 -mfloat-abi=hard ";
                    break;
            }

            switch (settings.CompileSettings.Fpu)
            {
                case FPUSupport.Soft:
                    {
                        result += "-mfpu=fpv4-sp-d16 -mfloat-abi=softfp ";
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
                        result += "-O0 ";
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

            // Referenced includes
            var referencedIncludes = project.GetReferencedIncludes();

            referencedIncludes.Select(s => result += $"-I\"{ Path.Combine(project.CurrentDirectory, s)}\" ").ToList();

            // global includes
            var globalIncludes = superProject.GetGlobalIncludes();

            globalIncludes.Select(s => result += $"-I\"{s}\" ").ToList();

            // includes
            project.Includes.Select(s => result += $"-I\"{ Path.Combine(project.CurrentDirectory, s.Value)}\" ").ToList();

            var referencedDefines = project.GetReferencedDefines();
            referencedDefines.Select(s => result += $"-D{s} ").ToList();

            var toolchainIncludes = GetToolchainIncludes(file);
            toolchainIncludes.Select(s => result += $"-isystem\"{s}\" ").ToList();

            // global includes
            var globalDefines = superProject.GetGlobalDefines();

            globalDefines.Select(s => result += $"-D{s} ").ToList();

            project.Defines.Select(s => result += $"-D{s.Value} ").ToList();

            superProject.ToolChainArguments.Select(s => result += $" {s}").ToList();

            superProject.CompilerArguments.Select(s => result += $" {s}").ToList();

            // TODO factor out this code from here!
            if (file != null)
            {
                switch (file.Extension)
                {
                    case ".cc":
                    case ".c":
                        {
                            superProject.CCompilerArguments.Select(s => result += $" {s}");
                        }
                        break;

                    case ".cpp":
                        {
                            superProject.CppCompilerArguments.Select(s => result += $" {s}");
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

        public async override Task<bool> InstallAsync(IConsole console, IProject project)
        {
            if (await PackageManager.EnsurePackage("AvalonStudio.Toolchains.Clang", (project as CPlusPlusProject).ToolchainVersion, console) == PackageEnsureStatus.Installed)
            {
                // this ensures content directory is re-evaluated if we just installed the toolchain.
                _contentDirectory = null;
            }

            await base.InstallAsync(console, project);

            return true;
        }
    }
}