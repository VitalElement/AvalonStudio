using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AvalonStudio.Models.Tools.Debuggers
{
    class DataListRegisterValuesCommand : Command <GDBResponse<List<Tuple<int, string>>>>
    {
        public override int TimeoutMs
        {
            get
            {
                return DefaultCommandTimeout;
            }
        }

        public DataListRegisterValuesCommand()
        {

        }

        public DataListRegisterValuesCommand(List<int> valueIndexes)
        {
            foreach (int index in valueIndexes)
            {
                registerList += index.ToString() + " ";
            }
        }

        private string registerList = string.Empty;

        public override string Encode()
        {
            return string.Format("-data-list-register-values x {0}", registerList);
        }

        protected override GDBResponse<List<Tuple<int, string>>> Decode(string response)
        {
            var result = new GDBResponse<List<Tuple<int, string>>>(DecodeResponseCode(response));

            if (result.Response == ResponseCode.Done)
            {
                result.Value = new List<Tuple<int, string>>();

                var registerValues = response.Substring(22).ToArray();

                foreach (string registerValue in registerValues)
                {
                    var pairs = registerValue.RemoveBraces().ToNameValuePairs();

                    int num = -1;
                    string val = string.Empty;

                    foreach (var pair in pairs)
                    {
                        switch (pair.Name)
                        {
                            case "number":
                                num = Convert.ToInt32(pair.Value);
                                break;

                            case "value":
                                val = pair.Value;
                                break;
                        }
                    }

                    result.Value.Add(new Tuple<int, string>(num, val));
                }
            }
             
            return result;
        }

        public override void OutOfBandDataReceived(string data)
        {
            
        }
    }
}
