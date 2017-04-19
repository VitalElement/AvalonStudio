namespace AvalonStudio.Languages.CSharp.OmniSharp
{
    using System.Collections.Generic;
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
}