namespace AvalonStudio.Debugging.GDB
{
    public class GDBSetCommand : Command<GDBResponse<string>>
    {
        private string var;
        private string value;

        public GDBSetCommand(string var, string value)
        {
            this.value = value;
            this.var = var;
        }

        public override int TimeoutMs
        {
            get
            {
                return DefaultCommandTimeout;
            }
        }

        public override string Encode()
        {
            return string.Format("-gdb-set {0} {1}", var, value);
        }

        public override void OutOfBandDataReceived(string data)
        {
            
        }

        protected override GDBResponse<string> Decode(string response)
        {
            return new GDBResponse<string>(DecodeResponseCode(response));
        }
    }
}
