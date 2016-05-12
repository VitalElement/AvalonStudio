namespace AvalonStudio.Projects.CPlusPlus
{
    using AvalonStudio.Projects.Standard;
    using AvalonStudio.Utils;
    using Debugging;
    using Extensibility;
    using Extensibility.Menus;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Converters;
    using Avalonia.Controls;
    using Platforms;
    using Shell;
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Dynamic;
    using System.IO;
    using System.Linq;
    using TestFrameworks;
    using Toolchains;

    public class CPlusPlusProject : SerializedObject<CPlusPlusProject>, IStandardProject
    {
        public const string ProjectExtension = "acproj";

        public static string GenerateProjectFileName(string name)
        {
            return string.Format("{0}.{1}", name, ProjectExtension);
        }
        
        [JsonIgnore]
        public bool IsBuilding { get; set; }

        private static bool IsExcluded(List<string> exclusionFilters, string path)
        {
            bool result = false;


            var filter = exclusionFilters.FirstOrDefault(f => path.Contains(f));

            result = !string.IsNullOrEmpty(filter);

            return result;
        }

        public static void PopulateFiles(CPlusPlusProject project, StandardProjectFolder folder)
        {
            var files = Directory.EnumerateFiles(folder.Location);

            files = files.Where((f) => !IsExcluded(project.ExcludedFiles, project.CurrentDirectory.MakeRelativePath(f).ToAvalonPath()) && Path.GetExtension(f) != '.' + ProjectExtension);

            foreach (var file in files)
            {
                var sourceFile = SourceFile.FromPath(project, folder, file.ToPlatformPath());
                project.SourceFiles.InsertSorted(sourceFile);
                folder.Items.Add(sourceFile);
            }
        }

        public static StandardProjectFolder GetSubFolders(CPlusPlusProject project, IProjectFolder parent, string path)
        {
            StandardProjectFolder result = new StandardProjectFolder(path);

            var folders = Directory.GetDirectories(path);

            if (folders.Count() > 0)
            {
                foreach (var folder in folders.Where((f) => !IsExcluded(project.ExcludedFiles, project.CurrentDirectory.MakeRelativePath(f).ToAvalonPath())))
                {
                    result.Items.Add(GetSubFolders(project, result, folder));
                }
            }

            PopulateFiles(project, result);

            result.Parent = parent;
            result.Project = project;

            return result;
        }

        private void LoadFiles()
        {
            Items.Add(new ReferenceFolder(this));
            var folders = GetSubFolders(this, this, CurrentDirectory);

            //Items = new ObservableCollection<IProjectItem>();

            foreach (var item in folders.Items)
            {
                item.Parent = this;        
                Items.Add(item);
            }

            foreach (var file in SourceFiles)
            {
                file.Project = this;
                file.Parent = this;
            }
        }

        public static CPlusPlusProject Load(string filename, ISolution solution)
        {
            if (!File.Exists(filename))
            {
                Console.WriteLine("Unable for find project file: " + filename);
            }

            var project = Deserialize(filename);

            for (int i = 0; i < project.Includes.Count; i++)
            {
                project.Includes[i].Value = project.Includes[i].Value.ToAvalonPath();
            }

            for (int i = 0; i < project.ExcludedFiles.Count; i++) {
                project.ExcludedFiles[i] = project.ExcludedFiles[i].ToAvalonPath();
            }

            project.Project = project;
            project.Location = filename;
            project.SetSolution(solution);

            project.LoadFiles();

            return project;
        }



        public static CPlusPlusProject Create(ISolution solution, string directory, string name)
        {
            CPlusPlusProject result = null;

            var projectFile = Path.Combine(directory, GenerateProjectFileName(name));

            if (!File.Exists(projectFile))
            {
                var project = new CPlusPlusProject { Name = name };
                project.SetSolution(solution);
                project.Location = projectFile;

                project.LoadFiles();
                project.Save();

                result = project;
            }

            return result;
        }

        public void Save()
        {
            UnloadedReferences.Clear();

            foreach (var reference in References)
            {
                UnloadedReferences.Add(new Reference() { Name = reference.Name });
            }

            Serialize(this.Location);
        }

        [JsonConstructor]
        public CPlusPlusProject(List<SourceFile> sourceFiles) : this()
        {
        }

        public CPlusPlusProject()
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
            SourceFiles = new List<ISourceFile>();
            CompilerArguments = new List<string>();
            ToolChainArguments = new List<string>();
            LinkerArguments = new List<string>();
            CCompilerArguments = new List<string>();
            CppCompilerArguments = new List<string>();
            BuiltinLibraries = new List<string>();
            ToolchainSettings = new ExpandoObject();
            DebugSettings = new ExpandoObject();
        }

        private static Dictionary<string, Tuple<string, string>> passwordCache = new Dictionary<string, Tuple<string, string>>();

        public void AddReference(IProject project)
        {
            References.InsertSorted(project);
        }

        public void RemoveReference(IProject project)
        {
            References.Remove(project);
        }

        /// <summary>
        /// Resolves each reference, cloning and updating Git referenced projects where possible.
        /// </summary>


        public void ResolveReferences()
        {
            foreach (var reference in UnloadedReferences)
            {
                var loadedReference = Solution.Projects.FirstOrDefault((p) => p.Name == reference.Name);

                if (loadedReference != null)
                {
                    var currentReference = References.FirstOrDefault((r) => r == loadedReference);

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

        public void SetSolution(ISolution solution)
        {
            this.Solution = solution;
        }

        [JsonIgnore]
        private string SolutionDirectory
        {
            get
            {
                return Directory.GetParent(CurrentDirectory).FullName;
            }
        }

        protected IList<string> GenerateReferencedIncludes()
        {
            List<string> result = new List<string>();

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
            List<string> result = new List<string>();

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

        public IList<string> GetReferencedIncludes()
        {
            List<string> result = new List<string>();

            foreach (var reference in References)
            {
                var standardReference = reference as CPlusPlusProject;

                result.AddRange(standardReference.GenerateReferencedIncludes());
            }

            return result;
        }

        public IList<string> GetReferencedDefines()
        {
            List<string> result = new List<string>();

            foreach (var reference in References)
            {
                var standardReference = reference as CPlusPlusProject;

                result.AddRange(standardReference.GenerateReferencedDefines());
            }

            return result;
        }


        public IList<string> GetGlobalIncludes()
        {
            List<string> result = new List<string>();

            foreach (var reference in References)
            {
                var standardReference = reference as CPlusPlusProject;

                result.AddRange(standardReference.GetGlobalIncludes());
            }

            foreach (var include in Includes.Where((i) => i.Global))
            {
                result.Add(Path.Combine(CurrentDirectory, include.Value).ToPlatformPath());
            }

            return result;
        }

        public IList<string> GetGlobalDefines()
        {
            List<string> result = new List<string>();

            foreach (var reference in References)
            {
                var standardReference = reference as CPlusPlusProject;

                result.AddRange(standardReference.GetGlobalDefines());
            }

            foreach (var define in Defines.Where((i) => i.Global))
            {
                result.Add(define.Value);
            }

            return result;
        }

        [JsonIgnore]
        public ISolution Solution { get; set; }

        [JsonIgnore]
        public string CurrentDirectory
        {
            get
            {
                return Path.GetDirectoryName(Location) + Platform.DirectorySeperator;
            }
        }

        [JsonIgnore]
        public string Location { get; internal set; }

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
                throw new Exception(string.Format("Unable to find reference {0}, in directory {1}", reference.Name, Solution.CurrentDirectory));
            }

            return result;
        }

        public string Name { get; set; }

        public ProjectType Type { get; set; }

        public bool ShouldSerializeReferences()
        {
            return UnloadedReferences.Count > 0;
        }

        [JsonProperty(PropertyName = "References")]
        public List<Reference> UnloadedReferences { get; set; }

        [JsonIgnore]
        public ObservableCollection<IProject> References
        {
            get; private set;
        }

        public bool ShouldSerializePublicIncludes()
        {
            return PublicIncludes.Count > 0;
        }

        public IList<string> PublicIncludes { get; private set; }


        public bool ShouldSerializeGlobalIncludes()
        {
            return GlobalIncludes.Count > 0;
        }

        public IList<string> GlobalIncludes { get; private set; }

        public bool ShouldSerializeIncludes()
        {
            return Includes.Count > 0;
        }

        public IList<Include> Includes { get; private set; }

        public bool ShouldSerializeDefines()
        {
            return Defines.Count > 0;
        }

        public IList<Definition> Defines { get; private set; }        


        [JsonIgnore]
        public IList<ISourceFile> SourceFiles
        {
            get; private set;
        }

        public bool ShouldSerializeCompilerArguments()
        {
            return CompilerArguments.Count > 0;
        }

        public IList<string> CompilerArguments { get; private set; }

        public bool ShouldSerializeCCompilerArguments()
        {
            return CCompilerArguments.Count > 0;
        }

        public IList<string> CCompilerArguments { get; private set; }

        public bool ShouldSerializeCppCompilerArguments()
        {
            return CppCompilerArguments.Count > 0;
        }

        public IList<string> CppCompilerArguments { get; private set; }

        public bool ShouldSerializeToolChainArguments()
        {
            return ToolChainArguments.Count > 0;
        }

        public IList<string> ToolChainArguments { get; private set; }

        public bool ShouldSerializeLinkerArguments()
        {
            return LinkerArguments.Count > 0;
        }

        public IList<string> LinkerArguments { get; private set; }

        public bool ShouldSerializeBuiltinLibraries()
        {
            return BuiltinLibraries.Count > 0;
        }

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
            string outputDirectory = string.Empty;

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
        public string Executable { get; set; }

        //static LlilumToolchain GetLLilumToolchain()
        //{
        //    var gccSettings = new ToolchainSettings();
        //    gccSettings.ToolChainLocation = @"C:\VEStudio\AppData\Repos\AvalonStudio.Toolchains.Llilum";
        //    gccSettings.IncludePaths.Add("GCC\\arm-none-eabi\\include\\c++\\4.9.3");
        //    gccSettings.IncludePaths.Add("GCC\\arm-none-eabi\\include\\c++\\4.9.3\\arm-none-eabi\\thumb");
        //    gccSettings.IncludePaths.Add("GCC\\lib\\gcc\\arm-none-eabi\\4.9.3\\include");

        //    return new LlilumToolchain(gccSettings);
        //}

        public IProject Load(ISolution solution, string filePath)
        {
            var result = CPlusPlusProject.Load(filePath, solution);

            return result;
        }

        public int CompareTo(IProject other)
        {
            return this.Name.CompareTo(other.Name);
        }

        public void RemoveFile(ISourceFile file)
        {
            file.Parent.Items.Remove(file);

            ExcludedFiles.Add(file.Project.CurrentDirectory.MakeRelativePath(file.Location));
            this.SourceFiles.Remove(file);
            Save();
        }

        private void RemoveFiles(CPlusPlusProject project, IProjectFolder folder)
        {
            foreach (var item in folder.Items)
            {
                if (item is IProjectFolder)
                {
                    RemoveFiles(project, item as IProjectFolder);
                }

                if (item is ISourceFile)
                {
                    project.SourceFiles.Remove(item as ISourceFile);
                }
            }
        }

        public void RemoveFolder(IProjectFolder folder)
        {
            folder.Parent.Items.Remove(folder);

            ExcludedFiles.Add(folder.Project.CurrentDirectory.MakeRelativePath(folder.Location));

            RemoveFiles(this, folder);

            Save();
        }

        public ISourceFile FindFile(ISourceFile path)
        {
            return SourceFiles.BinarySearch(f => f, path);
        }

        public void AddFile(ISourceFile file)
        {
            // TODO how will this work with subdirs.
            SourceFiles.InsertSorted(file);
            Items.Add(file);
        }

        public void AddFolder(IProjectFolder folder)
        {
            throw new NotImplementedException();
        }



        [JsonIgnore]
        public IToolChain ToolChain
        {
            get
            {
                IToolChain result = IoC.Get<IShell>().ToolChains.FirstOrDefault((tc) => tc.GetType().ToString() == ToolchainReference);

                return result;
            }
            set
            {
                ToolchainReference = value.GetType().ToString();
            }
        }

        [JsonIgnore]
        public IDebugger Debugger
        {
            get
            {
                IDebugger result = IoC.Get<IShell>().Debuggers.FirstOrDefault((tc) => tc.GetType().ToString() == DebuggerReference);

                return result;
            }
            set
            {
                DebuggerReference = value.GetType().ToString();
            }
        }

        [JsonIgnore]
        public ITestFramework TestFramework
        {
            get
            {
                ITestFramework result = IoC.Get<IShell>().TestFrameworks.FirstOrDefault((tf) => tf.GetType().ToString() == TestFrameworkReference);

                return result;
            }
            set
            {
                TestFrameworkReference = value.GetType().ToString();
            }
        }   

        [JsonProperty(PropertyName = "Toolchain")]
        public string ToolchainReference { get; set; }

        [JsonProperty(PropertyName = "Debugger")]
        public string DebuggerReference { get; set; }

        [JsonProperty(PropertyName ="TestFramework")]
        public string TestFrameworkReference { get; set; }

        [JsonIgnore]
        public ObservableCollection<IProjectItem> Items
        {
            get; private set;
        }

        public List<string> ExcludedFiles
        {
            get; set;
        }

        [JsonIgnore]
        public IList<IMenuItem> ProjectMenuItems
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        [JsonIgnore]
        public IList<TabItem> ConfigurationPages
        {
            get
            {
                var result = new List<TabItem>();

                result.Add(new TypeSettingsForm() { DataContext = new TypeSettingsFormViewModel(this) });
                result.Add(new IncludePathSettingsForm() { DataContext = new IncludePathSettingsFormViewModel(this) });
                result.Add(new ReferencesSettingsForm() { DataContext = new ReferenceSettingsFormViewModel(this) });
                result.Add(new ToolchainSettingsForm() { DataContext = new ToolchainSettingsFormViewModel(this) });
                result.Add(new DebuggerSettingsForm() { DataContext = new DebuggerSettingsFormViewModel(this) });

                return result;
            }
        }

        [JsonConverter(typeof(ExpandoObjectConverter))]
        public dynamic ToolchainSettings { get; set; }

        [JsonConverter(typeof(ExpandoObjectConverter))]
        public dynamic DebugSettings { get; set; }

        [JsonIgnore]
        public string Extension
        {
            get
            {
                return CPlusPlusProject.ProjectExtension;
            }
        }

        [JsonIgnore]
        public IProject Project { get; set; }

        [JsonIgnore]
        public IProjectFolder Parent { get; set; }

        public bool ShouldSerializeHidden()
        {
            return Hidden;
        }

        public bool Hidden { get; set; }
    }
}
