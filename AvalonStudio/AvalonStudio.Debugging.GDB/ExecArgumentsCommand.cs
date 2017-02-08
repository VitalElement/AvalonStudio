namespace AvalonStudio.Debugging.GDB
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class ExecArgumentsCommand : Command<GDBResponse<string>>
    {
        private string _args;

        public ExecArgumentsCommand(string args)
        {
            _args = args;
        }

        public override int TimeoutMs => DefaultCommandTimeout;

        public override string Encode()
        {
            return $"-exec-arguments {_args}";
        }

        public override void OutOfBandDataReceived(string data)
        {
            
        }

        protected override GDBResponse<string> Decode(string response)
        {
            return new GDBResponse<string>(DecodeResponseCode(response));
        }
    }
}
