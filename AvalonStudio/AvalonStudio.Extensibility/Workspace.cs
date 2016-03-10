namespace AvalonStudio.Extensibility
{
    using System.Collections.Generic;
    using System.ComponentModel.Composition;
    using Debugging;
    using Languages;
    using Toolchains;
    using Projects;
    using TestFrameworks;

    [Export(typeof(Workspace))]
    public class Workspace
    {
        private readonly IEnumerable<ILanguageService> languageServices;
        private readonly IEnumerable<IProjectTemplate> projectTemplates;
        private readonly IEnumerable<IToolChain> toolChains;
        private readonly IEnumerable<IDebugger> debuggers;
        private readonly IEnumerable<IProject> projectTypes;
        private readonly IEnumerable<ITestFramework> testFrameworks;

        public static Workspace Instance = null;

        [ImportingConstructor]
        public Workspace([ImportMany] IEnumerable<ILanguageService> languageServices, [ImportMany] IEnumerable<IProject> projectTypes, [ImportMany] IEnumerable<IProjectTemplate> projectTemplates, [ImportMany] IEnumerable<IToolChain> toolChains, [ImportMany] IEnumerable<IDebugger> debuggers, [ImportMany] IEnumerable<ITestFramework> testFrameworks)
        {            
            this.languageServices = languageServices;
            this.projectTemplates = projectTemplates;
            this.toolChains = toolChains;
            this.debuggers = debuggers;
            this.projectTypes = projectTypes;
            this.testFrameworks = testFrameworks;
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

        public IEnumerable<ITestFramework> TestFrameworks
        {
            get
            {
                return testFrameworks;
            }
        }
    }
}
