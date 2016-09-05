﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AvalonStudio.Debugging;
using AvalonStudio.TestFrameworks;
using AvalonStudio.Toolchains;
using AvalonStudio.Projects.Raw;
using AvalonStudio.Platforms;
using AvalonStudio.Utils;
using System.IO;
using System.Dynamic;

namespace AvalonStudio.Projects.OmniSharp
{
    public class OmniSharpProject : FileSystemProject
    {
        public static OmniSharpProject Create(ISolution solution, string path, AvalonStudio.Languages.CSharp.OmniSharp.Project project)
        {
            OmniSharpProject result = new OmniSharpProject();
            result.Solution = solution;
            result.Location = path;

            result.LoadFiles();

            //foreach(var file in project.SourceFiles)
            //{
            //    var sourceFile = File.FromPath(result, result, file.ToPlatformPath());
            //    result.SourceFiles.InsertSorted(sourceFile);
            //    result.Items.Add(sourceFile);
            //}

            return result;
        }

        public OmniSharpProject() : base(true)
        {
            ExcludedFiles = new List<string>();
            Items = new ObservableCollection<IProjectItem>();            
            References = new ObservableCollection<IProject>();            
            ToolchainSettings = new ExpandoObject();
            DebugSettings = new ExpandoObject();
            Project = this;
        }        

        public override IList<object> ConfigurationPages
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public override string CurrentDirectory
        {
            get { return Path.GetDirectoryName(Location) + Platform.DirectorySeperator; }
        }

        public override IDebugger Debugger
        {
            get; set;
        }

        public override dynamic DebugSettings { get; set; }

        public override string Executable { get; set; }

        public override string Extension
        {
            get
            {
                throw new NotImplementedException();
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
            get; set;
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
