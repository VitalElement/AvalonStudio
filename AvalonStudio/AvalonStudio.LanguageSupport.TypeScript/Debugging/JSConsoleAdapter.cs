using AvalonStudio.Utils;

namespace AvalonStudio.LanguageSupport.TypeScript.Debugging
{
    public class JSConsoleAdapter
    {
        private readonly IConsole output;

        public JSConsoleAdapter(IConsole output)
        {
            this.output = output;
        }

        public void log(params object[] args)
        {
            output.WriteLine("[log] " + string.Join(" ", args));
        }

        public void error(params object[] args)
        {
            output.WriteLine("[err] " + string.Join(" ", args));
        }
    }
}