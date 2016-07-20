namespace AvalonStudio.Debugging.GDB
{
	public class VarDeleteCommand : Command<ResponseCode>
	{
		private readonly string commandText;

		public VarDeleteCommand(string id)
		{
			commandText = string.Format("-var-delete {0}", id);
		}

		public override int TimeoutMs
		{
			get { return DefaultCommandTimeout; }
		}

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