using Avalonia.Threading;
using AvalonStudio.MVVM;
using AvalonStudio.Projects;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace AvalonStudio.Debugging.GDB.Remote
{
    public class RemoteGdbSettingsFormViewModel : ViewModel<IProject>
    {
        private readonly RemoteGdbSettings settings;

        public RemoteGdbSettingsFormViewModel(IProject model) : base(model)
        {
            settings = model.GetDebuggerSettings<RemoteGdbSettings>();

            _port = settings.Port;
            _initCommands = settings.GDBInitCommands;
        }

        private void Save()
        {
            settings.Port = _port;
            settings.GDBInitCommands = _initCommands;

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

    }
}