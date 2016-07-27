namespace AvalonStudio.Debugging.GDB
{
    public class ExecInterruptCommand : Command<GDBResponse<string>>
    {
        public override int TimeoutMs
        {
            get
            {
                return DefaultCommandTimeout;
            }
        }

        public override string Encode()
        {
            return "-exec-interrupt";
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
