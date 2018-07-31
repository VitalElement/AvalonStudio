using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Text;
using System;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AvalonStudio.Projects.OmniSharp.Roslyn
{
    class FileTextLoader : TextLoader
    {
        private readonly ISourceFile _file;
        private readonly Encoding _defaultEncoding;

        public FileTextLoader(ISourceFile file, Encoding defaultEncoding)
        {
            _file = file;
            _defaultEncoding = defaultEncoding;
        }

        protected virtual SourceText CreateText(Stream stream, Workspace workspace)
        {
            return SourceText.From(stream, _defaultEncoding);
        }

        public override Task<TextAndVersion> LoadTextAndVersionAsync(Workspace workspace, DocumentId documentId, CancellationToken cancellationToken)
        {
            var fileInfo = new System.IO.FileInfo(_file.Location);
            DateTime prevLastWriteTime = fileInfo.LastWriteTimeUtc;

            TextAndVersion textAndVersion;

            // Open file for reading with FileShare mode read/write/delete so that we do not lock this file.
            using (var stream = _file.OpenText())
            {
                var version = VersionStamp.Create(prevLastWriteTime);
                var text = CreateText(stream, workspace);
                textAndVersion = TextAndVersion.Create(text, version, _file.Location);
            }

            // Check if the file was definitely modified and closed while we were reading. In this case, we know the read we got was
            // probably invalid, so throw an IOException which indicates to our caller that we should automatically attempt a re-read.
            // If the file hasn't been closed yet and there's another writer, we will rely on file change notifications to notify us
            // and reload the file.
            fileInfo = new System.IO.FileInfo(_file.Location);
            DateTime newLastWriteTime = fileInfo.LastWriteTimeUtc;
            if (!newLastWriteTime.Equals(prevLastWriteTime))
            {
                var message = string.Format(WorkspacesResources.File_was_externally_modified_colon_0, _file.Location);
                throw new IOException(message);
            }

            return Task.FromResult(textAndVersion);
        }
    }
}
