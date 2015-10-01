using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AvalonStudio.Models.Tools.Debuggers
{
    public class VarDeleteCommand : Command<ResponseCode>
    {
        public override int TimeoutMs
        {
            get
            {
                return DefaultCommandTimeout;
            }
        }

        public VarDeleteCommand (string id)
        {
            this.commandText = string.Format ("-var-delete {0}", id);
        }

        private string commandText;

        public override string Encode ()
        {
            return commandText;
        }

        protected override ResponseCode Decode (string response)
        {
            return DecodeResponseCode (response);
        }

        public override void OutOfBandDataReceived (string data)
        {            
        }
    }
}
