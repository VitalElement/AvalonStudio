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
        public TerminalViewModel() : base("Terminal")
        {
        }

        public override Location DefaultLocation => Location.BottomRight;

        public void Activation()
        {
            var provider = IoC.Get<IPsuedoTerminalProvider>();

            var terminal = provider.Create(80, 32, "c:\\", null, @"C:\Windows\System32\WindowsPowerShell\v1.0\powershell.exe");

            Connection = new PsuedoTerminalConnection(terminal);
        }

        public void BeforeActivation()
        {
        }

        private IConnection _connection;

        public IConnection Connection
        {
            get { return _connection; }
            set { this.RaiseAndSetIfChanged(ref _connection, value); }
        }

    }
}
