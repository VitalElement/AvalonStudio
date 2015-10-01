using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AvalonStudio.Models.Tools.Debuggers
{
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

    public class MemoryBytes
    {
        public static byte[] StringToByteArray(String hex)
        {
            int NumberChars = hex.Length;
            byte[] bytes = new byte[NumberChars / 2];

            for (int i = 0; i < NumberChars; i += 2)
            {
                bytes[i / 2] = Convert.ToByte(hex.Substring(i, 2), 16);
            }

            return bytes;
        }

        public static List<MemoryBytes> FromDataString (string data)
        {
            var result = new List<MemoryBytes>();

            var responsePair = data.Substring(6).ToNameValuePair();

            if(responsePair.Name == "memory")
            {
                var memoryBlocks = responsePair.Value.ToArray();

                foreach(var memoryBlock in memoryBlocks)
                {
                    var block = new MemoryBytes();

                    var pairs = memoryBlocks[0].RemoveBraces().ToNameValuePairs();

                    foreach (var pair in pairs)
                    {
                        switch (pair.Name)
                        {
                            case "begin":
                                block.Address = Convert.ToUInt64(pair.Value, 16);
                                break;

                            case "offset":
                                block.Offset = Convert.ToUInt64(pair.Value, 16);
                                break;

                            case "end":
                                block.End = Convert.ToUInt64(pair.Value, 16);
                                break;

                            case "contents":
                                block.Data = StringToByteArray(pair.Value);
                                block.Values = pair.Value;
                                break;
                        }
                    }

                    result.Add(block);
                }                
            }
            
            return result;
        }


        public ulong Address { get; set; }

        public ulong Offset { get; private set; }
        
        public ulong End { get; private set; }
        
        public byte[] Data { get; set; } 

        public string Values { get; set; }
    }
}
