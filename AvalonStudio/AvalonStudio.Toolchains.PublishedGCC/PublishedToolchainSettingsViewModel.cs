using Avalonia.Threading;
using AvalonStudio.MVVM;
using AvalonStudio.Packages;
using AvalonStudio.Projects;
using AvalonStudio.Toolchains.CustomGCC;
using ReactiveUI;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace AvalonStudio.Toolchains.PublishedGCC
{
    public class PublishedToolchainSettingsViewModel : HeaderedViewModel<IProject>
    {
        private string _version;
        private PackageMetaData _selectedPackage;

        private ObservableCollection<PackageMetaData> _availableToolchains;
        private PublishedGCCToolchainSettings _settings;
        private ObservableCollection<string> _versions;
        private string _selectedVersion;
        private bool _initialised;

        public PublishedToolchainSettingsViewModel(IProject model) : base("Platform", model)
        {
            _settings = model.GetToolchainSettings<PublishedGCCToolchainSettings>();
            
            Dispatcher.UIThread.InvokeAsync(async () =>
            {
                AvailableToolchains = new ObservableCollection<PackageMetaData>(await GccConfigurationsManager.GetRemotePackagesAsync());

                if (!string.IsNullOrEmpty(_settings.Toolchain))
                {
                    SelectedPackage = AvailableToolchains.FirstOrDefault(tc => tc.Title == _settings.Toolchain);

                    await LoadVersions();

                    if (!string.IsNullOrEmpty(_settings.Version))
                    {
                        SelectedVersion = Versions.FirstOrDefault(v => v == _settings.Version);

                        if(SelectedVersion == null)
                        {
                            SelectedVersion = Versions.FirstOrDefault();
                        }
                    }
                    else
                    {
                        SelectedVersion = Versions.FirstOrDefault();
                    }
                }
                _initialised = true;
                Save();
            });
        }

        public ObservableCollection<PackageMetaData> AvailableToolchains
        {
            get { return _availableToolchains; }
            set { this.RaiseAndSetIfChanged(ref _availableToolchains, value); }
        }

        public ObservableCollection<string> Versions
        {
            get { return _versions; }
            set { this.RaiseAndSetIfChanged(ref _versions, value); }
        }

        public string SelectedVersion
        {
            get { return _selectedVersion; }
            set
            {
                this.RaiseAndSetIfChanged(ref _selectedVersion, value);

                if (_initialised)
                {
                    Save();
                }
            }
        }

        private void Save()
        {
            _settings.Toolchain = SelectedPackage?.Title;
            _settings.Version = SelectedVersion;

            Model.SetToolchainSettings(_settings);
            Model.Save();
        }

        private async Task LoadVersions()
        {
            var versions = await PackageManager.FindPackages(SelectedPackage.Title);

            var versionData = await versions.FirstOrDefault().GetVersionsAsync();

            Versions = new ObservableCollection<string>(versionData.Select(v=>v.Version.ToNormalizedString()));
        }

        public PackageMetaData SelectedPackage
        {
            get { return _selectedPackage; }
            set
            {
                this.RaiseAndSetIfChanged(ref _selectedPackage, value);

                if (_initialised)
                {
                    if (value != null)
                    {
                        Dispatcher.UIThread.InvokeAsync(async () =>
                        {
                            await LoadVersions();

                            SelectedVersion = Versions.FirstOrDefault();
                        });
                    }
                }
            }
        }
    }
}