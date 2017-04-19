namespace AvalonStudio.Debugging.DotNetCore
{
    using System;
    using System.Diagnostics.SymbolStore;
    using System.Reflection.Metadata;

    internal class SymbolVariable : ISymbolVariable
    {
        private string _name;
        private LocalVariableAttributes _attributes;
        private int _index;

        public SymbolVariable(string name, LocalVariableAttributes attributes, int index)
        {
            _name = name;
            _attributes = attributes;
            _index = index;
        }

        public int AddressField1 => _index;

        public int AddressField2 => throw new NotImplementedException();

        public int AddressField3 => throw new NotImplementedException();

        public SymAddressKind AddressKind => throw new NotImplementedException();

        public object Attributes => _attributes;

        public int EndOffset => throw new NotImplementedException();

        public string Name => _name;

        public int StartOffset => throw new NotImplementedException();

        public byte[] GetSignature()
        {
            throw new NotImplementedException();
        }
    }
}