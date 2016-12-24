using System;
using System.Collections.Generic;

namespace AvalonStudio.Debugging.GDB
{
	public class DataListChangedRegistersCommand : Command<GDBResponse<List<int>>>
	{
		public override int TimeoutMs
		{
			get { return DefaultCommandTimeout; }
		}

		public override string Encode()
		{
			return "-data-list-changed-registers";
		}

		protected override GDBResponse<List<int>> Decode(string response)
		{
			var result = new GDBResponse<List<int>>(DecodeResponseCode(response));

			if (result.Response == ResponseCode.Done)
			{
				result.Value = new List<int>();

				foreach (var s in response.Substring(24).ToArray())
				{
					result.Value.Add(Convert.ToInt32(s.RemoveBraces()));
				}
			}

			return result;
		}

		public override void OutOfBandDataReceived(string data)
		{
		}
	}
}