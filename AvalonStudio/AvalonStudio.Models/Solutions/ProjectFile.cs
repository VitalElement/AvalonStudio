
//using NClang;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.IO;
using System.Xml.Serialization;
using AvalonStudio.Utils;
using NClang;
using AvalonStudio.Projects;

namespace AvalonStudio.Models.Solutions
{
    public enum CStandard
    {
        [Description("")]
        None,
        [Description("c99")]
        C99,
        [Description("c11")]
        C11,
        [Description("c++11")]
        CPP11,
        [Description("c++14")]
        CPP14,
        [Description("c++1z")]
        CPP17
    }

    public enum FileType
    {
        Asm,
        C,
        CPlusPlus,
        ObjectiveC,
        ObjectiveCPlusPlus,
        Header,
        File,
        Inherit
    }

    public class ProjectFile : ProjectItem, ISourceFile
    {
        #region Constructors
        private ProjectFile()
            : base(null, null, null)
        {
            this.TranslationUnitIsDirty = true;
        }

        public ProjectFile(Solution solution, Project project, ProjectFolder container, string location, FileType language = FileType.Inherit, CStandard standard = CStandard.None)
            : base(solution, project, container)
        {
            this.UserData = new ProjectItemUserData(Guid.NewGuid());
            this.LocationRelativeToParent = container.CurrentDirectory.MakeRelativePath(location);
            this.TranslationUnitIsDirty = true;           
        }

        ~ProjectFile()
        {
        }
        #endregion

        public void MoveTo (ProjectFolder destination)
        {
            var destinationPath = Path.Combine(destination.CurrentDirectory, FileName);
            System.IO.File.Move(Location, destinationPath);

            Container.RemoveItem(this);            
            
            LocationRelativeToParent = destination.CurrentDirectory.MakeRelativePath(destinationPath);
            //CleanTranslationUnit();

            Container = destination;
            destination.AddChild(this);
        }

        public override string Title
        {
            get
            {
                return FileName;
            }
        }

        #region Properties       
        public bool IsHeaderFile
        {
            get
            {
                return Path.GetExtension(Location) == ".h";
            }
        }

        public bool IsCodeFile
        {
            get
            {
                bool result = false;

                switch (Path.GetExtension(FileName))
                {
                    case ".c":
                    case ".cpp":
                    case ".h":
                    case ".hpp":
                        result = true;
                        break;
                }

                return result;
            }
        }

        [XmlIgnore]
        public bool TranslationUnitIsDirty { get; set; }

        // Quickly reparse a file after it has been saved.
        public void QuickReparse()
        {
            GenerateTranslationUnit();
        }

        // Fully reparse file i.e. if compiler settings such as incldes or defines change. This does not mean in file includes.
        public void FullReparse()
        {
            CleanTranslationUnit();
            GenerateTranslationUnit();
        }

        public FileType FileType
        {
            get
            {
                FileType result = FileType.C;

                switch (Path.GetExtension(FileName))
                {
                    case ".c":
                        result = FileType.C;
                        break;

                    case ".cpp":
                        result = FileType.CPlusPlus;
                        break;

                    case ".h":
                        result = FileType.Header;
                        break;
                }

                return result;
            }
        }

        public Project DefaultProject
        {
            get
            {
                if (Project is CatchTestProject)
                {
                    return Project;
                }
                else
                {
                    return Solution.DefaultProject;
                }
            }
        }


        private NClang.ClangTranslationUnit GenerateTranslationUnit()
        {
            var DefaultProject = Solution.DefaultProject;

            NClang.ClangTranslationUnit result = null;

            if (TranslationUnitActive)
            {
                if (System.IO.File.Exists(this.Location) && this.IsCodeFile)
                {
                    var args = new List<string>();

                    var arguments = this.DefaultProject.IncludeArguments;

                    foreach (string argument in arguments)
                    {
                        args.Add(argument.Replace("\"", ""));
                    }

                    foreach (var define in this.DefaultProject.SelectedConfiguration.Defines)
                    {
                        if (define != string.Empty)
                        {
                            args.Add(string.Format("-D{0}", define));
                        }
                    }

                    if (this.FileType == Solutions.FileType.CPlusPlus || this.FileType == FileType.Header)
                    {
                        args.Add("-xc++");
                        args.Add("-std=c++14");
                    }

                    if (VEStudioSettings.This.ShowAllWarnings)
                    {
                        args.Add("-Weverything");
                    }

                    if (translationUnit == null || TranslationUnitIsDirty)
                    {
                        if (this.translationUnit != null)
                        {
                            this.translationUnit.Reparse(Project.UnsavedFiles.ToArray(), this.translationUnit.DefaultReparseOptions);

                            result = this.translationUnit;
                        }
                        else
                        {
                            result = this.Solution.NClangIndex.ParseTranslationUnit(this.Location, args.ToArray(), Project.UnsavedFiles.ToArray(), TranslationUnitFlags.CacheCompletionResults | TranslationUnitFlags.PrecompiledPreamble);
                        }
                    }

                    TranslationUnitIsDirty = false;
                }
            }

            return result;
        }

        [XmlIgnore]
        public List<NClang.ClangDiagnostic> Diagnostics
        {
            get
            {
                var results = new List<NClang.ClangDiagnostic>();

                if (IsCodeFile)
                {
                    if (TranslationUnit != null)
                    {
                        results.AddRange(TranslationUnit.DiagnosticSet.Items);
                    }
                }

                return results;
            }
        }

        [XmlIgnore]
        public NClang.ClangTranslationUnit TranslationUnit
        {
            get
            {
                if (TranslationUnitIsDirty || this.translationUnit == null)
                {
                    this.translationUnit = GenerateTranslationUnit();
                }

                return translationUnit;
            }
        }

        private bool translationUnitActive;
        [XmlIgnore]
        public bool TranslationUnitActive
        {
            get { return translationUnitActive; }
            set { translationUnitActive = value; }
        }

        public void CleanTranslationUnit()
        {
            if (translationUnit != null)
            {
                this.translationUnit.Dispose();
                this.translationUnit = null;
            }
        }

        public override string CurrentDirectory
        {
            get
            {
                return Path.GetDirectoryName(base.CurrentDirectory);
            }
        }

        [XmlIgnore]
        public override string FileName
        {
            get { { return Path.GetFileName(this.LocationRelativeToParent); } }
            set
            {
                var newLocation = Path.Combine(CurrentDirectory, value);

                if (newLocation.NormalizePath() != this.Location.NormalizePath())
                {
                    System.IO.File.Move(this.Location, newLocation);
                    this.LocationRelativeToParent = Container.CurrentDirectory.MakeRelativePath(newLocation);
                    this.SaveChanges();

                    Container.Children.Remove(this);
                    Container.AddChild(this);
                }
            }
        }

        public string File
        {
            get
            {
                return Location;
            }
        }

        public Language Language
        {
            get
            {
                switch(System.IO.Path.GetExtension(File))
                {
                    case ".cpp":
                        return Language.Cpp;

                    case ".c":
                        return Language.C;

                    case ".h":
                        return Language.C;

                    case ".hpp":
                        return Language.Cpp;

                    default:
                        throw new Exception("Unknown extension.");
                        
                }
            }
        }

        IProject ISourceFile.Project
        {
            get
            {
                return Project as IProject; 
            }
        }
        #endregion

        #region Private Members
        private NClang.ClangTranslationUnit translationUnit;
        #endregion
    }

    public abstract class IndexType
    {
        public int Line { get; set; }
        public int Column { get; set; }
        public string TypeName { get; set; }
    }

    public class GeneralIndexType : IndexType
    {

    }

    public class EnumConstantIndexType : IndexType
    {

    }

    public class CodeIndex
    {
        public CodeIndex()
        {
            Items = new List<Declaration>();
            HeaderTypes = new List<IndexType>();
            MainFileTypes = new List<IndexType>();
        }

        public List<Declaration> Items { get; set; }
        public List<IndexType> HeaderTypes { get; set; }
        public List<IndexType> MainFileTypes { get; set; }
    }
}
