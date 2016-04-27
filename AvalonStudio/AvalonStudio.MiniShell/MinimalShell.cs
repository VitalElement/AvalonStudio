namespace AvalonStudio.Shell
{
    using AvalonStudio.Debugging;
    using AvalonStudio.Extensibility.Plugin;
    using AvalonStudio.Languages;
    using AvalonStudio.Projects;
    using AvalonStudio.TestFrameworks;
    using AvalonStudio.Toolchains;
    using Extensibility;
    using System.Collections.Generic;
    using System.ComponentModel.Composition;
    using Documents;
    using Extensibility.Dialogs;
    using System;
    using System.Collections.ObjectModel;
    using System.Threading.Tasks;

    [Export(typeof(IShell))]
    public class MinimalShell : IShell
    {
        public static IShell Instance = null;

        [ImportingConstructor]
        public MinimalShell([ImportMany] IEnumerable<ILanguageService> languageServices, [ImportMany] IEnumerable<IProject> projectTypes, [ImportMany] IEnumerable<IProjectTemplate> projectTemplates, [ImportMany] IEnumerable<IToolChain> toolChains, [ImportMany] IEnumerable<IDebugger> debuggers, [ImportMany] IEnumerable<ITestFramework> testFrameworks, [ImportMany] IEnumerable<ICodeTemplate> codeTemplates)
        {
            this.languageServices = languageServices;
            this.projectTemplates = projectTemplates;
            this.toolChains = toolChains;
            this.debuggers = debuggers;
            this.projectTypes = projectTypes;
            this.testFrameworks = testFrameworks;
            this.codeTemplates = codeTemplates;

            IoC.RegisterConstant(this, typeof(IShell));
        }

        private readonly IEnumerable<ILanguageService> languageServices;
        private readonly IEnumerable<IProjectTemplate> projectTemplates;
        private readonly IEnumerable<ICodeTemplate> codeTemplates;
        private readonly IEnumerable<IToolChain> toolChains;
        private readonly IEnumerable<IDebugger> debuggers;
        private readonly IEnumerable<IProject> projectTypes;
        private readonly IEnumerable<ITestFramework> testFrameworks;
        private readonly IEnumerable<IPlugin> plugins;

        public event EventHandler SolutionChanged;

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

        public IEnumerable<ICodeTemplate> CodeTemplates
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

        public ISolution CurrentSolution
        {
            get
            {
                throw new NotImplementedException();
            }

            set
            {
                throw new NotImplementedException();
            }
        }

        public ObservableCollection<object> Tools
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public object BottomSelectedTool
        {
            get
            {
                throw new NotImplementedException();
            }

            set
            {
                throw new NotImplementedException();
            }
        }

        public ModalDialogViewModelBase ModalDialog
        {
            get
            {
                throw new NotImplementedException();
            }

            set
            {
                throw new NotImplementedException();
            }
        }

        public Task<IEditor> OpenDocument(ISourceFile file, int line, int column = 1, bool debugHighlight = false, bool selectLine = false)
        {
            throw new NotImplementedException();
        }

        public void InvalidateCodeAnalysis()
        {
            throw new NotImplementedException();
        }

        public void Debug(IProject project)
        {
            throw new NotImplementedException();
        }

        public void Build(IProject project)
        {
            throw new NotImplementedException();
        }

        public void Clean(IProject project)
        {
            throw new NotImplementedException();
        }

        public void Debug()
        {
            throw new NotImplementedException();
        }

        public void Build()
        {
            throw new NotImplementedException();
        }

        public void Clean()
        {
            throw new NotImplementedException();
        }

        public IProject GetDefaultProject()
        {
            throw new NotImplementedException();
        }
    }
}
