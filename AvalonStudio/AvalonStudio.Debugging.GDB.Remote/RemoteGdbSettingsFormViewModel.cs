using AvalonStudio.MVVM;
using AvalonStudio.Projects;
using ReactiveUI;

namespace AvalonStudio.Debugging.GDB.Remote
{
    public class RemoteGdbSettingsFormViewModel : ViewModel<IProject>
    {
        private readonly RemoteGdbSettings settings;

        public RemoteGdbSettingsFormViewModel(IProject model) : base(model)
        {
            settings = model.GetDebuggerSettings<RemoteGdbSettings>();

            _port = settings.Port;
            _preInitCommand = settings.PreInitCommand;
            _initCommands = settings.GDBInitCommands;
            _postInitCommand = settings.PostInitCommand;
            _postInitCommandArgs = settings.PostInitCommandArgs;
            _preInitCommandArgs = settings.PreInitCommandArgs;
            _gdbExitCommands = settings.GDBExitCommands;
            _host = settings.Host;
        }

        private void Save()
        {
            settings.Port = _port;
            settings.GDBInitCommands = _initCommands;
            settings.PostInitCommand = _postInitCommand;
            settings.PreInitCommand = _preInitCommand;
            settings.PreInitCommandArgs = _preInitCommandArgs;
            settings.PostInitCommandArgs = _postInitCommandArgs;
            settings.GDBExitCommands = _gdbExitCommands;
            settings.Host = _host;

            Model.SetDebuggerSettings(settings);
            Model.Save();
        }

        private string _port;
        public string Port
        {
            get { return _port; }
            set
            {
                this.RaiseAndSetIfChanged(ref _port, value);
                Save();
            }
        }

        private string _host;

        public string Host
        {
            get { return _host; }
            set
            {
                this.RaiseAndSetIfChanged(ref _host, value);
                Save();
            }
        }


        private string _initCommands;

        public string InitCommands
        {
            get { return _initCommands; }
            set
            {
                this.RaiseAndSetIfChanged(ref _initCommands, value);
                Save();
            }
        }

        private string _preInitCommand;

        public string PreInitCommand
        {
            get { return _preInitCommand; }
            set
            {
                this.RaiseAndSetIfChanged(ref _preInitCommand, value);
                Save();
            }
        }

        private string _preInitCommandArgs;

        public string PreInitCommandArgs
        {
            get { return _preInitCommandArgs; }
            set
            {
                this.RaiseAndSetIfChanged(ref _preInitCommandArgs, value);
                Save();
            }
        }

        private string _postInitCommand;

        public string PostInitCommand
        {
            get { return _postInitCommand; }
            set
            {
                this.RaiseAndSetIfChanged(ref _postInitCommand, value);
                Save();
            }
        }

        private string _postInitCommandArgs;

        public string PostInitCommandArgs
        {
            get { return _postInitCommandArgs; }
            set
            {
                this.RaiseAndSetIfChanged(ref _postInitCommandArgs, value);
                Save();
            }
        }

        private string _gdbExitCommands;

        public string GDBExitCommands
        {
            get { return _gdbExitCommands; }
            set
            {
                this.RaiseAndSetIfChanged(ref _gdbExitCommands, value);
                Save();
            }
        }

    }
}