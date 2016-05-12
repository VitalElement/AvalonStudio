namespace AvalonStudio.Debugging.GDB
{
    public class VarEvaluateExpressionCommand : Command<GDBResponse<string>>
    {
        public override int TimeoutMs
        {
            get
            {
                return DefaultCommandTimeout;
            }
        }

        public VarEvaluateExpressionCommand (string expression)
        {
            commandText = string.Format ("-var-evaluate-expression {0}", expression);
        }

        private string commandText;

        public override string Encode ()
        {
            return commandText;
        }

        protected override GDBResponse<string> Decode (string response)
        {
            var result = new GDBResponse<string> (DecodeResponseCode (response));

            if(result.Response == ResponseCode.Done)
            {
                result.Value = response.Substring (6).ToNameValuePair().Value;
            }

            return result;
        }

        public override void OutOfBandDataReceived (string data)
        {
            
        }
    }
}
