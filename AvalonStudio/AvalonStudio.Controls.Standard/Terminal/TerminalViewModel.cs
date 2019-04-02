using AvalonStudio.CommandLineTools;
using AvalonStudio.Extensibility;
using AvalonStudio.Extensibility.Studio;
using AvalonStudio.MVVM;
using AvalonStudio.Platforms;
using AvalonStudio.Shell;
using AvalonStudio.Terminals;
using AvalonStudio.Terminals.Unix;
using AvalonStudio.Terminals.Win32;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Composition;
using System.Reactive.Linq;
using VtNetCore.Avalonia;
using VtNetCore.VirtualTerminal;

namespace AvalonStudio.Controls.Standard.Terminal
{
    [ExportToolControl]
    [Export(typeof(IExtension))]
    [Export(typeof(TerminalViewModel))]
    [Shared]
    public class TerminalViewModel : ToolViewModel, IActivatableExtension
    {
        private IConnection _connection;
        private bool _terminalVisible;
        private VirtualTerminalController _terminal;
        private IStudio _studio;
        private object _createLock = new object();
        static IPsuedoTerminalProvider s_provider = Platform.PlatformIdentifier == Platforms.PlatformID.Win32NT ? new Win32PsuedoTerminalProvider() : new UnixPsuedoTerminalProvider() as IPsuedoTerminalProvider;

        public TerminalViewModel() : base("Terminal")
        {
        }

        public override Location DefaultLocation => Location.Bottom;

        private void CreateConnection(string workingDirectory = null)
        {
            lock (_createLock)
            {
                if (workingDirectory == null)
                {
                    _studio = IoC.Get<IStudio>();

                    workingDirectory = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);

                    var solution = _studio.CurrentSolution;

                    if (solution != null)
                    {
                        if (solution?.StartupProject != null)
                        {
                            workingDirectory = solution.StartupProject.CurrentDirectory;
                        }
                        else
                        {
                            workingDirectory = solution.CurrentDirectory;
                        }
                    }
                }

                CloseConnection();

                var args = new List<string>();

                if(Platform.PlatformIdentifier == Platforms.PlatformID.MacOSX)
                {
                    args.Add("-l");
                }

                var shellExecutable = PlatformSupport.ResolveFullExecutablePath((Platform.PlatformIdentifier == Platforms.PlatformID.Win32NT ? "powershell" : "bash") + Platform.ExecutableExtension);

                if (shellExecutable != null)
                {
                    var terminal = s_provider.Create(80, 32, workingDirectory, null, shellExecutable, args.ToArray());

                    Connection = new PsuedoTerminalConnection(terminal);

                    Terminal = new VirtualTerminalController();

                    TerminalVisible = true;

                    Connection.Connect();

                    Connection.Closed += Connection_Closed;
                }
            }
        }

        private void CloseConnection ()
        {
            if (Connection != null)
            {
                System.Console.WriteLine("Closing Terminal");
                Connection.Closed -= Connection_Closed;
                Connection.Disconnect();
                Connection = null;
            }
        }

        public override void OnOpen()
        {
            CreateConnection();

            Observable.FromEventPattern<SolutionChangedEventArgs>(_studio, nameof(_studio.SolutionChanged)).Subscribe(args =>
            {
                CreateConnection();
            });

            base.OnOpen();
        }

        public override bool OnClose()
        {
            CloseConnection();
            return base.OnClose();
        }

        public void Activation()
        {

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

        public VirtualTerminalController Terminal
        {
            get => _terminal;
            set => this.RaiseAndSetIfChanged(ref _terminal,value);
        }

        public bool TerminalVisible
        {
            get { return _terminalVisible; }
            set { this.RaiseAndSetIfChanged(ref _terminalVisible, value); }
        }
    }
}
