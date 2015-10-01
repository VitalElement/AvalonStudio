using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AvalonStudio.Models.Solutions;
using AvalonStudio.Models.Tools.Compiler;

namespace AvalonStudio.Models.Tools.Debuggers
{
    public class LocalDebugAdaptor : GDBDebugAdaptor
    {
        public LocalDebugAdaptor ()
        {

        }

        public override bool Start(ToolChain toolchain, IConsole console, Project project)
        {
            bool result = base.Start(toolchain, console, project);

            if (result)
            {
                new SetCommand("new-console", "on").Execute(this);
            }

            return result;
        }

        public override string Name
        {
            get { return "Local Executable Debug Adaptor"; }
        }

        public override void Run ()
        {
            base.Run ();
        }
    }
}
