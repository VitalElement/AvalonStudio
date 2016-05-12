﻿namespace AvalonStudio.Debugging.GDB
{
    public class ContinueCommand : Command<GDBResponse<string>>
    {
        public override int TimeoutMs
        {
            get
            {
                return DefaultCommandTimeout;
            }
        }

        public override string Encode()
        {
            return "-exec-continue";
        }

        protected override GDBResponse<string> Decode(string response)
        {
            return new GDBResponse<string>(DecodeResponseCode(response));
        }

        public override void OutOfBandDataReceived(string data)
        {
            //throw new NotImplementedException ();
        }
    }
}
