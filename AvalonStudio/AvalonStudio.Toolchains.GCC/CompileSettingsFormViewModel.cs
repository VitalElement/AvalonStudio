using Avalonia.Controls;
using AvalonStudio.Extensibility;
using AvalonStudio.MVVM;
using AvalonStudio.Projects;
using AvalonStudio.Projects.Standard;
using AvalonStudio.Toolchains.Standard;
using AvalonStudio.Utils;
using ReactiveUI;
using System;
using System.Collections.ObjectModel;
using System.Linq;

namespace AvalonStudio.Toolchains.GCC
{
    public class CompileSettingsFormViewModel : HeaderedViewModel<IProject>
    {
        private int clanguageStandardSelectedIndex;

        private string compilerArguments;

        private int cppLanguageStandardSelectedIndex;

        private bool cppSupport;

        private bool debugSymbols;

        private ObservableCollection<string> defines;

        private string defineText;

        private bool exceptions;

        private int fpuSelectedIndex;

        private ObservableCollection<string> includePaths;

        private string miscOptions;

        private int optimizationLevelSelectedIndex;

        private int optimizationPreferenceSelectedIndex;

        private bool rtti;

        private string selectedDefine;

        private string selectedInclude;
        private readonly CompileSettings settings = new CompileSettings();

        public CompileSettingsFormViewModel(IProject project) : base("Compiler", project)
        {
            settings = project.GetToolchainSettings<GccToolchainSettings>().CompileSettings;

            defines = new ObservableCollection<string>(settings.Defines);
            includePaths = new ObservableCollection<string>(settings.Includes);

            miscOptions = settings.CustomFlags;

            optimizationLevelSelectedIndex = (int)settings.Optimization;
            optimizationPreferenceSelectedIndex = (int)settings.OptimizationPreference;
            cppLanguageStandardSelectedIndex = (int)settings.CppLanguageStandard;
            clanguageStandardSelectedIndex = (int)settings.CLanguageStandard;
            fpuSelectedIndex = (int)settings.Fpu;
            debugSymbols = settings.DebugInformation;
            rtti = settings.Rtti;
            exceptions = settings.Exceptions;

            AddDefineCommand = ReactiveCommand.Create();
            // new RoutingCommand(AddDefine, (o) => DefineText != string.Empty && DefineText != null && !Defines.Contains(DefineText));
            AddDefineCommand.Subscribe(AddDefine);

            RemoveDefineCommand = ReactiveCommand.Create();
            // new RoutingCommand(RemoveDefine, (o) => SelectedDefine != string.Empty && SelectedDefine != null);
            RemoveDefineCommand.Subscribe(RemoveDefine);

            AddIncludePathCommand = ReactiveCommand.Create();
            AddIncludePathCommand.Subscribe(AddIncludePath);

            RemoveIncludePathCommand = ReactiveCommand.Create();
            RemoveIncludePathCommand.Subscribe(RemoveIncludePath);

            UpdateCompileString();
        }

        public ReactiveCommand<object> AddIncludePathCommand { get; }
        public ReactiveCommand<object> RemoveIncludePathCommand { get; }
        public ReactiveCommand<object> AddDefineCommand { get; }
        public ReactiveCommand<object> RemoveDefineCommand { get; }

        public string[] CLanguageStandards
        {
            get { return Enum.GetNames(typeof(CLanguageStandard)); }
        }

        public int CLanguageStandardSelectedIndex
        {
            get
            {
                return clanguageStandardSelectedIndex;
            }
            set
            {
                clanguageStandardSelectedIndex = value;
                UpdateCompileString();
            }
        }

        public string[] CppLanguageStandards
        {
            get { return Enum.GetNames(typeof(CppLanguageStandard)); }
        }

        public int CppLanguageStandardSelectedIndex
        {
            get
            {
                return cppLanguageStandardSelectedIndex;
            }
            set
            {
                cppLanguageStandardSelectedIndex = value;
                UpdateCompileString();
            }
        }

        public string[] FpuOptions
        {
            get { return Enum.GetNames(typeof(FPUSupport)); }
        }

        public int FpuSelectedIndex
        {
            get
            {
                return fpuSelectedIndex;
            }
            set
            {
                fpuSelectedIndex = value;

                UpdateCompileString();
            }
        }

        public string[] OptimizationPreferenceOptions
        {
            get { return Enum.GetNames(typeof(OptimizationPreference)); }
        }

        public int OptimizationPreferenceSelectedIndex
        {
            get
            {
                return optimizationPreferenceSelectedIndex;
            }
            set
            {
                optimizationPreferenceSelectedIndex = value;
                //OnPropertyChanged();
                UpdateCompileString();
            }
        }

        public string[] OptimizationLevelOptions => Enum.GetNames(typeof(OptimizationLevel));

        public int OptimizationLevelSelectedIndex
        {
            get
            {
                return optimizationLevelSelectedIndex;
            }
            set
            {
                optimizationLevelSelectedIndex = value;
                //OnPropertyChanged();
                UpdateCompileString();
            }
        }

        public bool CppSupport
        {
            get
            {
                return cppSupport;
            }
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

        public bool DebugSymbols
        {
            get
            {
                return debugSymbols;
            }
            set
            {
                debugSymbols = value;
                //OnPropertyChanged();
                UpdateCompileString();
            }
        }

        public bool Rtti
        {
            get
            {
                return rtti;
            }
            set
            {
                rtti = value;
                //OnPropertyChanged();
                UpdateCompileString();
            }
        }

        public bool Exceptions
        {
            get
            {
                return exceptions;
            }
            set
            {
                exceptions = value;
                OnPropertyChanged();
                UpdateCompileString();
            }
        }

        public string MiscOptions
        {
            get
            {
                return miscOptions;
            }
            set
            {
                miscOptions = value;
                OnPropertyChanged();
                UpdateCompileString();
            }
        }

        public string CompilerArguments
        {
            get { return compilerArguments; }
            set { this.RaiseAndSetIfChanged(ref compilerArguments, value); }
        }

        public string DefineText
        {
            get
            {
                return defineText;
            }
            set
            {
                defineText = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<string> Defines
        {
            get
            {
                return defines;
            }
            set
            {
                defines = value;
                OnPropertyChanged();
                UpdateCompileString();
            }
        }

        public ObservableCollection<string> IncludePaths
        {
            get
            {
                return includePaths;
            }
            set
            {
                includePaths = value;
                OnPropertyChanged();
            }
        }

        public string SelectedInclude
        {
            get
            {
                return selectedInclude;
            }
            set
            {
                selectedInclude = value;
                OnPropertyChanged();
            }
        }

        public string SelectedDefine
        {
            get
            {
                return selectedDefine;
            }
            set
            {
                selectedDefine = value;
                DefineText = value;
                OnPropertyChanged();
            }
        }

        public void UpdateCompileString()
        {
            Save();

            if (Model.ToolChain != null && Model.ToolChain is StandardToolChain)
            {
                CompilerArguments = (Model.ToolChain as StandardToolChain).GetCompilerArguments(Model as IStandardProject,
                    Model as IStandardProject, null);
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
            var fbd = new OpenFolderDialog();

            fbd.InitialDirectory = Model.CurrentDirectory;

            var result = await fbd.ShowAsync();

            if (result != string.Empty)
            {
                var newInclude = Model.CurrentDirectory.MakeRelativePath(result);

                if (newInclude == string.Empty)
                {
                    // TODO Platform specific?
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
            settings.CppLanguageStandard = (CppLanguageStandard)cppLanguageStandardSelectedIndex;
            settings.CLanguageStandard = (CLanguageStandard)clanguageStandardSelectedIndex;
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

            var currentSettings = Model.GetToolchainSettings<GccToolchainSettings>();
            currentSettings.CompileSettings = settings;

            Model.SetToolchainSettings(currentSettings);

            Model.Save();
        }

        private void OnPropertyChanged()
        {
        }
    }
}