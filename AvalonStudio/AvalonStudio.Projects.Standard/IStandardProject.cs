namespace AvalonStudio.Projects.Standard
{
    using Newtonsoft.Json;
    using Newtonsoft.Json.Converters;
    using System.Collections.Generic;
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
        string Executable { get; }

        IList<string> BuiltinLibraries { get; }

        IList<string> ToolChainArguments { get; }

        IList<string> LinkerArguments { get; }

        IList<string> CompilerArguments { get; }

        IList<string> CCompilerArguments { get; }

        IList<string> CppCompilerArguments { get; }

        IList<string> Defines { get; }

        IList<string> PublicIncludes { get; }

        IList<string> GlobalIncludes { get; }

        IList<string> Includes { get; }
        IList<string> GetReferencedIncludes();
        IList<string> GetGlobalIncludes();
    }
}
