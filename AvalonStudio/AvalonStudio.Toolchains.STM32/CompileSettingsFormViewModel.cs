namespace AvalonStudio.Toolchains.STM32
{
    using AvalonStudio.MVVM;
    using AvalonStudio.Projects;
    using AvalonStudio.Utils;
    using Extensibility.Utils;
    using ReactiveUI;
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Dynamic;
    using System.Linq;
    using System.Windows.Input;

    public class CompileSettingsViewModel : ViewModel<IProject>
    {
        private CompileSettings settings = new CompileSettings();
        public CompileSettingsViewModel(IProject project) : base (project)
        {
            //var config = project.SelectedConfiguration;
            //cppSupport = config.CppSupport;
            //miscOptions = config.MiscCompilerArguments;
            //includePaths = new ObservableCollection<string>(config.IncludePaths);            
            try
            {
                if (Model.ToolchainSettings.STM32ToolchainSettings.CompileSettings is ExpandoObject)
                {
                    settings = (Model.ToolchainSettings.STM32ToolchainSettings.CompileSettings as ExpandoObject).GetConcreteType<CompileSettings>();
                }
                else
                {
                    settings = Model.ToolchainSettings.STM32ToolchainSettings.CompileSettings;
                }
            }
            catch (Exception e)
            {
                Model.ToolchainSettings.STM32ToolchainSettings = new STM32ToolchainSettings();
            }
           

            defines = new ObservableCollection<string>(settings.Defines.ToList());
            

            //optimizationLevelSelectedIndex = (int)config.Optimization;
            //optimizationPreferenceSelectedIndex = (int)config.OptimizationPreference;
            //fpuSelectedIndex = (int)config.Fpu;
            //debugSymbols = config.DebugSymbols;
            //rtti = config.Rtti;
            //exceptions = config.Exceptions;

            // this.project = project;
            AddDefineCommand = ReactiveCommand.Create();// new RoutingCommand(AddDefine, (o) => DefineText != string.Empty && DefineText != null && !Defines.Contains(DefineText));
            AddDefineCommand.Subscribe(AddDefine);

            RemoveDefineCommand = ReactiveCommand.Create();// new RoutingCommand(RemoveDefine, (o) => SelectedDefine != string.Empty && SelectedDefine != null);
            RemoveDefineCommand.Subscribe(RemoveDefine);
            //AddIncludePathCommand = new RoutingCommand(AddIncludePath);
            //RemoveIncludePathCommand = new RoutingCommand(RemoveIncludePath, RemoveIncludePathCanExecute);

            UpdateCompileString();
        }        

        public void UpdateCompileString()
        {
            Save();

            
            //if (project.SelectedConfiguration.ToolChain is StandardToolChain)
            //{
            //    var cTc = project.SelectedConfiguration.ToolChain as StandardToolChain;

            //    CompilerArguments = cTc.GetCompilerArguments(project, FileType.CPlusPlus);
            //}
        }

        private void AddDefine(object param)
        {
            Defines.Add(DefineText);
            DefineText = string.Empty;
            UpdateCompileString();
        }

        private void RemoveDefine(object param)
        {
            Defines.Remove(SelectedDefine);
            UpdateCompileString();
        }

        private void AddIncludePath(object param)
        {
            //FolderBrowserDialog fbd = new FolderBrowserDialog();

            //fbd.SelectedPath = project.CurrentDirectory;

            //if (fbd.ShowDialog() == DialogResult.OK)
            //{
            //    string newInclude = project.CurrentDirectory.MakeRelativePath(fbd.SelectedPath);

            //    if (newInclude == string.Empty)
            //    {
            //        newInclude = "\\";
            //    }

            //    IncludePaths.Add(newInclude);
            //}

            UpdateCompileString();
        }

        private bool AddIncludePathCanExecute(object param)
        {
            return true;
        }

        private void RemoveIncludePath(object param)
        {
            includePaths.Remove(SelectedInclude);
            UpdateCompileString();
        }

        private bool RemoveIncludePathCanExecute(object param)
        {
            return true;
        }

        public void Save()
        {
            settings.Defines = defines.ToList();
            base.Model.ToolchainSettings.STM32ToolchainSettings.CompileSettings = settings;
            //base.Model.CompilerSettings.Defines = defines.ToList();
            //var config = project.SelectedConfiguration;

            //config.CppSupport = cppSupport;
            //config.MiscCompilerArguments = miscOptions;
            //config.Defines = defines.ToList();
            //config.IncludePaths = includePaths.ToList();
            //config.Optimization = (OptimizationLevel)optimizationLevelSelectedIndex;
            //config.OptimizationPreference = (OptimizationPreference)optimizationPreferenceSelectedIndex;
            //config.Fpu = (FPUSupport)fpuSelectedIndex;
            //config.DebugSymbols = debugSymbols;
            //config.Exceptions = exceptions;
            //config.Rtti = rtti;
            Model.Save();
            //project.SaveChanges();
        }

        public ReactiveCommand<object> AddIncludePathCommand { get; private set; }
        public ReactiveCommand<object> RemoveIncludePathCommand { get; private set; }
        public ReactiveCommand<object> AddDefineCommand { get; private set; }
        public ReactiveCommand<object> RemoveDefineCommand { get; private set; }

        public string[] FpuOptions
        {
            get
            {
                return null;
                //throw new NotImplementedException();
                //return Enum.GetNames(typeof(FPUSupport));
            }
        }

        private int fpuSelectedIndex;
        public int FpuSelectedIndex
        {
            get { return fpuSelectedIndex; }
            set
            {
                fpuSelectedIndex = value; //OnPropertyChanged();

                //Workspace.This.BeginDispatchUi(() =>
                //{
                //    switch ((FPUSupport)value)
                //    {
                //        case FPUSupport.Soft:
                //        case FPUSupport.Hard:
                //            Defines.Remove("__FPU_USED");
                //            Defines.Add("__FPU_USED");
                //            break;

                //        default:
                //            Defines.Remove("__FPU_USED");
                //            break;
                //    }

                //    UpdateCompileString();
                //});
            }
        }


        public string[] OptimizationPreferenceOptions
        {
            get
            {
                return null;
                //throw new NotImplementedException();
                //return Enum.GetNames(typeof(OptimizationPreference));
            }
        }

        private int optimizationPreferenceSelectedIndex;
        public int OptimizationPreferenceSelectedIndex
        {
            get { return optimizationPreferenceSelectedIndex; }
            set
            {
                optimizationPreferenceSelectedIndex = value;
                //OnPropertyChanged(); 
                UpdateCompileString();
            }
        }


        public string[] OptimizationLevelOptions
        {
            get
            {
                return null;
                //throw new NotImplementedException();
                //return Enum.GetNames(typeof(OptimizationLevel));
            }
        }


        private int optimizationLevelSelectedIndex;
        public int OptimizationLevelSelectedIndex
        {
            get { return optimizationLevelSelectedIndex; }
            set
            {
                optimizationLevelSelectedIndex = value;
                //OnPropertyChanged(); 
                UpdateCompileString();
            }
        }

        private bool cppSupport;
        public bool CppSupport
        {
            get { return cppSupport; }
            set
            {
                cppSupport = value;
                //OnPropertyChanged();
                if (value)
                {
                    Defines.Add("SUPPORT_CPLUSPLUS");
                }
                else
                {
                    Defines.Remove("SUPPORT_CPLUSPLUS");
                }

                UpdateCompileString();
            }
        }

        private bool debugSymbols;
        public bool DebugSymbols
        {
            get { return debugSymbols; }
            set
            {
                debugSymbols = value;
                //OnPropertyChanged();
                UpdateCompileString();
            }
        }

        private bool rtti;
        public bool Rtti
        {
            get { return rtti; }
            set
            {
                rtti = value;
                //OnPropertyChanged(); 
                UpdateCompileString();
            }
        }

        private void OnPropertyChanged()
        {
            // TODO Remove this.
        }

        private bool exceptions;
        public bool Exceptions
        {
            get { return exceptions; }
            set { exceptions = value; OnPropertyChanged(); UpdateCompileString(); }
        }

        private string miscOptions;
        public string MiscOptions
        {
            get { return miscOptions; }
            set { miscOptions = value; OnPropertyChanged(); UpdateCompileString(); }
        }

        private string compilerArguments;
        public string CompilerArguments
        {
            get { return compilerArguments; }
            set { compilerArguments = value; OnPropertyChanged(); }
        }


        private string defineText;
        public string DefineText
        {
            get { return defineText; }
            set { defineText = value; OnPropertyChanged(); }
        }


        private ObservableCollection<string> defines;
        public ObservableCollection<string> Defines
        {
            get { return defines; }
            set { defines = value; OnPropertyChanged(); UpdateCompileString(); }
        }

        private ObservableCollection<string> includePaths;
        public ObservableCollection<string> IncludePaths
        {
            get { return includePaths; }
            set { includePaths = value; OnPropertyChanged(); }
        }

        private string selectedInclude;
        public string SelectedInclude
        {
            get { return selectedInclude; }
            set { selectedInclude = value; OnPropertyChanged(); }
        }

        private string selectedDefine;
        public string SelectedDefine
        {
            get { return selectedDefine; }
            set { selectedDefine = value; DefineText = value; OnPropertyChanged(); }
        }

        //new private ToolChainSettings model;
        //public ToolChainSettings Model
        //{
        //    get { return model; }
        //    set { model = value; OnPropertyChanged(); }
        //}
    }
}
