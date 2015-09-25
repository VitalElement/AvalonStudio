
using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Reflection;
//using AvalonStudio.Models.ProjectTemplates;
using AvalonStudio.Models.Tools.Compiler;
namespace AvalonStudio.Models
{
    public class VEStudioService
    {
        public static void Initialise()
        {
            if (!Directory.Exists(DefaultAppDataFolder))
            {
                Directory.CreateDirectory(DefaultAppDataFolder);
            }

            if (!Directory.Exists(DefaultProjectFolder))
            {
                Directory.CreateDirectory(DefaultProjectFolder);
            }           
             
            if(!Directory.Exists(DefaultExtensionsFolder))
            {
                Directory.CreateDirectory(DefaultExtensionsFolder);
            }

            //// Enumerate extensions and install Project templates. (this could also work for toolchains)
            //var assemblies = Directory.EnumerateFiles(DefaultExtensionsFolder);

            //foreach(var assembly in assemblies)
            //{
            //    Console.WriteLine(assembly);

            //    var instance = Assembly.LoadFrom(assembly);

            //    var types = instance.GetTypes().ToList();

            //    var templates = types.Where(t => t.IsSubclassOf(typeof(ProjectTemplate)));

            //    foreach(var templateType in templates)
            //    {
            //        Templates.Add((ProjectTemplate)Activator.CreateInstance(templateType));
            //    }
            //}
        }

        public const string NewLine = "\r\n";
        public const string SolutionExtension = ".vesln";
        public const string ProjectExtension = ".veproj";
        public const string ProjectUserDataExtension = ".veuser";
        public const string DefaultAppDataFolder = @"c:\VEStudio";
        public const string SolutionPackagesFolderName = "Packages";
        public const string DefaultProjectFolder = DefaultAppDataFolder + "\\Projects";
        public const string DefaultExtensionsFolder = DefaultAppDataFolder + "\\Extensions";
        public const string LayoutFileName = "layout.xml";
        public const string SettingsFileName = "vestudio.xml";
        public const string RepoCatalogFolderName = "RepoCatalog";
        public const string RepoBaseFolderName = "Repos";
        public const string PackageIndexFileName = "Package.xml";
        public const string ProductName = "Code Thunder";

        public static string RepoBaseFolder
        {
            get
            {
                return Path.Combine(AppDataFolder, RepoBaseFolderName);
            }
        }

        public static string RepoCatalogFolder
        {
            get
            {
                return Path.Combine(AppDataFolder, RepoCatalogFolderName);
            }
        }

        public static string SettingsFile
        {
            get
            {
                return Path.Combine(AppDataFolder, SettingsFileName);
            }
        }

        public static string AppDataFolder
        {
            get
            {
                return @"c:\VEStudio\AppData\";
            }
        }

        public static ToolChain[] ToolChains = new ToolChain[]
        {
            new Arm(),
            new ClangToolChain (),
            new GCCToolChain (),
            new BitThunderToolChain(),
            new MinGWToolChain(),
            new CustomProcessToolChain(),
            new PicXC32ToolChain ()
        };

        //public static ObservableCollection<ProjectTemplate> Templates = new ObservableCollection<ProjectTemplate>()
        //{
        //    new BlankProjectTemplate(),        
        //    new CatchTestProjectTemplate()
        //};
    }
}
