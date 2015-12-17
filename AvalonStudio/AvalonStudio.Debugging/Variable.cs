namespace AvalonStudio.Debugging
{
    using System;

    public class Variable
    {
        public static Variable FromDataString(string data)
        {
            var result= new Variable();

            foreach (var pair in data.ToNameValuePairs())
            {
                switch(pair.Name)
                {
                    case "name":
                        result.Name = pair.Value;
                        break;

                    case "arg":
                        result.IsArgument = pair.Value == "1";
                        break;

                    case "type":
                        result.Type = pair.Value;
                        break;

                    case "value":
                        result.Value = pair.Value;
                        break;

                    default:
                        throw new NotImplementedException();
                }
            }

            return result;
        }


        public string Name { get; set; }
        public bool IsArgument { get; set; }
        public string Type { get; set; }
        public string Value { get; set; }
    }
}
