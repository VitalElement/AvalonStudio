namespace AvalonStudio.Projects.CPlusPlus
{
    using AvalonStudio.Projects.Standard;
    using AvalonStudio.Utils;
    using Newtonsoft.Json;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using Toolchains;
    using Extensibility.Menus;
    using Toolchains.Llilum;
    using Extensibility;
    using Perspex.Controls;
    using Newtonsoft.Json.Linq;
    using Newtonsoft.Json.Converters;
    using System.Dynamic;
    using Extensibility.Platform;
    using System.Collections.ObjectModel;

    public class CPlusPlusProject : SerializedObject<CPlusPlusProject>, IStandardProject
    {        
        public const string ProjectExtension = "acproj";

        public static string GenerateProjectFileName(string name)
        {
            return string.Format("{0}.{1}", name, ProjectExtension);
        }

        [JsonIgnore]
        public bool IsBuilding { get; set; }

        public static void PopulateFiles(CPlusPlusProject project, StandardProjectFolder folder)
        {
            var files = Directory.EnumerateFiles(folder.Path);

            files = files.Where((f) => !project.ExcludedFiles.Contains(project.CurrentDirectory.MakeRelativePath(f)) && Path.GetExtension(f) != '.' + ProjectExtension);                

            foreach (var file in files)
            {
                var sourceFile = new SourceFile() { Project = project, File = file };
                project.SourceFiles.Add(sourceFile);
                folder.Items.Add(sourceFile);
            }
        }

        public static StandardProjectFolder GetSubFolders(CPlusPlusProject project, string path)
        {
            StandardProjectFolder result = new StandardProjectFolder(path);

            var folders = Directory.GetDirectories(path);

            if (folders.Count() > 0)
            {
                foreach (var folder in folders)
                {
                    result.Items.Add(GetSubFolders(project, folder));
                }
            }

            PopulateFiles(project, result);

            return result;
        }

        private void LoadFiles ()
        {
            var folders = GetSubFolders(this, CurrentDirectory);

            Items = new List<IProjectItem>();

            foreach (var item in folders.Items)
            {
                Items.Add(item);
            }

            foreach (var file in SourceFiles)
            {
                (file as SourceFile)?.SetProject(this);
            }
        }

        public static CPlusPlusProject Load(string filename, ISolution solution)
        {
            var project = Deserialize(filename);

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

            foreach(var reference in References)
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
            Items = new List<IProjectItem>();
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
        }

        private static Dictionary<string, Tuple<string, string>> passwordCache = new Dictionary<string, Tuple<string, string>>();

        /// <summary>
        /// Resolves each reference, cloning and updating Git referenced projects where possible.
        /// </summary>


        public void ResolveReferences()
        {
            foreach (var reference in UnloadedReferences)
            {
                var referenceDirectory = Path.Combine(Solution.CurrentDirectory, reference.Name);


                var projectFile = Path.Combine(referenceDirectory, reference.Name + "." + Extension);

                if (File.Exists(projectFile))
                {
                    var loadedReference = Solution.Projects.FirstOrDefault((p) => p.Name == reference.Name);

                    if (loadedReference != null)
                    {
                        References.Add(loadedReference);
                    }
                    else
                    {
                        Console.WriteLine("Implement placeholder reference here.");
                    }
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

                if(loadedReference == null)
                {
                    // What to do in this situation?
                    throw new NotImplementedException();
                }

                result.AddRange(loadedReference.GenerateReferencedIncludes());
            }

            foreach (var includePath in Includes.Where(i=>i.Exported))
            {
                result.Add(Path.Combine(CurrentDirectory, includePath.Value));
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

        public IList<string> GetGlobalIncludes()
        {
            List<string> result = new List<string>();

            foreach (var reference in References)
            {
                var standardReference = reference as CPlusPlusProject;

                result.AddRange(standardReference.GetGlobalIncludes());
            }

            foreach (var include in GlobalIncludes)
            {
                result.Add(Path.Combine(CurrentDirectory, include));
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

        [JsonIgnore]
        public string Executable
        {
            get
            {
                throw new NotImplementedException();
            }
        }

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

        [JsonIgnore]
        public IToolChain ToolChain
        {
            get
            {
                IToolChain result = Workspace.Instance.ToolChains.FirstOrDefault((tc) => tc.GetType().ToString() == ToolchainReference);

                return result;
            }
            set
            {
                ToolchainReference = value.GetType().ToString();
            }
        }

        [JsonProperty(PropertyName = "Toolchain")]
        public string ToolchainReference
        {
            get; set;
        }

        [JsonIgnore]
        public IList<IProjectItem> Items
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
                result.Add(new TargetSettingsForm());
                result.Add(new IncludePathSettingsForm() { DataContext = new IncludePathSettingsFormViewModel(this) });
                result.Add(new ReferencesSettingsForm() { DataContext = new ReferenceSettingsFormViewModel(this) });
                result.Add(new ToolchainSettingsForm() { DataContext = new ToolchainSettingsFormViewModel(this) });
                result.Add(new ComponentSettingsForm());
                result.Add(new DebuggerSettingsForm());

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
    }
}
