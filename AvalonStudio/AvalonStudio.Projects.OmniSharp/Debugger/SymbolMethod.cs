namespace AvalonStudio.Debugging.DotNetCore
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.SymbolStore;
    using System.Linq;
    using System.Reflection.Metadata;

    public class SymbolMethod : ISymbolMethod
    {
        private List<SequencePoint> _sequencePoints;
        private SymbolToken _token;
        private ISymbolScope _rootScope;

        public SymbolMethod(SequencePointCollection sequencePoints, SymbolToken token)
        {
            _sequencePoints = sequencePoints.ToList();

            _token = token;
        }

        internal void SetRootScope(ISymbolScope rootScope)
        {
            _rootScope = rootScope;
        }

        public List<SequencePoint> SequencePoints => _sequencePoints;

        public ISymbolDocument Document { get; set; }

        public SymbolToken Token => _token;

        public int SequencePointCount => _sequencePoints.Count();

        public ISymbolScope RootScope => _rootScope;

        public ISymbolNamespace GetNamespace()
        {
            throw new NotImplementedException();
        }

        public int GetOffset(ISymbolDocument document, int line, int column)
        {
            throw new NotImplementedException();
        }

        public ISymbolVariable[] GetParameters()
        {
            throw new NotImplementedException();
        }

        public int[] GetRanges(ISymbolDocument document, int line, int column)
        {
            throw new NotImplementedException();
        }

        public ISymbolScope GetScope(int offset)
        {
            throw new NotImplementedException();
        }

        public void GetSequencePoints(int[] offsets, ISymbolDocument[] documents, int[] lines, int[] columns, int[] endLines, int[] endColumns)
        {
            int i = 0;

            foreach (var sequencePoint in _sequencePoints)
            {
                offsets[i] = sequencePoint.Offset;
                documents[i] = Document;
                lines[i] = sequencePoint.StartLine;
                columns[i] = sequencePoint.StartColumn;
                endLines[i] = sequencePoint.EndLine;
                endColumns[i] = sequencePoint.EndColumn;
                i++;
            }
        }

        public bool GetSourceStartEnd(ISymbolDocument[] docs, int[] lines, int[] columns)
        {
            throw new NotImplementedException();
        }
    }
}