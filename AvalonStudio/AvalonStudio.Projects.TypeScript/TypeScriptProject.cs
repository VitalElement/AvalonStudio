﻿using AvalonStudio.Debugging;
using AvalonStudio.Extensibility;
using AvalonStudio.Platforms;
using AvalonStudio.Shell;
using AvalonStudio.TestFrameworks;
using AvalonStudio.Toolchains;
using AvalonStudio.Utils;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Dynamic;
using System.IO;
using System.Linq;
using TSBridge;

namespace AvalonStudio.Projects.TypeScript
{
    public class TypeScriptProject : FileSystemProject, IProject
    {
        public static TypeScriptProject Create(ISolution solution, string directory)
        {
            TypeScriptProject result = new TypeScriptProject();

            var projectName = new DirectoryInfo(directory).Name;

            //Create new project with default name and extension
            var projectFileLocation = Path.Combine(directory, projectName + $".{result.Extension}");

            result.Solution = solution;
            result.Location = projectFileLocation;

            //Create Main.TS file
            var indexFileLocation = Path.Combine(directory, "main.ts");
            if (!System.IO.File.Exists(indexFileLocation))
            {
                System.IO.File.WriteAllText(indexFileLocation, @"
class Program {
    static main() {
        console.log(""Hello, World!"");
    }
}

Program.main();
");
            }
            //Create TypeScript project file
            var tsProjectFileLocation = Path.Combine(directory, "tsconfig.json");
            if (!System.IO.File.Exists(tsProjectFileLocation))
            {
                System.IO.File.WriteAllText(tsProjectFileLocation, @"
{
    ""compilerOptions"": {
        ""target"": ""es5"",
        ""module"": ""commonjs"",
        ""sourceMap"": true
    }
}
");
            }
            result.Save();

            result.LoadFiles();

            return result;
        }

        public override IProject Load(ISolution solution, string filename)
        {
            TypeScriptProject result = new TypeScriptProject();
            result.Location = filename;

            //TODO: Load TS language service from here

            result.Solution = solution;
            result.LoadFiles();

            return result;
        }

        public TypeScriptProject() : base(true)
        {
            ExcludedFiles = new List<string>();
            Items = new ObservableCollection<IProjectItem>();
            References = new ObservableCollection<IProject>();
            ToolchainSettings = new ExpandoObject();
            DebugSettings = new ExpandoObject();
            Project = this;

            var tsContext = new TypeScriptContext();
            tsContext.LoadComponents();
            IoC.RegisterConstant(tsContext, typeof(TypeScriptContext));
            TypeScriptContext = tsContext;
        }

        [JsonIgnore]
        public TypeScriptContext TypeScriptContext { get; private set; }

        [JsonIgnore]
        public override IList<object> ConfigurationPages
        {
            get
            {
                throw new NotImplementedException();
                //return null;
            }
        }

        [JsonIgnore]
        public override string CurrentDirectory => Path.GetDirectoryName(Location) + Platform.DirectorySeperator;

        [JsonIgnore]
        public override IDebugger Debugger
        {
            get
            {
                var tsToolchain = IoC.Get<IShell>().Debuggers.FirstOrDefault(tc => tc.GetType().ToString() == "AvalonStudio.Debugging.TypeScript.TypeScriptDebugger");
                return tsToolchain;
            }
            set
            {
                throw new NotSupportedException();
            }
        }

        [JsonConverter(typeof(ExpandoObjectConverter))]
        public override dynamic DebugSettings { get; set; }

        public override List<string> ExcludedFiles { get; set; }

        public override string Executable { get; set; }

        public override string Extension => "tsproj";

        public override bool Hidden { get; set; }

        public override ObservableCollection<IProjectItem> Items { get; }

        [JsonIgnore]
        public override string Location { get; set; }

        [JsonIgnore]
        public override string LocationDirectory => CurrentDirectory;

        [JsonIgnore]
        public override string Name => Path.GetFileNameWithoutExtension(Location);

        [JsonIgnore]
        public override IProjectFolder Parent { get; set; }

        [JsonIgnore]
        public override IProject Project { get; set; }

        [JsonIgnore]
        public override ObservableCollection<IProject> References { get; }

        [JsonIgnore]
        public override ISolution Solution { get; set; }

        public override ITestFramework TestFramework { get; set; }

        [JsonIgnore]
        public override IToolChain ToolChain
        {
            get
            {
                var tsToolchain = IoC.Get<IShell>().ToolChains.FirstOrDefault(tc => tc.GetType().ToString() == "AvalonStudio.Toolchains.TypeScript.TypeScriptToolchain");
                return tsToolchain;
            }
            set
            {
                throw new NotSupportedException();
            }
        }

        [JsonConverter(typeof(ExpandoObjectConverter))]
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

        public override void RemoveReference(IProject project)
        {
            throw new NotImplementedException();
        }

        public override void ResolveReferences()
        {
            //throw new NotImplementedException();
        }

        public override void Save()
        {
            //TODO: Anything with references?
            SerializedObject.Serialize(Location, this); //Write the project
        }
    }
}