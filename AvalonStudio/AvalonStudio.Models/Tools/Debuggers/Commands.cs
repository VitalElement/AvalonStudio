using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AvalonStudio.Models.Tools.Debuggers
{
    public abstract class Command
    {
        protected const Int32 DefaultCommandTimeout = 5000;

        public abstract string Encode();

        protected static ResponseCode DecodeResponseCode (string response)
        {
            ResponseCode result = ResponseCode.Error;

            if (response == string.Empty)
            {
                result = ResponseCode.Timeout;
            }
            else
            {
                var split = response.Split(new char[] { ',' }, 2);

                switch (split[0])
                {
                    case "^done":
                    case "^running":
                    case "^connected":
                        result = ResponseCode.Done;
                        break;

                    case "^exit":
                        result = ResponseCode.Exit;
                        break;
                }
            }

            return result;
        }

        public abstract void OutOfBandDataReceived (string data);

        public abstract Int32 TimeoutMs { get; }
    }

    public abstract class Command<T> : Command
    {        
        protected abstract T Decode (string response);

        public T Execute (GDBDebugger gdb)
        {
            var response = gdb.SendCommand (this, TimeoutMs);            

            T result;

            result = Decode(response);

            return result;
        }
    }
}
