﻿namespace AvalonStudio.Debugging.GDB
{
    using System.Collections.Generic;
    using System.Linq;
    using AvalonStudio.Debugging;

    public class VarUpdateCommand : Command<GDBResponse<List<VariableObjectChange>>>
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
            return string.Format("-var-update 2 *");
        }

        protected override GDBResponse<List<VariableObjectChange>> Decode(string response)
        {
            var result = new GDBResponse<List<VariableObjectChange>>(DecodeResponseCode(response));

            if (result.Response == ResponseCode.Done)
            {
                result.Value = new List<VariableObjectChange>();

                var pair = response.Substring(6).ToNameValuePair();

                if (pair.Name == "changelist")
                {
                    var changes = pair.Value.ToArray();

                    foreach (var change in changes)
                    {
                        result.Value.Add(VariableObjectChange.FromDataString(change.RemoveBraces()));
                    }
                }
            }

            return result;
        }

        public override void OutOfBandDataReceived(string data)
        {

        }
    }
}
