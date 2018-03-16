using System;
using System.IO;

namespace AvalonStudio.Projects
{
    public interface ISourceFile : IProjectItem, IComparable<ISourceFile>, IDeleteable, IComparable<string>
    {
        string ContentType { get; }

        string FilePath { get; }
        string Extension { get; }
        string CurrentDirectory { get; }
        string Location { get; }

        void RaiseFileModifiedEvent();

        Stream OpenText();

        event EventHandler FileModifiedExternally;
    }
}