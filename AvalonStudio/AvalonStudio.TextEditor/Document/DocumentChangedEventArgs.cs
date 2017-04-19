using System;

namespace AvalonStudio.TextEditor.Document
{
    public class DocumentChangeEventArgs : TextChangeEventArgs
    {
        private volatile OffsetChangeMap offsetChangeMap;

        /// <summary>
        ///     Creates a new DocumentChangeEventArgs object.
        /// </summary>
        public DocumentChangeEventArgs(int offset, string removedText, string insertedText)
            : this(offset, removedText, insertedText, null)
        {
        }

        /// <summary>
        ///     Creates a new DocumentChangeEventArgs object.
        /// </summary>
        public DocumentChangeEventArgs(int offset, string removedText, string insertedText, OffsetChangeMap offsetChangeMap)
            : base(offset, removedText, insertedText)
        {
            SetOffsetChangeMap(offsetChangeMap);
        }

        /// <summary>
        ///     Creates a new DocumentChangeEventArgs object.
        /// </summary>
        public DocumentChangeEventArgs(int offset, ITextSource removedText, ITextSource insertedText,
            OffsetChangeMap offsetChangeMap)
            : base(offset, removedText, insertedText)
        {
            SetOffsetChangeMap(offsetChangeMap);
        }

        /// <summary>
        ///     Gets the OffsetChangeMap associated with this document change.
        /// </summary>
        /// <remarks>The OffsetChangeMap instance is guaranteed to be frozen and thus thread-safe.</remarks>
        public OffsetChangeMap OffsetChangeMap
        {
            get
            {
                var map = offsetChangeMap;
                if (map == null)
                {
                    // create OffsetChangeMap on demand
                    map = OffsetChangeMap.FromSingleElement(CreateSingleChangeMapEntry());
                    offsetChangeMap = map;
                }
                return map;
            }
        }

        /// <summary>
        ///     Gets the OffsetChangeMap, or null if the default offset map (=single replacement) is being used.
        /// </summary>
        internal OffsetChangeMap OffsetChangeMapOrNull
        {
            get { return offsetChangeMap; }
        }

        internal OffsetChangeMapEntry CreateSingleChangeMapEntry()
        {
            return new OffsetChangeMapEntry(Offset, RemovalLength, InsertionLength);
        }

        /// <summary>
        ///     Gets the new offset where the specified offset moves after this document change.
        /// </summary>
        public override int GetNewOffset(int offset, AnchorMovementType movementType = AnchorMovementType.Default)
        {
            if (offsetChangeMap != null)
                return offsetChangeMap.GetNewOffset(offset, movementType);
            return CreateSingleChangeMapEntry().GetNewOffset(offset, movementType);
        }

        private void SetOffsetChangeMap(OffsetChangeMap offsetChangeMap)
        {
            if (offsetChangeMap != null)
            {
                if (!offsetChangeMap.IsFrozen)
                    throw new ArgumentException("The OffsetChangeMap must be frozen before it can be used in DocumentChangeEventArgs");
                if (!offsetChangeMap.IsValidForDocumentChange(Offset, RemovalLength, InsertionLength))
                    throw new ArgumentException("OffsetChangeMap is not valid for this document change", "offsetChangeMap");
                this.offsetChangeMap = offsetChangeMap;
            }
        }

        /// <inheritdoc />
        public override TextChangeEventArgs Invert()
        {
            var map = OffsetChangeMapOrNull;
            if (map != null)
            {
                map = map.Invert();
                map.Freeze();
            }
            return new DocumentChangeEventArgs(Offset, InsertedText, RemovedText, map);
        }
    }
}