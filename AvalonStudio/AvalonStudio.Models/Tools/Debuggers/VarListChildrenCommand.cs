using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AvalonStudio.Models.Tools.Debuggers
{
    public class VarListChildrenCommand : Command<GDBResponse<List<VariableObject>>>
    {
        public override int TimeoutMs
        {
            get
            {
                return DefaultCommandTimeout;
            }
        }

        public VarListChildrenCommand(VariableObject variable)
        {
            this.variable = variable;
            commandText = string.Format ("-var-list-children {0}", variable.Id);
        }

        private VariableObject variable;
        private string commandText;

        public override string Encode ()
        {
            return commandText;
        }

        protected override GDBResponse<List<VariableObject>> Decode (string response)
        {
            var result = new GDBResponse<List<VariableObject>> (DecodeResponseCode (response));

            if(result.Response == ResponseCode.Done)
            {
                result.Value = new List<VariableObject> ();

                var pairs = response.Substring (6).ToNameValuePairs ();

                foreach(var pair in pairs)
                {
                    switch(pair.Name)
                    {
                        case "children":
                            var children = pair.Value.ToArray ();

                            foreach(var child in children)
                            {
                                var childPair = child.ToNameValuePair ();

                                result.Value.Add (VariableObject.FromDataString (this.variable, childPair.Value.RemoveBraces()));
                            }

                            break;
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
