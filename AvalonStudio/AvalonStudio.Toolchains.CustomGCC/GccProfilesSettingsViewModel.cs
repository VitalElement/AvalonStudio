using Avalonia;
using Avalonia.Controls;
using AvalonStudio.Extensibility;
using AvalonStudio.Extensibility.Settings;
using ReactiveUI;
using System.Collections.ObjectModel;
using System.Reactive;

namespace AvalonStudio.Toolchains.CustomGCC
{
    class GccProfilesSettingsViewModel : SettingsViewModel, IActivatableExtension
    {
        private CustomGCCToolchainProfiles _settings;
        private ObservableCollection<string> _profiles;
        private string _instanceName;
        private string _selectedProfile;
        private string _basePath;

        public GccProfilesSettingsViewModel() : base("GCC Profiles")
        {
            SaveCommand = ReactiveCommand.Create(() =>
            {
                if (!string.IsNullOrEmpty(InstanceName))
                {
                    if (!_settings.Profiles.ContainsKey(InstanceName))
                    {
                        _settings.Profiles[InstanceName] = new CustomGCCToolchainProfile();

                        Profiles.Add(InstanceName);
                    }

                    var profile = _settings.Profiles[InstanceName];

                    _settings.Save();
                }
            });

            BrowseCommand = ReactiveCommand.CreateFromTask(async () =>
            {
                var fbd = new OpenFolderDialog();

                var result = await fbd.ShowAsync(Application.Current.MainWindow);

                if (!string.IsNullOrEmpty(result))
                {
                    BasePath = result;

                    Save();
                }
            });
        }

        public void Activation()
        {
            IoC.Get<ISettingsManager>()?.RegisterSettingsDialog("Toolchains", this);
        }

        public void BeforeActivation()
        {

        }

        public override void OnDialogLoaded()
        {
            base.OnDialogLoaded();

            _settings = CustomGCCToolchainProfiles.Instance;

            Profiles = new ObservableCollection<string>(_settings.Profiles.Keys);
        }

        private void Load()
        {
            if (_settings.Profiles.ContainsKey(SelectedProfile))
            {
                var profile = _settings.Profiles[SelectedProfile];

                InstanceName = SelectedProfile;

                BasePath = profile.BasePath;
            }
        }

        private void Save()
        {
            if(!_settings.Profiles.ContainsKey(InstanceName))
            {
                _settings.Profiles[InstanceName] = new CustomGCCToolchainProfile();
                Profiles.Add(InstanceName);
            }

            _settings.Profiles[InstanceName].BasePath = BasePath;

            _settings.Save();
        }

        public ReactiveCommand<Unit, Unit> SaveCommand { get; }

        public ObservableCollection<string> Profiles
        {
            get { return _profiles; }
            set { this.RaiseAndSetIfChanged(ref _profiles, value); }
        }

        public string InstanceName
        {
            get { return _instanceName; }
            set { this.RaiseAndSetIfChanged(ref _instanceName, value); }
        }

        public string SelectedProfile
        {
            get { return _selectedProfile; }
            set
            {
                this.RaiseAndSetIfChanged(ref _selectedProfile, value);
                if (!string.IsNullOrEmpty(value)) { Load(); }
            }
        }

        public string BasePath
        {
            get { return _basePath; }
            set { this.RaiseAndSetIfChanged(ref _basePath, value); }
        }

        public ReactiveCommand<Unit, Unit> BrowseCommand { get; }
    }
}
