using Avalonia.Threading;
using AvalonStudio.MVVM;
using AvalonStudio.Packaging;
using AvalonStudio.Projects;
using ReactiveUI;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace AvalonStudio.Toolchains.PublishedGCC
{
    public class PublishedToolchainSettingsViewModel : HeaderedViewModel<IProject>
    {
        private string _selectedPackage;

        private ObservableCollection<string> _availableToolchains;
        private PublishedGCCToolchainSettings _settings;
        private ObservableCollection<Package> _versions;
        private Package _selectedVersion;
        private bool _initialised;

        public PublishedToolchainSettingsViewModel(IProject model) : base("Platform", model)
        {
            _settings = model.GetToolchainSettings<PublishedGCCToolchainSettings>();

            Dispatcher.UIThread.InvokeAsync(async () =>
            {
                try
                {
                    var packages = await PackageManager.ListToolchains();

                    AvailableToolchains = new ObservableCollection<string>(packages);
                }
                catch (System.Exception)
                {
                    AvailableToolchains = new ObservableCollection<string>();
                }

                if (!string.IsNullOrEmpty(_settings.Toolchain))
                {
                    SelectedPackage = AvailableToolchains.FirstOrDefault((tc => tc == _settings.Toolchain));

                    await LoadVersions();

                    if (!string.IsNullOrEmpty(_settings.Version))
                    {
                        SelectedVersion = Versions?.FirstOrDefault(v => v.Version == Version.Parse(_settings.Version));

                        if (SelectedVersion == null)
                        {
                            SelectedVersion = Versions?.FirstOrDefault();
                        }
                    }
                    else
                    {
                        SelectedVersion = Versions?.FirstOrDefault();
                    }
                }
                _initialised = true;
                Save();
            });
        }

        public ObservableCollection<string> AvailableToolchains
        {
            get { return _availableToolchains; }
            set { this.RaiseAndSetIfChanged(ref _availableToolchains, value); }
        }

        public ObservableCollection<Package> Versions
        {
            get { return _versions; }
            set { this.RaiseAndSetIfChanged(ref _versions, value); }
        }

        public Package SelectedVersion
        {
            get { return _selectedVersion; }
            set
            {
                this.RaiseAndSetIfChanged(ref _selectedVersion, value);

                if (_initialised && _selectedVersion != null)
                {
                    Save();
                }
            }
        }

        private void Save()
        {
            if (SelectedPackage != null && SelectedVersion != null)
            {
                _settings.Toolchain = SelectedPackage;
                _settings.Version = SelectedVersion.Version.ToString();

                Model.SetToolchainSettings(_settings);
                Model.Save();
            }
        }

        private async Task LoadVersions()
        {
            var packages = await PackageManager.ListToolchainPackages(SelectedPackage);

            if (packages != null)
            {
                Versions = new ObservableCollection<Package>(packages);
            }
        }

        public string SelectedPackage
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