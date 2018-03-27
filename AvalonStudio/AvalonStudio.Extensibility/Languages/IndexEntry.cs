using AvaloniaEdit.Document;
using AvalonStudio.Extensibility.Utils;
using AvalonStudio.Languages;
using ReactiveUI;
using System;
using System.Collections;

namespace AvalonStudio.Extensibility.Languages
{
    public class IndexEntry : TextSegment, IComparable<IndexEntry>
    {
        public IndexEntry(string spelling, int offset, int endOffset, CursorKind kind)
        {
            Spelling = spelling;
            Kind = kind;
            StartOffset = offset;
            EndOffset = endOffset;
        }

        /// <summary>
        /// Gets whether <paramref name="segment"/> fully contains the specified segment.
        /// </summary>
        /// <remarks>
        /// Use <c>segment.Contains(offset, 0)</c> to detect whether a segment (end inclusive) contains offset;
        /// use <c>segment.Contains(offset, 1)</c> to detect whether a segment (end exclusive) contains offset.
        /// </remarks>
        public bool Contains(int offset, int length)
        {
            return StartOffset <= offset && offset + length < EndOffset;
        }

        /// <summary>
        /// Gets whether <paramref name="thisSegment"/> fully contains the specified segment.
        /// </summary>
        public bool Contains(ISegment segment)
        {
            return StartOffset <= segment.Offset && segment.EndOffset <= EndOffset;
        }

        public string Spelling { get; set; }

        public CursorKind Kind { get; set; }

        public int CompareTo(IndexEntry other)
        {
            if(other.StartOffset <= StartOffset && other.EndOffset >= EndOffset)
            {
                return Length.CompareTo(other.Length);
            }
            else
            {
                return StartOffset.CompareTo(other.StartOffset);
            }
        }
    }

    public class IndexTree
    {
        private TreeNode<IndexEntry> _root;
        private TreeNode<IndexEntry> _navigationTree;        

        public IndexTree()
        {
            _root = new TreeNode<IndexEntry>(new IndexEntry("", 0, int.MaxValue, CursorKind.TranslationUnit));
            _navigationTree = new TreeNode<IndexEntry>(new IndexEntry("", 0, int.MaxValue, CursorKind.TranslationUnit));
        }

        public void Add(IndexEntry newEntry)
        {
            var node = _root.FindLowestTreeNode(entry => entry.Data.Contains(newEntry));

            node.AddChild(newEntry);

            switch(newEntry.Kind)
            {
                case CursorKind.Namespace:
                    node = _navigationTree;
                    break;
                
                case CursorKind.ClassDeclaration:
                    node = _navigationTree.FindLowestTreeNode(entry => entry.Data.Contains(newEntry) && (entry.Data.Kind == CursorKind.Namespace || entry.Data.Kind == CursorKind.TranslationUnit));
                    break;

                default:
                    node = _navigationTree.FindTreeNode(entry => entry.Data.Contains(newEntry) && (entry.Data.Kind != CursorKind.TranslationUnit && entry.Data.Kind != CursorKind.Namespace));
                    break;
            }            

            node.AddChild(newEntry);
        }

        public TreeNode<IndexEntry> FullTree => _root;

        public TreeNode<IndexEntry> NavigationTree => _navigationTree;
    }
        

}