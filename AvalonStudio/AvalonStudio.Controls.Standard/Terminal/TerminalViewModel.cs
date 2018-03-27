using AvalonStudio.Extensibility.Plugin;
using AvalonStudio.MVVM;
using AvalonStudio.Platforms;
using ReactiveUI;
using System;
using VtNetCore.Avalonia;

namespace AvalonStudio.Controls.Standard.Terminal
{
    public class TerminalViewModel : ToolViewModel, IExtension
    {
        public TerminalViewModel() : base("Terminal")
        {
            Connection = new ProcessConnection(new ProcessRunner("/bin/sh", ""));
            Connection.Connect();
        }

        public override Location DefaultLocation => Location.BottomRight;

        public void Activation()
        {
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
