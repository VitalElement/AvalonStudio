namespace AvalonStudio.Debugging.GDB
{
	public class TargetDownloadCommand : Command<GDBResponse<string>>
	{
		public override int TimeoutMs
		{
			get { return DefaultCommandTimeout; }
		}

		public override string Encode()
		{
			return "-target-download";
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