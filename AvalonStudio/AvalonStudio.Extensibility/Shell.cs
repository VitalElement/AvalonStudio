namespace AvalonStudio.Extensibility
{
    using System.Collections.Generic;
    using System.ComponentModel.Composition;
    using Debugging;
    using Toolchains;
    using Projects;
    using TestFrameworks;
    using AvalonStudio.Languages;
    using ReactiveUI;
    using MVVM;
    using Splat;

    [Export(typeof(Shell))]
    public class Shell
    {
        private readonly IEnumerable<ILanguageService> languageServices;
        private readonly IEnumerable<IProjectTemplate> projectTemplates;
        private readonly IEnumerable<ICodeTemplate> codeTemplates;
        private readonly IEnumerable<IToolChain> toolChains;
        private readonly IEnumerable<IDebugger> debuggers;
        private readonly IEnumerable<IProject> projectTypes;
        private readonly IEnumerable<ITestFramework> testFrameworks;
        private readonly IEnumerable<IToolMetaData> tools;

        public static Shell Instance = null;

        [ImportingConstructor]
        public Shell([ImportMany] IEnumerable<ILanguageService> languageServices, [ImportMany] IEnumerable<IProject> projectTypes, [ImportMany] IEnumerable<IProjectTemplate> projectTemplates, [ImportMany] IEnumerable<IToolChain> toolChains, [ImportMany] IEnumerable<IDebugger> debuggers, [ImportMany] IEnumerable<ITestFramework> testFrameworks, [ImportMany] IEnumerable<ICodeTemplate> codeTemplates, [ImportMany] IEnumerable<IToolMetaData> tools)
        {
            this.tools = tools;            
            this.languageServices = languageServices;
            this.projectTemplates = projectTemplates;
            this.toolChains = toolChains;
            this.debuggers = debuggers;
            this.projectTypes = projectTypes;
            this.testFrameworks = testFrameworks;
            this.codeTemplates = codeTemplates;

            foreach(var tool in tools)
            {
                var viewType = typeof(IViewFor<>);
                viewType = viewType.MakeGenericType(tool.ViewModelType);

                Locator.CurrentMutable.Register(tool.Factory, viewType);
            }
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

        public IEnumerable<ICodeTemplate> CodeTempates
        {
            get
            {
                return codeTemplates;
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
