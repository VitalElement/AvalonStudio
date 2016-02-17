namespace AvalonStudio.Controls.ViewModels
{
    using Perspex.Controls;
    using Projects;
    using ReactiveUI;
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    //using Toolchains.STM32;

    public class ProjectConfigurationDialogViewModel : ModalDialogViewModelBase
    {
        public ProjectConfigurationDialogViewModel() : base ("Project Properties", true, false)
        {
            //this.compileContent = new CompileSettingsForm();

        }

        public ProjectConfigurationDialogViewModel(IProject project, Action onClose)
            : base("Project Configuration", true, false)
        {
            this.configPages = new List<TabItem>();
            configPages.AddRange(project.ConfigurationPages);

            if (project.ToolChain != null)
            {
                configPages.AddRange(project.ToolChain.GetConfigurationPages(project));
            }



            //this.project = project;
            //isLibrary = project.SelectedConfiguration.IsLibrary;
            //this.debugSettings = new DebuggerSettingsFormViewModel(this, project);

            //SetExecutionOptionsVisibility(!isLibrary);

            //ExportedIncludes = new ObservableCollection<string>(project.ExportedIncludes);
            //AddExportedIncludePathCommand = new RoutingCommand(AddExportedIncludePath);
            //RemoveExportedIncludePathCommand = new RoutingCommand(RemoveExportedIncludePath, RemoveExportedIncludePathCanExecute);
            //ToolChains = new ObservableCollection<ToolChainPackageViewModel>();

            //Task.Factory.StartNew(() =>
            //{
            //    repo = new DeviceRepository();

            //    selectedDevice = repo.GetDevice(project.SelectedConfiguration.SelectedDeviceId);
            //    OnPropertyChanged(() => SelectedDevice);   // update ui with selected device without changing link / compile settings.

            //    var manuf = repo.GetManufacturers().ToList();

            //    Workspace.This.DispatchUi(() =>
            //    {
            //        Manufacturers = manuf;
            //    });
            //});

            Task.Factory.StartNew(async () =>
            {
                //var repo = await Repository.DownloadCatalog();

                //Workspace.This.DispatchUi(() =>
                //{
                //    foreach (var package in repo.Packages)
                //    {
                //        if (package is ToolChainPackage)
                //        {
                //            ToolChains.Add(new ToolChainPackageViewModel(package as ToolChainPackage));
                //        }
                //    }

                //    if (project.SelectedConfiguration.ToolChain != null)
                //    {
                //        SelectedToolChain = ToolChains.FirstOrDefault((tc) => tc.Model.ToolChainType == project.SelectedConfiguration.ToolChain.GetType());
                //    }

                //    IsEnabled = true;
                //});
            });

            //compileSettings = new CompileSettingsViewModel(project);
            //linkSettings = new LinkSettingsFormViewModel(project);

            OKCommand = ReactiveCommand.Create();

            OKCommand.Subscribe((o)=>           
            {

                //project.ExportedIncludes = ExportedIncludes.ToList();
                //project.SelectedConfiguration.IsLibrary = isLibrary;

                //if (IsEnabled) //Otherwise we can still be downloading this toolchains....
                //{
                //    project.SelectedConfiguration.ToolChain = VEStudioService.ToolChains.FirstOrDefault((tc) => tc.GetType() == selectedToolChain.Model.ToolChainType);
                //}

                //compileSettings.Save();
                //linkSettings.Save();
                //debugSettings.Save();

                //project.SaveChanges();

                //Workspace.This.InvalidateCodeAnalysis();

                //await project.Clean(Workspace.This.StudioConsole, Workspace.This.ProcessCancellationToken);

                onClose();
                this.Close();
            });
        }

        //private DebuggerSettingsFormViewModel debugSettings;
        //public DebuggerSettingsFormViewModel DebugSettings
        //{
        //    get { return debugSettings; }
        //    set { debugSettings = value; OnPropertyChanged(); }
        //}

        private void OnPropertyChanged()
        {

        }

        private object compileContent;
        public object CompileContent
        {
            get { return compileContent; }
            set { compileContent = value; }
        }

        private List<TabItem> configPages;
        public List<TabItem> ConfigPages
        {
            get { return configPages; }
            set { configPages = value; }
        }



        //private void SelectDevice()
        //{
        //    if (selectedDevice != null)
        //    {
        //        if (selectedDevice.Id != project.SelectedConfiguration.SelectedDeviceId)
        //        {
        //            var currentDevice = repo.GetDevice(project.SelectedConfiguration.SelectedDeviceId);

        //            project.SelectedConfiguration.SelectedDeviceId = selectedDevice.Id;
        //            project.SelectedConfiguration.SelectedDeviceName = selectedDevice.Name;

        //            if (currentDevice != null)
        //            {
        //                compileSettings.Defines.Remove(currentDevice.Define);
        //            }

        //            if (selectedDevice.Define != string.Empty)
        //            {
        //                compileSettings.Defines.Add(selectedDevice.Define);
        //            }
        //        }

        //        if (selectedDevice.Core.Fpu)
        //        {
        //            compileSettings.FpuSelectedIndex = 2;
        //        }
        //        else
        //        {
        //            compileSettings.FpuSelectedIndex = 0;
        //        }

        //        project.SelectedConfiguration.Mcpu = selectedDevice.Core.Mcpu;
        //        project.SelectedConfiguration.March = selectedDevice.Core.March;

        //        linkSettings.UseMemoryLayout = true;
        //        linkSettings.DiscardUnusedSections = true;
        //        linkSettings.NotUseStandardStartup = true;

        //        linkSettings.InRom1Start = string.Format("0x{0:X8}", selectedDevice.RomBase);
        //        linkSettings.InRom1Size = string.Format("0x{0:X8}", selectedDevice.RomSize);
        //        linkSettings.InRam1Start = string.Format("0x{0:X8}", selectedDevice.RamBase);
        //        linkSettings.InRam1Size = string.Format("0x{0:X8}", selectedDevice.RamSize);

        //        linkSettings.InRom2Start = string.Format("0x{0:X8}", 0);
        //        linkSettings.InRom2Size = string.Format("0x{0:X8}", 0);
        //        linkSettings.InRam2Start = string.Format("0x{0:X8}", 0);
        //        linkSettings.InRam2Size = string.Format("0x{0:X8}", 0);
        //    }

        //    compileSettings.UpdateCompileString();
        //    linkSettings.UpdateLinkerString();
        //}

        private void SetExecutionOptionsVisibility(bool visible)
        {
            if (visible)
            {
                //ExecutableOptionsVisibility = Visibility.Visible;
            }
            else
            {
                //ExecutableOptionsVisibility = Visibility.Collapsed;
            }
        }

        private bool executableOptionsVisibility;
        public bool ExecutableOptionsVisibility
        {
            get { return executableOptionsVisibility; }
            set { executableOptionsVisibility = value; OnPropertyChanged(); }
        }


        //private Manufacturer selectedManufacturer;
        //public Manufacturer SelectedManufacturer
        //{
        //    get { return selectedManufacturer; }
        //    set
        //    {
        //        selectedManufacturer = value;

        //        if (value != null)
        //        {
        //            Task.Factory.StartNew(() =>
        //            {
        //                try
        //                {
        //                    var devs = repo.GetDevices().Where((d) => d.Manufacturer.Id == value.Id);

        //                    var devList = devs.ToList();

        //                    Workspace.This.DispatchUi(() =>
        //                    {
        //                        Devices = devList;
        //                    });
        //                }
        //                catch (Exception)
        //                {
        //                }
        //            });
        //        }

        //        OnPropertyChanged();
        //    }
        //}

        //private Device selectedDevice;
        //public Device SelectedDevice
        //{
        //    get { return selectedDevice; }
        //    set { selectedDevice = value; SelectDevice(); OnPropertyChanged(); }
        //}



    //    private List<Manufacturer> manufacturers;
    //    public List<Manufacturer> Manufacturers
    //    {
    //        get { return manufacturers; }
    //        set { manufacturers = value; OnPropertyChanged(); }
    //    }

    //    private List<Device> devices;
    //    public List<Device> Devices
    //    {
    //        get { return devices; }
    //        set { devices = value; OnPropertyChanged(); }
    //    }

    //    private DeviceRepository repo;

    //    private bool isEnabled = false;
    //    public bool IsEnabled
    //    {
    //        get { return isEnabled; }
    //        set { isEnabled = value; OnPropertyChanged(); }
    //    }

    //    private Project project;
    //    private void AddExportedIncludePath(object param)
    //    {
    //        FolderBrowserDialog fbd = new FolderBrowserDialog();

    //        fbd.SelectedPath = project.CurrentDirectory;

    //        if (fbd.ShowDialog() == DialogResult.OK)
    //        {
    //            string newInclude = project.Solution.CurrentDirectory.MakeRelativePath(fbd.SelectedPath);

    //            if (newInclude == string.Empty)
    //            {
    //                newInclude = "\\";
    //            }

    //            ExportedIncludes.Add(newInclude);
    //        }
    //    }

    //    private bool AddExportedIncludePathCanExecute(object param)
    //    {
    //        return true;
    //    }

    //    private void RemoveExportedIncludePath(object param)
    //    {
    //        ExportedIncludes.Remove(SelectedExportedInclude);
    //    }

    //    private bool RemoveExportedIncludePathCanExecute(object param)
    //    {
    //        return true;
    //    }

    //    public ICommand AddExportedIncludePathCommand { get; private set; }
    //    public ICommand RemoveExportedIncludePathCommand { get; private set; }

    //    private string selectedExportedInclude;
    //    public string SelectedExportedInclude
    //    {
    //        get { return selectedExportedInclude; }
    //        set { selectedExportedInclude = value; OnPropertyChanged(); }
    //    }


    //    private ObservableCollection<string> exportedIncludes;
    //    public ObservableCollection<string> ExportedIncludes
    //    {
    //        get { return exportedIncludes; }
    //        set { exportedIncludes = value; OnPropertyChanged(); }
    //    }

    //    private CompileSettingsViewModel compileSettings;
    //    public CompileSettingsViewModel CompileSettings
    //    {
    //        get { return compileSettings; }
    //        set { compileSettings = value; OnPropertyChanged(); }
    //    }


    //    private LinkSettingsFormViewModel linkSettings;
    //    public LinkSettingsFormViewModel LinkSettings
    //    {
    //        get { return linkSettings; }
    //        set { linkSettings = value; OnPropertyChanged(); }
    //    }

    //    private bool isLibrary;
    //    public bool IsLibrary
    //    {
    //        get { return isLibrary; }
    //        set
    //        {
    //            isLibrary = value;

    //            SetExecutionOptionsVisibility(!value);

    //            OnPropertyChanged();
    //        }
    //    }


    //    public List<Target> AvailableTargets
    //    {
    //        get { return Target.Targets; }
    //    }


    //    private ToolChainPackageViewModel selectedToolChain;
    //    public ToolChainPackageViewModel SelectedToolChain
    //    {
    //        get { return selectedToolChain; }
    //        set { selectedToolChain = value; OnPropertyChanged(); }
    //    }


    //    private ObservableCollection<ToolChainPackageViewModel> toolChains;
    //    public ObservableCollection<ToolChainPackageViewModel> ToolChains
    //    {
    //        get { return toolChains; }
    //        set { toolChains = value; OnPropertyChanged(); }
    //    }

    //    public override ICommand OKCommand { get; protected set; }
    }
}
