using AvalonStudio.Debugging;
using AvalonStudio.Platforms;
using AvalonStudio.TestFrameworks;
using AvalonStudio.Toolchains;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;

namespace AvalonStudio.Projects.TypeScript
{
    public class TypeScriptProject : FileSystemProject
    {
        public TypeScriptProject() : base(true)
        {
        }

        public override IList<object> ConfigurationPages
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public override string CurrentDirectory => Path.GetDirectoryName(Location) + Platform.DirectorySeperator;

        public override IDebugger Debugger { get; set; }

        public override dynamic DebugSettings { get; set; }

        public override List<string> ExcludedFiles { get; set; }

        public override string Executable { get; set; }

        public override string Extension => "tsproj";

        public override bool Hidden { get; set; }

        public override ObservableCollection<IProjectItem> Items { get; }

        public override string Location { get; set; }

        public override string LocationDirectory => CurrentDirectory;

        public override string Name => Path.GetFileNameWithoutExtension(Location);

        public override IProjectFolder Parent { get; set; }

        public override IProject Project { get; set; }

        public override ObservableCollection<IProject> References { get; }

        public override ISolution Solution { get; set; }

        public override ITestFramework TestFramework { get; set; }

        //TODO: Set up TS toolchain
        public override IToolChain ToolChain { get; set; }

        public override dynamic ToolchainSettings { get; set; }

        public override void AddReference(IProject project)
        {
            throw new NotImplementedException();
        }

        public override int CompareTo(IProjectItem other)
        {
            return string.Compare(Name, other.Name, StringComparison.CurrentCulture);
        }

        public override int CompareTo(IProject other)
        {
            return string.Compare(Name, other.Name, StringComparison.CurrentCulture);
        }

        public override int CompareTo(string other)
        {
            return Location.CompareFilePath(other);
        }

        public override int CompareTo(IProjectFolder other)
        {
            return Location.CompareFilePath(other.Location);
        }

        public override void ExcludeFile(ISourceFile file)
        {
            throw new NotImplementedException();
        }

        public override void ExcludeFolder(IProjectFolder folder)
        {
            throw new NotImplementedException();
        }

        public override IProject Load(ISolution solution, string filePath)
        {
            var result = LoadProject(solution, filePath);

            return result;
        }

        private IProject LoadProject(ISolution solution, string filePath)
        {
            //TODO: Actually load project
            return null;
        }

        public override void RemoveReference(IProject project)
        {
            throw new NotImplementedException();
        }

        public override void ResolveReferences()
        {
            throw new NotImplementedException();
        }

        public override void Save()
        {
            throw new NotImplementedException();
        }
    }
}