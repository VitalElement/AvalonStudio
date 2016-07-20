namespace AvalonStudio.Debugging.GDB
{
	public class RemoveBreakPointCommand : Command<GDBResponse<string>>
	{
		private readonly string command;

		public RemoveBreakPointCommand(LiveBreakPoint breakPoint)
		{
			command = string.Format("-break-delete {0}", breakPoint.Number);
		}

		public override int TimeoutMs
		{
			get { return DefaultCommandTimeout; }
		}

		public override string Encode()
		{
			return command;
		}

		protected override GDBResponse<string> Decode(string response)
		{
			return new GDBResponse<string>(DecodeResponseCode(response));
		}

		public override void OutOfBandDataReceived(string data)
		{
		}
	}
}