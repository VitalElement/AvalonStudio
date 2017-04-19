using System;

namespace AvalonStudio.Projects
{
    public enum Language
    {
        C,
        Cpp
    }

    public interface ISourceFile : IProjectItem, IComparable<ISourceFile>, IComparable<string>
    {
        string FilePath { get; }
        string Extension { get; }
        string CurrentDirectory { get; }
        string Location { get; }

        void RaiseFileModifiedEvent();

        event EventHandler FileModifiedExternally;
    }
}