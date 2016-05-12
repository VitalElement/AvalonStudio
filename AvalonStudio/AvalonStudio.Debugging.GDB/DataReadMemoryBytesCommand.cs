namespace AvalonStudio.Debugging.GDB
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class DataReadMemoryBytesCommand : Command<GDBResponse<List<MemoryBytes>>>
    {
        public override int TimeoutMs
        {
            get
            {
                return -1;
            }
        }

        public DataReadMemoryBytesCommand(ulong address, ulong offset, ulong count)
        {
            commandText = string.Format("-data-read-memory-bytes -o {0} {1} {2}", offset, address, count);
        }

        private string commandText;

        public override void OutOfBandDataReceived(string data)
        {
            // throw new NotImplementedException();
        }

        protected override GDBResponse<List<MemoryBytes>> Decode(string response)
        {
            var result = new GDBResponse<List<MemoryBytes>>(DecodeResponseCode(response));

            if (result.Response == ResponseCode.Done)
            {
                result.Value = MemoryBytes.FromDataString(response);
            }

            return result;
        }

        public override string Encode()
        {
            return commandText;
        }
    }
}
