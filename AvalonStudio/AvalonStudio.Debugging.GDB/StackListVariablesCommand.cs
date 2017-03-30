using System.Collections.Generic;

namespace AvalonStudio.Debugging.GDB
{
	public class StackListVariablesCommand : Command<GDBResponse<List<Variable>>>
	{
		public override int TimeoutMs
		{
			get { return DefaultCommandTimeout; }
		}

		public override string Encode()
		{
			return "-stack-list-variables --skip-unavailable 0";
		}

		protected override GDBResponse<List<Variable>> Decode(string response)
		{
			if (response != string.Empty)
			{
				var result = new GDBResponse<List<Variable>>(DecodeResponseCode(response));

				if (result.Response == ResponseCode.Done)
				{
					var data = response.Substring(16, response.Length - 16).ToArray();

					result.Value = new List<Variable>();

					foreach (var obj in data)
					{
						result.Value.Add(obj.RemoveBraces().VariableFromDataString());
					}
				}

				return result;
			}
			return null;
		}

		public override void OutOfBandDataReceived(string data)
		{
			//throw new NotImplementedException();
		}
	}
}