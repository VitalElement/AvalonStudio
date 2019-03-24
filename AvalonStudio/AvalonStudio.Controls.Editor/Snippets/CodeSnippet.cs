using System;

namespace AvalonStudio.Controls.Editor.Snippets
{
    public class CodeSnippet : IComparable<CodeSnippet>
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string Snippet { get; set; }

        public int CompareTo(CodeSnippet other)
        {
            return Name.CompareTo(other.Name);
        }
    }
}
