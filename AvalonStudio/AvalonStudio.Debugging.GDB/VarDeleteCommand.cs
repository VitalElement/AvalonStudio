namespace AvalonStudio.Debugging.GDB
{
    public class VarDeleteCommand : Command<ResponseCode>
    {
        public override int TimeoutMs
        {
            get
            {
                return DefaultCommandTimeout;
            }
        }

        public VarDeleteCommand(string id)
        {
            this.commandText = string.Format("-var-delete {0}", id);
        }

        private string commandText;

        public override string Encode()
        {
            return commandText;
        }

        protected override ResponseCode Decode(string response)
        {
            return DecodeResponseCode(response);
        }

        public override void OutOfBandDataReceived(string data)
        {
        }
    }
}
