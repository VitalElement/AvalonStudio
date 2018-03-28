using AvalonStudio.Extensibility;
using AvalonStudio.Extensibility.Plugin;
using AvalonStudio.MVVM;
using AvalonStudio.Platforms.Terminals;
using ReactiveUI;
using VtNetCore.Avalonia;

namespace AvalonStudio.Controls.Standard.Terminal
{
    public class TerminalViewModel : ToolViewModel, IExtension
    {
        private IConnection _connection;
        private bool _terminalVisible;

        public TerminalViewModel() : base("Terminal")
        {
        }

        public override Location DefaultLocation => Location.BottomRight;

        public void Activation()
        {
            var provider = IoC.Get<IPsuedoTerminalProvider>();

            if (provider != null)
            {
                var terminal = provider.Create(80, 32, "c:\\", null, @"C:\Windows\System32\WindowsPowerShell\v1.0\powershell.exe");

                Connection = new PsuedoTerminalConnection(terminal);

                TerminalVisible = true;
            }
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
