using System;

namespace AvalonStudio.Debugging.GDB
{
	public class SetCommand : Command<GDBResponse<string>>
	{
		private readonly string name;
		private readonly string value;

		public SetCommand(string name, string value)
		{
			this.name = name;
			this.value = value;
		}

		public override int TimeoutMs
		{
			get { return DefaultCommandTimeout; }
		}

		public override string Encode()
		{
			return string.Format("-gdb-set {0} {1}", name, value);
		}

		protected override GDBResponse<string> Decode(string response)
		{
			return new GDBResponse<string>(DecodeResponseCode(response));
		}

		public override void OutOfBandDataReceived(string data)
		{
			throw new NotImplementedException();
		}
	}
}