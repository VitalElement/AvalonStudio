using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AvalonStudio.Models.Tools.Debuggers
{
    public class RemoveBreakPointCommand : Command<GDBResponse<string>>
    {
        public override int TimeoutMs
        {
            get
            {
                return DefaultCommandTimeout;
            }
        }

        public RemoveBreakPointCommand(LiveBreakPoint breakPoint)
        {
            command = string.Format ("-break-delete {0}", breakPoint.Number);
        }

        private string command;

        public override string Encode ()
        {
            return command;
        }

        protected override GDBResponse<string> Decode (string response)
        {
            return new GDBResponse<string> (DecodeResponseCode (response));
        }

        public override void OutOfBandDataReceived (string data)
        {
            
        }
    }
}
