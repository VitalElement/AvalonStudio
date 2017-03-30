namespace AvalonStudio.Debugging.GDB
{
	public class SetBreakPointCommand : Command<GDBResponse<LiveBreakPoint>>
	{
		private readonly string commandText;

		public SetBreakPointCommand(string filename, uint linenumber)
		{
			commandText = string.Format("-break-insert {0}:{1}", filename, linenumber);
		}

		public SetBreakPointCommand(string filename, string function)
		{
			commandText = string.Format("-break-insert {0}:{1}", filename, function);
		}

		public SetBreakPointCommand(string function)
		{
			commandText = string.Format("-break-insert {0}", function);
		}

		public override int TimeoutMs
		{
			get { return DefaultCommandTimeout; }
		}

		public override string Encode()
		{
			return commandText;
		}

		protected override GDBResponse<LiveBreakPoint> Decode(string response)
		{
			var result = new GDBResponse<LiveBreakPoint>(DecodeResponseCode(response));

			if (result.Response == ResponseCode.Done)
			{
				var split = response.Split(new[] {','}, 2);

				if (split[1].Substring(0, 4) == "bkpt")
				{
					// TODO if breakpoint may come back with multiple addresses.                    
					result.Value = split[1].Substring(6, split[1].Length - 6 - 1).ToNameValuePairs().LiveBreakPointFromArgumentList();
				}
			}

			return result;
		}

		public override void OutOfBandDataReceived(string data)
		{
			// throw new NotImplementedException ();
		}
	}
}