namespace AvalonStudio.TextEditor.Document
{
    using LineNode = DocumentLine;

    // A tree node in the document line tree.
    // For the purpose of the invariants, "children", "descendents", "siblings" etc. include the DocumentLine object,
    // it is treated as a third child node between left and right.

    // Originally, this was a separate class, with a reference to the documentLine. The documentLine had a reference
    // back to the node. To save memory, the same object is used for both the documentLine and the line node.
    // This saves 16 bytes per line (8 byte object overhead + two pointers).
    //	sealed class LineNode
    //	{
    //		internal readonly DocumentLine documentLine;
    partial class DocumentLine
    {
        internal bool color;
        internal DocumentLine left, right, parent;

        /// <summary>
        ///     The number of lines in this node and its child nodes.
        ///     Invariant:
        ///     nodeTotalCount = 1 + left.nodeTotalCount + right.nodeTotalCount
        /// </summary>
        internal int nodeTotalCount;

        /// <summary>
        ///     The total text length of this node and its child nodes.
        ///     Invariant:
        ///     nodeTotalLength = left.nodeTotalLength + documentLine.TotalLength + right.nodeTotalLength
        /// </summary>
        internal int nodeTotalLength;

        internal LineNode LeftMost
        {
            get
            {
                var node = this;
                while (node.left != null)
                    node = node.left;
                return node;
            }
        }

        internal LineNode RightMost
        {
            get
            {
                var node = this;
                while (node.right != null)
                    node = node.right;
                return node;
            }
        }

        // optimization note: I tried packing color and isDeleted into a single byte field, but that
        // actually increased the memory requirements. The JIT packs two bools and a byte (delimiterSize)
        // into a single DWORD, but two bytes get each their own DWORD. Three bytes end up in the same DWORD, so
        // apparently the JIT only optimizes for memory when there are at least three small fields.
        // Currently, DocumentLine takes 36 bytes on x86 (8 byte object overhead, 3 pointers, 3 ints, and another DWORD
        // for the small fields).
        // TODO: a possible optimization would be to combine 'totalLength' and the small fields into a single uint.
        // delimiterSize takes only two bits, the two bools take another two bits; so there's still
        // 28 bits left for totalLength. 268435455 characters per line should be enough for everyone :)

        /// <summary>
        ///     Resets the line to enable its reuse after a document rebuild.
        /// </summary>
        internal void ResetLine()
        {
            totalLength = delimiterLength = 0;
            isDeleted = color = false;
            left = right = parent = null;
        }

        internal LineNode InitLineNode()
        {
            nodeTotalCount = 1;
            nodeTotalLength = TotalLength;
            return this;
        }
    }
}