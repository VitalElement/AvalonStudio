using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AvalonStudio.Models
{
    public interface IConsole
    {
        void WriteLine(string data);

        void WriteLine();

        void Write(string data);

        void Write (char data);

        void Clear ();
    }
}
