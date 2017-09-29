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
            cppLanguageStandardSelectedIndex = (int)settings.CppLanguageStandard;
            clanguageStandardSelectedIndex = (int)settings.CLanguageStandard;
            fpuSelectedIndex = (int)settings.Fpu;
            debugSymbols = settings.DebugInformation;
            rtti = settings.Rtti;
            exceptions = settings.Exceptions;

            AddDefineCommand = ReactiveCommand.Create(AddDefine, this.WhenAnyValue(x => x.DefineText, define => !string.IsNullOrEmpty(define) && !Defines.Contains(define)));

            RemoveDefineCommand = ReactiveCommand.Create(RemoveDefine, this.WhenAnyValue(x => x.SelectedDefine, selected => !string.IsNullOrEmpty(selected)));

            AddIncludePathCommand = ReactiveCommand.Create(AddIncludePath);

            RemoveIncludePathCommand = ReactiveCommand.Create(RemoveIncludePath);

            UpdateCompileString();
        }

        public ReactiveCommand AddIncludePathCommand { get; }
        public ReactiveCommand RemoveIncludePathCommand { get; }
        public ReactiveCommand AddDefineCommand { get; }
        public ReactiveCommand RemoveDefineCommand { get; }

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
                this.RaiseAndSetIfChanged(ref clanguageStandardSelectedIndex, value);
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
                this.RaiseAndSetIfChanged(ref cppLanguageStandardSelectedIndex, value);
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
                this.RaiseAndSetIfChanged(ref fpuSelectedIndex, value);

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
                this.RaiseAndSetIfChanged(ref optimizationLevelSelectedIndex, value);
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
                this.RaiseAndSetIfChanged(ref cppSupport, value);

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
                this.RaiseAndSetIfChanged(ref debugSymbols, value);
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
                this.RaiseAndSetIfChanged(ref rtti, value);
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
                this.RaiseAndSetIfChanged(ref exceptions, value);
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
                this.RaiseAndSetIfChanged(ref miscOptions, value);
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
                this.RaiseAndSetIfChanged(ref defineText, value);
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
                this.RaiseAndSetIfChanged(ref defines, value);
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
                this.RaiseAndSetIfChanged(ref includePaths, value);
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
                this.RaiseAndSetIfChanged(ref selectedInclude, value);
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
                this.RaiseAndSetIfChanged(ref selectedDefine, value);
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

        private void AddDefine()
        {
            Defines.Add(DefineText);
            DefineText = string.Empty;
            UpdateCompileString();
        }

        private void RemoveDefine()
        {
            Defines.Remove(SelectedDefine);
            UpdateCompileString();
        }

        private async void AddIncludePath()
        {
            var fbd = new OpenFolderDialog();

            fbd.InitialDirectory = Model.CurrentDirectory;

            var result = await fbd.ShowAsync();

            if (!string.IsNullOrEmpty(result))
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

        private void RemoveIncludePath()
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
            settings.Fpu = (FPUSupport)fpuSelectedIndex;
            settings.DebugInformation = debugSymbols;
            settings.Exceptions = exceptions;
            settings.Rtti = rtti;

            var currentSettings = Model.GetToolchainSettings<GccToolchainSettings>();
            currentSettings.CompileSettings = settings;

            Model.SetToolchainSettings(currentSettings);

            Model.Save();
        }
    }
}