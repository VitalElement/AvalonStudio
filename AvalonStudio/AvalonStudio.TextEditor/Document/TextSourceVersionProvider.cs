using AvalonStudio.TextEditor.Utils;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace AvalonStudio.TextEditor.Document
{
#if !NREFACTORY

    /// <summary>
    ///     Provides ITextSourceVersion instances.
    /// </summary>
    public class TextSourceVersionProvider
    {
        private Version currentVersion;

        /// <summary>
        ///     Creates a new TextSourceVersionProvider instance.
        /// </summary>
        public TextSourceVersionProvider()
        {
            currentVersion = new Version(this);
        }

        /// <summary>
        ///     Gets the current version.
        /// </summary>
        public ITextSourceVersion CurrentVersion
        {
            get { return currentVersion; }
        }

        /// <summary>
        ///     Replaces the current version with a new version.
        /// </summary>
        /// <param name="change">Change from current version to new version</param>
        public void AppendChange(TextChangeEventArgs change)
        {
            if (change == null)
                throw new ArgumentNullException("change");
            currentVersion.change = change;
            currentVersion.next = new Version(currentVersion);
            currentVersion = currentVersion.next;
        }

        [DebuggerDisplay("Version #{id}")]
        private sealed class Version : ITextSourceVersion
        {
            // ID used for CompareAge()
            private readonly int id;

            // Reference back to the provider.
            // Used to determine if two checkpoints belong to the same document.
            private readonly TextSourceVersionProvider provider;

            // the change from this version to the next version
            internal TextChangeEventArgs change;

            internal Version next;

            internal Version(TextSourceVersionProvider provider)
            {
                this.provider = provider;
            }

            internal Version(Version prev)
            {
                provider = prev.provider;
                id = unchecked(prev.id + 1);
            }

            public bool BelongsToSameDocumentAs(ITextSourceVersion other)
            {
                var o = other as Version;
                return o != null && provider == o.provider;
            }

            public int CompareAge(ITextSourceVersion other)
            {
                if (other == null)
                    throw new ArgumentNullException("other");
                var o = other as Version;
                if (o == null || provider != o.provider)
                    throw new ArgumentException("Versions do not belong to the same document.");
                // We will allow overflows, but assume that the maximum distance between checkpoints is 2^31-1.
                // This is guaranteed on x86 because so many checkpoints don't fit into memory.
                return Math.Sign(unchecked(id - o.id));
            }

            public IEnumerable<TextChangeEventArgs> GetChangesTo(ITextSourceVersion other)
            {
                var result = CompareAge(other);
                var o = (Version)other;
                if (result < 0)
                    return GetForwardChanges(o);
                if (result > 0)
                    return o.GetForwardChanges(this).Reverse().Select(change => change.Invert());
                return Empty<TextChangeEventArgs>.Array;
            }

            public int MoveOffsetTo(ITextSourceVersion other, int oldOffset, AnchorMovementType movement)
            {
                var offset = oldOffset;
                foreach (var e in GetChangesTo(other))
                {
                    offset = e.GetNewOffset(offset, movement);
                }
                return offset;
            }

            private IEnumerable<TextChangeEventArgs> GetForwardChanges(Version other)
            {
                // Return changes from this(inclusive) to other(exclusive).
                for (var node = this; node != other; node = node.next)
                {
                    yield return node.change;
                }
            }
        }
    }

#endif
}