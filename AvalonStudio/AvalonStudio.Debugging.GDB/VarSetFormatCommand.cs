namespace AvalonStudio.Debugging.GDB
{
	public class VarSetFormatCommand : Command<GDBResponse<string>>
	{
		private readonly string commandText;

		public VarSetFormatCommand(string id, string format)
		{
			commandText = string.Format("-var-set-format {0} {1}", id, format);
		}

		public override int TimeoutMs
		{
			get { return DefaultCommandTimeout; }
		}

		public override void OutOfBandDataReceived(string data)
		{
			//throw new NotImplementedException();
		}

		protected override GDBResponse<string> Decode(string response)
		{
			return new GDBResponse<string>(DecodeResponseCode(response));
		}

		public override string Encode()
		{
			return commandText;
		}
	}
}