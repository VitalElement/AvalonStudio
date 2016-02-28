namespace AvalonStudio.Extensibility
{
    using System.Collections.Generic;
    using System.ComponentModel.Composition;
    using Debugging;
    using Languages;
    using Toolchains;
    using Projects;

    [Export(typeof(Workspace))]
    public class Workspace
    {
        private readonly IEnumerable<ILanguageService> languageServices;
        private readonly IEnumerable<IProjectTemplate> projectTemplates;
        private readonly IEnumerable<IToolChain> toolChains;
        private readonly IEnumerable<IDebugger> debuggers;
        private readonly IEnumerable<IProject> projectTypes;

        public static Workspace Instance = null;

        [ImportingConstructor]
        public Workspace([ImportMany] IEnumerable<ILanguageService> languageServices, [ImportMany] IEnumerable<IProject> projectTypes, [ImportMany] IEnumerable<IProjectTemplate> projectTemplates, [ImportMany] IEnumerable<IToolChain> toolChains, [ImportMany] IEnumerable<IDebugger> debuggers)
        {            
            this.languageServices = languageServices;
            this.projectTemplates = projectTemplates;
            this.toolChains = toolChains;
            this.debuggers = debuggers;
            this.projectTypes = projectTypes;
        }        

        public IEnumerable<IProject> ProjectTypes
        {
            get
            {
                return projectTypes;
            }
        }

        public IEnumerable<IProjectTemplate> ProjectTemplates
        {
            get
            {
                return projectTemplates;
            }
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
