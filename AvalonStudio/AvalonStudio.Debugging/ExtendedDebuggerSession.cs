using Mono.Debugging.Client;
using System;
using System.Collections.Generic;
using System.Text;

namespace AvalonStudio.Debugging
{
    public abstract class ExtendedDebuggerSession : DebuggerSession
    {
        public List<Register> GetRegisters()
        {
            return OnGetRegisters();
        }

        public Dictionary<int, string> GetRegisterChanges()
        {
            return OnGetRegisterChanges();
        }

        protected abstract List<Register> OnGetRegisters();

        protected abstract Dictionary<int, string> OnGetRegisterChanges();
    }
}
