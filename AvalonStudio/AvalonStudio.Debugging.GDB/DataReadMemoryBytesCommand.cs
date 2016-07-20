using System.Collections.Generic;

namespace AvalonStudio.Debugging.GDB
{
	public class DataReadMemoryBytesCommand : Command<GDBResponse<List<MemoryBytes>>>
	{
		private readonly string commandText;

		public DataReadMemoryBytesCommand(ulong address, ulong offset, ulong count)
		{
			commandText = string.Format("-data-read-memory-bytes -o {0} {1} {2}", offset, address, count);
		}

		public override int TimeoutMs
		{
			get { return -1; }
		}

		public override void OutOfBandDataReceived(string data)
		{
			// throw new NotImplementedException();
		}

		protected override GDBResponse<List<MemoryBytes>> Decode(string response)
		{
			var result = new GDBResponse<List<MemoryBytes>>(DecodeResponseCode(response));

			if (result.Response == ResponseCode.Done)
			{
				result.Value = MemoryBytes.FromDataString(response);
			}

			return result;
		}

		public override string Encode()
		{
			return commandText;
		}
	}
}