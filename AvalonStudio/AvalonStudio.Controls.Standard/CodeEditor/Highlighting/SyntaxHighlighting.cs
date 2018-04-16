using AvaloniaEdit.Document;
using AvalonStudio.Languages;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace AvalonStudio.Controls.Standard.CodeEditor.Highlighting
{
    public class OffsetSyntaxHighlightingData : IComparable<OffsetSyntaxHighlightingData>, ISegment
    {
        public OffsetSyntaxHighlightingData()
        {

        }

        public OffsetSyntaxHighlightingData(ISegment segment)
        {
            Offset = segment.Offset;
            Length = segment.Length;
        }

        public OffsetSyntaxHighlightingData(int offset, int length)
        {
            Offset = offset;
            Length = length;
        }

        public int Offset { get; set; }
        public int Length { get; set; }
        public int EndOffset => Offset + Length;

        public int CompareTo(OffsetSyntaxHighlightingData other)
        {
            if (Offset > other.Offset)
            {
                return 1;
            }

            if (Offset == other.Offset)
            {
                return 0;
            }

            return -1;
        }
    }

    public class HighlightingSegment : OffsetSyntaxHighlightingData
    {
        readonly ScopeStack scopeStack;

        internal ScopeStack ScopeStack
        {
            get
            {
                return scopeStack;
            }
        }

        public string ColorStyleKey
        {
            get
            {
                return scopeStack.IsEmpty ? "" : scopeStack.Peek();
            }
        }

        public HighlightingSegment(int offset, int length, ScopeStack scopeStack) : base(offset, length)
        {
            this.scopeStack = scopeStack;
        }

        public HighlightingSegment(ISegment segment, ScopeStack scopeStack) : base(segment)
        {
            this.scopeStack = scopeStack;
        }

        public HighlightingSegment WithOffsetAndLength(int offset, int length)
        {
            return new HighlightingSegment(offset, length, scopeStack);
        }

        public HighlightingSegment WithOffset(int offset)
        {
            return new HighlightingSegment(offset, Length, scopeStack);
        }

        public HighlightingSegment WithLength(int length)
        {
            return new HighlightingSegment(Offset, length, scopeStack);
        }
    }

    public class LineEventArgs : EventArgs
    {
        readonly IDocumentLine line;

        public IDocumentLine Line
        {
            get
            {
                return line;
            }
        }

        public LineEventArgs(IDocumentLine line)
        {
            if (line == null)
                throw new ArgumentNullException("line");
            this.line = line;
        }
    }


    public sealed class ScopeStack : IEnumerable<string>
    {
        public static readonly ScopeStack Empty = new ScopeStack(null, ImmutableStack<string>.Empty, 0, null);

        public string FirstElement
        {
            get;
            private set;
        }

        public bool IsEmpty
        {
            get
            {
                return stack.IsEmpty;
            }
        }

        public int Count
        {
            get;
            private set;
        }

        ImmutableStack<string> stack;
        ScopeStack parent;

        public ScopeStack(string first)
        {
            FirstElement = first;
            stack = ImmutableStack<string>.Empty.Push(first);
            parent = ScopeStack.Empty;
            Count = 1;
        }

        // The reasoning for having a constructor which takes a parent is that we want to make allocations
        // only onthe less common operation - Push. Having the allocation of the parent ScopeStack happening
        // on Pop, rather than retaining the one on Push, would yield an allocation hot loop, given that:
        // We already allocate it once on push.
        // We pass the same scope stack multiple times.
        ScopeStack(string first, ImmutableStack<string> immutableStack, int count, ScopeStack parent)
        {
            this.FirstElement = first;
            this.stack = immutableStack;
            this.Count = count;
            this.parent = parent;
        }

        public ScopeStack Push(string item)
        {
            return new ScopeStack(FirstElement, stack.Push(item), Count + 1, this);
        }

        public ScopeStack Pop()
        {
            if (parent == null)
                throw new InvalidOperationException("ScopeStack is empty");
            return parent;
        }

        public string Peek()
        {
            return stack.Peek();
        }

        public ImmutableStack<string>.Enumerator GetEnumerator()
        {
            return stack.GetEnumerator();
        }

        IEnumerator<string> IEnumerable<string>.GetEnumerator()
        {
            return ((IEnumerable<string>)stack).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable<string>)stack).GetEnumerator();
        }
    }


    public sealed class HighlightedLine
    {
        public ISegment TextSegment { get; }
        /// <summary>
        /// The segment offsets are 0 at line start regardless of where the line is inside the document.
        /// </summary>
        public IReadOnlyList<HighlightingSegment> Segments { get; private set; }
        public HighlightedLine(ISegment textSegment, IReadOnlyList<HighlightingSegment> segments)
        {
            TextSegment = textSegment;
            Segments = segments;
        }
    }

    /// <summary>
    /// The basic interface for all syntax modes
    /// </summary>
    public interface ISyntaxHighlighting : IDisposable
    {
        /// <summary>
        /// Gets colorized segments (aka chunks) from offset to offset + length.
        /// </summary>
        /// <param name='line'>
        /// The starting line at (offset). This is the same as Document.GetLineByOffset (offset).
        /// </param>
        Task<HighlightedLine> GetHighlightedLineAsync(IDocumentLine line, CancellationToken cancellationToken);
        Task<ScopeStack> GetScopeStackAsync(int offset, CancellationToken cancellationToken);

        event EventHandler<LineEventArgs> HighlightingStateChanged;
    }

    public sealed class DefaultSyntaxHighlighting : ISyntaxHighlighting
    {
        public static readonly DefaultSyntaxHighlighting Instance = new DefaultSyntaxHighlighting();

        DefaultSyntaxHighlighting()
        {
        }

        public Task<HighlightedLine> GetHighlightedLineAsync(IDocumentLine line, CancellationToken cancellationToken)
        {
            return Task.FromResult(new HighlightedLine(line, new[] { new HighlightingSegment(0, line.Length, ScopeStack.Empty) }));
        }

        public Task<ScopeStack> GetScopeStackAsync(int offset, CancellationToken cancellationToken)
        {
            return Task.FromResult(ScopeStack.Empty);
        }

        public event EventHandler<LineEventArgs> HighlightingStateChanged { add { } remove { } }

        public void Dispose()
        {
        }
    }

    public class SyntaxHighlighting : ISyntaxHighlighting
    {
        readonly SyntaxHighlightingDefinition definition;

        public IDocument Document
        {
            get;
            set;
        }

        internal SyntaxHighlightingDefinition Definition
        {
            get
            {
                return definition;
            }
        }

        public SyntaxHighlighting(SyntaxHighlightingDefinition definition, IDocument document)
        {
            this.definition = definition;
            Document = document;

            document.TextChanged += Handle_TextChanged;
        }

        public void Dispose()
        {
            Document.TextChanged -= Handle_TextChanged;
        }

        async void Handle_TextChanged(object sender, TextChangeEventArgs e)
        {
            var newOffset = e.GetNewOffset(e.Offset);
            var ln = Document.GetLineByOffset(newOffset).LineNumber;
            if (ln < stateCache.Count)
            {
                var line = Document.GetLineByOffset(newOffset);
                var lastState = GetState(line);

                var high = new Highlighter(this, lastState);
                await high.GetColoredSegments(Document, line.Offset, line.Length + line.DelimiterLength);
                OnHighlightingStateChanged(new LineEventArgs(line));
                stateCache.RemoveRange(ln - 1, stateCache.Count - ln + 1);
            }
        }

        public Task<HighlightedLine> GetHighlightedLineAsync(IDocumentLine line, CancellationToken cancellationToken)
        {
            if (Document == null)
            {
                return DefaultSyntaxHighlighting.Instance.GetHighlightedLineAsync(line, cancellationToken);
            }
            var snapshot = Document.CreateSnapshot();
            var high = new Highlighter(this, GetState(line));
            int offset = line.Offset;
            int length = line.Length;
            //return Task.Run (async delegate {
            return high.GetColoredSegments(snapshot, offset, length);
            //});
        }

        public async Task<ScopeStack> GetScopeStackAsync(int offset, CancellationToken cancellationToken)
        {
            var line = Document.GetLineByOffset(offset);
            var state = GetState(line);
            var lineOffset = line.Offset;
            if (lineOffset == offset)
            {
                return state.ScopeStack;
            }

            var high = new Highlighter(this, state);
            foreach (var seg in (await high.GetColoredSegments(Document, lineOffset, line.Length)).Segments)
            {
                if (seg.Contains(offset - lineOffset))
                    return seg.ScopeStack;
            }
            return high.State.ScopeStack;
        }

        List<HighlightState> stateCache = new List<HighlightState>();

        HighlightState GetState(IDocumentLine line)
        {
            var pl = line?.PreviousLine;
            if (pl == null)
                return HighlightState.CreateNewState(this);
            if (stateCache.Count == 0)
                stateCache.Add(HighlightState.CreateNewState(this));
            var ln = line.LineNumber;
            if (ln <= stateCache.Count)
            {
                return stateCache[ln - 1].Clone();
            }
            var lastState = stateCache[stateCache.Count - 1];
            var cur = Document.GetLineByNumber(stateCache.Count);
            if (cur != null && cur.Offset < line.Offset)
            {
                do
                {
                    var high = new Highlighter(this, lastState.Clone());
                    high.GetColoredSegments(Document, cur.Offset, cur.Length + cur.DelimiterLength).Wait();
                    stateCache.Add(lastState = high.State);
                    cur = cur.NextLine;
                } while (cur != null && cur.Offset < line.Offset);
            }

            return lastState.Clone();
        }


        class HighlightState : IEquatable<HighlightState>
        {
            public ImmutableStack<SyntaxContext> ContextStack;
            public ImmutableStack<SyntaxMatch> MatchStack;
            public ScopeStack ScopeStack;


            public static HighlightState CreateNewState(SyntaxHighlighting highlighting)
            {
                return new HighlightState
                {
                    ContextStack = ImmutableStack<SyntaxContext>.Empty.Push(highlighting.definition.MainContext),
                    ScopeStack = new ScopeStack(highlighting.definition.Scope),
                    MatchStack = ImmutableStack<SyntaxMatch>.Empty
                };
            }


            internal HighlightState Clone()
            {
                return new HighlightState
                {
                    ContextStack = this.ContextStack,
                    ScopeStack = this.ScopeStack,
                    MatchStack = this.MatchStack
                };
            }


            public bool Equals(HighlightState other)
            {
                return ContextStack.SequenceEqual(other.ContextStack) && ScopeStack.SequenceEqual(other.ScopeStack) && MatchStack.SequenceEqual(other.MatchStack);
            }
        }

        class Highlighter
        {
            HighlightState state;
            SyntaxHighlighting highlighting;
            ImmutableStack<SyntaxContext> ContextStack { get { return state.ContextStack; } set { state.ContextStack = value; } }
            ImmutableStack<SyntaxMatch> MatchStack { get { return state.MatchStack; } set { state.MatchStack = value; } }
            ScopeStack ScopeStack { get { return state.ScopeStack; } set { state.ScopeStack = value; } }

            public HighlightState State
            {
                get
                {
                    return state;
                }
            }

            public Highlighter(SyntaxHighlighting highlighting, HighlightState state)
            {
                this.highlighting = highlighting;
                this.state = state;
            }

            static readonly TimeSpan matchTimeout = TimeSpan.FromMilliseconds(500);

            public Task<HighlightedLine> GetColoredSegments(ITextSource text, int startOffset, int length)
            {
                if (ContextStack.IsEmpty)
                    return Task.FromResult(new HighlightedLine(new SimpleSegment(startOffset, length), new[] { new HighlightingSegment(0, length, ScopeStack.Empty) }));
                SyntaxContext currentContext = null;
                List<SyntaxContext> lastContexts = new List<SyntaxContext>();
                System.Text.RegularExpressions.Match match = null;
                SyntaxMatch curMatch = null;
                var segments = new List<HighlightingSegment>();
                int offset = 0;
                int curSegmentOffset = 0;
                int endOffset = offset + length;
                int lastMatch = -1;
                var highlightedSegment = new SimpleSegment(startOffset, length);
                string lineText = text.GetText(startOffset, length);

                int timeoutOccursAt;
                unchecked
                {
                    timeoutOccursAt = Environment.TickCount + (int)matchTimeout.TotalMilliseconds;
                }
                restart:
                if (lastMatch == offset)
                {
                    if (lastContexts.Contains(currentContext))
                    {
                        offset++;
                        length--;
                    }
                    else
                    {
                        lastContexts.Add(currentContext);
                    }
                }
                else
                {
                    lastContexts.Clear();
                    lastContexts.Add(currentContext);
                }
                if (length <= 0)
                    goto end;
                lastMatch = offset;
                currentContext = ContextStack.Peek();
                match = null;
                curMatch = null;
                foreach (var m in currentContext.Matches)
                {
                    if (m.GotTimeout)
                        continue;
                    var r = m.GetRegex();
                    if (r == null)
                        continue;
                    try
                    {
                        var possibleMatch = r.Match(lineText, offset, length/*, matchTimeout*/);
                        if (possibleMatch.Success)
                        {
                            if (match == null || possibleMatch.Index < match.Index)
                            {
                                match = possibleMatch;
                                curMatch = m;
                                // Console.WriteLine (match.Index + " possible match : " + m + "/" + possibleMatch.Index + "-" + possibleMatch.Length);
                            }
                            else
                            {
                                // Console.WriteLine (match.Index + " skip match : " + m + "/" + possibleMatch.Index + "-" + possibleMatch.Length);
                            }
                        }
                        else
                        {
                            // Console.WriteLine ("fail match : " + m);
                        }
                    }
                    catch (Exception)//RegexMatchTimeoutException)
                    {
                        //LoggingService.LogWarning("Warning: Regex " + m.Match + " timed out on line:" + text.GetTextAt(offset, length));
                        m.GotTimeout = true;
                        continue;
                    }
                }
                if (Environment.TickCount >= timeoutOccursAt)
                {
                    curMatch.GotTimeout = true;
                    goto end;
                }

                if (match != null)
                {
                    // Console.WriteLine (match.Index + " taken match : " + curMatch + "/" + match.Index + "-" + match.Length);
                    var matchEndOffset = match.Index + match.Length;
                    if (curSegmentOffset < match.Index && match.Length > 0)
                    {
                        segments.Add(new HighlightingSegment(curSegmentOffset, match.Index - curSegmentOffset, ScopeStack));
                        curSegmentOffset = match.Index;
                    }
                    if (curMatch.Pop)
                    {
                        PopMetaContentScopeStack(currentContext, curMatch);
                    }

                    PushScopeStack(curMatch.Scope);

                    if (curMatch.Captures.Groups.Count > 0)
                    {
                        for (int i = 0; i < curMatch.Captures.Groups.Count; ++i)
                        {
                            var capture = curMatch.Captures.Groups[i];
                            var grp = match.Groups[capture.Item1];
                            if (grp == null || grp.Length == 0)
                                continue;
                            if (curSegmentOffset < grp.Index)
                            {
                                ReplaceSegment(segments, new HighlightingSegment(curSegmentOffset, grp.Index - curSegmentOffset, ScopeStack));
                            }
                            ReplaceSegment(segments, new HighlightingSegment(grp.Index, grp.Length, ScopeStack.Push(capture.Item2)));
                            curSegmentOffset = Math.Max(curSegmentOffset, grp.Index + grp.Length);
                        }
                    }

                    if (curMatch.Captures.NamedGroups.Count > 0)
                    {
                        for (int i = 0; i < curMatch.Captures.NamedGroups.Count; ++i)
                        {
                            var capture = curMatch.Captures.NamedGroups[i];
                            var grp = match.Groups[capture.Item1];
                            if (grp == null || grp.Length == 0)
                                continue;
                            if (curSegmentOffset < grp.Index)
                            {
                                ReplaceSegment(segments, new HighlightingSegment(curSegmentOffset, grp.Index - curSegmentOffset, ScopeStack));
                            }
                            ReplaceSegment(segments, new HighlightingSegment(grp.Index, grp.Length, ScopeStack.Push(capture.Item2)));
                            curSegmentOffset = grp.Index + grp.Length;
                        }
                    }

                    if (curMatch.Scope.Count > 0 && curSegmentOffset < matchEndOffset && match.Length > 0)
                    {
                        segments.Add(new HighlightingSegment(curSegmentOffset, matchEndOffset - curSegmentOffset, ScopeStack));
                        curSegmentOffset = matchEndOffset;
                    }

                    if (curMatch.Pop)
                    {
                        if (matchEndOffset - curSegmentOffset > 0)
                            segments.Add(new HighlightingSegment(curSegmentOffset, matchEndOffset - curSegmentOffset, ScopeStack));
                        //if (curMatch.Scope != null)
                        //	scopeStack = scopeStack.Pop ();
                        PopStack(currentContext, curMatch);
                        curSegmentOffset = matchEndOffset;
                    }
                    else if (curMatch.Set != null)
                    {
                        // if (matchEndOffset - curSegmentOffset > 0)
                        //	segments.Add (new ColoredSegment (curSegmentOffset, matchEndOffset - curSegmentOffset, ScopeStack));
                        //if (curMatch.Scope != null)
                        //	scopeStack = scopeStack.Pop ();
                        PopMetaContentScopeStack(currentContext, curMatch);
                        PopStack(currentContext, curMatch);
                        //curSegmentOffset = matchEndOffset;
                        var nextContexts = curMatch.Set.GetContexts(currentContext);
                        PushStack(curMatch, nextContexts);
                        goto skip;
                    }
                    else if (curMatch.Push != null)
                    {
                        var nextContexts = curMatch.Push.GetContexts(currentContext);
                        PushStack(curMatch, nextContexts);
                    }
                    else
                    {
                        if (curMatch.Scope.Count > 0)
                        {
                            for (int i = 0; i < curMatch.Scope.Count; i++)
                                ScopeStack = ScopeStack.Pop();
                        }
                    }

                    if (curSegmentOffset < matchEndOffset && match.Length > 0)
                    {
                        segments.Add(new HighlightingSegment(curSegmentOffset, matchEndOffset - curSegmentOffset, ScopeStack));
                        curSegmentOffset = matchEndOffset;
                    }
                    skip:
                    length -= curSegmentOffset - offset;
                    offset = curSegmentOffset;
                    goto restart;
                }

                end:
                if (endOffset - curSegmentOffset > 0)
                {
                    segments.Add(new HighlightingSegment(curSegmentOffset, endOffset - curSegmentOffset, ScopeStack));
                }

                return Task.FromResult(new HighlightedLine(highlightedSegment, segments));
            }

            void PushStack(SyntaxMatch curMatch, IEnumerable<SyntaxContext> nextContexts)
            {
                if (nextContexts != null)
                {
                    bool first = true;
                    foreach (var nextContext in nextContexts)
                    {
                        var ctx = nextContext;
                        if (curMatch.WithPrototype != null)
                            ctx = new SyntaxContextWithPrototype(nextContext, curMatch.WithPrototype);

                        if (first)
                        {
                            MatchStack = MatchStack.Push(curMatch);
                            first = false;
                        }
                        else
                        {
                            MatchStack = MatchStack.Push(null);
                        }
                        ContextStack = ContextStack.Push(ctx);
                        PushScopeStack(ctx.MetaScope);
                        PushScopeStack(ctx.MetaContentScope);
                    }
                }
            }

            void PushScopeStack(IReadOnlyList<string> scopeList)
            {
                if (scopeList == null)
                    return;
                for (int i = 0; i < scopeList.Count; ++i)
                {
                    var scope = scopeList[i];
                    ScopeStack = ScopeStack.Push(scope);
                }
            }

            void PopScopeStack(IReadOnlyList<string> scopeList)
            {
                if (scopeList == null)
                    return;
                for (int i = 0; !ScopeStack.IsEmpty && i < scopeList.Count; i++)
                    ScopeStack = ScopeStack.Pop();
            }

            void PopMetaContentScopeStack(SyntaxContext currentContext, SyntaxMatch curMatch)
            {
                if (ContextStack.Count() == 1)
                {
                    return;
                }
                PopScopeStack(currentContext.MetaContentScope);
            }

            void PopStack(SyntaxContext currentContext, SyntaxMatch curMatch)
            {
                if (ContextStack.Count() == 1)
                {
                    MatchStack = MatchStack.Clear();
                    ScopeStack = new ScopeStack(highlighting.definition.Scope);
                    return;
                }
                ContextStack = ContextStack.Pop();
                if (!MatchStack.IsEmpty)
                {
                    PopScopeStack(MatchStack.Peek()?.Scope);
                    MatchStack = MatchStack.Pop();
                }
                PopScopeStack(currentContext.MetaScope);

                if (curMatch.Scope.Count > 0 && !ScopeStack.IsEmpty)
                {
                    for (int i = 0; i < curMatch.Scope.Count; i++)
                        ScopeStack = ScopeStack.Pop();
                }
            }
        }

        internal static void ReplaceSegment(List<HighlightingSegment> list, HighlightingSegment newSegment)
        {
            if (list.Count == 0)
            {
                list.Add(newSegment);
                return;
            }
            int i = list.Count;
            while (i > 0 && list[i - 1].EndOffset > newSegment.Offset)
            {
                i--;
            }
            if (i >= list.Count)
            {
                list.Add(newSegment);
                return;
            }

            int j = i;
            while (j + 1 < list.Count && newSegment.EndOffset > list[j + 1].Offset)
            {
                j++;
            }
            var startItem = list[i];
            var endItem = list[j];
            list.RemoveRange(i, j - i + 1);
            var lengthAfter = endItem.EndOffset - newSegment.EndOffset;
            if (lengthAfter > 0)
                list.Insert(i, new HighlightingSegment(newSegment.EndOffset, lengthAfter, endItem.ScopeStack));

            list.Insert(i, newSegment);
            var lengthBefore = newSegment.Offset - startItem.Offset;
            if (lengthBefore > 0)
                list.Insert(i, new HighlightingSegment(startItem.Offset, lengthBefore, startItem.ScopeStack));
        }

        void OnHighlightingStateChanged(LineEventArgs e)
        {
            HighlightingStateChanged?.Invoke(this, e);
        }

        public event EventHandler<LineEventArgs> HighlightingStateChanged;
    }

}
