using System.Collections.Generic;

namespace AvalonStudio.Debugging.GDB
{
	public class StackListFramesCommand : Command<GDBResponse<List<Frame>>>
	{
		private readonly string commandText;

		public StackListFramesCommand()
		{
			commandText = "-stack-list-frames";
		}

		public override int TimeoutMs
		{
			get { return DefaultCommandTimeout; }
		}

		public override string Encode()
		{
			return commandText;
		}

		protected override GDBResponse<List<Frame>> Decode(string response)
		{
			var result = new GDBResponse<List<Frame>>(DecodeResponseCode(response));

			if (result.Response == ResponseCode.Done)
			{
				var data = response.Substring(12, response.Length - 12).ToArray();

				result.Value = new List<Frame>();

				foreach (var obj in data)
				{
					result.Value.Add(obj.Substring(6, obj.Length - 6).FrameFromDataString());
				}
			}

			return result;
		}

		public override void OutOfBandDataReceived(string data)
		{
		}
	}
}