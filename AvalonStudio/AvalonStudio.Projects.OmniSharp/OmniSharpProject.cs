﻿using AvalonStudio.Debugging;
using AvalonStudio.Extensibility;
using AvalonStudio.Platforms;
using AvalonStudio.Shell;
using AvalonStudio.TestFrameworks;
using AvalonStudio.Toolchains;
using Microsoft.CodeAnalysis;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Dynamic;
using System.IO;
using System.Linq;

namespace AvalonStudio.Projects.OmniSharp
{
    public class OmniSharpProject : FileSystemProject
    {
        public static OmniSharpProject Create(Project roslynProject, ISolution solution, string path, List<string> unresolvedReferences)
        {
            OmniSharpProject result = new OmniSharpProject
            {
                Solution = solution,
                Location = path,
                RoslynProject = roslynProject,
                UnresolvedReferences = unresolvedReferences
            };

            return result;
        }

        public OmniSharpProject() : base(true)
        {
            ExcludedFiles = new List<string>();
            Items = new ObservableCollection<IProjectItem>();
            References = new ObservableCollection<IProject>();
            ToolchainSettings = new ExpandoObject();
            DebugSettings = new ExpandoObject();
            Settings = new ExpandoObject();
            Project = this;            
        }

        public List<string> UnresolvedReferences { get; set; }

        public Project RoslynProject { get; set; }

        public override IList<object> ConfigurationPages
        {
            get
            {
                return new List<object>() { new ToolchainSettingsFormViewModel() };
            }
        }

        public override string CurrentDirectory
        {
            get { return Path.GetDirectoryName(Location) + Platforms.Platform.DirectorySeperator; }
        }

        public override IDebugger Debugger2
        {
            get
            {
                var shell = IoC.Get<IShell>();

                var debugger = shell.Debugger2s.FirstOrDefault(tc => tc.GetType().ToString() == "AvalonStudio.Debugging.DotNetCore.DotNetCoreDebugger");

                return debugger;
            }
            set
            {
            }
        }

        public override dynamic Settings { get; set; }

        public override dynamic DebugSettings { get; set; }

        public override string Executable { get; set; }

        public override string Extension
        {
            get
            {
                return ".csproj";
            }
        }

        public override bool Hidden
        {
            get; set;
        }

        public override ObservableCollection<IProjectItem> Items { get; }

        public override string Location
        {
            get; set;
        }

        public override string LocationDirectory => CurrentDirectory;

        public override string Name
        {
            get { return Path.GetFileNameWithoutExtension(Location); }
        }

        public override IProjectFolder Parent { get; set; }

        public override IProject Project { get; set; }

        public override ObservableCollection<IProject> References { get; }

        public override ISolution Solution
        {
            get; set;
        }

        public override ITestFramework TestFramework
        {
            get; set;
        }

        public override IToolChain ToolChain
        {
            get
            {
                var shell = IoC.Get<IShell>();

                var toolchain = shell.ToolChains.FirstOrDefault(tc => tc.GetType().ToString() == "AvalonStudio.Toolchains.MSBuild.MSBuildToolchain");

                return toolchain;
            }
            set
            {
            }
        }

        public override dynamic ToolchainSettings { get; set; }

        public override void AddReference(IProject project)
        {
            throw new NotImplementedException();
        }

        public override int CompareTo(IProjectFolder other)
        {
            return Location.CompareFilePath(other.Location);
        }

        public override int CompareTo(string other)
        {
            return Location.CompareFilePath(other);
        }

        public override int CompareTo(IProject other)
        {
            return Name.CompareTo(other.Name);
        }

        public override int CompareTo(IProjectItem other)
        {
            return Name.CompareTo(other.Name);
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

        public override List<string> ExcludedFiles { get; set; }
    }
}