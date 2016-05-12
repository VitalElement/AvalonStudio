namespace AvalonStudio.Debugging.GDB
{
    using System;

    public class SetCommand : Command<GDBResponse<string>>
    {
        public override int TimeoutMs
        {
            get
            {
                return DefaultCommandTimeout;
            }
        }

        public SetCommand(string name, string value)
        {
            this.name = name;
            this.value = value;
        }

        private string name;
        private string value;

        public override string Encode()
        {
            return string.Format("-gdb-set {0} {1}", name, value);
        }

        protected override GDBResponse<string> Decode(string response)
        {
            return new GDBResponse<string>(DecodeResponseCode(response));
        }

        public override void OutOfBandDataReceived(string data)
        {
            throw new NotImplementedException();
        }
    }
}
