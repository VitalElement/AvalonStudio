namespace AvalonStudio.Debugging.GDB
{
	public class RunCommand : Command<GDBResponse<string>>
	{
        private string _args;

        public RunCommand()
        {
            _args = string.Empty;
        }

        public RunCommand(string args)
        {
            _args = args;
        }

		public override int TimeoutMs
		{
			get { return DefaultCommandTimeout; }
		}

		public override string Encode()
		{
            if (_args == string.Empty)
            {
                return "-exec-run";
            }
            else
            {
                return $"-exec-run {_args}";
            }
		}

		protected override GDBResponse<string> Decode(string response)
		{
			return new GDBResponse<string>(DecodeResponseCode(response));
		}

		public override void OutOfBandDataReceived(string data)
		{
			//throw new NotImplementedException ();
		}
	}
}