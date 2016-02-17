namespace AvalonStudio.Projects.VEBuild
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

    public class CPlusPlusProject : SerializedObject<CPlusPlusProject>, IStandardProject
    {
        public const string solutionExtension = "vsln";
        public const string ProjectExtension = "vproj";

        public static string GenerateProjectFileName(string name)
        {
            return string.Format("{0}.{1}", name, ProjectExtension);
        }

        [JsonIgnore]
        public bool IsBuilding { get; set; }

        public static void PopulateFiles(IProject project, StandardProjectFolder folder)
        {
            var files = Directory.GetFiles(folder.Path);

            foreach (var file in files)
            {
                folder.Items.Add(new SourceFile() { Project = project, File = file });
            }
        }

        public static StandardProjectFolder GetSubFolders(IProject project, string path)
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

        public static CPlusPlusProject Load(string filename, ISolution solution)
        {
            var project = Deserialize(filename);

            project.Location = filename;
            project.SetSolution(solution);

            var folders = GetSubFolders(project, project.CurrentDirectory);

            project.Items = new List<IProjectItem>();

            foreach (var item in folders.Items)
            {
                project.Items.Add(item);
            }

            foreach (var file in project.SourceFiles)
            {
                (file as SourceFile)?.SetProject(project);

                var pathStructure = file.Location.Split(Path.DirectorySeparatorChar);


                foreach (var part in pathStructure)
                {

                }
            }

            return project;
        }



        public static CPlusPlusProject Create(ISolution solution, string directory, string name)
        {
            CPlusPlusProject result = null;

            var projectFile = Path.Combine(directory, CPlusPlusProject.GenerateProjectFileName(name));

            if (!File.Exists(projectFile))
            {
                var project = new CPlusPlusProject { Name = name };
                project.SetSolution(solution);
                project.Location = projectFile;
                project.Save();

                result = project;
            }

            return result;
        }

        public void Save()
        {
            Serialize(this.Location);
        }

        [JsonConstructor]
        public CPlusPlusProject(List<SourceFile> sourceFiles) : this()
        {
            if (sourceFiles != null)
            {
                SourceFiles = sourceFiles.Cast<ISourceFile>().ToList();
            }
        }

        public CPlusPlusProject()
        {
            Items = new List<IProjectItem>();
            UnloadedReferences = new List<Reference>();
            StaticLibraries = new List<string>();
            References = new List<IProject>();
            PublicIncludes = new List<string>();
            GlobalIncludes = new List<string>();
            Includes = new List<string>();
            SourceFiles = new List<ISourceFile>();
            CompilerArguments = new List<string>();
            ToolChainArguments = new List<string>();
            LinkerArguments = new List<string>();
            CCompilerArguments = new List<string>();
            CppCompilerArguments = new List<string>();
            BuiltinLibraries = new List<string>();
            Defines = new List<string>();
            ToolchainSettings = new ExpandoObject();
        }

        private static Dictionary<string, Tuple<string, string>> passwordCache = new Dictionary<string, Tuple<string, string>>();

        /// <summary>
        /// Resolves each reference, cloning and updating Git referenced projects where possible.
        /// </summary>
        public void ResolveReferences(IConsole console)
        {
            foreach (var reference in UnloadedReferences)
            {
                var referenceDirectory = Path.Combine(SolutionDirectory, reference.Name);

                //if (!string.IsNullOrEmpty(reference.GitUrl))
                //{
                //    if (!Directory.Exists(referenceDirectory))
                //    {
                //        var options = new CloneOptions();
                //        options.OnProgress = (serveroutput) =>
                //        {
                //            console?.OverWrite(serveroutput);
                //            return true;
                //        };

                //        options.OnTransferProgress = (progress) =>
                //        {
                //            console?.OverWrite(string.Format("{0} / {1} objects, {2} bytes transferred", progress.ReceivedObjects, progress.TotalObjects, progress.ReceivedBytes));
                //            return true;
                //        };

                //        options.CredentialsProvider = (url, user, cred) =>
                //        {
                //            var domain = new Uri(url).GetLeftPart(UriPartial.Authority);
                //            var credentials = new UsernamePasswordCredentials();

                //            Tuple<string, string> userNamePassword;

                //            if (passwordCache.TryGetValue(domain, out userNamePassword))
                //            {
                //                credentials.Username = userNamePassword.Item1;
                //                credentials.Password = userNamePassword.Item2;
                //            }
                //            else
                //            {
                //                Console.WriteLine("Credentials required for: " + url);

                //                Console.WriteLine("Please enter your username: ");
                //                credentials.Username = Console.ReadLine();

                //                string pass = "";
                //                Console.WriteLine("Please enter your password:");

                //                ConsoleKeyInfo key;

                //                do
                //                {
                //                    key = Console.ReadKey(true);

                //                    if (key.Key != ConsoleKey.Backspace && key.Key != ConsoleKey.Enter)
                //                    {
                //                        pass += key.KeyChar;
                //                        Console.Write("*");
                //                    }
                //                    else
                //                    {
                //                        if (key.Key == ConsoleKey.Backspace && pass.Length > 0)
                //                        {
                //                            pass = pass.Substring(0, (pass.Length - 1));
                //                            Console.Write("\b \b");
                //                        }
                //                    }
                //                }
                //                // Stops Receving Keys Once Enter is Pressed
                //                while (key.Key != ConsoleKey.Enter);

                //                credentials.Password = pass;

                //                passwordCache.Add(domain, new Tuple<string, string>(credentials.Username, credentials.Password));
                //            }

                //            return credentials;
                //        };

                //        console?.WriteLine();
                //        console?.WriteLine(string.Format("Cloning Reference {0}", reference.Name));

                //        options.Checkout = false;

                //        Repository.Clone(reference.GitUrl, referenceDirectory, options);
                //    }

                //    if (Repository.IsValid(referenceDirectory))
                //    {
                //        var repo = new Repository(referenceDirectory);

                //        string checkout = "HEAD";

                //        if (!string.IsNullOrEmpty(reference.Revision))
                //        {
                //            checkout = reference.Revision;
                //        }

                //        repo.Checkout(checkout);
                //    }
                //}

                var projectFile = Path.Combine(referenceDirectory, reference.Name + "." + ProjectExtension);

                if (File.Exists(projectFile))
                {
                    // Here it would be better to find the existing project first, then load.
                    var project = CPlusPlusProject.Load(projectFile, Solution);

                    project = Solution.AddProject(project) as CPlusPlusProject;
                    // This is done to make sure there is only ever 1 instance of the project.

                    var currentReference = References.Where((p) => p.Name == project.Name).FirstOrDefault();

                    if (currentReference == null)
                    {
                        References.Add(project);
                    }

                    project.ResolveReferences(console);
                }
            }
        }

        public void ResolveReferences()
        {
            ResolveReferences(null);
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

            foreach (var reference in UnloadedReferences)
            {
                var loadedReference = GetReference(reference);

                result.AddRange(loadedReference.GenerateReferencedIncludes());
            }

            foreach (var includePath in PublicIncludes)
            {
                result.Add(Path.Combine(CurrentDirectory, includePath));
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
                return Path.GetDirectoryName(Location);
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
            return References.Count > 0;
        }

        [JsonProperty(PropertyName = "References")]
        public List<Reference> UnloadedReferences { get; set; }

        [JsonIgnore]
        public IList<IProject> References
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

        public IList<string> Includes { get; private set; }

        public bool ShouldSerializeDefines()
        {
            return Defines.Count > 0;
        }

        public IList<string> Defines { get; private set; }

        public bool ShouldSerializeSourceFiles()
        {
            return SourceFiles.Count > 0;
        }

        public IList<ISourceFile> SourceFiles { get; }

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

        static LlilumToolchain GetLLilumToolchain()
        {
            var gccSettings = new ToolchainSettings();
            gccSettings.ToolChainLocation = @"C:\VEStudio\AppData\Repos\AvalonStudio.Toolchains.Llilum";
            gccSettings.IncludePaths.Add("GCC\\arm-none-eabi\\include\\c++\\4.9.3");
            gccSettings.IncludePaths.Add("GCC\\arm-none-eabi\\include\\c++\\4.9.3\\arm-none-eabi\\thumb");
            gccSettings.IncludePaths.Add("GCC\\lib\\gcc\\arm-none-eabi\\4.9.3\\include");

            return new LlilumToolchain(gccSettings);
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
    }
}
