namespace AvalonStudio.Debugging.GDB
{
	public class StepOverCommand : Command<GDBResponse<string>>
	{
		public override int TimeoutMs
		{
			get { return DefaultCommandTimeout; }
		}

		public override string Encode()
		{
			return "-exec-next";
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