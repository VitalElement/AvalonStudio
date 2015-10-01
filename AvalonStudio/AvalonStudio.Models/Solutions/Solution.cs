namespace AvalonStudio.Models.Solutions
{
    //using NClang;
    using System;
    using System.Collections.ObjectModel;
    using System.IO;
    using System.Linq;
    using System.Xml.Serialization;
    //using AvalonStudio.Models.Settings;

    [Serializable]
    public class Solution : SolutionFolder
    {
        public Solution () : base (null, null, string.Empty)
        {
            Solution = this;
            LoadedProjects = new ObservableCollection<Project>();
            UnloadedChildren = new ObservableCollection<Item>();                                   
           // NClangIndex = NClang.ClangService.CreateIndex ();
            //FormattingOptions = ClangFormatSettings.Default;            
        }

        //internal NClang.ClangIndex NClangIndex;

        [XmlIgnore]
        public string CurrentDirectory { get { return Path.GetDirectoryName (OpenedLocation) + "/"; } }

        [XmlIgnore]
        public string OpenedLocation { get; set; }

        public ProjectFile FindFile (string file)
        {
            ProjectFile result = null;

            foreach(var project in LoadedProjects)
            {
                result = project.FindFile (file);

                if(result != null)
                {
                    break;
                }
            }

            return result;
        }        

        public static Solution CreateSolution (string location, string name)
        {            
            Solution result = new Solution();

            if (Directory.Exists(location) && !File.Exists(Path.Combine(location, name)))
            {
                result.OpenedLocation = Path.Combine(location, name + AvalonStudioService.SolutionExtension);            

                result.SaveChanges();

                //Project project = Project.Create(result, result, name);

                //result.LoadProjects();
            }

            return result;
        }

        public static Solution LoadSolution (string location)
        {
            Solution result = null;

            XmlSerializer serializer = new XmlSerializer (typeof (Solution));

            StreamReader reader = new StreamReader (location);

            result = (Solution)serializer.Deserialize (reader);

            reader.Close ();

            result.OpenedLocation = location;

            result.LoadProjects();

            foreach(var project in result.LoadedProjects)
            {
				if (project != null)
				{
					project.LoadReferences ();
				}
				else
				{
					Console.WriteLine ("Corrupted project file?");
				}
            }                                                          

            return result;
        }        

        public void LoadProjects ()
        {
            LoadedProjects.Clear();
           
            foreach (var item in UnloadedChildren)
            {
                if (item is UnloadedProject)
                {
                    var project = Project.LoadProject(this, item as UnloadedProject);

                    if (project != null)
                    {
                        LoadedProjects.Add(project);
                    }
                }
                else if (item is SolutionFolder)
                {
                    var folder = item as SolutionFolder;

                    folder.Solution = this;

                    var parent = GetParent(item);

                    if (parent != null)
                    {
                        parent.Children.Add(item);
                    }
                }
            }

            foreach (var project in LoadedProjects)
            {
                var parent = GetParent(project);

                if (parent != null)
                {
                    parent.Children.Add(project);
                }
                else
                {
                    throw new Exception("Unable to find parent folder of Project, project file maybe corrupt.");
                }
            }
        }

        [XmlIgnore]
        public Project DefaultProject
        {
            get
            {
                if (LoadedProjects.Count > 0 && DefaultProjectIndex >= 0 && DefaultProjectIndex < LoadedProjects.Count)
                {
                    return (Project)LoadedProjects[DefaultProjectIndex];
                }
                else
                {
                    return null;
                }
            }

            set
            {
                DefaultProjectIndex = LoadedProjects.IndexOf(value);
            }
        }

        public int DefaultProjectIndex { get; set; }    
        
        public bool ContainsProject (Project project)
        {
            bool result = false;

            var existing = LoadedProjects.FirstOrDefault((p) => p.Location == project.Location);

            if(existing != null)
            {
                result = true;
            }

            return result;
        }            

        public string AddNewProject (string projectName)
        {
            string folderName = Path.GetDirectoryName (this.OpenedLocation) + "\\" + projectName;
            string projectLocation = folderName + "\\" + projectName + AvalonStudioService.ProjectExtension;

            if (!Directory.Exists (folderName))
            {
                Directory.CreateDirectory (folderName);
            }

            this.UnloadedChildren.Add (new UnloadedProject(this, projectLocation));

            this.SaveChanges ();

            return projectLocation;
        }

        public void SaveChanges ()
        {
            XmlSerializer serializer = new XmlSerializer (typeof (Solution));

            TextWriter textWriter = new StreamWriter (this.OpenedLocation);

            serializer.Serialize (textWriter, this);

            textWriter.Close ();

            foreach (Project project in LoadedProjects)
            {
                project.SerializeToXml ();
            }
        }

        public string FileName
        {
            get {  { return Path.GetFileNameWithoutExtension (this.OpenedLocation); } }
        }

        public ObservableCollection<Item> UnloadedChildren { get; set; }

        /// This is a flat list of all the loaded projects, where as children holds the structure of the solution.
        [XmlIgnore]
        public ObservableCollection<Project> LoadedProjects { get; set; }

        //public ClangFormatSettings FormattingOptions { get; set; }        
    }
}
