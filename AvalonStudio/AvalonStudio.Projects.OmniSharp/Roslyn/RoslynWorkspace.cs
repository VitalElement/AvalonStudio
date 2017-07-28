// Copyright (c) Microsoft.  All Rights Reserved.  Licensed under the Apache License, Version 2.0.  See License.txt in the project root for license information.

using AsyncRpc;
using AsyncRpc.Transport.Tcp;
using AvalonStudio.MSBuildHost;
using AvalonStudio.Projects.OmniSharp.Roslyn;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Host;
using Microsoft.CodeAnalysis.Text;
using RoslynPad.Roslyn.Diagnostics;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace RoslynPad.Roslyn
{
    public sealed class RoslynWorkspace : Workspace
    {
        private readonly NuGetConfiguration _nuGetConfiguration;
        private readonly ConcurrentDictionary<string, DirectiveInfo> _referencesDirectives;
        private readonly Dictionary<DocumentId, SourceTextContainer> _openDocumentTextLoaders;

        private IMsBuildHostService msBuildHostService;
        public DocumentId OpenDocumentId { get; private set; }


        internal RoslynWorkspace(HostServices host, NuGetConfiguration nuGetConfiguration)
            : base(host, WorkspaceKind.Host)
        {
            _nuGetConfiguration = nuGetConfiguration;
            _referencesDirectives = new ConcurrentDictionary<string, DirectiveInfo>();

            msBuildHostService = new Engine().CreateProxy<IMsBuildHostService>(new TcpClientTransport(IPAddress.Loopback, 9000));

            _openDocumentTextLoaders = new Dictionary<DocumentId, SourceTextContainer>();
        }

        public async Task<Project> AddProject(string projectFile)
        {            
            var res = await msBuildHostService.GetVersion();

            var refs = await msBuildHostService.GetTaskItem("ResolveAssemblyReferences", projectFile);

            var id = ProjectId.CreateNewId();
            OnProjectAdded(ProjectInfo.Create(id, VersionStamp.Create(), Path.GetFileNameWithoutExtension(projectFile), "", LanguageNames.CSharp, projectFile));

            foreach (var reference in refs.Data.Items)
            {
                OnMetadataReferenceAdded(id, MetadataReference.CreateFromFile(reference.ItemSpec));
            }

            return CurrentSolution.GetProject(id);
        }

        public DocumentId AddDocument(Project project, AvalonStudio.Projects.ISourceFile file)
        {   
            var id = DocumentId.CreateNewId(project.Id);
            OnDocumentAdded(DocumentInfo.Create(id, file.Name, filePath: file.FilePath, loader: new FileTextLoader(file.FilePath, System.Text.Encoding.UTF8)));

            return id;
        }

        public DocumentId GetDocumentId (AvalonStudio.Projects.ISourceFile file)
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

        public void OpenDocument(AvalonStudio.Projects.ISourceFile file, SourceTextContainer textContainer, Action<DiagnosticsUpdatedArgs> onDiagnosticsUpdated, Action<SourceText> onTextUpdated)
        {
            var documentId = GetDocumentId(file);

            if (onTextUpdated != null)
            {
                ApplyingTextChange += (d, s) => onTextUpdated(s);
            }

            OnDocumentOpened(documentId, textContainer);
            OnDocumentContextUpdated(documentId);

            _openDocumentTextLoaders.Add(documentId, textContainer);
        }

        public void CloseDocument(AvalonStudio.Projects.ISourceFile file)
        {
            var documentId = GetDocumentId(file);
            var textContainer = _openDocumentTextLoaders[documentId];

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

        public event Action<DocumentId, SourceText> ApplyingTextChange;

        protected override void Dispose(bool finalize)
        {
            base.Dispose(finalize);

            ApplyingTextChange = null;
        }

        protected override void ApplyDocumentTextChanged(DocumentId document, SourceText newText)
        {
            if (OpenDocumentId != document)
            {
                return;
            }

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

        public ImmutableArray<string> ReferencesDirectives => _referencesDirectives.Select(x => x.Key).ToImmutableArray();

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

        private MetadataReference ResolveReference(string name)
        {
            if (_nuGetConfiguration != null)
            {
                name = _nuGetConfiguration.ResolveReference(name);
            }
            if (File.Exists(name))
            {
                //return RoslynHost.CreateMetadataReference(name);
            }
            try
            {
                // var assemblyName = GlobalAssemblyCache.Instance.ResolvePartialName(name);
                /* if (assemblyName == null)
                 {
                     return null;
                 }
                 var assembly = Assembly.Load(assemblyName.ToString());*/
                // return RoslynHost.CreateMetadataReference(assembly.Location);
                throw new NotImplementedException();
            }
            catch (Exception)
            {
                return null;
            }
        }

        public bool HasReference(string text)
        {
            DirectiveInfo info;
            if (_referencesDirectives.TryGetValue(text, out info))
            {
                return info.IsActive;
            }
            return false;
        }
    }
}
