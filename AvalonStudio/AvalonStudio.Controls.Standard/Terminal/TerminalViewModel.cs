using AvalonStudio.CommandLineTools;
using AvalonStudio.Extensibility;
using AvalonStudio.Extensibility.Studio;
using AvalonStudio.MVVM;
using AvalonStudio.Platforms;
using AvalonStudio.Platforms.Terminals;
using AvalonStudio.Shell;
using ReactiveUI;
using System;
using System.Composition;
using System.Reactive.Linq;
using VtNetCore.Avalonia;

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
        private IStudio _studio;
        private object _createLock = new object();

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

                var provider = IoC.Get<IPsuedoTerminalProvider>();

                if (provider != null)
                {
                    var shellExecutable = PlatformSupport.ResolveFullExecutablePath("powershell" + Platform.ExecutableExtension);

                    if (shellExecutable != null)
                    {
                        System.Console.WriteLine("Starting terminal");
                        var terminal = provider.Create(80, 32, workingDirectory, null, shellExecutable);

                        Connection = new PsuedoTerminalConnection(terminal);

                        TerminalVisible = true;

                        Connection.Closed += Connection_Closed;
                    }
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

        public bool TerminalVisible
        {
            get { return _terminalVisible; }
            set { this.RaiseAndSetIfChanged(ref _terminalVisible, value); }
        }
    }
}