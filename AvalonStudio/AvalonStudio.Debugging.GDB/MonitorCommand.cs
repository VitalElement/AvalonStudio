using System;

namespace AvalonStudio.Debugging.GDB
{
	public class MonitorCommand : Command<GDBResponse<string>>
	{
		private readonly string arguments;

		public MonitorCommand(string arguments)
		{
			this.arguments = arguments;
		}

		public override int TimeoutMs
		{
			get { return DefaultCommandTimeout; }
		}

		public override string Encode()
		{
			return string.Format("monitor {0}", arguments);
		}

		protected override GDBResponse<string> Decode(string response)
		{
			return new GDBResponse<string>(DecodeResponseCode(response));
		}

		public override void OutOfBandDataReceived(string data)
		{
			Console.WriteLine(data);
		}
	}
}