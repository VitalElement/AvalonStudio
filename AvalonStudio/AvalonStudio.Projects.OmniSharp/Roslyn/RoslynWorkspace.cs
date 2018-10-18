using AvalonStudio.Extensibility.Projects;
using AvalonStudio.Platforms;
using AvalonStudio.Projects.OmniSharp.MSBuild;
using AvalonStudio.Projects.OmniSharp.Roslyn.Common;
using AvalonStudio.Projects.OmniSharp.Roslyn.Diagnostics;
using AvalonStudio.Projects.OmniSharp.Roslyn.Editor;
using AvalonStudio.Utils;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Host;
using Microsoft.CodeAnalysis.Host.Mef;
using Microsoft.CodeAnalysis.Text;
using Roslyn.Utilities;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Composition;
using System.Composition.Hosting;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AvalonStudio.Projects.OmniSharp.Roslyn
{
    public sealed class RoslynWorkspace : Workspace
    {
        private static Dictionary<ISolution, RoslynWorkspace> s_solutionWorkspaces = new Dictionary<ISolution, RoslynWorkspace>();

        private CompositionHost _compositionContext;
        private readonly NuGetConfiguration _nuGetConfiguration;
        private readonly Dictionary<DocumentId, AvalonEditTextContainer> _openDocumentTextLoaders;
        private readonly ConcurrentDictionary<DocumentId, Action<DiagnosticsUpdatedArgs>> _diagnosticsUpdatedNotifiers;
        private readonly BlockingCollection<MSBuildHost> _buildNodes;
        private readonly string dotnetPath;
        private readonly string sdkPath;

        [ImportingConstructor]
        internal RoslynWorkspace(HostServices host, NuGetConfiguration nuGetConfiguration, CompositionHost compositionContext, string dotnetPath, string sdkPath)
            : base(host, WorkspaceKind.Host)
        {
            this.dotnetPath = dotnetPath;
            this.sdkPath = sdkPath;
            _nuGetConfiguration = nuGetConfiguration;

            _openDocumentTextLoaders = new Dictionary<DocumentId, AvalonEditTextContainer>();
            _diagnosticsUpdatedNotifiers = new ConcurrentDictionary<DocumentId, Action<DiagnosticsUpdatedArgs>>();

            _compositionContext = compositionContext;

            _buildNodes = new BlockingCollection<MSBuildHost>(Environment.ProcessorCount);

            DiagnosticProvider.Enable(this, DiagnosticProvider.Options.Semantic | DiagnosticProvider.Options.Syntax);

            //this.EnableDiagnostics(DiagnosticOptions.Semantic | DiagnosticOptions.Syntax);

            GetService<IDiagnosticService>().DiagnosticsUpdated += OnDiagnosticsUpdated;
        }

        public void DisposeNodes()
        {
            foreach (var node in _buildNodes)
            {
                node.Dispose();
            }

            _buildNodes.Dispose();
        }

        public async Task InitialiseBuildNodesAsync(bool returnAfter1Ready = false)
        {
            var tasks = new List<Task>();

            for (int i = 0; i < Environment.ProcessorCount; i++)
            {
                var newNode = new MSBuildHost(DotNetCliService.Instance.Info.BasePath, i + 1);

                tasks.Add(newNode.EnsureConnectionAsync().ContinueWith(t =>
                {
                    _buildNodes.Add(newNode);
                }));
            }

            if (returnAfter1Ready)
            {
                await Task.WhenAny(tasks);
            }
            else
            {
                await Task.WhenAll(tasks);
            }
        }

        public static Task<RoslynWorkspace> CreateWorkspaceAsync(ISolution solution)
        {
            return Task.Run(() =>
            {
                lock (s_solutionWorkspaces)
                {
                    if (!s_solutionWorkspaces.ContainsKey(solution))
                    {
                        //await PackageManager.EnsurePackage("AvalonStudio.Languages.CSharp", IoC.Get<IConsole>());

                        //var dotnetDirectory = Path.Combine(PackageManager.GetPackageDirectory("AvalonStudio.Languages.CSharp"), "content");

                        var currentDir = Platforms.Platform.ExecutionPath;

                        var loadedAssemblies = AppDomain.CurrentDomain.GetAssemblies();

                        var assemblies = new[]
                        {
                    // Microsoft.CodeAnalysis.Workspaces
                    typeof(WorkspacesResources).GetTypeInfo().Assembly,
                    // Microsoft.CodeAnalysis.CSharp.Workspaces 
                    typeof(CSharpWorkspaceResources).GetTypeInfo().Assembly,
                    // Microsoft.CodeAnalysis.Features
                    typeof(FeaturesResources).GetTypeInfo().Assembly,
                    // Microsoft.CodeAnalysis.CSharp.Features
                    typeof(CSharpFeaturesResources).GetTypeInfo().Assembly,
                    typeof(RoslynWorkspace).Assembly,
                };

                        var partTypes = MefHostServices.DefaultAssemblies.Concat(assemblies)
                                .Distinct()
                                .SelectMany(x => x.GetTypes())
                                .ToArray();

                        var compositionContext = new ContainerConfiguration()
                            .WithParts(partTypes)
                            .CreateContainer();

                        var host = MefHostServices.Create(compositionContext);

                        var workspace = new RoslynWorkspace(host, null, compositionContext, DotNetCliService.Instance.Info.Executable, DotNetCliService.Instance.Info.BasePath);

                        compositionContext.GetExport<ICodeFixService>();
                        var diagnosticService = compositionContext.GetExport<IDiagnosticService>();

                        // TODO implement IPickMemberService.

                        workspace.RegisterWorkspace(solution);

                        workspace.InitialiseBuildNodesAsync(true).Wait();
                    }

                    return s_solutionWorkspaces[solution];
                }
            });
        }

        public static void DisposeWorkspace(ISolution solution)
        {
            lock (s_solutionWorkspaces)
            {
                if (s_solutionWorkspaces.ContainsKey(solution))
                {
                    var workspace = s_solutionWorkspaces[solution];

                    s_solutionWorkspaces.Remove(solution);

                    workspace.Dispose();

                    workspace.DisposeNodes();
                }
            }
        }

        public static RoslynWorkspace GetWorkspace(ISolution solution, bool create = true)
        {
            lock (s_solutionWorkspaces)
            {
                if (!s_solutionWorkspaces.ContainsKey(solution))
                {
                    if (!create)
                    {
                        return null;
                    }

                    //await PackageManager.EnsurePackage("AvalonStudio.Languages.CSharp", IoC.Get<IConsole>());

                    //var dotnetDirectory = Path.Combine(PackageManager.GetPackageDirectory("AvalonStudio.Languages.CSharp"), "content");

                    var currentDir = Platforms.Platform.ExecutionPath;

                    var loadedAssemblies = AppDomain.CurrentDomain.GetAssemblies();

                    var assemblies = new[]
                    {
                    // Microsoft.CodeAnalysis.Workspaces
                    typeof(WorkspacesResources).GetTypeInfo().Assembly,
                    // Microsoft.CodeAnalysis.CSharp.Workspaces 
                    typeof(CSharpWorkspaceResources).GetTypeInfo().Assembly,
                    // Microsoft.CodeAnalysis.Features
                    typeof(FeaturesResources).GetTypeInfo().Assembly,
                    // Microsoft.CodeAnalysis.CSharp.Features
                    typeof(CSharpFeaturesResources).GetTypeInfo().Assembly,
                    typeof(RoslynWorkspace).Assembly,
                };

                    var partTypes = MefHostServices.DefaultAssemblies.Concat(assemblies)
                            .Distinct()
                            .SelectMany(x => x.GetTypes())
                            .ToArray();

                    var compositionContext = new ContainerConfiguration()
                        .WithParts(partTypes)
                        .CreateContainer();

                    var host = MefHostServices.Create(compositionContext);

                    var workspace = new RoslynWorkspace(host, null, compositionContext, DotNetCliService.Instance.Info.Executable, DotNetCliService.Instance.Info.BasePath);

                    compositionContext.GetExport<ICodeFixService>();

                    workspace.RegisterWorkspace(solution);

                    workspace.InitialiseBuildNodesAsync(true).Wait();
                }

                return s_solutionWorkspaces[solution];
            }
        }

        internal RoslynWorkspace RegisterWorkspace(ISolution solution) => s_solutionWorkspaces[solution] = this;

        private void OnDiagnosticsUpdated(object sender, DiagnosticsUpdatedArgs diagnosticsUpdatedArgs)
        {
            var documentId = diagnosticsUpdatedArgs?.DocumentId;

            if (documentId == null) return;

            if (_diagnosticsUpdatedNotifiers.TryGetValue(documentId, out var notifier))
            {
                notifier(diagnosticsUpdatedArgs);
            }
        }

        public TService GetService<TService>()
        {
            return _compositionContext.GetExport<TService>();
        }

        public async Task<(Project project, List<string> projectReferences, string targetPath)> AddProject(string solutionDir, string projectFile)
        {
            var buildHost = _buildNodes.Take();

            var (info, projectReferences, targetPath) = await buildHost.LoadProject(solutionDir, projectFile);

            _buildNodes.Add(buildHost);

            if (info != null)
            {
                OnProjectAdded(info);
                return (CurrentSolution.GetProject(info.Id), projectReferences, targetPath);
            }
            else
            {
                return (null, projectReferences, targetPath);
            }
        }

        public async Task<(Project project, List<string> projectReferences, string targetPath)> ReevaluateProject(IProject project)
        {
            var proj = project as OmniSharpProject;

            var buildHost = _buildNodes.Take();

            var (info, projectReferences, targetPath) = await buildHost.LoadProject(project.Solution.CurrentDirectory, project.Location, proj.RoslynProject.Id);

            _buildNodes.Add(buildHost);

            if(info != null)
            {
                return (CurrentSolution.GetProject(info.Id), projectReferences, targetPath);
            }
            else
            {
                return (null, projectReferences, targetPath);
            }
        }

        public ProjectId GetProjectId(IProject project)
        {
            var projects = CurrentSolution.Projects.Where(p => p.FilePath.CompareFilePath(Path.Combine(project.Location)) == 0);

            if (projects.Count() != 1)
            {
                throw new NotImplementedException();
            }

            return projects.First().Id;
        }

        public Project GetProject(IProject project)
        {
            var projects = CurrentSolution.Projects.Where(p => p.FilePath.CompareFilePath(Path.Combine(project.Location)) == 0);

            if (projects.Count() != 1)
            {
                throw new NotImplementedException();
            }

            return projects.First();
        }

        private void ResolveChildReferences(ProjectId project, ProjectId reference)
        {
            var refer = CurrentSolution.GetProject(reference);

            foreach (var child in refer.AllProjectReferences)
            {
                var proj = CurrentSolution.GetProject(project);

                if (!proj.AllProjectReferences.Contains(child))
                {
                    OnProjectReferenceAdded(project, child);
                }

                ResolveChildReferences(project, child.ProjectId);
            }
        }

        public bool ResolveReference(AvalonStudio.Projects.IProject project, string reference)
        {
            var referencePath = Path.Combine(project.LocationDirectory, reference).NormalizePath();
            var projects = CurrentSolution.Projects.Where(p => p.FilePath.CompareFilePath(referencePath) == 0);

            if (projects.Count() == 1)
            {
                var referencedProject = projects.First();

                var proj = GetProject(project);

                var newRef = new ProjectReference(referencedProject.Id);

                if (!proj.AllProjectReferences.Contains(newRef))
                {
                    OnProjectReferenceAdded(proj.Id, newRef);
                }

                ResolveChildReferences(proj.Id, referencedProject.Id);

                return true;
            }
            else
            {
                return false;
            }
        }

        public DocumentId AddDocument(Project project, ISourceFile file)
        {
            var id = DocumentId.CreateNewId(project.Id);
            OnDocumentAdded(DocumentInfo.Create(id, file.Name, filePath: file.FilePath, loader: new FileTextLoader(file, System.Text.Encoding.UTF8)));

            return id;
        }

        public void RemoveDocument (Project project, ISourceFile file)
        {
            var id = GetDocumentId(file);
            OnDocumentRemoved(id);
        }

        public DocumentId GetDocumentId(ISourceFile file)
        {
            var ids = CurrentSolution.GetDocumentIdsWithFilePath(file.Location);

            if (ids.Length != 1)
            {
                throw new NotImplementedException();
            }

            return ids.First();
        }

        public Document GetDocument(ISourceFile file)
        {
            var documentId = GetDocumentId(file);

            return CurrentSolution.GetDocument(documentId);
        }

        public void OpenDocument(ISourceFile file, AvalonEditTextContainer textContainer, Action<DiagnosticsUpdatedArgs> onDiagnosticsUpdated, Action<SourceText> onTextUpdated = null)
        {
            var documentId = GetDocumentId(file);

            var document = GetDocument(file);

            OnDocumentOpened(documentId, textContainer);
            OnDocumentContextUpdated(documentId);

            _diagnosticsUpdatedNotifiers[documentId] = onDiagnosticsUpdated;
            _openDocumentTextLoaders.Add(documentId, textContainer);

            if (onTextUpdated != null)
            {
                ApplyingTextChange += (d, s) =>
                {
                    if (documentId == d)
                    {
                        onTextUpdated(s);
                    }
                };
            }
        }

        public void CloseDocument(AvalonStudio.Projects.ISourceFile file)
        {
            var documentId = GetDocumentId(file);
            var textContainer = _openDocumentTextLoaders[documentId];

            var doc = GetDocument(file);

            var text = doc.GetTextAsync(CancellationToken.None).WaitAndGetResult_CanCallOnBackground(CancellationToken.None);
            var version = doc.GetTextVersionAsync(CancellationToken.None).WaitAndGetResult_CanCallOnBackground(CancellationToken.None);

            _openDocumentTextLoaders.Remove(documentId);
            _diagnosticsUpdatedNotifiers.TryRemove(documentId, out Action<DiagnosticsUpdatedArgs> value);
            textContainer.Dispose();

            OnDocumentClosed(documentId, TextLoader.From(TextAndVersion.Create(text, version, doc.FilePath)));
        }

        public new void SetCurrentSolution(Solution solution)
        {
            var oldSolution = CurrentSolution;
            var newSolution = base.SetCurrentSolution(solution);
            RaiseWorkspaceChangedEventAsync(WorkspaceChangeKind.SolutionChanged, oldSolution, newSolution);
        }

        public override bool CanOpenDocuments => true;

        public event Action<DocumentId, SourceText> ApplyingTextChange;

        public override bool CanApplyChange(ApplyChangesKind feature)
        {
            switch (feature)
            {
                case ApplyChangesKind.ChangeDocument:
                    return true;
                default:
                    return false;
            }
        }

        protected override void Dispose(bool finalize)
        {
            base.Dispose(finalize);

            ApplyingTextChange = null;

            GetService<IDiagnosticService>().DiagnosticsUpdated -= OnDiagnosticsUpdated;

            this.DisableDiagnostics();
        }

        public AvalonEditTextContainer GetContainer (DocumentId documentId)
        {
            if (_openDocumentTextLoaders.ContainsKey(documentId))
            {
                return _openDocumentTextLoaders[documentId];
            }

            return null;
        }

        protected override void ApplyDocumentTextChanged(DocumentId documentId, SourceText text)
        {
            if (_openDocumentTextLoaders.ContainsKey(documentId))
            {
                _openDocumentTextLoaders[documentId].UpdateText(text);
            }
            else
            {
                var document = this.CurrentSolution.GetDocument(documentId);
                if (document != null)
                {
                    Encoding encoding = DetermineEncoding(text, document);

                    SaveDocumentText(documentId, document.FilePath, text, encoding ?? new UTF8Encoding(encoderShouldEmitUTF8Identifier: false));
                    OnDocumentTextChanged(documentId, text, PreservationMode.PreserveValue);
                }
            }

            ApplyingTextChange?.Invoke(documentId, text);

            OnDocumentTextChanged(documentId, text, PreservationMode.PreserveIdentity);
        }

        private static Encoding DetermineEncoding(SourceText text, Document document)
        {
            if (text.Encoding != null)
            {
                return text.Encoding;
            }

            try
            {
                using (ExceptionHelpers.SuppressFailFast())
                {
                    using (var stream = new FileStream(document.FilePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                    {
                        var onDiskText = EncodedStringText.Create(stream);
                        return onDiskText.Encoding;
                    }
                }
            }
            catch (IOException)
            {
            }
            catch (InvalidDataException)
            {
            }

            return null;
        }

        private void SaveDocumentText(DocumentId id, string fullPath, SourceText newText, Encoding encoding)
        {
            try
            {
                using (ExceptionHelpers.SuppressFailFast())
                {
                    var dir = Path.GetDirectoryName(fullPath);
                    if (!Directory.Exists(dir))
                    {
                        Directory.CreateDirectory(dir);
                    }

                    Debug.Assert(encoding != null);
                    using (var writer = new StreamWriter(fullPath, append: false, encoding: encoding))
                    {
                        newText.Write(writer);
                    }
                }
            }
            catch (IOException exception)
            {
                this.OnWorkspaceFailed(new DocumentDiagnostic(WorkspaceDiagnosticKind.Failure, exception.Message, id));
            }
        }

        public new void ClearSolution()
        {
            base.ClearSolution();
        }

        internal void ClearOpenDocument(DocumentId documentId)
        {
            base.ClearOpenDocument(documentId);
        }

        internal new void RegisterText(SourceTextContainer textContainer)
        {
            base.RegisterText(textContainer);
        }

        internal new void UnregisterText(SourceTextContainer textContainer)
        {
            base.UnregisterText(textContainer);
        }
    }
}
