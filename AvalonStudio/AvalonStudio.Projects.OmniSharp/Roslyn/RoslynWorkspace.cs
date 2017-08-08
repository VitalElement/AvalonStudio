// Copyright (c) Microsoft.  All Rights Reserved.  Licensed under the Apache License, Version 2.0.  See License.txt in the project root for license information.

using AsyncRpc;
using AsyncRpc.Transport.Tcp;
using AvalonStudio.CommandLineTools;
using AvalonStudio.Extensibility;
using AvalonStudio.MSBuildHost;
using AvalonStudio.Platforms;
using AvalonStudio.Projects.OmniSharp.MSBuild;
using AvalonStudio.Projects.OmniSharp.Roslyn;
using AvalonStudio.Utils;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
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
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Threading.Tasks;

namespace RoslynPad.Roslyn
{
    public sealed class RoslynWorkspace : Workspace
    {
        private CompositionHost _compositionContext;
        private readonly NuGetConfiguration _nuGetConfiguration;
        private readonly Dictionary<DocumentId, AvalonEditTextContainer> _openDocumentTextLoaders;
        private readonly ConcurrentDictionary<DocumentId, Action<DiagnosticsUpdatedArgs>> _diagnosticsUpdatedNotifiers;
        private MSBuildHost buildHost;
        private readonly string sdkPath;

        internal RoslynWorkspace(HostServices host, NuGetConfiguration nuGetConfiguration, CompositionHost compositionContext, string sdkPath)
            : base(host, WorkspaceKind.Host)
        {
            this.sdkPath = sdkPath;
            _nuGetConfiguration = nuGetConfiguration;

            _openDocumentTextLoaders = new Dictionary<DocumentId, AvalonEditTextContainer>();
            _diagnosticsUpdatedNotifiers = new ConcurrentDictionary<DocumentId, Action<DiagnosticsUpdatedArgs>>();

            _compositionContext = compositionContext;
            GetService<IDiagnosticService>().DiagnosticsUpdated += OnDiagnosticsUpdated;

            this.EnableDiagnostics(DiagnosticOptions.Semantic | DiagnosticOptions.Syntax);
        }

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
                await buildHost.Connect(sdkPath);
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
            var projects = CurrentSolution.Projects.Where(p => p.FilePath.CompareFilePath(Path.Combine(project.LocationDirectory, reference)) == 0);

            if (projects.Count() != 1)
            {
                throw new NotImplementedException();
            }

            var referencedProject = projects.First();

            var proj = GetProject(project);

            var newRef = new ProjectReference(referencedProject.Id);

            if (!proj.AllProjectReferences.Contains(newRef))
            {
                OnProjectReferenceAdded(proj.Id, newRef);
            }

            ResolveChildReferences(proj.Id, referencedProject.Id);
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

        public void OpenDocument(AvalonStudio.Projects.ISourceFile file, AvalonEditTextContainer textContainer, Action<DiagnosticsUpdatedArgs> onDiagnosticsUpdated)
        {
            var documentId = GetDocumentId(file);

            var document = GetDocument(file);

            OnDocumentOpened(documentId, textContainer);
            OnDocumentContextUpdated(documentId);

            _diagnosticsUpdatedNotifiers[documentId] = onDiagnosticsUpdated;
            _openDocumentTextLoaders.Add(documentId, textContainer);
        }

        public void CloseDocument(AvalonStudio.Projects.ISourceFile file)
        {
            var documentId = GetDocumentId(file);
            var textContainer = _openDocumentTextLoaders[documentId];

            _openDocumentTextLoaders.Remove(documentId);
            _diagnosticsUpdatedNotifiers.Remove(documentId, out Action<DiagnosticsUpdatedArgs> value);

            OnDocumentClosed(documentId, TextLoader.From(textContainer, VersionStamp.Default));
        }

        public new void SetCurrentSolution(Solution solution)
        {
            var oldSolution = CurrentSolution;
            var newSolution = base.SetCurrentSolution(solution);
            RaiseWorkspaceChangedEventAsync(WorkspaceChangeKind.SolutionChanged, oldSolution, newSolution);
        }

        public override bool CanOpenDocuments => true;

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
