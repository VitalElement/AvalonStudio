namespace AvalonStudio.Debugging.GDB
{
    using System;

    public class SetBreakPointCommand : Command<GDBResponse<LiveBreakPoint>>
    {
        public override int TimeoutMs
        {
            get
            {
                return DefaultCommandTimeout;
            }
        }

        public SetBreakPointCommand(string filename, UInt32 linenumber)
        {
            commandText = string.Format("-break-insert {0}:{1}", filename, linenumber);
        }

        public SetBreakPointCommand(string filename, string function)
        {
            commandText = string.Format("-break-insert {0}:{1}", filename, function);
        }

        public SetBreakPointCommand(string function)
        {
            commandText = string.Format("-break-insert {0}", function);
        }

        private string commandText;

        public override string Encode()
        {
            return commandText;
        }

        protected override GDBResponse<LiveBreakPoint> Decode(string response)
        {
            GDBResponse<LiveBreakPoint> result = new GDBResponse<LiveBreakPoint>(DecodeResponseCode(response));

            if (result.Response == ResponseCode.Done)
            {
                var split = response.Split(new char[] { ',' }, 2);

                if (split[1].Substring(0, 4) == "bkpt")
                {
                    // TODO if breakpoint may come back with multiple addresses.                    
                    result.Value = LiveBreakPoint.FromArgumentList(split[1].Substring(6, split[1].Length - 6 - 1).ToNameValuePairs());
                }
            }

            return result;
        }

        public override void OutOfBandDataReceived(string data)
        {
            // throw new NotImplementedException ();
        }
    }
}
