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
    public class CPlusPlusProject : SerializedObject<CPlusPlusProject>, IStandardProject
    {
        private FileSystemWatcher fileSystemWatcher;
        private FileSystemWatcher folderSystemWatcher;
        private Dispatcher uiDispatcher;

        public const string ProjectExtension = "acproj";

        private static Dictionary<string, Tuple<string, string>> passwordCache =
            new Dictionary<string, Tuple<string, string>>();

        public event EventHandler FileAdded;

        [JsonConstructor]
        public CPlusPlusProject(List<SourceFile> sourceFiles) : this()
        {
        }

        public CPlusPlusProject() : this(true)
        {

        }

        public CPlusPlusProject(bool useDispatcher)
        {
            ExcludedFiles = new List<string>();
            Items = new ObservableCollection<IProjectItem>();
            Folders = new ObservableCollection<IProjectFolder>();
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
            Project = this;

            if (useDispatcher)
            {
                uiDispatcher = Dispatcher.UIThread;
            }
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

        public List<string> ExcludedFiles { get; set; }

        [JsonIgnore]
        public IList<IMenuItem> ProjectMenuItems
        {
            get { throw new NotImplementedException(); }
        }

        [JsonIgnore]
        public bool IsBuilding { get; set; }

        [JsonIgnore]
        public string LocationDirectory => CurrentDirectory;

        public void Save()
        {
            UnloadedReferences.Clear();

            foreach (var reference in References)
            {
                UnloadedReferences.Add(new Reference { Name = reference.Name });
            }

            Serialize(Location);
        }

        void Invoke(Action action)
        {
            if (uiDispatcher != null)
            {
                uiDispatcher.InvokeTaskAsync(() =>
                {
                    action();
                });
            }
            else
            {
                action();
            }
        }

        public void AddReference(IProject project)
        {
            References.InsertSorted(project);
        }

        public void RemoveReference(IProject project)
        {
            References.Remove(project);
        }

        /// <summary>
        ///     Resolves each reference, cloning and updating Git referenced projects where possible.
        /// </summary>
        public void ResolveReferences()
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
        public ISolution Solution { get; set; }

        [JsonIgnore]
        public string CurrentDirectory
        {
            get { return Path.GetDirectoryName(Location) + Platform.DirectorySeperator; }
        }

        [JsonIgnore]
        public string Location { get; internal set; }

        [JsonIgnore]
        public string Name
        {
            get { return Path.GetFileNameWithoutExtension(Location); }
        }

        public ProjectType Type { get; set; }

        [JsonIgnore]
        public ObservableCollection<IProject> References { get; }

        public IList<string> PublicIncludes { get; }

        public IList<string> GlobalIncludes { get; }

        public IList<Include> Includes { get; }

        public IList<Definition> Defines { get; }


        [JsonIgnore]
        public IList<ISourceFile> SourceFiles { get; }

        [JsonIgnore]
        public IList<IProjectFolder> Folders { get; }

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
            var result = Load(filePath, solution);

            return result;
        }

        public void RemoveFolder(string folder)
        {
            var existingFolder = FindFolder(folder);

            if (existingFolder != null)
            {
                RemoveFolder(existingFolder);
            }
            else
            {
                Console.WriteLine("Didnt find.");
            }
        }

        public void AddFolder(string fullPath)
        {
            var folder = FindFolder(Path.GetDirectoryName(fullPath) + "\\");

            var existing = FindFolder(fullPath);

            Console.WriteLine($"AddFolder: {fullPath}");

            if (existing != null)
            {
                Console.WriteLine("existing folder");
            }
            else
            {
                var newFolder = GetSubFolders(this, folder, fullPath);

                if (folder.Location == Project.CurrentDirectory)
                {
                    Project.AddFolder(newFolder);
                }
                else
                {
                    folder.AddFolder(newFolder);
                }
            }
        }

        public void RemoveFile(string fullPath)
        {
            var file = FindFile(fullPath);

            if (file != null)
            {
                RemoveFile(file);
            }
        }

        public void AddFile(string fullPath)
        {
            var folder = FindFolder(Path.GetDirectoryName(fullPath) + "\\");

            var sourceFile = SourceFile.FromPath(this, folder, fullPath.ToPlatformPath());
            SourceFiles.InsertSorted(sourceFile);

            if (folder.Location == Project.CurrentDirectory)
            {
                Project.Items.InsertSorted(sourceFile);
                sourceFile.Parent = Project;
            }
            else
            {
                folder.Items.InsertSorted(sourceFile);
                sourceFile.Parent = folder;
            }

            FileAdded?.Invoke(this, new EventArgs());
        }

        public void FileChanged (string fullPath)
        {
            var file = FindFile(fullPath);

            if(file != null)
            {
                file.RaiseFileModifiedEvent();
            }
        }

        private void FileSystemWatcher_Deleted(object sender, FileSystemEventArgs e)
        {
            Invoke(() =>
            {
                RemoveFile(e.FullPath);
            });
        }

        private void FileSystemWatcher_Renamed(object sender, RenamedEventArgs e)
        {
            Invoke(() =>
            {
                RemoveFile(e.OldFullPath);

                AddFile(e.FullPath);
            });
        }

        private void FileSystemWatcher_Created(object sender, FileSystemEventArgs e)
        {
            Invoke(() =>
            {
                AddFile(e.FullPath);
            });            
        }

        private void FileSystemWatcher_Changed(object sender, FileSystemEventArgs e)
        {
            Invoke(() =>
            {
                Console.WriteLine(e.ChangeType);
                FileChanged(e.FullPath);
            });
        }

        private void FolderSystemWatcher_Deleted(object sender, FileSystemEventArgs e)
        {
            Invoke(() =>
            {                
                RemoveFolder(e.FullPath);
            });
        }

        private void FolderSystemWatcher_Renamed(object sender, RenamedEventArgs e)
        {
            Invoke(() =>
            {
                RemoveFolder(e.OldFullPath);

                AddFolder(e.FullPath);
            });
        }

        private void FolderSystemWatcher_Created(object sender, FileSystemEventArgs e)
        {
            Invoke(() =>
            {
                AddFolder(e.FullPath);
            });
        }

        private void FolderSystemWatcher_Changed(object sender, FileSystemEventArgs e)
        {

        }

        public int CompareTo(IProject other)
        {
            return Name.CompareTo(other.Name);
        }

        public void ExcludeFile(ISourceFile file)
        {
            file.Parent.Items.Remove(file);

            ExcludedFiles.Add(file.Project.CurrentDirectory.MakeRelativePath(file.Location).ToAvalonPath());
            SourceFiles.Remove(file);
            Save();
        }

        public void ExcludeFolder(IProjectFolder folder)
        {
            folder.Parent.Items.Remove(folder);

            ExcludedFiles.Add(folder.Project.CurrentDirectory.MakeRelativePath(folder.Location).ToAvalonPath());

            RemoveFiles(this, folder);

            Save();
        }

        public void RemoveFile(ISourceFile file)
        {
            file.Parent.Items.Remove(file);
            SourceFiles.Remove(file);
        }

        public void RemoveFolder(IProjectFolder folder)
        {
            folder.Parent.Items.Remove(folder);
            RemoveFiles(this, folder);

            if(!Folders.Remove(folder))
            {
                Console.WriteLine("Remove Folder failed...");
            }
        }

        public ISourceFile FindFile(ISourceFile path)
        {
            return SourceFiles.BinarySearch(f => f, path);
        }

        public ISourceFile FindFile (string path)
        {
            return SourceFiles.BinarySearch(path);
        }

        public IProjectFolder FindFolder(string path)
        {
            return Folders.BinarySearch(path);
        }

        public void AddFile(ISourceFile file)
        {
            // TODO how will this work with subdirs.
            SourceFiles.InsertSorted(file);
            Items.InsertSorted(file);
        }

        public void AddFolder(IProjectFolder folder)
        {
            folder.Parent = this;
            Folders.InsertSorted(folder);
            Items.InsertSorted(folder);
        }


        [JsonIgnore]
        public IToolChain ToolChain
        {
            get
            {
                var result = IoC.Get<IShell>().ToolChains.FirstOrDefault(tc => tc.GetType().ToString() == ToolchainReference);

                return result;
            }
            set { ToolchainReference = value.GetType().ToString(); }
        }

        [JsonIgnore]
        public IDebugger Debugger
        {
            get
            {
                var result = IoC.Get<IShell>().Debuggers.FirstOrDefault(tc => tc.GetType().ToString() == DebuggerReference);

                return result;
            }
            set { DebuggerReference = value.GetType().ToString(); }
        }

        [JsonIgnore]
        public ITestFramework TestFramework
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
        public ObservableCollection<IProjectItem> Items { get; }

        [JsonIgnore]
        public IList<object> ConfigurationPages
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
        public dynamic ToolchainSettings { get; set; }

        [JsonConverter(typeof(ExpandoObjectConverter))]
        public dynamic DebugSettings { get; set; }

        [JsonIgnore]
        public string Extension
        {
            get { return ProjectExtension; }
        }

        [JsonIgnore]
        public IProject Project { get; set; }

        [JsonIgnore]
        public IProjectFolder Parent { get; set; }

        public bool Hidden { get; set; }

        public static string GenerateProjectFileName(string name)
        {
            return string.Format("{0}.{1}", name, ProjectExtension);
        }

        private static bool IsExcluded(List<string> exclusionFilters, string path)
        {
            var result = false;


            var filter = exclusionFilters.FirstOrDefault(f => path.Contains(f));

            result = !string.IsNullOrEmpty(filter);

            return result;
        }

        public static void PopulateFiles(CPlusPlusProject project, StandardProjectFolder folder)
        {
            var files = Directory.EnumerateFiles(folder.Location);

            files =
                files.Where(
                    f =>
                        !IsExcluded(project.ExcludedFiles, project.CurrentDirectory.MakeRelativePath(f).ToAvalonPath()) &&
                        Path.GetExtension(f) != '.' + ProjectExtension);

            foreach (var file in files)
            {
                var sourceFile = SourceFile.FromPath(project, folder, file.ToPlatformPath());
                project.SourceFiles.InsertSorted(sourceFile);
                folder.Items.InsertSorted(sourceFile);
            }
        }

        public static StandardProjectFolder GetSubFolders(CPlusPlusProject project, IProjectFolder parent, string path)
        {
            var result = new StandardProjectFolder(path);

            var folders = Directory.GetDirectories(path);

            if (folders.Count() > 0)
            {
                foreach (
                    var folder in
                        folders.Where(f => !IsExcluded(project.ExcludedFiles, project.CurrentDirectory.MakeRelativePath(f).ToAvalonPath()))
                    )
                {
                    result.Items.InsertSorted(GetSubFolders(project, result, folder));
                }
            }

            PopulateFiles(project, result);

            project.Folders.InsertSorted(result);
            result.Parent = parent;
            result.Project = project;

            return result;
        }

        internal void LoadFiles()
        {
            Items.InsertSorted(new ReferenceFolder(this));
            var folders = GetSubFolders(this, this, CurrentDirectory);

            //Items = new ObservableCollection<IProjectItem>();

            foreach (var item in folders.Items)
            {
                item.Parent = this;
                Items.InsertSorted(item);
            }

            foreach (var file in SourceFiles)
            {
                file.Project = this;
            }

            folderSystemWatcher = new FileSystemWatcher(CurrentDirectory);
            folderSystemWatcher.Changed += FolderSystemWatcher_Changed;
            folderSystemWatcher.Created += FolderSystemWatcher_Created;
            folderSystemWatcher.Renamed += FolderSystemWatcher_Renamed;
            folderSystemWatcher.Deleted += FolderSystemWatcher_Deleted;
            folderSystemWatcher.NotifyFilter = NotifyFilters.DirectoryName;
            folderSystemWatcher.IncludeSubdirectories = true;
            folderSystemWatcher.EnableRaisingEvents = true;

            fileSystemWatcher = new FileSystemWatcher(CurrentDirectory);
            fileSystemWatcher.Changed += FileSystemWatcher_Changed;
            fileSystemWatcher.Created += FileSystemWatcher_Created;
            fileSystemWatcher.Renamed += FileSystemWatcher_Renamed;
            fileSystemWatcher.Deleted += FileSystemWatcher_Deleted;
            fileSystemWatcher.NotifyFilter = NotifyFilters.FileName | NotifyFilters.LastWrite;
            fileSystemWatcher.IncludeSubdirectories = true;
            fileSystemWatcher.EnableRaisingEvents = true;            
        }

        public static CPlusPlusProject Load(string filename, ISolution solution)
        {
            if (!File.Exists(filename))
            {
                Console.WriteLine("Unable for find project file: " + filename);
            }

            var project = Deserialize(filename);

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
                var project = new CPlusPlusProject();
                project.SetSolution(solution);
                project.Location = projectFile;
                
                project.Save();
                project.LoadFiles();

                result = project;
            }

            return result;
        }

        public void SetSolution(ISolution solution)
        {
            Solution = solution;
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

        public int CompareTo(IProjectFolder other)
        {
            return Location.CompareFilePath(other.Location);
        }

        public int CompareTo(string other)
        {
            return Location.CompareFilePath(other);
        }

        public int CompareTo(IProjectItem other)
        {
            return Name.CompareTo(other.Name);
        }

        public void RegisterFile(ISourceFile file)
        {
            SourceFiles.InsertSorted(file);
        }

        public void RegisterFolder(IProjectFolder folder)
        {
            Folders.InsertSorted(folder);
        }

        public void UnregisterFile(ISourceFile file)
        {
            SourceFiles.Remove(file);
        }

        public void UnregisterFolder(IProjectFolder folder)
        {
            Folders.Remove(folder);
        }
    }
}