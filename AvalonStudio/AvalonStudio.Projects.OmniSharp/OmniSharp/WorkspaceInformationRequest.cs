namespace AvalonStudio.Languages.CSharp.OmniSharp
{
    using System.Collections.Generic;

    public class DotNet
    {
        public List<object> Projects { get; set; }
        public object RuntimePath { get; set; }
    }

    public class Project
    {
        public string ProjectGuid { get; set; }
        public string Path { get; set; }
        public string AssemblyName { get; set; }
        public string TargetPath { get; set; }
        public string TargetFramework { get; set; }
        public List<string> SourceFiles { get; set; }
    }

    public class MsBuild
    {
        public string SolutionPath { get; set; }
        public List<Project> Projects { get; set; }
    }

    public class CsxFileProjects
    {
    }

    public class CsxReferences
    {
    }

    public class CsxLoadReferences
    {
    }

    public class CsxUsings
    {
    }

    public class ScriptCs
    {
        public List<object> CsxFilesBeingProcessed { get; set; }
        public CsxFileProjects CsxFileProjects { get; set; }
        public CsxReferences CsxReferences { get; set; }
        public CsxLoadReferences CsxLoadReferences { get; set; }
        public CsxUsings CsxUsings { get; set; }
        public List<object> ScriptPacks { get; set; }
        public List<object> CommonReferences { get; set; }
        public List<object> CommonUsings { get; set; }
        public object RootPath { get; set; }
    }

    public class Workspace
    {
        public DotNet DotNet { get; set; }
        public MsBuild MsBuild { get; set; }
        public ScriptCs ScriptCs { get; set; }
    }

    public class WorkspaceInformationRequest : OmniSharpRequest<Workspace>
    {        
        public bool ExcludeSourceFiles { get; set; }

        public override string EndPoint
        {
            get
            {
                return "projects";
            }
        }
    }
}
