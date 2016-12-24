using System.Collections.Generic;

namespace AvalonStudio.Debugging.GDB
{
	public class DataDisassembleCommand : Command<GDBResponse<List<InstructionLine>>>
	{
		private readonly string commandText;

		public DataDisassembleCommand(string file, int line, int numLines)
		{
			commandText = string.Format("-data-disassemble -f \"{0}\" -l {1} -n {2} -- 2)", file, line, numLines);
		}

		public DataDisassembleCommand(ulong startRelativeToPC, ulong endRelativeToPC)
		{
			commandText = string.Format("-data-disassemble -s \"$pc - {0}\" -e \"$pc + {1}\" -- 2",
				roundToMultiple((int) startRelativeToPC, 4), roundToMultiple((int) endRelativeToPC, 4));
		}

		public DataDisassembleCommand(ulong startAddress, uint count)
		{
			commandText = string.Format("-data-disassemble -s {0} -e {1} -- 2", startAddress, startAddress + count);
		}

		public override int TimeoutMs
		{
			get { return DefaultCommandTimeout; }
		}

		private static int roundToMultiple(int i, int multiple)
		{
			if (multiple < 0)
				multiple *= -1;
			var prem = i%multiple; // get remainder
			if (prem < 0)
				prem += multiple; // turn into positive remainder
			i -= prem; // round i toward minus infinity
			if (prem*2 > multiple) // round upward 
				return i + multiple;
			if (prem*2 < multiple) // round downward
				return i;
			//bitch case - round to even
			if (i/multiple%2 == 0) // is current rounding even?
				return i;
			return i + multiple;
		}

		public override string Encode()
		{
			return commandText;
		}

		protected override GDBResponse<List<InstructionLine>> Decode(string response)
		{
			var result = new GDBResponse<List<InstructionLine>>(DecodeResponseCode(response));

			if (result.Response == ResponseCode.Done)
			{
				result.Value = new List<InstructionLine>();

				var lines = response.Substring(16).ToArray();

				foreach (var line in lines)
				{
					result.Value.AddRange(SourceLine.FromDataString(line));
				}
			}

			return result;
		}

		public override void OutOfBandDataReceived(string data)
		{
		}
	}
}