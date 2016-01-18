namespace AvalonStudio.Extensibility
{
    using System.Collections.Generic;
    using System.ComponentModel.Composition;
    using Debugging;
    using Languages;    
    using Toolchains;

    [Export(typeof(Workspace))]
    public class Workspace
    {
        private readonly IEnumerable<ILanguageService> languageServices;
        private readonly IEnumerable<IToolChain> toolChains;
        private readonly IEnumerable<IDebugger> debuggers;

        public static Workspace Instance = null;

        [ImportingConstructor]
        public Workspace([ImportMany] IEnumerable<ILanguageService> languageServices, [ImportMany] IEnumerable<IToolChain> toolChains, [ImportMany] IEnumerable<IDebugger> debuggers)
        {
            this.languageServices = languageServices;
            this.toolChains = toolChains;
            this.debuggers = debuggers;
        }

        public IEnumerable<ILanguageService> LanguageServices
        {
            get
            {
                return languageServices;
            }
        }

        public IEnumerable<IToolChain> ToolChains
        {
            get
            {
                return toolChains;
            }
        }

        public IEnumerable<IDebugger> Debuggers
        {
            get
            {
                return debuggers;
            }
        }
    }
}
