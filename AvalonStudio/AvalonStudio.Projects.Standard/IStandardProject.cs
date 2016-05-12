namespace AvalonStudio.Projects.Standard
{
    using Newtonsoft.Json;
    using Newtonsoft.Json.Converters;
    using System.Collections.Generic;
    using Toolchains;

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

        string GetObjectDirectory(IStandardProject superProject);

        string GetBuildDirectory(IStandardProject superProject);

        string GetOutputDirectory(IStandardProject superProject);

        string BuildDirectory { get; }
        string LinkerScript { get;  }

        IList<string> BuiltinLibraries { get; }

        IList<string> StaticLibraries { get; }

        IList<string> ToolChainArguments { get; }

        IList<string> LinkerArguments { get; }

        IList<string> CompilerArguments { get; }

        IList<string> CCompilerArguments { get; }

        IList<string> CppCompilerArguments { get; }

        IList<Definition> Defines { get; }

        IList<string> PublicIncludes { get; }

        IList<string> GlobalIncludes { get; }

        IList<Include> Includes { get; }
        IList<string> GetReferencedIncludes();
        IList<string> GetGlobalIncludes();

        IList<string> GetReferencedDefines();
        IList<string> GetGlobalDefines();        
        
        IList<ISourceFile> SourceFiles { get; }
    }
}
