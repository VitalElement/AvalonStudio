using System;
using System.Collections.Generic;

namespace AvalonStudio.Debugging.GDB
{
	internal class DataListRegisterValuesCommand : Command<GDBResponse<List<Tuple<int, string>>>>
	{
		private readonly string registerList = string.Empty;

		public DataListRegisterValuesCommand()
		{
		}

		public DataListRegisterValuesCommand(List<int> valueIndexes)
		{
			foreach (var index in valueIndexes)
			{
				registerList += index + " ";
			}
		}

		public override int TimeoutMs
		{
			get { return DefaultCommandTimeout; }
		}

		public override string Encode()
		{
			return string.Format("-data-list-register-values x {0}", registerList);
		}

		protected override GDBResponse<List<Tuple<int, string>>> Decode(string response)
		{
			var result = new GDBResponse<List<Tuple<int, string>>>(DecodeResponseCode(response));

			if (result.Response == ResponseCode.Done)
			{
				result.Value = new List<Tuple<int, string>>();

				var registerValues = response.Substring(22).ToArray();

				foreach (var registerValue in registerValues)
				{
					var pairs = registerValue.RemoveBraces().ToNameValuePairs();

					var num = -1;
					var val = string.Empty;

					foreach (var pair in pairs)
					{
						switch (pair.Name)
						{
							case "number":
								num = Convert.ToInt32(pair.Value);
								break;

							case "value":
								val = pair.Value;
								break;
						}
					}

					result.Value.Add(new Tuple<int, string>(num, val));
				}
			}

			return result;
		}

		public override void OutOfBandDataReceived(string data)
		{
		}
	}
}