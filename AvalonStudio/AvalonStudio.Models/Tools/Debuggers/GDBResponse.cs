using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AvalonStudio.Models.Tools.Debuggers
{
    public enum ResponseCode
    {
        Done,
        Error,
        Exit,
        Timeout
    }

    public class GDBResponse<T>
    {
        public GDBResponse ()
        {
            this.Response = ResponseCode.Timeout;
        }

        public GDBResponse (ResponseCode response)
        {
            this.Response = response;
        }

        public ResponseCode Response { get; private set; }
        public T Value { get; set; }
    }
}
