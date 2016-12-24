using System.Collections.Generic;

namespace AvalonStudio.Debugging.GDB
{
	public class VarListChildrenCommand : Command<GDBResponse<List<VariableObject>>>
	{
		private readonly string commandText;

		private readonly VariableObject variable;

		public VarListChildrenCommand(VariableObject variable)
		{
			this.variable = variable;
			commandText = string.Format("-var-list-children {0}", variable.Id);
		}

		public override int TimeoutMs
		{
			get { return DefaultCommandTimeout; }
		}

		public override string Encode()
		{
			return commandText;
		}

		protected override GDBResponse<List<VariableObject>> Decode(string response)
		{
			var result = new GDBResponse<List<VariableObject>>(DecodeResponseCode(response));

			if (result.Response == ResponseCode.Done)
			{
				result.Value = new List<VariableObject>();

				var pairs = response.Substring(6).ToNameValuePairs();

				foreach (var pair in pairs)
				{
					switch (pair.Name)
					{
						case "children":
							var children = pair.Value.ToArray();

							foreach (var child in children)
							{
								var childPair = child.ToNameValuePair();

								result.Value.Add(VariableObject.FromDataString(variable, childPair.Value.RemoveBraces()));
							}

							break;
					}
				}
			}

			return result;
		}

		public override void OutOfBandDataReceived(string data)
		{
		}
	}
}