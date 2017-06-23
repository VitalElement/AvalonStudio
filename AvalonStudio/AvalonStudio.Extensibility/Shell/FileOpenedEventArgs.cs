using AvalonStudio.Documents;
using AvalonStudio.Projects;
using System;

namespace AvalonStudio.Shell
{
    public class FileOpenedEventArgs : EventArgs
    {
        public ISourceFile File { get; set; }
        public IEditor Editor { get; set; }
    }
}
