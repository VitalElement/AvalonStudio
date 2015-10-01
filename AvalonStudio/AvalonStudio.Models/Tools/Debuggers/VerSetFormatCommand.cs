using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AvalonStudio.Models.Tools.Debuggers
{
    public class VerSetFormatCommand : Command<GDBResponse<string>>
    {
        public override int TimeoutMs
        {
            get
            {
                return DefaultCommandTimeout;
            }
        }

        public VerSetFormatCommand(string id, string format)
        {
            commandText = string.Format("-var-set-format {0} {1}", id, format);
        }

        private string commandText;

        public override void OutOfBandDataReceived(string data)
        {
            //throw new NotImplementedException();
        }

        protected override GDBResponse<string> Decode(string response)
        {
            return new GDBResponse<string>(DecodeResponseCode(response));
        }

        public override string Encode()
        {
            return commandText;
        }
    }
}
