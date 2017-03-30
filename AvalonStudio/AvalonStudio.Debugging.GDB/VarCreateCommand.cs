namespace AvalonStudio.Debugging.GDB
{
	public class VarCreateCommand : Command<GDBResponse<VariableObject>>
	{
		private readonly string commandText;
		private readonly string expression;

		public VarCreateCommand(string id, VariableObjectType type, string expression)
		{
			var typeChar = '@';

			if (type == VariableObjectType.Fixed)
			{
				typeChar = '*';
			}

			this.expression = expression;
			commandText = string.Format("-var-create {0} {1} \"{2}\"", id, typeChar, expression);
		}

		public override int TimeoutMs
		{
			get { return DefaultCommandTimeout; }
		}

		public override string Encode()
		{
			return commandText;
		}

		protected override GDBResponse<VariableObject> Decode(string response)
		{
			var result = new GDBResponse<VariableObject>(DecodeResponseCode(response));

			if (result.Response == ResponseCode.Done)
			{
                result.Value = response.Substring(6).VariableObjectFromDataString(null, expression);
            }

			return result;
		}

		public override void OutOfBandDataReceived(string data)
		{
			//throw new NotImplementedException ();
		}
	}
}