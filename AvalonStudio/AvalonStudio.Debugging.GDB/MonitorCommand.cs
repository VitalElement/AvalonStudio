namespace AvalonStudio.Debugging.GDB
{
    using System;

    public class MonitorCommand : Command<GDBResponse<string>>
    {
        public override int TimeoutMs
        {
            get
            {
                return DefaultCommandTimeout;
            }
        }

        public MonitorCommand(string arguments)
        {
            this.arguments = arguments;
        }

        string arguments;
        public override string Encode()
        {
            return string.Format("monitor {0}", arguments);
        }

        protected override GDBResponse<string> Decode(string response)
        {
            return new GDBResponse<string>(DecodeResponseCode(response));
        }

        public override void OutOfBandDataReceived(string data)
        {
            Console.WriteLine(data);
        }
    }
}
