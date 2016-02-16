namespace AvalonStudio.Models.Solutions
{
    using Projects.Standard;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using System.Xml.Serialization;
    using VEUtils;
    using Projects;
    using Toolchains;
    using Extensibility.Menus;
    using Extensibility;
    using Perspex.Controls;

    [XmlInclude(typeof(BitThunderApplicationProject))]
    [XmlInclude(typeof(CatchTestProject))]
    [XmlInclude(typeof(HiddenProject))]
    public class Project : ProjectFolder, IStandardProject
    {
        public override bool ShouldSerializeId()
        {
            return false;
        }

        public override bool ShouldSerializeParentId()
        {
            return false;
        }

        #region Constructors
        protected Project ()
            : base (null, null, null)
        {
            this.LoadedReferences = new List<Project>();
            this.References = new List<string>();        
            this.UnsavedFiles = new List<NClang.ClangUnsavedFile> ();
            this.Configurations = new List<ProjectConfiguration>();                        
        }

        public Project (Solution solution, Item container)
            : base (solution, null, container)
        {            
            this.Project = this;
            this.LoadedReferences = new List<Project>();
            this.References = new List<string>();
            this.ExportedIncludes = new List<string>();            
            this.UnsavedFiles = new List<NClang.ClangUnsavedFile> ();
            this.Configurations = new List<ProjectConfiguration>();          
        }
        #endregion

        public UnloadedProject GetUnloadedProject()
        {
            UnloadedProject result = new UnloadedProject(Solution.GetParent(this));

            result.FileName = this.LocationRelativeToParent;
            result.Id = this.Id;

            return result;
        }

        #region Static Methods
        public static Project Create (Solution solution, SolutionFolder container, string name)
        {
            Project result = new Project (solution, container); 

            string newFolder = Path.Combine (solution.CurrentDirectory, name);

            if (!Directory.Exists (newFolder))
            {
                Directory.CreateDirectory (newFolder);
            }

            result.LocationRelativeToParent = Path.Combine (name, name + AvalonStudioService.ProjectExtension);

            result.Configurations.Add(new ProjectConfiguration() { Name = "Default" });

            result.SerializeToXml ();

            container.AddProject (result, result.GetUnloadedProject());            

            return result;
        }

        /// <summary>
        /// This loads just the project file, no user data or references and does not attach to a solution.
        /// </summary>
        /// <param name="filename"></param>
        /// <returns></returns>
        public static Project LoadProjectInIsolation (string filename)
        {
            Project result = null;

            try
            {
                var serializer = new XmlSerializer(typeof(Project));

                var reader = new StreamReader(filename);

                result = (Project)serializer.Deserialize(reader);                

                reader.Close();                                            
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }

            return result;
        }

        public static Project LoadProject (Solution solution, UnloadedProject unloadedProject)
        {
            Project result = null;

			Console.WriteLine (string.Format ("Loading: {0}", Path.Combine(solution.CurrentDirectory, unloadedProject.FileName)));

            try
            {
				string fullPath = Path.Combine(solution.CurrentDirectory, unloadedProject.FileName).ConvertPathForOS();
				result = LoadProjectInIsolation(fullPath);

                result.LocationRelativeToParent = solution.CurrentDirectory.MakeRelativePath(fullPath);                

                result.SetSolution (solution, result, null);

                result.Id = unloadedProject.Id;
                result.ParentId = unloadedProject.ParentId;
                result.UserData.Id = result.Id; // incase the userfile doesnt contain user data for the project.

                bool changed = false;

                if (result.Id == Guid.Empty)
                {
                    result.Id = Guid.NewGuid();

                    changed = true;
                }

                if (File.Exists(result.UserFileLocation))
                {
                    var serializer = new XmlSerializer(typeof(List<ProjectItemUserData>));

                    var reader = new StreamReader(result.UserFileLocation);

                    var userData = (List<ProjectItemUserData>)serializer.Deserialize(reader);

                    reader.Close();

                    result.ApplyUserData(userData);
                }
                else
                {
                    // create new user data...
                    result.VisitAllChildren((f) =>
                    {
                        f.UserData = new ProjectItemUserData(f.Id);
                        return false;
                    });

                    changed = true;
                }

                result.VisitAllChildren((f) =>
                {
                    changed = true;

                    if (f.Id == Guid.Empty)
                    {
                        f.Id = Guid.NewGuid();
                    }

                    return false;
                });

                result.VisitAllFolders((f) =>
                {
                    changed = true;

                    f.SortChildren();

                    return false;
                });

                result.SortChildren();
                changed = true;

                if (changed)
                {
                    result.SaveChanges();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine (e);
            }                     

            return result;
        }

        [XmlIgnore]
        public string Executable { get; set; }

        //[XmlIgnore]
        //public UnloadedProject UnloadedProject { get; set; }            

        public void ApplyUserData (List<ProjectItemUserData> userDatas)
        {
            foreach (var userData in userDatas)
            {
                VisitAllChildren((f) =>
                {
                    f.UserData.Id = f.Id;   // this will fix the link between user data and the file incase the link was corrupted.

                    if(f.Id == userData.Id)
                    {
                        f.UserData = userData;

                        return true;
                    }

                    return false;
                });
            }
        }

        public List<ProjectItemUserData> GetUserData ()
        {
            List<ProjectItemUserData> result = new List<ProjectItemUserData>();

            VisitAllChildren((f) =>
            {
                result.Add(f.UserData);

                return false;
            });

            return result;
        }

        public virtual void LoadReferences ()
        {
            this.LoadedReferences.Clear();

            foreach (string projectLocation in this.References)
            {
				var proj = this.Solution.LoadedProjects.FirstOrDefault((p) => this.Solution.CurrentDirectory.MakeRelativePath(p.Location) == projectLocation.ConvertPathForOS());

                if(proj != null)
                {
                    LoadedReferences.Add(proj);
                }   
                else
                {
                    Console.WriteLine("Project reference missing from solution.");
                }             
            }
        }
        #endregion

        #region Public Properties        
        public List<string> ExportedIncludes { get; set; }          

        public List<ProjectConfiguration> Configurations { get; set; }        

        //[XmlIgnore]
        //public virtual GDBDebugAdaptor SelectedDebugAdaptor
        //{
        //    get
        //    {
        //        if (UserData.SelectedDebugAdaptorIndex == -1 || VEStudioSettings.This.InstalledDebugAdaptors.Count == 0)
        //        {
        //            return new LocalDebugAdaptor();
        //        }

        //        return VEStudioSettings.This.InstalledDebugAdaptors [UserData.SelectedDebugAdaptorIndex];
        //    }
        //    set
        //    {
        //        UserData.SelectedDebugAdaptorIndex = VEStudioSettings.This.InstalledDebugAdaptors.IndexOf (value);
        //    }
        //}

        private List<string> GetReferenceIncludes(Project reference)
        {
            List<string> result = new List<string>();

            foreach (var child in reference.LoadedReferences)
            {
                result.AddRange(GetReferenceIncludes(child));
            }            

            foreach (var includePath in reference.ExportedIncludes)
            {
                result.Add(includePath);
            }

            return result;
        }

        [XmlIgnore]
        public string[] IncludeArguments
        {
            get
            {
                List<string> arguments = new List<string>();

                if (SelectedConfiguration.ToolChain != null && SelectedConfiguration.ToolChain.Settings != null)
                {
                    if (SelectedConfiguration.ToolChain.Settings.IncludePaths != null)
                    {
                        foreach (var include in SelectedConfiguration.ToolChain.Settings.IncludePaths)
                        {
                            string formatString = "-I\"{0}\"";
							string includeArgument = Path.Combine(SelectedConfiguration.ToolChain.Settings.ToolChainLocation, include.ConvertPathForOS());

                            arguments.Add(string.Format(formatString, includeArgument));
                        }
                    }
                }

                foreach (var reference in this.LoadedReferences)
                {
                    var includes = GetReferenceIncludes(reference);
                    
                    foreach (var includePath in includes)
                    {
                        string formatString = "-I\"{0}\"";
						string includeArgument = Path.Combine(this.Solution.CurrentDirectory, includePath.ConvertPathForOS());

                        if (includePath == "\\")
                        {
                            includeArgument = this.CurrentDirectory;
                        }

                        arguments.Add(string.Format(formatString, includeArgument));
                    }
                }

                foreach (string includePath in this.SelectedConfiguration.IncludePaths)
                {
                    string formatString = "-I\"{0}\"";
					string includeArgument = Path.Combine(this.CurrentDirectory, includePath.ConvertPathForOS());

                    if (includePath == "\\")
                    {
                        includeArgument = this.CurrentDirectory;
                    }

                    arguments.Add(string.Format(formatString, includeArgument));
                }

                return arguments.ToArray();
            }
        }

        

        [XmlIgnore]
        public ProjectConfiguration SelectedConfiguration
        {
            get
            {
                return Configurations[UserData.SelectedConfigurationIndex];
            }
        }      

        public override string CurrentDirectory
        {
            get
            {
                return Path.GetDirectoryName (this.Location) + "/";
            }
        }

        [XmlIgnore]
        public List<Project> LoadedReferences { get; private set; }

        public List<string> References { get; set; }

        public override string Location
        {
            get { return Path.Combine(this.Solution.CurrentDirectory, LocationRelativeToParent); }
        }

        public string UserFileLocation
        {
            get
            {
                return Path.Combine(this.Solution.CurrentDirectory, Path.Combine(Path.GetDirectoryName(LocationRelativeToParent), Path.GetFileNameWithoutExtension(LocationRelativeToParent) + AvalonStudioService.ProjectUserDataExtension));
            }
        }

        [XmlIgnore]
        public List<NClang.ClangUnsavedFile> UnsavedFiles { get; private set; }

        public ProjectType Type
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public bool IsBuilding
        {
            get; set;
        }

        public string BuildDirectory
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public string LinkerScript
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public IList<string> BuiltinLibraries
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public IList<string> ToolChainArguments
        {
            get
            {
                return new List<string>();
            }
        }

        public IList<string> LinkerArguments
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public IList<string> CompilerArguments
        {
            get
            {
                return new List<string>();
            }
        }

        public IList<string> CCompilerArguments
        {
            get
            {
                return new List<string>();
            }
        }

        public IList<string> CppCompilerArguments
        {
            get
            {
                return new List<string>();
            }
        }

        public IList<string> Defines
        {
            get
            {
                return this.SelectedConfiguration.Defines;
            }
        }

        public IList<string> PublicIncludes
        {
            get
            {
                return this.ExportedIncludes;                
            }
        }

        public IList<string> GlobalIncludes
        {
            get
            {
                return new List<string>();
            }
        }

        public IList<string> Includes
        {
            get
            {
                return this.SelectedConfiguration.IncludePaths;
            }
        }

        public string Name
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        ISolution IProject.Solution
        {
            get
            {
                return Solution;
            }            
        }

        IList<IProject> IProject.References
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public IList<ISourceFile> Items
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
                return SelectedConfiguration.ToolChain;
            }
        }

        IList<IProjectItem> IProjectFolder.Items
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public IList<ISourceFile> SourceFiles
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public IList<IMenuItem> ProjectMenuItems
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public IList<string> StaticLibraries
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public IList<ConfigPage> ConfigurationPages
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        IList<TabItem> IProject.ConfigurationPages
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        dynamic DebugSettings
        {
            get
            {
                throw new NotImplementedException();
            }
        }   

        dynamic IProject.DebugSettings
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public dynamic ToolchainSettings
        {
            get
            {
                throw new NotImplementedException();
            }
        }
        #endregion

        public static string NormalizePath (string path)
        {
            return Path.GetFullPath (new Uri (path).LocalPath)
                       .TrimEnd (Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar);
        }

        #region Public Methods
        public async Task<bool> Build (Tools.IConsole console, CancellationTokenSource cancellationSource)
        {		
            return await this.SelectedConfiguration.ToolChain.Build(console, this, cancellationSource);
        }

        public async Task Clean (Tools.IConsole console, CancellationTokenSource cancellationSource)
        {
            await this.SelectedConfiguration.ToolChain.Clean(console, this, cancellationSource);
        }

        public ProjectFile FindFile (string file)
        {
            ProjectFile result = null;

            if (file != null)
            {
                var folder = new DirectoryInfo (file);

                this.VisitAllFiles ((f) =>
                {
                    string fileLocation = NormalizePath(f.Location);
                    string otherLocation = NormalizePath(folder.FullName);
                    if (string.Compare(fileLocation, otherLocation, true) == 0)
                    {
                        result = f;
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                });
            }

            return result;
        }

        public void SerializeToXml ()
        {           
            try
            {
                var serializer = new XmlSerializer(typeof(Project));

                var textWriter = new StreamWriter(this.Location);

                serializer.Serialize(textWriter, this);

                textWriter.Close();

                serializer = new XmlSerializer(typeof(List<ProjectItemUserData>));
                textWriter = new StreamWriter(this.UserFileLocation);

                serializer.Serialize(textWriter, GetUserData());
                textWriter.Close();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        public string GetObjectDirectory(IStandardProject superProject)
        {
            throw new NotImplementedException();
        }

        public string GetBuildDirectory(IStandardProject superProject)
        {
            throw new NotImplementedException();
        }

        public string GetOutputDirectory(IStandardProject superProject)
        {
            throw new NotImplementedException();
        }

        public IList<string> GetReferencedIncludes()
        {
            List<string> result = new List<string>();
            foreach (var reference in this.LoadedReferences)
            {
                var includes = GetReferenceIncludes(reference);

                result.AddRange(includes);                
            }

            //List<string> arguments = new List<string>();

            //if (SelectedConfiguration.ToolChain != null && SelectedConfiguration.ToolChain.Settings != null)
            //{
            //    if (SelectedConfiguration.ToolChain.Settings.IncludePaths != null)
            //    {
            //        foreach (var include in SelectedConfiguration.ToolChain.Settings.IncludePaths)
            //        {
            //            string formatString = "-I\"{0}\"";
            //            string includeArgument = Path.Combine(SelectedConfiguration.ToolChain.Settings.ToolChainLocation, include.ConvertPathForOS());

            //            arguments.Add(string.Format(formatString, includeArgument));
            //        }
            //    }
            //}

            return result;
        }

        public IList<string> GetGlobalIncludes()
        {
            return new List<string>();
        }

        public void Save()
        {
            throw new NotImplementedException();
        }

        public void ResolveReferences()
        {
            throw new NotImplementedException();
        }
        #endregion
    }
}
