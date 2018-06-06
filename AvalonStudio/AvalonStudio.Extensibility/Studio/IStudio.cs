using System;
using System.Collections.Generic;
using AvalonStudio.Languages;
using AvalonStudio.Projects;
using AvalonStudio.Shell;
using AvalonStudio.TestFrameworks;

namespace AvalonStudio.Extensibility.Studio
{
    public interface IStudio
    {
        IEnumerable<Lazy<ILanguageService, LanguageServiceMetadata>> LanguageServices { get; }
        IEnumerable<Lazy<ISolutionType, SolutionTypeMetadata>> SolutionTypes {get;}
        IEnumerable<Lazy<IProjectType, ProjectTypeMetadata>> ProjectTypes { get; }
        IEnumerable<Lazy<IEditorProvider>> EditorProviders { get; }
        IEnumerable<Lazy<ITestFramework>> TestFrameworks { get; }
    }
}