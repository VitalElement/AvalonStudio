namespace AvalonStudio.Debugging.DotNetCore
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.SymbolStore;
    using System.Reflection.Metadata;
    using System.Text;

    class SymbolVariable : ISymbolVariable
    {
        private string _name;
        private LocalVariableAttributes _attributes;

        public SymbolVariable(string name, LocalVariableAttributes attributes)
        {
            _name = name;
            _attributes = attributes;
        }

        public int AddressField1 => throw new NotImplementedException();

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
