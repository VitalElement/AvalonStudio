using AvalonStudio.Projects;
using AvalonStudio.Toolchains.Standard;
using AvalonStudio.Utils;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AvalonStudio.Toolchains.TypeScript
{
    public class TypeScriptToolchain : StandardToolChain
    {
        public override string Description => "TypeScript Toolchain";

        public override string ExecutableExtension => "js";

        public override string StaticLibraryExtension => "js";

        public override Version Version => new Version(0, 1, 1, 0);

        public override bool CanHandle(IProject project)
        {
            throw new NotImplementedException();
        }

        public override CompileResult Compile(IConsole console, Projects.Standard.IStandardProject superProject, Projects.Standard.IStandardProject project, ISourceFile file, string outputFile)
        {
            throw new NotImplementedException();
        }

        public override string GetCompilerArguments(Projects.Standard.IStandardProject superProject, Projects.Standard.IStandardProject project, ISourceFile sourceFile)
        {
            throw new NotImplementedException();
        }

        public override IList<object> GetConfigurationPages(IProject project)
        {
            throw new NotImplementedException();
        }

        public override string GetLinkerArguments(Projects.Standard.IStandardProject superProject, Projects.Standard.IStandardProject project)
        {
            throw new NotImplementedException();
        }

        public override List<string> GetToolchainIncludes()
        {
            throw new NotImplementedException();
        }

        public override LinkResult Link(IConsole console, Projects.Standard.IStandardProject superProject, Projects.Standard.IStandardProject project, CompileResult assemblies, string outputPath)
        {
            var result = new LinkResult()
            {
                ExitCode = 0,
            };
            //TODO: Executable for JS?
            //I'm assuming this would be an entrypoint module or something
            return result;
        }

        public override async Task<bool> PostBuild(IConsole console, IProject project, LinkResult linkResult)
        {
            return true;
        }

        public override async Task<bool> PreBuild(IConsole console, IProject project)
        {
            return true;
        }

        public override void ProvisionSettings(IProject project)
        {
            throw new NotImplementedException();
        }

        public override ProcessResult Size(IConsole console, Projects.Standard.IStandardProject project, LinkResult linkResult)
        {
            throw new NotImplementedException();
        }

        public override bool SupportsFile(ISourceFile file)
        {
            throw new NotImplementedException();
        }
    }
}