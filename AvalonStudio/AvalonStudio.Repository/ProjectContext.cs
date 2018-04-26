using NuGet.Common;
using NuGet.Packaging;
using NuGet.ProjectManagement;
using System.Xml.Linq;

namespace AvalonStudio.Packages
{
    public class ProjectContext : INuGetProjectContext
    {
        private ILogger _logger;

        public ProjectContext(ILogger logger)
        {
            _logger = logger;
            PackageExtractionContext = new PackageExtractionContext(PackageSaveMode.Files, XmlDocFileSaveMode.Skip, logger, null);
        }

        public void Log(MessageLevel level, string message, params object[] args)
        {
            _logger.LogInformation(string.Format(message, args));
        }

        public FileConflictAction ResolveFileConflict(string message) => FileConflictAction.Ignore;

        public PackageExtractionContext PackageExtractionContext { get; set; }

        public XDocument OriginalPackagesConfig { get; set; }

        public ISourceControlManagerProvider SourceControlManagerProvider => null;

        public ExecutionContext ExecutionContext => null;

        public void ReportError(string message)
        {
        }

        public NuGetActionType ActionType { get; set; }
        public TelemetryServiceHelper TelemetryService { get; set; }
    }
}