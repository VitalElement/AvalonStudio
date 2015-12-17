namespace AvalonStudio.Debugging.GDB
{
    using AvalonStudio.Debugging;
    using System.Collections.Generic;
    using System.Linq;

    public class StackListFramesCommand : Command<GDBResponse<List<Frame>>>
    {
        public override int TimeoutMs
        {
            get
            {
                return DefaultCommandTimeout;
            }
        }

        public StackListFramesCommand()
        {
            commandText = "-stack-list-frames";
        }

        private string commandText;

        public override string Encode()
        {
            return commandText;
        }

        protected override GDBResponse<List<Frame>> Decode(string response)
        {
            var result = new GDBResponse<List<Frame>>(DecodeResponseCode(response));

            if (result.Response == ResponseCode.Done)
            {
                var data = response.Substring(12, response.Length - 12).ToArray();

                result.Value = new List<Frame>();

                foreach (string obj in data)
                {
                    result.Value.Add(Frame.FromDataString(obj.Substring(6, obj.Length - 6)));
                }
            }

            return result;
        }

        public override void OutOfBandDataReceived(string data)
        {
        }

    }
}
