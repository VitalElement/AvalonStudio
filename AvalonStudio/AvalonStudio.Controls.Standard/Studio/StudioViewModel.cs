using AvalonStudio.Extensibility.Studio;
using AvalonStudio.Languages;
using AvalonStudio.Projects;
using AvalonStudio.Shell;
using AvalonStudio.TestFrameworks;
using System;
using System.Collections.Generic;
using System.Composition;

namespace AvalonStudio.Controls.Standard.Studio
{
    [Export(typeof(IStudio))]
    [Shared]
    class StudioViewModel : IStudio
    {
        [ImportingConstructor]
        public StudioViewModel([ImportMany] IEnumerable<Lazy<IEditorProvider>> editorProviders,
            [ImportMany] IEnumerable<Lazy<ILanguageService, LanguageServiceMetadata>> languageServices,
            [ImportMany] IEnumerable<Lazy<ISolutionType, SolutionTypeMetadata>> solutionTypes,
            [ImportMany] IEnumerable<Lazy<IProjectType, ProjectTypeMetadata>> projectTypes,
            [ImportMany] IEnumerable<Lazy<ITestFramework>> testFrameworks)
        {
            EditorProviders = editorProviders;
            LanguageServices = languageServices;
            SolutionTypes = solutionTypes;
            ProjectTypes = projectTypes;
            TestFrameworks = testFrameworks;
        }

        public IEnumerable<Lazy<ISolutionType, SolutionTypeMetadata>> SolutionTypes { get; }

        public IEnumerable<Lazy<IProjectType, ProjectTypeMetadata>> ProjectTypes { get; }

        public IEnumerable<Lazy<ILanguageService, LanguageServiceMetadata>> LanguageServices { get; }

        public IEnumerable<Lazy<ITestFramework>> TestFrameworks { get; }

        public IEnumerable<Lazy<IEditorProvider>> EditorProviders { get; }
    }
}
