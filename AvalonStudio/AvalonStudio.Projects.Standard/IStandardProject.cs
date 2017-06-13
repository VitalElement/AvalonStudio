using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;

namespace AvalonStudio.Projects.Standard
{
    [JsonConverter(typeof(StringEnumConverter))]
    public enum ProjectType
    {
        Executable,
        SharedLibrary,
        StaticLibrary,
        SuperProject
    }

    public interface IStandardProject : IProject
    {
        ProjectType Type { get; }

        bool IsBuilding { get; set; }

        string BuildDirectory { get; }
        string LinkerScript { get; }

        IList<string> BuiltinLibraries { get; }

        IList<string> StaticLibraries { get; }

        IList<string> ToolChainArguments { get; }

        IList<string> LinkerArguments { get; }

        IList<string> CompilerArguments { get; }

        IList<string> CCompilerArguments { get; }

        IList<string> CppCompilerArguments { get; }

        IList<Definition> Defines { get; }

        IList<Include> Includes { get; }

        IList<ISourceFile> SourceFiles { get; }

        string GetObjectDirectory(IStandardProject superProject);

        string GetBuildDirectory(IStandardProject superProject);

        string GetOutputDirectory(IStandardProject superProject);

        IList<string> GetReferencedIncludes();

        IList<string> GetGlobalIncludes();

        IList<string> GetReferencedDefines();

        IList<string> GetGlobalDefines();

        void VisitSourceFiles(Action<IStandardProject, IStandardProject, ISourceFile> visitor);

        IList<string> PreBuildCommands { get; }

        IList<string> PostBuildCommands { get; }
    }
}