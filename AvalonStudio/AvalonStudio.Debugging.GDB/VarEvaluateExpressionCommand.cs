namespace AvalonStudio.Debugging.GDB
{
	public class VarEvaluateExpressionCommand : Command<GDBResponse<string>>
	{
		private readonly string commandText;

		public VarEvaluateExpressionCommand(string expression)
		{
			commandText = string.Format("-var-evaluate-expression {0}", expression);
		}

		public override int TimeoutMs
		{
			get { return DefaultCommandTimeout; }
		}

		public override string Encode()
		{
			return commandText;
		}

		protected override GDBResponse<string> Decode(string response)
		{
			var result = new GDBResponse<string>(DecodeResponseCode(response));

			if (result.Response == ResponseCode.Done)
			{
				result.Value = response.Substring(6).ToNameValuePair().Value;
			}

			return result;
		}

		public override void OutOfBandDataReceived(string data)
		{
		}
	}
}