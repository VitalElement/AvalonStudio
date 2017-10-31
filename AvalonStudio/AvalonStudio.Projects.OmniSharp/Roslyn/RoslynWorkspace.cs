// Copyright (c) Microsoft.  All Rights Reserved.  Licensed under the Apache License, Version 2.0.  See License.txt in the project root for license information.

using AvalonStudio.Extensibility;
using AvalonStudio.Packages;
using AvalonStudio.Platforms;
using AvalonStudio.Projects.OmniSharp.DotnetCli;
using AvalonStudio.Projects.OmniSharp.MSBuild;
using AvalonStudio.Projects.OmniSharp.Roslyn;
using AvalonStudio.Utils;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Host;
using Microsoft.CodeAnalysis.Host.Mef;
using Microsoft.CodeAnalysis.Text;
using RoslynPad.Editor.Windows;
using RoslynPad.Roslyn.Diagnostics;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Composition.Hosting;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace RoslynPad.Roslyn
{
    public sealed class RoslynWorkspace : Workspace
    {
        private static Dictionary<AvalonStudio.Projects.ISolution, RoslynWorkspace> s_solutionWorkspaces = new Dictionary<AvalonStudio.Projects.ISolution, RoslynWorkspace>();

        private CompositionHost _compositionContext;
        private readonly NuGetConfiguration _nuGetConfiguration;
        private readonly Dictionary<DocumentId, AvalonEditTextContainer> _openDocumentTextLoaders;
        private readonly ConcurrentDictionary<DocumentId, Action<DiagnosticsUpdatedArgs>> _diagnosticsUpdatedNotifiers;
        private MSBuildHost buildHost;
        private readonly string dotnetPath;
        private readonly string sdkPath;

        internal RoslynWorkspace(HostServices host, NuGetConfiguration nuGetConfiguration, CompositionHost compositionContext, string dotnetPath, string sdkPath)
            : base(host, WorkspaceKind.Host)
        {
            this.dotnetPath = dotnetPath;
            this.sdkPath = sdkPath;
            _nuGetConfiguration = nuGetConfiguration;

            _openDocumentTextLoaders = new Dictionary<DocumentId, AvalonEditTextContainer>();
            _diagnosticsUpdatedNotifiers = new ConcurrentDictionary<DocumentId, Action<DiagnosticsUpdatedArgs>>();

            _compositionContext = compositionContext;
            GetService<IDiagnosticService>().DiagnosticsUpdated += OnDiagnosticsUpdated;

            DiagnosticProvider.Enable(this, DiagnosticProvider.Options.Semantic | DiagnosticProvider.Options.Syntax);
            this.EnableDiagnostics(DiagnosticOptions.Semantic | DiagnosticOptions.Syntax);
        }

        public static RoslynWorkspace GetWorkspace(AvalonStudio.Projects.ISolution solution)
        {
            if (!s_solutionWorkspaces.ContainsKey(solution))
            {

                //await PackageManager.EnsurePackage("AvalonStudio.Languages.CSharp", IoC.Get<IConsole>());

                var dotnetDirectory = Path.Combine(PackageManager.GetPackageDirectory("AvalonStudio.Languages.CSharp"), "content");
                var dotnet = new DotNetCliService(Path.Combine(dotnetDirectory, "dotnet"));

                var dotnetInfo = dotnet.GetInfo();

                var currentDir = AvalonStudio.Platforms.Platform.ExecutionPath;

                var loadedAssemblies = System.AppDomain.CurrentDomain.GetAssemblies();

                var assemblies = new[]
                {
                    loadedAssemblies.First(a=>a.FullName.StartsWith("Microsoft.CodeAnalysis")),
                    loadedAssemblies.First(a=>a.FullName.StartsWith("Microsoft.CodeAnalysis.CSharp")),
                    loadedAssemblies.First(a => a.FullName.StartsWith("Microsoft.CodeAnalysis.Features")),
                    typeof(DiagnosticsService).Assembly,
                };

                var partTypes = MefHostServices.DefaultAssemblies.Concat(assemblies)
                        .Distinct()
                        .SelectMany(x => x.GetTypes())
                        //.Concat(new[] { typeof(DocumentationProviderServiceFactory) })
                        .ToArray();

                var compositionContext = new ContainerConfiguration()
                    .WithParts(partTypes)
                    .CreateContainer();

                var host = MefHostServices.Create(compositionContext);

                var workspace = new RoslynWorkspace(host, null, compositionContext, Path.Combine(dotnetDirectory, "dotnet"), dotnetInfo.BasePath);

                workspace.RegisterWorkspace(solution);
            }

            return s_solutionWorkspaces[solution];
        }

        internal RoslynWorkspace RegisterWorkspace(AvalonStudio.Projects.ISolution solution) => s_solutionWorkspaces[solution] = this;

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

        public async Task<(Project project, List<string> projectReferences)> AddProject(string solutionDir, string projectFile)
        {
            if (buildHost == null)
            {
                buildHost = new MSBuildHost();
                await buildHost.Connect(dotnetPath, sdkPath);
            }

            var loadData = await buildHost.LoadProject(solutionDir, projectFile);

            OnProjectAdded(loadData.info);

            return (CurrentSolution.GetProject(loadData.info.Id), loadData.projectReferences);
        }

        public ProjectId GetProjectId(AvalonStudio.Projects.IProject project)
        {
            var projects = CurrentSolution.Projects.Where(p => p.FilePath.CompareFilePath(Path.Combine(project.Location)) == 0);

            if (projects.Count() != 1)
            {
                throw new NotImplementedException();
            }

            return projects.First().Id;
        }

        public Project GetProject(AvalonStudio.Projects.IProject project)
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

        public void ResolveReference(AvalonStudio.Projects.IProject project, string reference)
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
            }
            else
            {
                // TODO: mark as unresolved.
            }
        }

        public DocumentId AddDocument(Project project, AvalonStudio.Projects.ISourceFile file)
        {
            var id = DocumentId.CreateNewId(project.Id);
            OnDocumentAdded(DocumentInfo.Create(id, file.Name, filePath: file.FilePath, loader: new FileTextLoader(file.FilePath, System.Text.Encoding.UTF8)));

            return id;
        }

        public DocumentId GetDocumentId(AvalonStudio.Projects.ISourceFile file)
        {
            var ids = CurrentSolution.GetDocumentIdsWithFilePath(file.Location);

            if (ids.Length != 1)
            {
                throw new NotImplementedException();
            }

            return ids.First();
        }

        public Document GetDocument(AvalonStudio.Projects.ISourceFile file)
        {
            var documentId = GetDocumentId(file);

            return CurrentSolution.GetDocument(documentId);
        }

        public void OpenDocument(AvalonStudio.Projects.ISourceFile file, AvalonEditTextContainer textContainer, Action<DiagnosticsUpdatedArgs> onDiagnosticsUpdated, Action<SourceText> onTextUpdated)
        {
            var documentId = GetDocumentId(file);

            var document = GetDocument(file);

            OnDocumentOpened(documentId, textContainer);
            OnDocumentContextUpdated(documentId);

            _diagnosticsUpdatedNotifiers[documentId] = onDiagnosticsUpdated;
            _openDocumentTextLoaders.Add(documentId, textContainer);

            if (onTextUpdated != null)
            {
                ApplyingTextChange += (d, s) => onTextUpdated(s);
            }
        }

        public void CloseDocument(AvalonStudio.Projects.ISourceFile file)
        {
            var documentId = GetDocumentId(file);
            var textContainer = _openDocumentTextLoaders[documentId];

            _openDocumentTextLoaders.Remove(documentId);
            _diagnosticsUpdatedNotifiers.TryRemove(documentId, out Action<DiagnosticsUpdatedArgs> value);

            OnDocumentClosed(documentId, TextLoader.From(textContainer, VersionStamp.Default));
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
        }

        protected override void ApplyDocumentTextChanged(DocumentId document, SourceText newText)
        {
            _openDocumentTextLoaders[document].UpdateText(newText);

            ApplyingTextChange?.Invoke(document, newText);

            OnDocumentTextChanged(document, newText, PreservationMode.PreserveIdentity);
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

        private class DirectiveInfo
        {
            public MetadataReference MetadataReference { get; }

            public bool IsActive { get; set; }

            public DirectiveInfo(MetadataReference metadataReference)
            {
                MetadataReference = metadataReference;
                IsActive = true;
            }
        }
    }
}
