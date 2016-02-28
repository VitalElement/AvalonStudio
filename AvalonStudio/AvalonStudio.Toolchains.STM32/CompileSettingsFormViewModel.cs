namespace AvalonStudio.Toolchains.STM32
{
    using AvalonStudio.MVVM;
    using AvalonStudio.Projects;
    using AvalonStudio.Utils;
    using Perspex.Controls;
    using Projects.Standard;
    using ReactiveUI;
    using Standard;
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Dynamic;
    using System.Linq;
    using System.Windows.Input;

    public class CompileSettingsViewModel : ViewModel<IProject>
    {
        private CompileSettings settings = new CompileSettings();
        public CompileSettingsViewModel(IProject project) : base(project)
        {
            try
            {
                settings = STM32GCCToolchain.GetSettings(project).CompileSettings;
            }
            catch (Exception e)
            {
                Model.ToolchainSettings.STM32ToolchainSettings = new STM32ToolchainSettings();
            }

            defines = new ObservableCollection<string>(settings.Defines);
            includePaths = new ObservableCollection<string>(settings.Includes);

            //var config = project.SelectedConfiguration;
            //cppSupport = config.CppSupport;
            miscOptions = settings.CustomFlags;
            //includePaths = new ObservableCollection<string>(config.IncludePaths);            


            optimizationLevelSelectedIndex = (int)settings.Optimization;
            optimizationPreferenceSelectedIndex = (int)settings.OptimizationPreference;
            fpuSelectedIndex = (int)settings.Fpu;
            debugSymbols = settings.DebugInformation;
            rtti = settings.Rtti;
            exceptions = settings.Exceptions;

            AddDefineCommand = ReactiveCommand.Create();// new RoutingCommand(AddDefine, (o) => DefineText != string.Empty && DefineText != null && !Defines.Contains(DefineText));
            AddDefineCommand.Subscribe(AddDefine);

            RemoveDefineCommand = ReactiveCommand.Create();// new RoutingCommand(RemoveDefine, (o) => SelectedDefine != string.Empty && SelectedDefine != null);
            RemoveDefineCommand.Subscribe(RemoveDefine);

            AddIncludePathCommand = ReactiveCommand.Create();
            AddIncludePathCommand.Subscribe(AddIncludePath);

            RemoveIncludePathCommand = ReactiveCommand.Create();
            RemoveIncludePathCommand.Subscribe(RemoveIncludePath);

            UpdateCompileString();
        }

        public void UpdateCompileString()
        {
            Save();

            if (Model.ToolChain != null && Model.ToolChain is StandardToolChain)
            {
                CompilerArguments = (Model.ToolChain as StandardToolChain).GetCompilerArguments(Model as IStandardProject, Model as IStandardProject, null);
            }
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

        private async void AddIncludePath(object param)
        {
            OpenFolderDialog fbd = new OpenFolderDialog();

            fbd.InitialDirectory = Model.CurrentDirectory;

            var result = await fbd.ShowAsync();

            if (result != string.Empty)
            {
                string newInclude = Model.CurrentDirectory.MakeRelativePath(result);

                if (newInclude == string.Empty)
                {
                    newInclude = "\\";
                }

                IncludePaths.Add(newInclude);
            }

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
            settings.CustomFlags = miscOptions;
            //base.Model.CompilerSettings.Defines = defines.ToList();
            //var config = project.SelectedConfiguration;

            //config.CppSupport = cppSupport;
            //config.MiscCompilerArguments = miscOptions;
            //config.Defines = defines.ToList();
            settings.Includes = includePaths.ToList();            
            settings.Optimization = (OptimizationLevel)optimizationLevelSelectedIndex;
            settings.OptimizationPreference = (OptimizationPreference)optimizationPreferenceSelectedIndex;
            settings.Fpu = (FPUSupport)fpuSelectedIndex;
            settings.DebugInformation = debugSymbols;
            settings.Exceptions = exceptions;
            settings.Rtti = rtti;

            Model.ToolchainSettings.STM32ToolchainSettings.CompileSettings = settings;
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
                return Enum.GetNames(typeof(FPUSupport));
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

                UpdateCompileString();
                //});
            }
        }


        public string[] OptimizationPreferenceOptions
        {
            get
            {
                return Enum.GetNames(typeof(OptimizationPreference));
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
                return Enum.GetNames(typeof(OptimizationLevel));
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
            set { this.RaiseAndSetIfChanged(ref compilerArguments, value); }
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
