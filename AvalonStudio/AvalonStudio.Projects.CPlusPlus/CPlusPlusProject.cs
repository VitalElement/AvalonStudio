using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Dynamic;
using System.IO;
using System.Linq;
using AvalonStudio.Debugging;
using AvalonStudio.Extensibility;
using AvalonStudio.Extensibility.Menus;
using AvalonStudio.Platforms;
using AvalonStudio.Projects.Standard;
using AvalonStudio.Shell;
using AvalonStudio.TestFrameworks;
using AvalonStudio.Toolchains;
using AvalonStudio.Utils;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using AvalonStudio.Extensibility.Threading;
using System.Threading.Tasks;
using Avalonia.Threading;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("AvalonStudio.Projects.CPlusPlus.UnitTests")]

namespace AvalonStudio.Projects.CPlusPlus
{    
    public class CPlusPlusProject : FileSystemProject, IStandardProject
    {

        public const string ProjectExtension = "acproj";

        private static Dictionary<string, Tuple<string, string>> passwordCache =
            new Dictionary<string, Tuple<string, string>>();

        [JsonConstructor]
        public CPlusPlusProject(List<SourceFile> sourceFiles) : this()
        {
        }

        public CPlusPlusProject() : this(true)
        {

        }

        public CPlusPlusProject(bool useDispatcher) : base (useDispatcher)
        {
            ExcludedFiles = new List<string>();
            Items = new ObservableCollection<IProjectItem>();
            UnloadedReferences = new List<Reference>();
            StaticLibraries = new List<string>();
            References = new ObservableCollection<IProject>();
            PublicIncludes = new List<string>();
            GlobalIncludes = new List<string>();
            Includes = new List<Include>();
            Defines = new List<Definition>();
            CompilerArguments = new List<string>();
            ToolChainArguments = new List<string>();
            LinkerArguments = new List<string>();
            CCompilerArguments = new List<string>();
            CppCompilerArguments = new List<string>();
            BuiltinLibraries = new List<string>();
            ToolchainSettings = new ExpandoObject();
            DebugSettings = new ExpandoObject();
            Project = this;
        }

        [JsonIgnore]
        private string SolutionDirectory
        {
            get { return Directory.GetParent(CurrentDirectory).FullName; }
        }

        [JsonProperty(PropertyName = "References")]
        public List<Reference> UnloadedReferences { get; set; }

        [JsonProperty(PropertyName = "Toolchain")]
        public string ToolchainReference { get; set; }

        [JsonProperty(PropertyName = "Debugger")]
        public string DebuggerReference { get; set; }

        [JsonProperty(PropertyName = "TestFramework")]
        public string TestFrameworkReference { get; set; }

        public override List<string> ExcludedFiles { get; set; }

        [JsonIgnore]
        public IList<IMenuItem> ProjectMenuItems
        {
            get { throw new NotImplementedException(); }
        }

        [JsonIgnore]
        public bool IsBuilding { get; set; }

        [JsonIgnore]
        public override string LocationDirectory => CurrentDirectory;

        public override void Save()
        {
            UnloadedReferences.Clear();

            foreach (var reference in References)
            {
                UnloadedReferences.Add(new Reference { Name = reference.Name });
            }

            SerializedObject.Serialize(Location, this);
        }

        public override void AddReference(IProject project)
        {
            References.InsertSorted(project);
        }

        public override void RemoveReference(IProject project)
        {
            References.Remove(project);
        }

        /// <summary>
        ///     Resolves each reference, cloning and updating Git referenced projects where possible.
        /// </summary>
        public override void ResolveReferences()
        {
            foreach (var reference in UnloadedReferences)
            {
                var loadedReference = Solution.Projects.FirstOrDefault(p => p.Name == reference.Name);

                if (loadedReference != null)
                {
                    var currentReference = References.FirstOrDefault(r => r == loadedReference);

                    if (currentReference == null)
                    {
                        AddReference(loadedReference);
                    }
                    else
                    {
                        throw new Exception("The same Reference can not be added more than once.");
                    }
                }
                else
                {
                    Console.WriteLine("Implement placeholder reference here.");
                }
            }
        }

        public IList<string> GetReferencedIncludes()
        {
            var result = new List<string>();

            foreach (var reference in References)
            {
                var standardReference = reference as CPlusPlusProject;

                result.AddRange(standardReference.GenerateReferencedIncludes());
            }

            return result;
        }

        public IList<string> GetReferencedDefines()
        {
            var result = new List<string>();

            foreach (var reference in References)
            {
                var standardReference = reference as CPlusPlusProject;

                result.AddRange(standardReference.GenerateReferencedDefines());
            }

            return result;
        }


        public IList<string> GetGlobalIncludes()
        {
            var result = new List<string>();

            foreach (var reference in References)
            {
                var standardReference = reference as CPlusPlusProject;

                result.AddRange(standardReference.GetGlobalIncludes());
            }

            foreach (var include in Includes.Where(i => i.Global))
            {
                result.Add(Path.Combine(CurrentDirectory, include.Value).ToPlatformPath());
            }

            return result;
        }

        public IList<string> GetGlobalDefines()
        {
            var result = new List<string>();

            foreach (var reference in References)
            {
                var standardReference = reference as CPlusPlusProject;

                result.AddRange(standardReference.GetGlobalDefines());
            }

            foreach (var define in Defines.Where(i => i.Global))
            {
                result.Add(define.Value);
            }

            return result;
        }

        [JsonIgnore]
        public override ISolution Solution { get; set; }

        [JsonIgnore]
        public override string CurrentDirectory
        {
            get { return Path.GetDirectoryName(Location) + Platform.DirectorySeperator; }
        }

        [JsonIgnore]
        public override string Location { get; set; }

        [JsonIgnore]
        public override string Name
        {
            get { return Path.GetFileNameWithoutExtension(Location); }
        }

        public ProjectType Type { get; set; }

        [JsonIgnore]
        public override ObservableCollection<IProject> References { get; }

        public IList<string> PublicIncludes { get; }

        public IList<string> GlobalIncludes { get; }

        public IList<Include> Includes { get; }

        public IList<Definition> Defines { get; }
        
        public IList<string> CompilerArguments { get; }

        public IList<string> CCompilerArguments { get; }

        public IList<string> CppCompilerArguments { get; }

        public IList<string> ToolChainArguments { get; }

        public IList<string> LinkerArguments { get; }

        public string GetObjectDirectory(IStandardProject superProject)
        {
            return Path.Combine(GetOutputDirectory(superProject), "obj");
        }

        public string GetBuildDirectory(IStandardProject superProject)
        {
            return Path.Combine(GetOutputDirectory(superProject), "bin");
        }

        public string GetOutputDirectory(IStandardProject superProject)
        {
            var outputDirectory = string.Empty;

            if (string.IsNullOrEmpty(superProject.BuildDirectory))
            {
                outputDirectory = Path.Combine(superProject.CurrentDirectory, "build");
            }

            if (!string.IsNullOrEmpty(superProject.BuildDirectory))
            {
                outputDirectory = Path.Combine(superProject.CurrentDirectory, superProject.BuildDirectory);
            }

            if (this != superProject)
            {
                outputDirectory = Path.Combine(outputDirectory, Name);
            }

            return outputDirectory;
        }

        public IList<string> BuiltinLibraries { get; set; }
        public IList<string> StaticLibraries { get; set; }

        public string BuildDirectory { get; set; }
        public string LinkerScript { get; set; }
        public override string Executable { get; set; }

        public override IProject Load(ISolution solution, string filePath)
        {
            var result = Load(filePath, solution);

            return result;
        }
       

        public override int CompareTo(IProject other)
        {
            return Name.CompareTo(other.Name);
        }

        public override void ExcludeFile(ISourceFile file)
        {
            file.Parent.Items.Remove(file);

            ExcludedFiles.Add(file.Project.CurrentDirectory.MakeRelativePath(file.Location).ToAvalonPath());
            SourceFiles.Remove(file);
            Save();
        }

        public override void ExcludeFolder(IProjectFolder folder)
        {
            folder.Parent.Items.Remove(folder);

            ExcludedFiles.Add(folder.Project.CurrentDirectory.MakeRelativePath(folder.Location).ToAvalonPath());

            RemoveFiles(this, folder);

            Save();
        }

        [JsonIgnore]
        public override IToolChain ToolChain
        {
            get
            {
                var result = IoC.Get<IShell>().ToolChains.FirstOrDefault(tc => tc.GetType().ToString() == ToolchainReference);

                return result;
            }
            set { ToolchainReference = value.GetType().ToString(); }
        }

        [JsonIgnore]
        public override IDebugger Debugger
        {
            get
            {
                var result = IoC.Get<IShell>().Debuggers.FirstOrDefault(tc => tc.GetType().ToString() == DebuggerReference);

                return result;
            }
            set { DebuggerReference = value.GetType().ToString(); }
        }

        [JsonIgnore]
        public override ITestFramework TestFramework
        {
            get
            {
                var result = IoC.Get<IShell>()
                    .TestFrameworks.FirstOrDefault(tf => tf.GetType().ToString() == TestFrameworkReference);

                return result;
            }
            set { TestFrameworkReference = value.GetType().ToString(); }
        }

        [JsonIgnore]
        public override ObservableCollection<IProjectItem> Items { get; }

        [JsonIgnore]
        public override IList<object> ConfigurationPages
        {
            get
            {
                var result = new List<object>();

                result.Add(new TypeSettingsFormViewModel(this));
                result.Add(new IncludePathSettingsFormViewModel(this));
                result.Add(new ReferenceSettingsFormViewModel(this));
                result.Add(new ToolchainSettingsFormViewModel(this));
                result.Add(new DebuggerSettingsFormViewModel(this));

                return result;
            }
        }

        [JsonConverter(typeof(ExpandoObjectConverter))]
        public override dynamic ToolchainSettings { get; set; }

        [JsonConverter(typeof(ExpandoObjectConverter))]
        public override dynamic DebugSettings { get; set; }

        [JsonIgnore]
        public override string Extension
        {
            get { return ProjectExtension; }
        }

        [JsonIgnore]
        public override IProject Project { get; set; }

        [JsonIgnore]
        public override IProjectFolder Parent { get; set; }

        public override bool Hidden { get; set; }

        public static string GenerateProjectFileName(string name)
        {
            return string.Format("{0}.{1}", name, ProjectExtension);
        }

        public static CPlusPlusProject Load(string filename, ISolution solution)
        {
            if (!System.IO.File.Exists(filename))
            {
                Console.WriteLine("Unable for find project file: " + filename);
            }

            var project = SerializedObject.Deserialize<CPlusPlusProject>(filename);

            for (var i = 0; i < project.Includes.Count; i++)
            {
                project.Includes[i].Value = project.Includes[i].Value.ToAvalonPath();
            }

            for (var i = 0; i < project.ExcludedFiles.Count; i++)
            {
                project.ExcludedFiles[i] = project.ExcludedFiles[i].ToAvalonPath();
            }

            project.Project = project;
            project.Location = filename;
            project.Solution = solution;

            project.Items.InsertSorted(new ReferenceFolder(project));
            project.LoadFiles();

            return project;
        }


        public static CPlusPlusProject Create(ISolution solution, string directory, string name)
        {
            CPlusPlusProject result = null;

            var projectFile = Path.Combine(directory, GenerateProjectFileName(name));

            if (!System.IO.File.Exists(projectFile))
            {
                var project = new CPlusPlusProject();
                project.Solution = solution;
                project.Location = projectFile;
                
                project.Save();
                project.LoadFiles();

                result = project;
            }

            return result;
        }

        protected IList<string> GenerateReferencedIncludes()
        {
            var result = new List<string>();

            foreach (var reference in References)
            {
                var loadedReference = reference as CPlusPlusProject;

                if (loadedReference == null)
                {
                    // What to do in this situation?
                    throw new NotImplementedException();
                }

                result.AddRange(loadedReference.GenerateReferencedIncludes());
            }

            foreach (var includePath in Includes.Where(i => i.Exported && !i.Global))
            {
                result.Add(Path.Combine(CurrentDirectory, includePath.Value).ToPlatformPath());
            }

            return result;
        }

        protected IList<string> GenerateReferencedDefines()
        {
            var result = new List<string>();

            foreach (var reference in References)
            {
                var loadedReference = reference as CPlusPlusProject;

                if (loadedReference == null)
                {
                    // What to do in this situation?
                    throw new NotImplementedException();
                }

                result.AddRange(loadedReference.GenerateReferencedDefines());
            }

            foreach (var define in Defines.Where(i => i.Exported && !i.Global))
            {
                result.Add(define.Value);
            }

            return result;
        }

        public CPlusPlusProject GetReference(Reference reference)
        {
            CPlusPlusProject result = null;

            foreach (var project in Solution.Projects)
            {
                if (project.Name == reference.Name)
                {
                    result = project as CPlusPlusProject;
                    break;
                }
            }

            if (result == null)
            {
                throw new Exception(string.Format("Unable to find reference {0}, in directory {1}", reference.Name,
                    Solution.CurrentDirectory));
            }

            return result;
        }

        public bool ShouldSerializeReferences()
        {
            return UnloadedReferences.Count > 0;
        }

        public bool ShouldSerializePublicIncludes()
        {
            return PublicIncludes.Count > 0;
        }


        public bool ShouldSerializeGlobalIncludes()
        {
            return GlobalIncludes.Count > 0;
        }

        public bool ShouldSerializeIncludes()
        {
            return Includes.Count > 0;
        }

        public bool ShouldSerializeDefines()
        {
            return Defines.Count > 0;
        }

        public bool ShouldSerializeCompilerArguments()
        {
            return CompilerArguments.Count > 0;
        }

        public bool ShouldSerializeCCompilerArguments()
        {
            return CCompilerArguments.Count > 0;
        }

        public bool ShouldSerializeCppCompilerArguments()
        {
            return CppCompilerArguments.Count > 0;
        }

        public bool ShouldSerializeToolChainArguments()
        {
            return ToolChainArguments.Count > 0;
        }

        public bool ShouldSerializeLinkerArguments()
        {
            return LinkerArguments.Count > 0;
        }

        public bool ShouldSerializeBuiltinLibraries()
        {
            return BuiltinLibraries.Count > 0;
        }

        private void RemoveFiles(CPlusPlusProject project, IProjectFolder folder)
        {
            foreach (var item in folder.Items)
            {
                if (item is IProjectFolder)
                {
                    RemoveFiles(project, item as IProjectFolder);
                    project.Folders.Remove(item as IProjectFolder);
                }

                if (item is ISourceFile)
                {
                    project.SourceFiles.Remove(item as ISourceFile);
                }
            }
        }

        public bool ShouldSerializeHidden()
        {
            return Hidden;
        }

        public override int CompareTo(IProjectFolder other)
        {
            return Location.CompareFilePath(other.Location);
        }

        public override int CompareTo(string other)
        {
            return Location.CompareFilePath(other);
        }

        public override int CompareTo(IProjectItem other)
        {
            return Name.CompareTo(other.Name);
        }
    }
}