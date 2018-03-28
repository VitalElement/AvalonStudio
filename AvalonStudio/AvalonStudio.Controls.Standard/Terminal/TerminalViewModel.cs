using AvalonStudio.Extensibility;
using AvalonStudio.Extensibility.Plugin;
using AvalonStudio.MVVM;
using AvalonStudio.Platforms.Terminals;
using AvalonStudio.Shell;
using ReactiveUI;
using System.Reactive.Linq;
using VtNetCore.Avalonia;
using System;
using System.Diagnostics;
using AvalonStudio.Platforms;
using AvalonStudio.CommandLineTools;

namespace AvalonStudio.Controls.Standard.Terminal
{
    public class TerminalViewModel : ToolViewModel, IExtension
    {
        private IConnection _connection;
        private bool _terminalVisible;
        private IShell _shell;

        public TerminalViewModel() : base("Terminal")
        {
        }

        public override Location DefaultLocation => Location.BottomRight;

        private void CreateConnection(string workingDirectory)
        {
            if (Connection != null)
            {
                Connection.Closed -= Connection_Closed;
                Connection.Disconnect();
                Connection = null;
            }

            var provider = IoC.Get<IPsuedoTerminalProvider>();

            if (provider != null)
            {
                var shellExecutable = PlatformSupport.ResolveFullExecutablePath("powershell" + Platform.ExecutableExtension);

                if (shellExecutable != null)
                {
                    var terminal = provider.Create(80, 32, workingDirectory, null, shellExecutable);

                    Connection = new PsuedoTerminalConnection(terminal);

                    TerminalVisible = true;

                    Connection.Closed += Connection_Closed;
                }
            }
        }

        public void Activation()
        {
            CreateConnection(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile));

            _shell = IoC.Get<IShell>();

            Observable.FromEventPattern<SolutionChangedEventArgs>(_shell, nameof(_shell.SolutionChanged)).Subscribe(args =>
            {
                if (args.EventArgs.NewValue != null)
                {
                    var workingDirectoy = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);

                    var solution = args.EventArgs.NewValue;

                    if (solution.StartupProject != null)
                    {
                        workingDirectoy = solution.StartupProject.CurrentDirectory;
                    }
                    else
                    {
                        workingDirectoy = solution.CurrentDirectory;
                    }

                    CreateConnection(workingDirectoy);                                        
                }
            });
        }

        private void Connection_Closed(object sender, System.EventArgs e)
        {
            (sender as IConnection).Closed -= Connection_Closed;
            TerminalVisible = false;
        }

        public void BeforeActivation()
        {
        }

        public IConnection Connection
        {
            get { return _connection; }
            set { this.RaiseAndSetIfChanged(ref _connection, value); }
        }

        public bool TerminalVisible
        {
            get { return _terminalVisible; }
            set { this.RaiseAndSetIfChanged(ref _terminalVisible, value); }
        }
    }
}
