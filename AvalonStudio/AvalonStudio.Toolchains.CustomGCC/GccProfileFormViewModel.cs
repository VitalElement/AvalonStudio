using AvalonStudio.MVVM;
using AvalonStudio.Projects;
using ReactiveUI;

namespace AvalonStudio.Toolchains.CustomGCC
{
    public class GccProfileFormViewModel : HeaderedViewModel<IProject>
    {
        private string _profileName;
        private string _prefix;
        private string _ccPrefix;
        private string _cppPrefix;
        private string _ldPrefix;
        private string _arPrefix;
        private string _sizePrefix;
        private string _ccName;
        private string _cppName;
        private string _ldName;
        private string _arName;
        private string _sizeName;
        private string _libraryQueryCommand;

        private CustomGCCToolchainProjectSettings _settings;

        public GccProfileFormViewModel(IProject model) : base("Toolchain Profiles", model)
        {
            _settings = model.GetToolchainSettings<CustomGCCToolchainProjectSettings>();

            _profileName = _settings.InstanceName;
            _prefix = _settings.Prefix;
            _ccPrefix = _settings.CCPrefix;
            _ccName = _settings.CCName;
            _cppPrefix = _settings.CPPPrefix;
            _cppName = _settings.CPPName;
            _ldPrefix = _settings.LDPrefix;
            _ldName = _settings.LDName;
            _arPrefix = _settings.ARPrefix;
            _arName = _settings.ARName;
            _sizePrefix = _settings.SizePrefix;
            _sizeName = _settings.SizeName;
            _libraryQueryCommand = _settings.LibraryQueryCommand;
        }

        private void Save()
        {
            Model.SetToolchainSettings(_settings);
            Model.Save();
        }

        public string ProfileName
        {
            get { return _profileName; }
            set
            {
                this.RaiseAndSetIfChanged(ref _profileName, value);
                _settings.InstanceName = value;
                Save();
            }
        }

        public string Prefix
        {
            get { return _prefix; }
            set
            {
                this.RaiseAndSetIfChanged(ref _prefix, value);
                _settings.Prefix = value;
                Save();
            }
        }

        public string CCPrefix
        {
            get { return _ccPrefix; }
            set
            {
                this.RaiseAndSetIfChanged(ref _ccPrefix, value);
                _settings.CCPrefix = value;
                Save();
            }
        }

        public string CCName
        {
            get { return _ccName; }
            set
            {
                this.RaiseAndSetIfChanged(ref _ccName, value);
                _settings.CCName = value;
                Save();
            }
        }

        public string CppPrefix
        {
            get { return _cppPrefix; }
            set
            {
                this.RaiseAndSetIfChanged(ref _cppPrefix, value);
                _settings.CPPPrefix = value;
                Save();
            }
        }

        public string CppName
        {
            get { return _cppName; }
            set
            {
                this.RaiseAndSetIfChanged(ref _ccName, value);
                _settings.CPPName = value;
                Save();
            }
        }

        public string LDPrefix
        {
            get { return _ldPrefix; }
            set
            {
                this.RaiseAndSetIfChanged(ref _ldPrefix, value);
                _settings.LDPrefix = value;
                Save();
            }
        }

        public string LDName
        {
            get { return _ldName; }
            set
            {
                this.RaiseAndSetIfChanged(ref _ldName, value);
                _settings.LDName = value;
                Save();
            }
        }

        public string ARPrefix
        {
            get { return _arPrefix; }
            set
            {
                this.RaiseAndSetIfChanged(ref _arPrefix, value);
                _settings.ARPrefix = value;
                Save();
            }
        }

        public string ArName
        {
            get { return _arName; }
            set
            {
                this.RaiseAndSetIfChanged(ref _arName, value);
                _settings.ARName = value;
                Save();
            }
        }

        public string SizePrefix
        {
            get { return _sizePrefix; }
            set
            {
                this.RaiseAndSetIfChanged(ref _sizePrefix, value);
                _settings.SizePrefix = value;
                Save();
            }
        }

        public string SizeName
        {
            get { return _sizeName; }
            set
            {
                this.RaiseAndSetIfChanged(ref _sizeName, value);
                _settings.SizeName = value;
                Save();
            }
        }

        public string LibraryQueryCommand
        {
            get { return _libraryQueryCommand; }
            set
            {
                this.RaiseAndSetIfChanged(ref _libraryQueryCommand, value);
                _settings.LibraryQueryCommand = value;
                Save();
            }
        }

    }
}