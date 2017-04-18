using AvalonStudio.Platforms;
using System;

namespace AvalonStudio.Languages
{
    public class UnsavedFile : IComparable<string>, IComparable<UnsavedFile>
    {
        public readonly string FileName;
        public string Contents;

        public UnsavedFile(string filename, string contents)
        {
            FileName = filename;
            Contents = contents;
        }

        public int CompareTo(string other)
        {
            return FileName.CompareFilePath(other);
        }

        public int CompareTo(UnsavedFile other)
        {
            return FileName.CompareFilePath(other.FileName);
        }
    }
}