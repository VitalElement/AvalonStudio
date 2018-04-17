namespace AvalonStudio.Projects.OmniSharp.ProjectTypes
{
    [ExportProjectType("csproj", Description, ProjectTypeGuid)]
    internal class CSharpProjectType : DotNetCoreCSharpProjectType
    {
        private const string ProjectTypeGuid = "fae04ec0-301f-11d3-bf4b-00c04f79efbc";
        private const string Description = "C# Project";
    }
}
