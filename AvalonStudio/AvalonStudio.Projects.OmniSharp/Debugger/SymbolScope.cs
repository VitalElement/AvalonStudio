namespace AvalonStudio.Debugging.DotNetCore
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.SymbolStore;

    internal class SymbolScope : ISymbolScope
    {
        private ISymbolMethod _method;
        private ISymbolScope _parent;
        private int _startOffset;
        private int _endOffset;
        private List<ISymbolScope> _children;
        private List<ISymbolVariable> _locals;

        public SymbolScope(ISymbolMethod method, ISymbolScope parent, int startOffset, int endOffset)
        {
            _children = new List<ISymbolScope>();
            _locals = new List<ISymbolVariable>();
            _startOffset = startOffset;
            _endOffset = endOffset;
            _parent = parent;
            _method = method;
        }

        internal void AddChild(ISymbolScope childScope)
        {
            _children.Add(childScope);
        }

        internal void AddLocal(ISymbolVariable variable)
        {
            _locals.Add(variable);
        }

        public int StartOffset => _startOffset;

        public int EndOffset => _endOffset;

        public ISymbolMethod Method => _method;

        public ISymbolScope Parent => _parent;

        public ISymbolScope[] GetChildren()
        {
            return _children.ToArray();
        }

        public ISymbolVariable[] GetLocals()
        {
            return _locals.ToArray();
        }

        public ISymbolNamespace[] GetNamespaces()
        {
            throw new NotImplementedException();
        }
    }
}