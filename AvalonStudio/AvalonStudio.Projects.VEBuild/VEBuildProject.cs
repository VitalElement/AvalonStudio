namespace AvalonStudio.Projects.VEBuild
{
    using AvalonStudio.Projects.Standard;
    using AvalonStudio.Utils;
    using LibGit2Sharp;
    using Newtonsoft.Json;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using Toolchains;

    public class VEBuildProject : SerializedObject<VEBuildProject>, IStandardProject
    {
        public const string solutionExtension = "vsln";
        public const string ProjectExtension = "vproj";

        public static string GenerateProjectFileName(string name)
        {
            return string.Format("{0}.{1}", name, ProjectExtension);
        }

        [JsonIgnore]
        public bool IsBuilding { get; set; }

        public static VEBuildProject Load(string filename, ISolution solution)
        {
            var project = Deserialize(filename);

            project.Location = filename;
            project.SetSolution(solution);

            foreach (var file in project.SourceFiles)
            {
                (file as SourceFile)?.SetProject(project);
            }

            return project;
        }

        public static VEBuildProject Create(string directory, string name)
        {
            VEBuildProject result = null;

            var projectFile = Path.Combine(directory, VEBuildProject.GenerateProjectFileName(name));

            if (!File.Exists(projectFile))
            {
                var project = new VEBuildProject { Name = name };
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
        public VEBuildProject(List<SourceFile> sourceFiles) : this()
        {
            SourceFiles = sourceFiles.Cast<ISourceFile>().ToList();
        }

        public VEBuildProject()
        {
            //Languages = new List<Language>();
            UnloadedReferences = new List<Reference>();
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

                if (!string.IsNullOrEmpty(reference.GitUrl))
                {
                    if (!Directory.Exists(referenceDirectory))
                    {
                        var options = new CloneOptions();
                        options.OnProgress = (serveroutput) =>
                        {
                            console?.OverWrite(serveroutput);
                            return true;
                        };

                        options.OnTransferProgress = (progress) =>
                        {
                            console?.OverWrite(string.Format("{0} / {1} objects, {2} bytes transferred", progress.ReceivedObjects, progress.TotalObjects, progress.ReceivedBytes));
                            return true;
                        };

                        options.CredentialsProvider = (url, user, cred) =>
                        {
                            var domain = new Uri(url).GetLeftPart(UriPartial.Authority);
                            var credentials = new UsernamePasswordCredentials();

                            Tuple<string, string> userNamePassword;

                            if (passwordCache.TryGetValue(domain, out userNamePassword))
                            {
                                credentials.Username = userNamePassword.Item1;
                                credentials.Password = userNamePassword.Item2;
                            }
                            else
                            {
                                Console.WriteLine("Credentials required for: " + url);

                                Console.WriteLine("Please enter your username: ");
                                credentials.Username = Console.ReadLine();

                                string pass = "";
                                Console.WriteLine("Please enter your password:");

                                ConsoleKeyInfo key;

                                do
                                {
                                    key = Console.ReadKey(true);

                                    if (key.Key != ConsoleKey.Backspace && key.Key != ConsoleKey.Enter)
                                    {
                                        pass += key.KeyChar;
                                        Console.Write("*");
                                    }
                                    else
                                    {
                                        if (key.Key == ConsoleKey.Backspace && pass.Length > 0)
                                        {
                                            pass = pass.Substring(0, (pass.Length - 1));
                                            Console.Write("\b \b");
                                        }
                                    }
                                }
                                // Stops Receving Keys Once Enter is Pressed
                                while (key.Key != ConsoleKey.Enter);

                                credentials.Password = pass;

                                passwordCache.Add(domain, new Tuple<string, string>(credentials.Username, credentials.Password));
                            }

                            return credentials;
                        };

                        console?.WriteLine();
                        console?.WriteLine(string.Format("Cloning Reference {0}", reference.Name));

                        options.Checkout = false;

                        Repository.Clone(reference.GitUrl, referenceDirectory, options);
                    }

                    if (Repository.IsValid(referenceDirectory))
                    {
                        var repo = new Repository(referenceDirectory);

                        string checkout = "HEAD";

                        if (!string.IsNullOrEmpty(reference.Revision))
                        {
                            checkout = reference.Revision;
                        }

                        repo.Checkout(checkout);
                    }
                }

                var projectFile = Path.Combine(referenceDirectory, reference.Name + "." + ProjectExtension);

                if (File.Exists(projectFile))
                {
                    // Here it would be better to find the existing project first, then load.
                    var project = VEBuildProject.Load(projectFile, Solution);

                    project = Solution.AddProject(project) as VEBuildProject;
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
                var standardReference = reference as VEBuildProject;

                result.AddRange(standardReference.GenerateReferencedIncludes());
            }

            return result;
        }

        public IList<string> GetGlobalIncludes()
        {
            List<string> result = new List<string>();

            foreach (var reference in References)
            {
                var standardReference = reference as VEBuildProject;

                result.AddRange(standardReference.GetGlobalIncludes());
            }

            foreach (var include in GlobalIncludes)
            {
                result.Add(Path.Combine(CurrentDirectory, include));
            }

            return result;
        }

        [JsonIgnore]
        public ISolution Solution { get; private set; }

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

        public VEBuildProject GetReference(Reference reference)
        {
            VEBuildProject result = null;

            foreach (var project in Solution.Projects)
            {
                if (project.Name == reference.Name)
                {
                    result = project as VEBuildProject;
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

        //public bool ShouldSerializeLanguages ()
        //{
        //    return Languages.Count > 0;
        //}

        //public List<Language> Languages { get; set; }
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


        public bool ShouldSerializeGlobalIncludes ()
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

        public string BuildDirectory { get; set; }
        public string LinkerScript { get; set; }

        public string Executable
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public IToolChain ToolChain
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        [JsonIgnore]
        public IList<IProjectItem> Items
        {
            get; private set;
        }
    }
}
