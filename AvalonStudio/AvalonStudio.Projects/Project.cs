namespace AvalonStudio.Projects
{
    using Newtonsoft.Json;
    using Newtonsoft.Json.Converters;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using LibGit2Sharp;
    using Utils;
    [JsonConverter(typeof(StringEnumConverter))]
    public enum ProjectType
    {
        Executable,
        SharedLibrary,
        StaticLibrary,
        SuperProject
    }

    [JsonConverter(typeof(StringEnumConverter))]
    public enum Language
    {
        C,
        Cpp
    }

    public class Project : SerializedObject<Project>
    {
        public static string GenerateProjectFileName (string name)
        {
            return string.Format("{0}.{1}", name, Solution.projectExtension);
        }

        [JsonIgnore]
        public bool IsBuilding { get; set; }

        public static Project Load(string filename, Solution solution)
        {
            var project = Deserialize(filename);

            project.Location = filename;
            project.SetSolution(solution);

            foreach (var file in project.SourceFiles)
            {
                file.SetProject(project);
            }

            return project;
        }

        public static Project Create (string directory, string name)
        {
            Project result = null;
                       
            var projectFile = Path.Combine(directory, Project.GenerateProjectFileName(name));

            if (!File.Exists(projectFile))
            {
                var project = new Project { Name = name };
                project.Location = projectFile;
                project.Save();

                result = project;
            }

            return result;
        }

        public void Save ()
        {
            Serialize(this.Location);
        }

        public Project()
        {
            //Languages = new List<Language>();
            References = new List<Reference>();
            PublicIncludes = new List<string>();
            Includes = new List<string>();
            SourceFiles = new List<SourceFile>();
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
            foreach (var reference in References)
            {
                var referenceDirectory = Path.Combine(SolutionDirectory, reference.Name);

                if (!string.IsNullOrEmpty(reference.GitUrl))
                {
                    if (!Directory.Exists(referenceDirectory))
                    {
                        var options = new CloneOptions();
                        options.OnProgress = (serveroutput) =>
                        {
                            console.OverWrite(serveroutput);
                            return true;
                        };

                        options.OnTransferProgress = (progress) =>
                        {
                            console.OverWrite(string.Format("{0} / {1} objects, {2} bytes transferred", progress.ReceivedObjects, progress.TotalObjects, progress.ReceivedBytes));
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

                        console.WriteLine();
                        console.WriteLine(string.Format("Cloning Reference {0}", reference.Name));

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

                var projectFile = Path.Combine(referenceDirectory, reference.Name + "." + Solution.projectExtension);

                if (File.Exists(projectFile))
                {
                    var project = Project.Load(projectFile, Solution);
                    Solution.AddProject(project);

                    project.ResolveReferences(console);
                }
            }
        }

        public void SetSolution(Solution solution)
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

        protected List<string> GenerateReferenceIncludes()
        {
            List<string> result = new List<string>();

            foreach (var reference in References)
            {
                var loadedReference = GetReference(reference);

                result.AddRange(loadedReference.GenerateReferenceIncludes());
            }

            foreach (var includePath in PublicIncludes)
            {
                result.Add(Path.Combine(CurrentDirectory, includePath));
            }

            return result;
        }

        public List<string> GetReferencedIncludes()
        {
            List<string> result = new List<string>();

            foreach (var reference in References)
            {
                var loadedReference = GetReference(reference);

                result.AddRange(loadedReference.GenerateReferenceIncludes());
            }

            return result;
        }

        [JsonIgnore]
        public Solution Solution { get; private set; }

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

        public Project GetReference(Reference reference)
        {
            Project result = null;

            foreach (var project in Solution.Projects)
            {
                if (project.Name == reference.Name)
                {
                    result = project;
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

        public List<Reference> References { get; set; }

        public bool ShouldSerializePublicIncludes()
        {
            return PublicIncludes.Count > 0;
        }

        public List<string> PublicIncludes { get; set; }

        public bool ShouldSerializeIncludes()
        {
            return Includes.Count > 0;
        }

        public List<string> Includes { get; set; }

        public bool ShouldSerializeDefines()
        {
            return Defines.Count > 0;
        }

        public List<string> Defines { get; set; }

        public bool ShouldSerializeSourceFiles()
        {
            return SourceFiles.Count > 0;
        }
        public List<SourceFile> SourceFiles { get; set; }

        public bool ShouldSerializeCompilerArguments()
        {
            return CompilerArguments.Count > 0;
        }

        public List<string> CompilerArguments { get; set; }

        public bool ShouldSerializeCCompilerArguments()
        {
            return CCompilerArguments.Count > 0;
        }

        public List<string> CCompilerArguments { get; set; }

        public bool ShouldSerializeCppCompilerArguments()
        {
            return CppCompilerArguments.Count > 0;
        }

        public List<string> CppCompilerArguments { get; set; }

        public bool ShouldSerializeToolChainArguments()
        {
            return ToolChainArguments.Count > 0;
        }
        public List<string> ToolChainArguments { get; set; }

        public bool ShouldSerializeLinkerArguments()
        {
            return LinkerArguments.Count > 0;
        }

        public List<string> LinkerArguments { get; set; }

        public bool ShouldSerializeBuiltinLibraries()
        {
            return BuiltinLibraries.Count > 0;
        }
        public List<string> BuiltinLibraries { get; set; }

        public string BuildDirectory { get; set; }
        public string LinkerScript { get; set; }

    }
}
