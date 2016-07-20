namespace AvalonStudio.Debugging.GDB
{
	public class FinishCommand : Command<GDBResponse<string>>
	{
		public override int TimeoutMs
		{
			get { return DefaultCommandTimeout; }
		}

		public override string Encode()
		{
			return "-exec-finish";
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