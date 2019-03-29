using Avalonia.Threading;
using AvalonStudio.MVVM;
using AvalonStudio.Packaging;
using AvalonStudio.Projects;
using ReactiveUI;
using System.Collections.ObjectModel;
using System.Reactive;
using System;
using System.Reactive.Linq;
using Avalonia.Controls;
using Avalonia;
using AvalonStudio.Utils;
using AvalonStudio.Platforms;

namespace AvalonStudio.Toolchains.GCC
{
    public class SysRootSettingsFormViewModel : HeaderedViewModel<IProject>
    {
        private GccToolchainSettings _settings;
        private string _sysRoot;
        private ObservableCollection<string> _packages;
        private string _searchTerm;
        private string _selectedPackage;
        private ObservableCollection<Package> _versions;
        private Package _selectedVersion;
        private bool _initialised;

        public SysRootSettingsFormViewModel(IProject project) : base("SysRoot", project)
        {
            _settings = project.GetToolchainSettings<GccToolchainSettings>();
            _sysRoot = _settings.SysRoot;

            Dispatcher.UIThread.InvokeAsync(async () =>
            {
                _initialised = true;

                try
                {
                    var packages = await PackageManager.ListPackages("sysroot");

                    Packages = new ObservableCollection<string>(packages);
                }
                catch (System.Exception)
                {
                    Packages = new ObservableCollection<string>();
                }

                if (!string.IsNullOrEmpty(_settings.SysRoot))
                {
                    SysRoot = _settings.SysRoot;
                }
            });

            this.WhenAnyValue(x => x.SelectedPackage).Subscribe(async x =>
            {
                if (x != null)
                {
                    var packages = await PackageManager.ListToolchainPackages(x, true);

                    Versions = new ObservableCollection<Package>(packages);
                }
            });

            this.WhenAnyValue(x => x.SelectedVersion).Subscribe(x =>
            {
                if (x != null)
                {
                    SysRoot = $"Package={x.Name}&Version={x.Version.ToString()}";
                }
                else
                {
                    SysRoot = "";
                }
            });

            this.WhenAnyValue(x => x.SysRoot).Subscribe(x =>
            {
                _settings.SysRoot = x;

                project.SetToolchainSettings(_settings);
                project.Save();
            });

            ClearCommand = ReactiveCommand.Create(() =>
            {
                SysRoot = "";
                SelectedPackage = null;
                SelectedVersion = null;
                Versions = null;
            });

            BrowseCommand = ReactiveCommand.CreateFromTask(async () =>
            {
                var fbd = new OpenFolderDialog();

                fbd.InitialDirectory = Model.CurrentDirectory;

                var result = await fbd.ShowAsync(Application.Current.MainWindow);

                if (!string.IsNullOrEmpty(result))
                {
                    var localSysrootPath = project.CurrentDirectory.MakeRelativePath(result).ToAvalonPath();

                    if (localSysrootPath == string.Empty)
                    {
                        localSysrootPath = $"./";
                    }

                    SysRoot = localSysrootPath;
                }
            });
        }

        public ObservableCollection<string> Packages
        {
            get { return _packages; }
            set { this.RaiseAndSetIfChanged(ref _packages, value); }
        }

        public ObservableCollection<Package> Versions
        {
            get { return _versions; }
            set { this.RaiseAndSetIfChanged(ref _versions, value); }
        }

        public string SearchTerm
        {
            get { return _searchTerm; }
            set { this.RaiseAndSetIfChanged(ref _searchTerm, value); }
        }

        public string SelectedPackage
        {
            get { return _selectedPackage; }
            set { this.RaiseAndSetIfChanged(ref _selectedPackage, value); }
        }

        public Package SelectedVersion
        {
            get { return _selectedVersion; }
            set { this.RaiseAndSetIfChanged(ref _selectedVersion, value); }
        }

        public string SysRoot
        {
            get { return _sysRoot; }
            set
            {
                if (_initialised) { this.RaiseAndSetIfChanged(ref _sysRoot, value); }
            }
        }

        public ReactiveCommand<Unit, Unit> BrowseCommand { get; }
        public ReactiveCommand<Unit, Unit> ClearCommand { get; }
    }
}