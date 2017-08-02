using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Text;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AvalonStudio.Projects.OmniSharp.Roslyn
{
    class FileTextLoader : TextLoader
    {
        private readonly string _path;
        private readonly Encoding _defaultEncoding;

        public FileTextLoader(string path, Encoding defaultEncoding)
        {
            _path = path;
            _defaultEncoding = defaultEncoding;
        }

        protected virtual SourceText CreateText(Stream stream, Workspace workspace)
        {
            return SourceText.From(stream, _defaultEncoding);
        }

        public override async Task<TextAndVersion> LoadTextAndVersionAsync(Workspace workspace, DocumentId documentId, CancellationToken cancellationToken)
        {
            System.Console.WriteLine($"Roslyn is Loading file async {_path}");
            var fileInfo = new System.IO.FileInfo(_path);
            DateTime prevLastWriteTime = fileInfo.LastWriteTime;

            TextAndVersion textAndVersion;

            // Open file for reading with FileShare mode read/write/delete so that we do not lock this file.
            using (var stream = new FileStream(_path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite | FileShare.Delete, bufferSize: 4096, useAsync: false))
            {
                var version = VersionStamp.Create(prevLastWriteTime);
                var text = CreateText(stream, workspace);
                textAndVersion = TextAndVersion.Create(text, version, _path);
            }

            // Check if the file was definitely modified and closed while we were reading. In this case, we know the read we got was
            // probably invalid, so throw an IOException which indicates to our caller that we should automatically attempt a re-read.
            // If the file hasn't been closed yet and there's another writer, we will rely on file change notifications to notify us
            // and reload the file.
            fileInfo = new System.IO.FileInfo(_path);
            DateTime newLastWriteTime = fileInfo.LastWriteTime;
            if (!newLastWriteTime.Equals(prevLastWriteTime))
            {
                var message = string.Format(WorkspacesResources.File_was_externally_modified_colon_0, _path);
                throw new IOException(message);
            }

            return textAndVersion;
        }

        internal override TextAndVersion LoadTextAndVersionSynchronously(Workspace workspace, DocumentId documentId, CancellationToken cancellationToken)
        {
            System.Console.WriteLine($"Roslyn is Loading file sync {_path}");
            var fileInfo = new System.IO.FileInfo(_path);
            DateTime prevLastWriteTime = fileInfo.LastWriteTime;

            TextAndVersion textAndVersion;

            // Open file for reading with FileShare mode read/write/delete so that we do not lock this file.
            using (var stream = new FileStream(_path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite | FileShare.Delete, bufferSize: 4096, useAsync: false))
            {
                var version = VersionStamp.Create(prevLastWriteTime);
                var text = CreateText(stream, workspace);
                textAndVersion = TextAndVersion.Create(text, version, _path);
            }

            // Check if the file was definitely modified and closed while we were reading. In this case, we know the read we got was
            // probably invalid, so throw an IOException which indicates to our caller that we should automatically attempt a re-read.
            // If the file hasn't been closed yet and there's another writer, we will rely on file change notifications to notify us
            // and reload the file.
            fileInfo = new System.IO.FileInfo(_path);
            DateTime newLastWriteTime = fileInfo.LastWriteTime;
            if (!newLastWriteTime.Equals(prevLastWriteTime))
            {
                var message = string.Format(WorkspacesResources.File_was_externally_modified_colon_0, _path);
                throw new IOException(message);
            }

            return textAndVersion;
        }

    }
}
