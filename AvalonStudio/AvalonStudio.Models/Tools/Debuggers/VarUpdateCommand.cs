using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AvalonStudio.Models.Tools.Debuggers
{    
    public class VariableObjectChange
    {
        public static VariableObjectChange FromDataString(string data)
        {
            VariableObjectChange result = new VariableObjectChange ();

            var pairs = data.ToNameValuePairs ();

            foreach (var pair in pairs)
            {
                switch (pair.Name)
                {
                    case "name":
                        result.Expression = pair.Value;
                        break;

                    case "value":
                        result.Value = pair.Value;
                        break;

                    case "in_scope":
                        switch(pair.Value)
                        {
                            case "true":
                                result.InScope = true;
                                break;

                            case "false":
                            case "invalid":
                                result.InScope = false;
                                break;
                        }
                        break;

                    case "type_changed":
                        switch (pair.Value)
                        {
                            case "true":
                                result.TypeChanged = true;
                                break;

                            case "false":
                                result.TypeChanged = false;
                                break;
                        }
                        break;

                    case "has_more":
                        result.HasMore = Convert.ToInt32 (pair.Value);
                        break;
                }
            }

            return result;
        }

        public string Expression { get; set; }
        public string Value { get; set; }
        public bool InScope { get; set; }
        public bool TypeChanged { get; set; }
        public int HasMore { get; set; }
    }

    public class VarUpdateCommand : Command<GDBResponse<List<VariableObjectChange>>>
    {
        public override int TimeoutMs
        {
            get
            {
                return DefaultCommandTimeout;
            }
        }

        public override string Encode ()
        {
            return string.Format ("-var-update 2 *");
        }

        protected override GDBResponse<List<VariableObjectChange>> Decode (string response)
        {
            var result = new GDBResponse<List<VariableObjectChange>> (DecodeResponseCode (response));

            if (result.Response == ResponseCode.Done)
            {
                result.Value = new List<VariableObjectChange>();

                var pair = response.Substring (6).ToNameValuePair();

                if(pair.Name == "changelist")
                {                    
                    var changes = pair.Value.ToArray ();

                    foreach(var change in changes)
                    {
                        result.Value.Add (VariableObjectChange.FromDataString (change.RemoveBraces ()));
                    }
                }
            }

            return result;
        }

        public override void OutOfBandDataReceived (string data)
        {
            
        }
    }
}
