using System.Collections.Generic;
using System.Linq;

namespace AvalonStudio.Debugging.GDB
{
	public class DataListRegisterNamesCommand : Command<GDBResponse<List<string>>>
	{
		public override int TimeoutMs
		{
			get { return DefaultCommandTimeout; }
		}

		public override string Encode()
		{
			return "-data-list-register-names";
		}

		protected override GDBResponse<List<string>> Decode(string response)
		{
			var result = new GDBResponse<List<string>>(DecodeResponseCode(response));

			if (result.Response == ResponseCode.Done)
			{
				var parseString = response.Substring(21).RemoveBraces().Replace("\"", string.Empty);

				result.Value = parseString.Split(',').ToList();
			}

			return result;
		}

		public override void OutOfBandDataReceived(string data)
		{
			//throw new NotImplementedException();
		}
	}
}